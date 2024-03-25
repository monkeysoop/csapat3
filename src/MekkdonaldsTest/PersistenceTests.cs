using Mekkdonalds.Persistence;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mekkdonalds.Test;

public class PersistenceTests
{
    private LogFile logFile;
    [SetUp]
    public async void Setup()
    {
        logFile = await new LogFileDataAccess().Load("""C: \Users\Sanyi\Documents\GitHub\csapat3\src\MekkdonaldsWPF\samples\random_20_log_json""");
    }

    [Test]
    public void Test1()
    {
        Assert.That(logFile, Is.Not.Null);
    }
}