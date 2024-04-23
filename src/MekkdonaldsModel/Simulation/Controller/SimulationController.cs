using System.Diagnostics.CodeAnalysis;

namespace Mekkdonalds.Simulation.Controller;

public sealed class SimulationController : Controller
{
#pragma warning disable CA1859
    private readonly IAssigner _pathFinder;
#pragma warning restore
    [NotNull]
    private Logger _logger;
    private readonly ILogFileDataAccess _logFileDataAccess;

    public SimulationController(string path, ISimDataAccess da, ControllerType algorithm)
    {
        _pathFinder = new Assigner.Assigner();
        Load(path, da, algorithm);

        _logFileDataAccess = da.LDA;

        _pathFinder.Ended += OnEnded;
    }

    private void OnEnded(object? sender, EventArgs e)
    {
        Timer.Change(Timeout.Infinite, Timeout.Infinite);

        SaveLog();
    }

    private async void Load(string path, ISimDataAccess da, ControllerType algorithm)
    {
        await Task.Run(async () =>
        {
            var config = await da.CDA.LoadAsync(path);

            _logger = new Logger(config.MapFile.Split('/')[^1].Replace(".map", ""));

            var b = await da.BDA.LoadAsync(config.MapFile);
            _board = b; // for some reason it only sets board this way ????????

            _robots.AddRange(await da.RDA.LoadAsync(config.AgentFile, _board.Width - 2, _board.Height - 2));
            _logger.LogStarts(_robots);

            var tasks = await da.PDA.LoadAsync(config.TaskFile, _board.Width - 2, _board.Height - 2);
            _logger.LogTasks(tasks);

            _pathFinder.Init(algorithm, b, _robots, tasks, _logger);

            LoadWalls();

            OnLoaded(this);
        });
    }

    public async void SaveLog()
    {
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
