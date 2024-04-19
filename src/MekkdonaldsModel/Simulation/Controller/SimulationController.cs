﻿using System.Diagnostics.CodeAnalysis;

namespace Mekkdonalds.Simulation.Controller;

public sealed class SimulationController : Controller
{
#pragma warning disable CA1859
    private readonly IAssigner _pathFinder;
#pragma warning restore
    [NotNull]
    private Logger _logger;
    private readonly ILogFileDataAccess _logFileDataAccess;

    public SimulationController(string path, ISimDataAccess da)
    {
        _pathFinder = new Assigner.Assigner();
        Load(path, da);

        _logFileDataAccess = da.LDA;

        _pathFinder.Ended += OnEnded;
    }

    private void OnEnded(object? sender, EventArgs e)
    {
        Timer.Change(Timeout.Infinite, Timeout.Infinite);

        SaveLog();
    }

    private async void Load(string path, ISimDataAccess da)
    {
        var config = await da.CDA.Load(path);

        _logger = new Logger(config.MapFile.Split('/')[^1].Replace(".map", ""));

        var b = await da.BDA.LoadAsync(config.MapFile);
        _board = b; // for some reason it only sets board this way ????????

        _robots.AddRange(await da.RDA.LoadAsync(config.AgentFile, _board.Width - 2, _board.Height - 2));
        _logger.LogStarts(_robots);

        var tasks = await da.PDA.LoadAsync(config.TaskFile, _board.Width - 2, _board.Height - 2);
        _logger.LogTasks(tasks);

        _pathFinder.Init(ControllerType.BFS, b, _robots, tasks, _logger);

        LoadWalls();

        OnLoaded(this);
    }

    private async void SaveLog()
    {
        foreach (var r in _robots)
        {
            while (r.History.Count < _pathFinder.TimeStamp + 1)
            {
                r.Step(Action.W);
            }
        }

        _logger.LogActualPaths(_robots);

        _logger.LogReplayLength(_pathFinder.TimeStamp + 1);

        await _logger.SaveAsync(_logFileDataAccess);
    }

    protected override void OnTick(object? state)
    {
        _pathFinder.Step();

        CallTick(this);
    }

    public override void StepForward()
    {
        if (!IsPlaying) OnTick(null);
    }
}
