namespace Mekkdonalds.Test.Persistence;

[TestOf(typeof(RobotsDataAccess))]
public class AgentsTests
{
    private RobotsDataAccess _robotsDataAccess;
    private List<Robot> _agents;

    [SetUp]
    public async Task Setup()
    {
        _robotsDataAccess = new();
        _agents = await _robotsDataAccess.LoadAsync("../../../../MekkdonaldsWPF/agents/random_20.agents", 32, 32);
    }

    [Test]
    public void LoadTest()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_agents!.All(x => x.Direction == Direction.North));
            Assert.That(_agents![0].Position, Is.EqualTo(new Point(7, 5)));
            Assert.That(_agents![^1].Position, Is.EqualTo(new Point(25, 1)));
            Assert.That(_agents![13].Position, Is.EqualTo(new Point(26, 24)));
        });
    }
}
