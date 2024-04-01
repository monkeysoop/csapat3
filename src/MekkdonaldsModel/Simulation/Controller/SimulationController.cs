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
    public SimulationController(string path, IConfigDataAccess ca, IBoardDataAccess ba, IRobotsDataAccess ra, IPackagesDataAccess pa)
    {
        _pathFinder = new Assigner.Assigner();
        Load(path, ca, ba, ra, pa);
    }

    private async void Load(string path, IConfigDataAccess da, IBoardDataAccess ba, IRobotsDataAccess ra, IPackagesDataAccess pa)
    {
        var config = await da.Load(path);

        var b = await ba.LoadAsync(config.MapFile);
        _board = b; // for some reason it only sets board this way ????????

        _robots.AddRange(await ra.LoadAsync(config.AgentFile, _board.Width - 2, _board.Height - 2));

        _pathFinder.Init(ControllerType.BFS, b, _robots, await pa.LoadAsync(config.TaskFile, _board.Width - 2, _board.Height - 2));

        LoadWalls();

        OnLoaded(this);
    }

    private void LoadWalls()
    {
        for (int y = 0; y < _board.Height; y++)
        {
            for (int x = 0; x < _board.Width; x++)
            {
                if (_board.GetValue(x, y) is Board2.WALL)
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
}
