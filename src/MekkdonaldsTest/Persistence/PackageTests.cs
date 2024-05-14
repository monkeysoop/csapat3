namespace Mekkdonalds.Test.Persistence;

[TestOf(typeof(PackagesDataAccess))]
public class PackageTests
{
    private PackagesDataAccess _packagesDataAccess;
    private List<Package> _tasks;

    [SetUp]
    public async Task Setup()
    {
        _packagesDataAccess = new();
        _tasks = await _packagesDataAccess.LoadAsync("../../../../MekkdonaldsWPF/tasks/random-32-32-20.tasks", 32, 32);

    }

    [Test]
    public void TestTasksLoad()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_tasks![0].Position, Is.EqualTo(new Point(7, 22)));
            Assert.That(_tasks![^1].Position, Is.EqualTo(new Point(2, 26)));
            Assert.That(_tasks![13].Position, Is.EqualTo(new Point(23, 27)));
        });

    }
}
