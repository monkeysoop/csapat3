using Mekkdonalds.Persistence;
using Mekkdonalds.Simulation;

using System.Drawing;

using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Test;

public class PersistenceTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void LogReadTest()
    {
        Assert.Multiple(async () =>
        {
            //Assert.IsTrue(l.AllValid); // It can't find AllValid in Logfile, because it searches for allValid instant of AllValid, and for some reason it's already written in camelcase in logfile :)

            var logFileDataAccess = new LogFileDataAccess();
            var log = await logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/samples/random_20_log.json");

            Assert.That(log.ActionModel, Is.EqualTo("MAPF_T"));
            Assert.That(log.TeamSize, Is.EqualTo(20));
            Assert.That(log.Start[0], Is.EqualTo((new Point(7, 5), Direction.East)));
            Assert.That(log.NumTaskFinished, Is.EqualTo(338));
            Assert.That(log.SumOfCost, Is.EqualTo(10000));
            Assert.That(log.Makespan, Is.EqualTo(500));
            Assert.That(log.ActualPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(log.PlannerPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(log.PlannerTimes[0], Is.EqualTo(0.312150264));
            Assert.That(log.Errors, Is.Empty);
            Assert.That(log.Events, Is.TypeOf<List<List<(int, int, string)>>>());
            Assert.That(log.Events[0][0], Is.EqualTo((0, 0, "assigned")));
            Assert.That(log.Tasks[0], Is.EqualTo((0, 21, 6)));

            log = await logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/samples/warehouse_100_log.json");
            Assert.That(log.Errors[0], Is.EqualTo((-1, -1, 1, "incorrect vector size")));
        });
    }

    [Test]
    public async Task LogWriteTest()
    {
        var logFileDataAccess = new LogFileDataAccess();
        var log = new LogFile()
        {
            ActionModel = "MAPF_T",
            TeamSize = 20,
            Start = [(new Point(4, 6), Direction.East)],
            NumTaskFinished = 338,
            SumOfCost = 10000,
            Makespan = 500,
            ActualPaths = [[Action.F]],
            PlannerPaths = [[Action.F]],
            PlannerTimes = [0.312150264],
            Errors = [],
            Events = [[(0, 0, "assigned")]],
            Tasks = [(0, 21, 6)]
        };

        await logFileDataAccess.SaveAsync("./test_log.json", log);

        log = await logFileDataAccess.LoadAsync("./test_log.json");

        Assert.Multiple(() =>
        {
            Assert.That(log.ActionModel, Is.EqualTo("MAPF_T"));
            Assert.That(log.TeamSize, Is.EqualTo(20));
            Assert.That(log.Start[0], Is.EqualTo((new Point(4, 6), Direction.East)));
            Assert.That(log.NumTaskFinished, Is.EqualTo(338));
            Assert.That(log.SumOfCost, Is.EqualTo(10000));
            Assert.That(log.Makespan, Is.EqualTo(500));
            Assert.That(log.ActualPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(log.PlannerPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(log.PlannerTimes[0], Is.EqualTo(0.312150264));
            Assert.That(log.Errors, Is.Empty);
            Assert.That(log.Events, Is.TypeOf<List<List<(int, int, string)>>>());
            Assert.That(log.Events[0][0], Is.EqualTo((0, 0, "assigned")));
            Assert.That(log.Tasks[0], Is.EqualTo((0, 21, 6)));
        });
    }
}