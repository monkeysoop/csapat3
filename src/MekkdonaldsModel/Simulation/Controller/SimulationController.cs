namespace Mekkdonalds.Simulation.Controller;

public sealed class SimulationController : Controller
{
    private Assigner.Assigner? _pathFinder;

    private Logger _logger;
    private readonly ILogFileDataAccess _logFileDataAccess;

    public SimulationController(string path, ISimDataAccess da, ControllerType algorithm, Type type)
    {
        Load(path, da, algorithm, type);

        _logFileDataAccess = da.LDA;

        _logger = new Logger("default");
    }

    private void OnEnded(object? sender, EventArgs e)
    {
        Timer.Change(Timeout.Infinite, Timeout.Infinite);

        SaveLog();
    }

    private async void Load(string path, ISimDataAccess da, ControllerType algorithm, Type type)
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

            if (!type.IsSubclassOf(typeof(Assigner.Assigner)))
            {
                throw new ArgumentException("Type must be a subclass of Assigner", nameof(type));
            }

            if (type.GetConstructor([typeof(ControllerType), typeof(Board), typeof(IEnumerable<Package>), typeof(IEnumerable<Robot>)]) is null)
            {
                throw new ArgumentException("Type must have a constructor with a single string parameter", nameof(type));
            }

            _pathFinder = (Assigner.Assigner)Activator.CreateInstance(type, algorithm, b, tasks, _robots, _logger)!;
            _pathFinder.Ended += OnEnded;

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
