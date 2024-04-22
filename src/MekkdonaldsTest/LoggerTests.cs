using Mekkdonalds.Simulation;
using NUnit.Framework.Legacy;
using System.Drawing;

namespace MekkdonaldsTest;

internal class LoggerTests
{
    private Logger _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Logger("test");
    }

    [Test]
    public void DirectoryCreateTest()
    {
        Assert.That(Directory.Exists("./logs"));
    }

    [Test]
    public void StartLogTests()
    {
        var rs = new List<Robot>();
        rs.AddRange(Enumerable.Range(0, 10).Select(_ => new Robot(new(Random.Shared.Next(30), Random.Shared.Next(30)), (Direction)Random.Shared.Next(4))));
        _logger.LogStarts(rs);
        Assert.That(rs, Is.EqualTo(_logger.GetLogFile().Start).AsCollection);
    }
}
