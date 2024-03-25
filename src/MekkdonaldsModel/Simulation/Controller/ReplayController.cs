namespace Mekkdonalds.Simulation.Controller;

public sealed class ReplayController : Controller
{
    public ReplayController(string path, object la)
    {
        Load(path, la);
    }

    private async void Load(string path, object la)
    {
        var log = new LogFile();
    }

    protected override void OnTick(object? state)
    {
        throw new NotImplementedException();
    }
}
