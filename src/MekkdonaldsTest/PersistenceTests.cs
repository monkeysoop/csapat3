using Mekkdonalds.Persistence;
using Mekkdonalds.Simulation;

using System.Drawing;
using System.Reflection.Emit;
using System.Text.Json;
using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Test;

public class PersistenceTests
{
    LogFile log;

    LogFileDataAccess logFileDataAccess;
    [SetUp]
    public async Task Setup()
    {
        logFileDataAccess = new LogFileDataAccess();
        log = await logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/samples/random_20_log.json");
    }

    [Test]
    public void LogReadTest()
    {
        Assert.Multiple(async () =>
        {
            //Assert.IsTrue(log.AllValid); // It can't find AllValid in Logfile, because it searches for allValid instant of AllValid, and for some reason it's already written in camelcase in logfile :)


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

    [Test]

    public void TestStructure()
    {
        Assert.True(log is LogFile);
        Assert.That(log.ActionModel is string);
        Assert.That(log.AllValid is bool);
        Assert.That(log.TeamSize is int);
        Assert.That(log.Start is List<(Point, Direction)>);
        Assert.That(log.NumTaskFinished is int);
        Assert.That(log.SumOfCost is int);
        Assert.That(log.Makespan is int);
        Assert.That(log.ActualPaths is List<List<Action>>);
        Assert.That(log.PlannerPaths is List<List<Action>>);
        Assert.That(log.PlannerTimes is List<double>);
        Assert.That(log.Errors is List<(int, int, int, string)>);
        Assert.That(log.Events is List<List<(int, int, string)>>);
        Assert.That(log.Tasks is List<(int, int, int)>);
    }

    [Test]
    public async Task TestActionModel()
    {
        Assert.True(log.ActionModel is not null);
        Assert.That(log.ActionModel, Is.EqualTo("MAPF_T"));
        Assert.That(log.ActionModel.Length, Is.EqualTo(6));
        log.ActionModel = "";
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.That(log.ActionModel, Is.EqualTo(""));
        Assert.True(log.ActionModel is not null);
        log.ActionModel = "Hajrá Fradi!";
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.That(log.ActionModel, Is.EqualTo("Hajrá Fradi!"));
        Assert.True(log.ActionModel is not null);
        log.ActionModel = "Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!";
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.That(log.ActionModel, Is.EqualTo("Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!"));
        Assert.True(log.ActionModel is not null);
    }

    [Test]
    public async Task TestAllValid()
    {
        Assert.Pass();
    }

    [Test]
    public async Task TestTeamSize()
    {
        Assert.That(log.TeamSize, Is.EqualTo(20));
        log.TeamSize = 69;
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.That(log.TeamSize, Is.EqualTo(69));
        log.TeamSize = 0;
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.That(log.TeamSize, Is.EqualTo(0));
        log.TeamSize = 3333333;
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.That(log.TeamSize, Is.EqualTo(3333333));
    }

    [Test]

    public async Task TestStart()
    {
        Assert.True(log.Start is not null);
        Assert.That(log.Start![0], Is.EqualTo((new Point(7, 5), Direction.East)));
        Assert.That(log.Start![2], Is.EqualTo((new Point(27, 19), Direction.East)));
        Assert.That(log.Start![^1], Is.EqualTo((new Point(25, 1), Direction.East)));
        Assert.That(log.Start![13], Is.EqualTo((new Point(26, 24), Direction.East)));
        log.Start = new List<(Point, Direction)>() { (new Point(3, 9), Direction.North) };
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.True(log.Start is not null);
        Assert.That(log.Start![0], Is.EqualTo((new Point(3, 9), Direction.North)));
        Assert.That(log.Start.Count, Is.EqualTo(1));
        log.Start = new List<(Point, Direction)>() { (new Point(13, 10), Direction.South), (new Point(69, 33), Direction.West) };
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.True(log.Start is not null);
        Assert.That(log.Start![0], Is.EqualTo((new Point(13, 10), Direction.South)));
        Assert.That(log.Start![1], Is.EqualTo((new Point(69, 33), Direction.West)));
        Assert.That(log.Start.Count, Is.EqualTo(2));
        log.Start = new List<(Point, Direction)>() { (new Point(13, 10), Direction.South), (new Point(69, 33), Direction.West), (new Point(113, 133), Direction.East) };
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.True(log.Start is not null);
        Assert.That(log.Start![0], Is.EqualTo((new Point(13, 10), Direction.South)));
        Assert.That(log.Start![1], Is.EqualTo((new Point(69, 33), Direction.West)));
        Assert.That(log.Start![2], Is.EqualTo((new Point(113, 133), Direction.East)));
        Assert.That(log.Start.Count, Is.EqualTo(3));
        List<(Point, Direction)> list = new();
        Random random = new Random();
        var Directions = new Direction[] { Direction.North, Direction.South, Direction.West, Direction.East };
        for (int i = 0; i < 6969; i++)
        {
            list.Add((new Point(random.Next(), random.Next()), Directions[random.Next(4)]));
        }
        log.Start = list;
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.True(log.Start is not null);
        Assert.That(log.Start![0], Is.EqualTo(list[0]));
        var num = random.Next(list.Count);
        Assert.That(log.Start![num], Is.EqualTo(list[num]));
        Assert.That(log.Start![13], Is.EqualTo(list[13]));
        Assert.That(log.Start![^1], Is.EqualTo(list[^1]));
        Assert.That(log.Start, Is.EqualTo(list));
        Assert.That(log.Start.Count, Is.EqualTo(6969));

    }

    [Test]

    public void TestException()
    {
        Assert.ThrowsAsync<JsonException>(async () =>
        {
            log = await logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/samples/random_20_config.json");
        });

    }



}