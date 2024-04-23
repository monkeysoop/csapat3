using System.Text.Json;

namespace Mekkdonalds.Test.Persistence;

[TestOf(typeof(Config))]
[TestOf(typeof(ConfigDataAccess))]
public class ConfigTests
{
    private ConfigDataAccess _configDataAccess;
    private Config _config;

    [SetUp]
    public async Task Setup()
    {
        _configDataAccess = new ConfigDataAccess();
        _config = await _configDataAccess.LoadAsync("../../../../MekkdonaldsWPF/configs/random_20_config.json");
    }

    [Test]
    public void LoadTest()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_config.MapFile, Is.EqualTo("maps/random-32-32-20.map"));
            Assert.That(_config.AgentFile, Is.EqualTo("agents/random_20.agents"));
            Assert.That(_config.TeamSize, Is.EqualTo(20));
            Assert.That(_config.TaskFile, Is.EqualTo("tasks/random-32-32-20.tasks"));
            Assert.That(_config.NumTasksReveal, Is.EqualTo(1));
            Assert.That(_config.TaskAssignmentStrategy, Is.EqualTo(Strategy.RoundRobin));
        });

    }


    [Test]
    public void ExceptionTest()
    {
        Assert.ThrowsAsync<JsonException>(async () =>
        {
            _config = await _configDataAccess.LoadAsync("../../../../MekkdonaldsWPF/logs/random_20_log.json");
        });
    }
}
