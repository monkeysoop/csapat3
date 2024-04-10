namespace Mekkdonalds.Simulation.Controller;

public sealed class ReplayController : Controller
{
    private readonly ConcurrentDictionary<Robot, List<Path>> Paths = [];

    public int TimeStamp { get; private set; }
    public int Length { get; private set; }

    public ReplayController(string path, object la)
    {
        Load(path, la);
    }

    private async void Load(string path, object la)
    {
        throw new NotImplementedException();
    }

    protected override void OnTick(object? state)
    {
        CallTick(this);
    }

    public void JumpTo(int time)
    {
        if (time < 0 || time > Length) throw new ArgumentOutOfRangeException(nameof(time), "Time must be between 0 and the length of the replay");

        throw new NotImplementedException();
    }

    public override void StepForward()
    {
        JumpTo(TimeStamp + 1);
    }

    public void StepBackward()
    {
        JumpTo(TimeStamp - 1);
    }
}
