namespace Mekkdonalds.Simulation.Controller;

internal class Model
{
    private IBoardDataAccess _boardDataAccess;
    private IRobotsDataAccess _packagesDataAccess;
    private IPackagesDataAccess _robotsDataAccess;

    private Board2 _board;
    private List<Robot> _robots;
    private List<Package> _packages;

    PathFinder _simulationController;
    

    public Model(IBoardDataAccess boardDataAccess, IRobotsDataAccess packagesDataAccess, IPackagesDataAccess robotsDataAccess, PathFinder simulationController) 
    {
        _boardDataAccess = boardDataAccess;
        _packagesDataAccess = packagesDataAccess;
        _robotsDataAccess = robotsDataAccess;

        _simulationController = simulationController;

        _board = new Board2(10, 10);
        _robots = new List<Robot>();
        _packages = new List<Package>();

    }

    public void NewSimulation(int height, int width)
    {
        _board = new Board2(height, width);
        _robots = new List<Robot>();
        _packages = new List<Package>();
    }

    public void CalculatePaths()
    {
    }
}
