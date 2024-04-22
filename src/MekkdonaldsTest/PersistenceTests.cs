using Mekkdonalds.Persistence;
using Mekkdonalds.Simulation;

using System.Drawing;
using System.Reflection.Emit;
using System.Text.Json;
using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Test;

public class PersistenceTests
{
    private LogFile log;
    private LogFileDataAccess logFileDataAccess;
    private ConfigDataAccess configDataAccess;
    private Config config;
    private BoardFileDataAccess boardFileDataAccess;
    private Board board;
    private RobotsDataAccess robotsDataAccess;
    private List<Robot> agents;
    private PackagesDataAccess packagesDataAccess;
    private List<Package> tasks;

    [SetUp]
    public async Task Setup()
    {
        logFileDataAccess = new LogFileDataAccess();
        log = await logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/logs/random_20_log.json");
        configDataAccess = new ConfigDataAccess();
        config = await configDataAccess.Load("../../../../MekkdonaldsWPF/configs/random_20_config.json");
        boardFileDataAccess = new();
        board = await boardFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/maps/random-32-32-20.map");
        robotsDataAccess = new();
        agents = await robotsDataAccess.LoadAsync("../../../../MekkdonaldsWPF/agents/random_20.agents", 32, 32);
        packagesDataAccess = new();
        tasks = await packagesDataAccess.LoadAsync("../../../../MekkdonaldsWPF/tasks/random-32-32-20.tasks", 32, 32);

    }

    [Test]

    public void TestTasksLoad()
    {
        Assert.Multiple(() =>
        {
            Assert.That(tasks is not null);
            Assert.That(tasks![0].Position, Is.EqualTo(new Point(7, 22)));
            Assert.That(tasks![^1].Position, Is.EqualTo(new Point(2, 26)));
            Assert.That(tasks![13].Position, Is.EqualTo(new Point(23, 27)));
        });

    }

    [Test]

    public void TestAgentsLoad()
    {
        Assert.Multiple(() =>
        {
            Assert.That(agents is not null);
            Assert.That(agents!.All(x => x.Direction == Direction.North));
            Assert.That(agents![0].Position, Is.EqualTo(new Point(7, 5)));
            Assert.That(agents![agents.Count - 1].Position, Is.EqualTo(new Point(25, 1)));
            Assert.That(agents![13].Position, Is.EqualTo(new Point(26, 24)));
        });
    }

    [Test]

    public void TestBoardLoading()
    {
        Assert.Multiple(() =>
        {
            Assert.That(board is not null);
            Assert.That(board!.Height, Is.EqualTo(34));
            Assert.That(board!.Width, Is.EqualTo(34));
            Assert.That(board.GetValue(0, 0), Is.EqualTo(1));
            Assert.That(board.GetValue(1, 1), Is.EqualTo(0));
            Assert.That(board.GetValue(board.Height - 1, board.Width - 1), Is.EqualTo(1));
            Assert.That(board.GetValue(3, 3), Is.EqualTo(0));
            Assert.That(board.GetValue(14, 13), Is.EqualTo(1));
        });


    }

    [Test]
    public void LogReadTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.AllValid is true);
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

            log = await logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/logs/warehouse_100_log.json");
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
            AllValid = true,
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

    public void TestLogStructure()
    {
        Assert.Multiple(() =>
        {
            Assert.That(log is LogFile);
            Assert.That(log.ActionModel is string);
            Assert.That(log.Start is List<(Point, Direction)>);
            Assert.That(log.ActualPaths is List<List<Action>>);
            Assert.That(log.PlannerPaths is List<List<Action>>);
            Assert.That(log.PlannerTimes is List<double>);
            Assert.That(log.Errors is List<(int, int, int, string)>);
            Assert.That(log.Events is List<List<(int, int, string)>>);
            Assert.That(log.Tasks is List<(int, int, int)>);
        });
    }

    [Test]
    public void TestActionModel()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.ActionModel is not null);
            Assert.That(log.ActionModel, Is.EqualTo("MAPF_T"));
            Assert.That(log.ActionModel!.Length, Is.EqualTo(6));
            log.ActionModel = "";
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.ActionModel is not null);
            Assert.That(log.ActionModel, Is.EqualTo(""));
            log.ActionModel = "Hajrá Fradi!";
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.ActionModel is not null);
            Assert.That(log.ActionModel, Is.EqualTo("Hajrá Fradi!"));
            log.ActionModel = "Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!";
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.ActionModel is not null);
            Assert.That(log.ActionModel, Is.EqualTo("Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!"));
        });
    }

    [Test]
    public void TestAllValid()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.AllValid);
            log.AllValid = false;
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(!log.AllValid);
        });
    }

    [Test]
    public void TestTeamSize()
    {
        Assert.Multiple(async () =>
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
        });

    }

    [Test]

    public void TestStart()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.Start is not null);
            Assert.That(log.Start![0], Is.EqualTo((new Point(7, 5), Direction.East)));
            Assert.That(log.Start![2], Is.EqualTo((new Point(27, 19), Direction.East)));
            Assert.That(log.Start![^1], Is.EqualTo((new Point(25, 1), Direction.East)));
            Assert.That(log.Start![13], Is.EqualTo((new Point(26, 24), Direction.East)));
            log.Start.Clear();
            log.Start.AddRange(new List<(Point, Direction)>() { (new Point(3, 9), Direction.North) });
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.Start is not null);
            Assert.That(log.Start![0], Is.EqualTo((new Point(3, 9), Direction.North)));
            Assert.That(log.Start.Count, Is.EqualTo(1));
            log.Start.Clear();
            log.Start.AddRange(new List<(Point, Direction)>() { (new Point(13, 10), Direction.South), (new Point(69, 33), Direction.West) });
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.Start is not null);
            Assert.That(log.Start![0], Is.EqualTo((new Point(13, 10), Direction.South)));
            Assert.That(log.Start![1], Is.EqualTo((new Point(69, 33), Direction.West)));
            Assert.That(log.Start.Count, Is.EqualTo(2));
            log.Start.Clear();
            log.Start.AddRange(new List<(Point, Direction)>() { (new Point(13, 10), Direction.South), (new Point(69, 33), Direction.West), (new Point(113, 133), Direction.East) });
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.Start is not null);
            Assert.That(log.Start![0], Is.EqualTo((new Point(13, 10), Direction.South)));
            Assert.That(log.Start![1], Is.EqualTo((new Point(69, 33), Direction.West)));
            Assert.That(log.Start![2], Is.EqualTo((new Point(113, 133), Direction.East)));
            Assert.That(log.Start.Count, Is.EqualTo(3));
            List<(Point, Direction)> list = new();
            Random random = new Random();
            for (int i = 0; i < 6969; i++)
            {
                list.Add((new Point(random.Next(), random.Next()), (Direction)random.Next(4)));
            }
            log.Start.Clear();
            log.Start.AddRange(list);
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.Start is not null);
            Assert.That(log.Start![0], Is.EqualTo(list[0]));
            var num = random.Next(list.Count);
            Assert.That(log.Start![num], Is.EqualTo(list[num]));
            Assert.That(log.Start![13], Is.EqualTo(list[13]));
            Assert.That(log.Start![^1], Is.EqualTo(list[^1]));
            Assert.That(log.Start, Is.EqualTo(list));
            Assert.That(log.Start.Count, Is.EqualTo(6969));
        });

    }

    [Test]

    public void TestNumTaskFinished()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.NumTaskFinished, Is.EqualTo(338));
            log.NumTaskFinished = 333333;
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.NumTaskFinished, Is.EqualTo(333333));
        });
    }

    [Test]

    public void TestSumOfCost()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.SumOfCost, Is.EqualTo(10000));
            log.SumOfCost = 333333333;
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.SumOfCost, Is.EqualTo(333333333));
        });
    }

    [Test]
    public void TesMakespan()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.Makespan, Is.EqualTo(500));
            log.Makespan = 33333333;
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.Makespan, Is.EqualTo(33333333));
        });
    }

    [Test]

    public void TestActualPaths()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.ActualPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(log.ActualPaths.All(x => { return x.All(y => { return y != Action.T; }); }), Is.True);
            Assert.That(log.ActualPaths[0], Is.InstanceOf<List<Action>>());
            Assert.That(log.ActualPaths[^1][^1], Is.EqualTo(Action.F));
            Assert.That(log.ActualPaths.All(x => { return x.All(y => { return y != Action.B; }); }), Is.True);
            List<List<Action>> list = [.. log.ActualPaths];
            list[0][0] = Action.C;
            list[^1][^1] = Action.R;
            log.ActualPaths.Clear();
            log.ActualPaths.AddRange(list);
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.ActualPaths[0][0], Is.EqualTo(Action.C));
            Assert.That(log.ActualPaths.All(x => { return x.All(y => { return y != Action.T; }); }), Is.True);
            Assert.That(log.ActualPaths[0], Is.InstanceOf<List<Action>>());
            Assert.That(log.ActualPaths[^1][^1], Is.EqualTo(Action.R));
            Assert.That(log.ActualPaths.All(x => { return x.All(y => { return y != Action.B; }); }), Is.True);
        });

    }

    [Test]

    public void TestPlannerPaths()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.ActualPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(log.ActualPaths[0], Is.InstanceOf<List<Action>>());
            Assert.That(log.ActualPaths[^1][^1], Is.EqualTo(Action.F));
            Assert.That(log.ActualPaths.All(x => { return x.All(y => { return y != Action.B; }); }), Is.True);
            List<List<Action>> list = [.. log.ActualPaths];
            list[0][0] = Action.C;
            list[^1][^1] = Action.R;
            log.ActualPaths.Clear();
            log.ActualPaths.AddRange(list);
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.ActualPaths[0][0], Is.EqualTo(Action.C));
            Assert.That(log.ActualPaths[0], Is.InstanceOf<List<Action>>());
            Assert.That(log.ActualPaths[^1][^1], Is.EqualTo(Action.R));
            Assert.That(log.ActualPaths.All(x => { return x.All(y => { return y != Action.B; }); }), Is.True);
        });

    }

    [Test]

    public void TestPlannerTimes()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(log.PlannerTimes[0], Is.EqualTo(0.312150264));
            Assert.That(log.PlannerTimes[^1], Is.EqualTo(0.0043267));
            Assert.That(log.PlannerTimes[13], Is.EqualTo(0.010686006));
            log.PlannerTimes[0] = 0.13;
            log.PlannerTimes[^1] = 0;
            log.PlannerTimes[13] = 0.69;
            await logFileDataAccess.SaveAsync("./test_log.json", log);
            log = await logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(log.PlannerTimes[0], Is.EqualTo(0.13));
            Assert.That(log.PlannerTimes[^1], Is.EqualTo(0));
            Assert.That(log.PlannerTimes[13], Is.EqualTo(0.69));
        });
    }

    [Test]

    public async Task TestErrors()
    {
        Assert.That(log.Errors, Is.Empty);
        var list = new List<(int, int, int, string)> { (3, 13, 33, "Mekk ÚR"), (3, 13, 21, "Adorján"), (0, 0, 0, "Endre"), (3, 23, 53, "Milán"), (3, 69, 69, "Randi") };
        log.Errors.Clear();
        log.Errors.AddRange(list);
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.That(log.Errors[0], Is.EqualTo((3, 13, 33, "Mekk ÚR")));
        Assert.That(log.Errors[^1], Is.EqualTo((3, 69, 69, "Randi")));
        Assert.That(log.Errors[3], Is.EqualTo((3, 23, 53, "Milán")));
    }
    [Test]
    public async Task TestEvents()
    {
        Assert.That(log.Events[0][0], Is.EqualTo((0, 0, "assigned")));
        Assert.That(log.Events[0][^1], Is.EqualTo((347, 484, "assigned")));
        Assert.That(log.Events[0][3], Is.EqualTo((32, 60, "finished")));
        Assert.That(log.Events[^1][0], Is.EqualTo((19, 0, "assigned")));
        Assert.That(log.Events[^1][3], Is.EqualTo((43, 85, "finished")));
        Assert.That(log.Events[^1][^1], Is.EqualTo((345, 482, "assigned")));
        Assert.That(log.Events[3][0], Is.EqualTo((3, 0, "assigned")));
        Assert.That(log.Events[3][^1], Is.EqualTo((343, 481, "assigned")));
        Assert.That(log.Events[3][3], Is.EqualTo((42, 62, "finished")));
        log.Events[0][0] = (13, 13, "Sanyika");
        log.Events[0][^1] = (13, 13, "Sanyika");
        log.Events[0][3] = (13, 13, "Sanyika");
        log.Events[^1][0] = (3, 69, "Randi");
        log.Events[^1][^1] = (3, 69, "Randi");
        log.Events[^1][3] = (3, 69, "Randi");
        log.Events[3][0] = (33, 33, "MEKK ÚR");
        log.Events[3][^1] = (33, 33, "MEKK ÚR");
        log.Events[3][3] = (33, 33, "MEKK ÚR");
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.That(log.Events[0][0], Is.EqualTo((13, 13, "Sanyika")));
        Assert.That(log.Events[0][^1], Is.EqualTo((13, 13, "Sanyika")));
        Assert.That(log.Events[0][3], Is.EqualTo((13, 13, "Sanyika")));
        Assert.That(log.Events[^1][0], Is.EqualTo((3, 69, "Randi")));
        Assert.That(log.Events[^1][3], Is.EqualTo((3, 69, "Randi")));
        Assert.That(log.Events[^1][^1], Is.EqualTo((3, 69, "Randi")));
        Assert.That(log.Events[3][0], Is.EqualTo((33, 33, "MEKK ÚR")));
        Assert.That(log.Events[3][^1], Is.EqualTo((33, 33, "MEKK ÚR")));
        Assert.That(log.Events[3][3], Is.EqualTo((33, 33, "MEKK ÚR")));
    }

    [Test]
    public async Task TestTasks()
    {
        Assert.That(log.Tasks[0], Is.EqualTo((0, 21, 6)));
        Assert.That(log.Tasks[13], Is.EqualTo((13, 26, 22)));
        Assert.That(log.Tasks[^1], Is.EqualTo((357, 6, 21)));
        log.Tasks[0] = (3, 13, 33);
        log.Tasks[13] = (3, 69, 69);
        log.Tasks[^1] = (3, 69, 90);
        await logFileDataAccess.SaveAsync("./test_log.json", log);
        log = await logFileDataAccess.LoadAsync("./test_log.json");
        Assert.That(log.Tasks[0], Is.EqualTo((3, 13, 33)));
        Assert.That(log.Tasks[13], Is.EqualTo((3, 69, 69)));
        Assert.That(log.Tasks[^1], Is.EqualTo((3, 69, 90)));


    }

    [Test]

    public void TestConfig()
    {
        Assert.That(config is not null);
        Assert.That(config.MapFile is not null);
        Assert.That(config.MapFile is string);
        Assert.That(config.MapFile, Is.EqualTo("maps/random-32-32-20.map"));
        Assert.That(config.AgentFile is not null);
        Assert.That(config.AgentFile is string);
        Assert.That(config.AgentFile, Is.EqualTo("agents/random_20.agents"));
        Assert.That(config.TeamSize is int);
        Assert.That(config.TeamSize, Is.EqualTo(20));
        Assert.That(config.TaskFile is not null);
        Assert.That(config.TaskFile is string);
        Assert.That(config.TaskFile, Is.EqualTo("tasks/random-32-32-20.tasks"));
        Assert.That(config.NumTasksReveal is int);
        Assert.That(config.NumTasksReveal, Is.EqualTo(1));
        Assert.That(config.TaskAssignmentStrategy is Strategy);
        Assert.That(config.TaskAssignmentStrategy, Is.EqualTo(Strategy.RoundRobin));

    }


    [Test]

    public void TestException()
    {
        Assert.ThrowsAsync<JsonException>(async () =>
        {
            log = await logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/configs/random_20_config.json");
        });
        Assert.ThrowsAsync<JsonException>(async () =>
        {
            config = await configDataAccess.Load("../../../../MekkdonaldsWPF/logs/random_20_log.json");
        });
    }



}
