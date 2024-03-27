using Mekkdonalds.Simulation.PathFinding;

namespace Mekkdonalds.Simulation.Controller;

public sealed class SimulationController : Controller
{
    private readonly PathFinder _pathFinder;

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
        _pathFinder = new Astar();
        Load(path, ca, ba, ra, pa);
    }

    private async void Load(string path, IConfigDataAccess da, IBoardDataAccess ba, IRobotsDataAccess ra, IPackagesDataAccess pa)
    {
        var config = await da.Load(path);

        var b = await ba.LoadAsync(config.MapFile);
        _board = b; // for some reason it only sets board this way ????????

        _robots.AddRange(await ra.LoadAsync(config.AgentFile, _board.Width - 2, _board.Height - 2));

        _pathFinder.Init(b ,await pa.LoadAsync(config.TaskFile, _board.Width - 2, _board.Height - 2));

        LoadWalls();

        InitPaths();

        OnLoaded(this);
    }

    private void LoadWalls()
    {
        for (int y = 0; y < _board.Height; y++)
        {
            for (int x = 0; x < _board.Width; x++)
            {
                if (_board.GetValue(x, y) == Board2.WALL)
                {
                    _walls.Add(new(x, y));
                }
            }
        }
    }

    private void InitPaths()
    {
        foreach (var r in _robots)
        {
            _pathFinder.Assign(r);
        }
    }

    protected override void OnTick(object? state)
    {
        foreach (var r in _robots)
        {
            _pathFinder.Step(r);
        } 

        CallTick(this);
    }
}
