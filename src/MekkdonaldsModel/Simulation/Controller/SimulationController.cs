namespace Mekkdonalds.Simulation.Controller;

public sealed class SimulationController : Controller
{
#pragma warning disable CA1859
    private readonly IAssigner _pathFinder;
#pragma warning restore

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">Path of the config file</param>
    /// <param name="ca"></param>
    /// <param name="ba"></param>
    /// <param name="ra"></param>
    /// <param name="pa"></param>
    public SimulationController(string path, ISimDataAccess da)
    {
        _pathFinder = new Assigner.Assigner();
        Load(path, da);
    }

    private async void Load(string path, ISimDataAccess da)
    {
        var config = await da.CDA.Load(path);

        var b = await da.BDA.LoadAsync(config.MapFile);
        _board = b; // for some reason it only sets board this way ????????

        _robots.AddRange(await da.RDA.LoadAsync(config.AgentFile, _board.Width - 2, _board.Height - 2));

        _pathFinder.Init(ControllerType.BFS, b, _robots, await da.PDA.LoadAsync(config.TaskFile, _board.Width - 2, _board.Height - 2));

        LoadWalls();

        OnLoaded(this);
    }

    private void LoadWalls()
    {
        for (int y = 0; y < _board.Height; y++)
        {
            for (int x = 0; x < _board.Width; x++)
            {
                if (_board.GetValue(x, y) is Board.WALL)
                {
                    _walls.Add(new(x, y));
                }
            }
        }
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
