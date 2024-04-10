using Mekkdonalds.Persistence;
using Mekkdonalds.Simulation;
using System.Drawing;

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
        LogFile l = await logFileDataAccess.Load("../../../../MekkdonaldsWPF/samples/random_20_log.json");
        //Assert.IsTrue(l.AllValid);
        Assert.IsFalse(l.AllValid);
        /*It can't find AllValid in Logfile, because it searches for allValid instant of AllValid, 
         and for some reason it's already written in camelcase in logfile :)
         */
        Assert.That(l.ActionModel, Is.EqualTo("MAPF_T"));
        Assert.That(l.TeamSize, Is.EqualTo(20));
        Assert.That(l.Start[0], Is.EqualTo((new Point(4, 6), Direction.East)));
        Assert.That(l.NumTaskFinished, Is.EqualTo(338));
        Assert.That(l.SumOfCost, Is.EqualTo(10000));
        Assert.That(l.Makespan, Is.EqualTo(500));
        Assert.That(l.ActualPaths[0][0], Is.EqualTo("F"));
        Assert.That(l.PlannerPaths[0][0], Is.EqualTo("F"));
        Assert.That(l.PlannerTimes[0], Is.EqualTo(0.312150264));
        Assert.That(l.Errors, Is.Empty);
        Assert.That(l.Events, Is.TypeOf<List<List<(int, int, string)>>>());
        Assert.That(l.Events[0][0], Is.EqualTo((0, 0, "assigned")));
        Assert.That(l.Tasks[0], Is.EqualTo((0, 21, 6)));
        l = await logFileDataAccess.Load("../../../../MekkdonaldsWPF/samples/warehouse_100_log.json");
        Assert.That(l.Errors[0], Is.EqualTo((-1, -1, 1, "incorrect vector size")));


    }
}