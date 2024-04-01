using Mekkdonalds.Persistence;

namespace Mekkdonalds.Test;

public class PersistenceTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        LogFileDataAccess logFileDataAccess = new();
        var l = await logFileDataAccess.Load("../../../../MekkdonaldsWPF/samples/random_20_log.json");
        Assert.Pass();
    }
}