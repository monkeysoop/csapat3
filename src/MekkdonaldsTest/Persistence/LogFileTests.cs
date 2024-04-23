using System.Text.Json;

using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Test.Persistence;

[TestOf(typeof(LogFile))]
[TestOf(typeof(LogFileDataAccess))]
internal class LogFileTests
{
    private LogFile _log;
    private LogFileDataAccess _logFileDataAccess;
    private bool _loadSuccessful;

    [SetUp]
    public async Task Setup()
    {
        _logFileDataAccess = new LogFileDataAccess();
        _log = await _logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/logs/random_20_log.json");
    }

    [Test]
    public void ReadTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.AllValid, Is.EqualTo(true));
            Assert.That(_log.ActionModel, Is.EqualTo("MAPF_T"));
            Assert.That(_log.TeamSize, Is.EqualTo(20));
            Assert.That(_log.Start[0], Is.EqualTo((new Point(7, 5), Direction.East)));
            Assert.That(_log.NumTaskFinished, Is.EqualTo(338));
            Assert.That(_log.SumOfCost, Is.EqualTo(10000));
            Assert.That(_log.Makespan, Is.EqualTo(500));
            Assert.That(_log.ActualPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(_log.PlannerPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(_log.PlannerTimes[0], Is.EqualTo(0.312150264));
            Assert.That(_log.Errors, Is.Empty);
            Assert.That(_log.Events, Is.TypeOf<List<List<(int, int, string)>>>());
            Assert.That(_log.Events[0][0], Is.EqualTo((0, 0, "assigned")));
            Assert.That(_log.Tasks[0], Is.EqualTo((0, 21, 6)));

            _log = await _logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/logs/warehouse_100_log.json");
            Assert.That(_log.Errors[0], Is.EqualTo((-1, -1, 1, "incorrect vector size")));
        });
        _loadSuccessful = true;
    }

    [Test]
    public async Task WriteTest()
    {
        if (!_loadSuccessful)
        {
            Assert.Inconclusive("Load test failed, can't test _log file saving");
        }

        var _logFileDataAccess = new LogFileDataAccess();
        var _log = new LogFile()
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

        await _logFileDataAccess.SaveAsync("./test_log.json", _log);

        _log = await _logFileDataAccess.LoadAsync("./test_log.json");

        Assert.Multiple(() =>
        {
            Assert.That(_log.ActionModel, Is.EqualTo("MAPF_T"));
            Assert.That(_log.TeamSize, Is.EqualTo(20));
            Assert.That(_log.Start[0], Is.EqualTo((new Point(4, 6), Direction.East)));
            Assert.That(_log.NumTaskFinished, Is.EqualTo(338));
            Assert.That(_log.SumOfCost, Is.EqualTo(10000));
            Assert.That(_log.Makespan, Is.EqualTo(500));
            Assert.That(_log.ActualPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(_log.PlannerPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(_log.PlannerTimes[0], Is.EqualTo(0.312150264));
            Assert.That(_log.Errors, Is.Empty);
            Assert.That(_log.Events, Is.TypeOf<List<List<(int, int, string)>>>());
            Assert.That(_log.Events[0][0], Is.EqualTo((0, 0, "assigned")));
            Assert.That(_log.Tasks[0], Is.EqualTo((0, 21, 6)));
        });
    }

    [Test]
    public void ActionModelTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.ActionModel is not null);
            Assert.That(_log.ActionModel, Is.EqualTo("MAPF_T"));
            Assert.That(_log.ActionModel!.Length, Is.EqualTo(6));
            _log.ActionModel = "";
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.ActionModel is not null);
            Assert.That(_log.ActionModel, Is.EqualTo(""));
            _log.ActionModel = "Hajrá Fradi!";
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.ActionModel is not null);
            Assert.That(_log.ActionModel, Is.EqualTo("Hajrá Fradi!"));
            _log.ActionModel = "Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!";
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.ActionModel is not null);
            Assert.That(_log.ActionModel, Is.EqualTo("Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!Hajrá Fradi!"));
        });
    }

    [Test]
    public void AllValidTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.AllValid);
            _log.AllValid = false;
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(!_log.AllValid);
        });
    }

    [Test]
    public void TeamSizeTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.TeamSize, Is.EqualTo(20));
            _log.TeamSize = 69;
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.TeamSize, Is.EqualTo(69));
            _log.TeamSize = 0;
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.TeamSize, Is.EqualTo(0));
            _log.TeamSize = 3333333;
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.TeamSize, Is.EqualTo(3333333));
        });

    }

    [Test]
    public void StartTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.Start is not null);
            Assert.That(_log.Start![0], Is.EqualTo((new Point(7, 5), Direction.East)));
            Assert.That(_log.Start![2], Is.EqualTo((new Point(27, 19), Direction.East)));
            Assert.That(_log.Start![^1], Is.EqualTo((new Point(25, 1), Direction.East)));
            Assert.That(_log.Start![13], Is.EqualTo((new Point(26, 24), Direction.East)));
            _log.Start.Clear();
            _log.Start.AddRange(new List<(Point, Direction)>() { (new Point(3, 9), Direction.North) });
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.Start is not null);
            Assert.That(_log.Start![0], Is.EqualTo((new Point(3, 9), Direction.North)));
            Assert.That(_log.Start.Count, Is.EqualTo(1));
            _log.Start.Clear();
            _log.Start.AddRange(new List<(Point, Direction)>() { (new Point(13, 10), Direction.South), (new Point(69, 33), Direction.West) });
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.Start is not null);
            Assert.That(_log.Start![0], Is.EqualTo((new Point(13, 10), Direction.South)));
            Assert.That(_log.Start![1], Is.EqualTo((new Point(69, 33), Direction.West)));
            Assert.That(_log.Start.Count, Is.EqualTo(2));
            _log.Start.Clear();
            _log.Start.AddRange(new List<(Point, Direction)>() { (new Point(13, 10), Direction.South), (new Point(69, 33), Direction.West), (new Point(113, 133), Direction.East) });
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.Start is not null);
            Assert.That(_log.Start![0], Is.EqualTo((new Point(13, 10), Direction.South)));
            Assert.That(_log.Start![1], Is.EqualTo((new Point(69, 33), Direction.West)));
            Assert.That(_log.Start![2], Is.EqualTo((new Point(113, 133), Direction.East)));
            Assert.That(_log.Start.Count, Is.EqualTo(3));
            List<(Point, Direction)> list = new();
            Random random = new Random();
            for (int i = 0; i < 6969; i++)
            {
                list.Add((new Point(random.Next(), random.Next()), (Direction)random.Next(4)));
            }
            _log.Start.Clear();
            _log.Start.AddRange(list);
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.Start is not null);
            Assert.That(_log.Start![0], Is.EqualTo(list[0]));
            var num = random.Next(list.Count);
            Assert.That(_log.Start![num], Is.EqualTo(list[num]));
            Assert.That(_log.Start![13], Is.EqualTo(list[13]));
            Assert.That(_log.Start![^1], Is.EqualTo(list[^1]));
            Assert.That(_log.Start, Is.EqualTo(list));
            Assert.That(_log.Start.Count, Is.EqualTo(6969));
        });

    }

    [Test]
    public void NumTaskFinishedTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.NumTaskFinished, Is.EqualTo(338));
            _log.NumTaskFinished = 333333;
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.NumTaskFinished, Is.EqualTo(333333));
        });
    }

    [Test]
    public void SumOfCostTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.SumOfCost, Is.EqualTo(10000));
            _log.SumOfCost = 333333333;
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.SumOfCost, Is.EqualTo(333333333));
        });
    }

    [Test]
    public void MakespanTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.Makespan, Is.EqualTo(500));
            _log.Makespan = 33333333;
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.Makespan, Is.EqualTo(33333333));
        });
    }

    [Test]
    public void ActualPathsTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.ActualPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(_log.ActualPaths.All(x => { return x.All(y => { return y != Action.T; }); }), Is.True);
            Assert.That(_log.ActualPaths[0], Is.InstanceOf<List<Action>>());
            Assert.That(_log.ActualPaths[^1][^1], Is.EqualTo(Action.F));
            Assert.That(_log.ActualPaths.All(x => { return x.All(y => { return y != Action.B; }); }), Is.True);
            List<List<Action>> list = [.. _log.ActualPaths];
            list[0][0] = Action.C;
            list[^1][^1] = Action.R;
            _log.ActualPaths.Clear();
            _log.ActualPaths.AddRange(list);
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.ActualPaths[0][0], Is.EqualTo(Action.C));
            Assert.That(_log.ActualPaths.All(x => { return x.All(y => { return y != Action.T; }); }), Is.True);
            Assert.That(_log.ActualPaths[0], Is.InstanceOf<List<Action>>());
            Assert.That(_log.ActualPaths[^1][^1], Is.EqualTo(Action.R));
            Assert.That(_log.ActualPaths.All(x => { return x.All(y => { return y != Action.B; }); }), Is.True);
        });

    }

    [Test]
    public void PlannerPathsTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.ActualPaths[0][0], Is.EqualTo(Action.F));
            Assert.That(_log.ActualPaths[0], Is.InstanceOf<List<Action>>());
            Assert.That(_log.ActualPaths[^1][^1], Is.EqualTo(Action.F));
            Assert.That(_log.ActualPaths.All(x => { return x.All(y => { return y != Action.B; }); }), Is.True);
            List<List<Action>> list = [.. _log.ActualPaths];
            list[0][0] = Action.C;
            list[^1][^1] = Action.R;
            _log.ActualPaths.Clear();
            _log.ActualPaths.AddRange(list);
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.ActualPaths[0][0], Is.EqualTo(Action.C));
            Assert.That(_log.ActualPaths[0], Is.InstanceOf<List<Action>>());
            Assert.That(_log.ActualPaths[^1][^1], Is.EqualTo(Action.R));
            Assert.That(_log.ActualPaths.All(x => { return x.All(y => { return y != Action.B; }); }), Is.True);
        });

    }

    [Test]
    public void PlannerTimesTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.PlannerTimes[0], Is.EqualTo(0.312150264));
            Assert.That(_log.PlannerTimes[^1], Is.EqualTo(0.0043267));
            Assert.That(_log.PlannerTimes[13], Is.EqualTo(0.010686006));
            _log.PlannerTimes[0] = 0.13;
            _log.PlannerTimes[^1] = 0;
            _log.PlannerTimes[13] = 0.69;
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.PlannerTimes[0], Is.EqualTo(0.13));
            Assert.That(_log.PlannerTimes[^1], Is.EqualTo(0));
            Assert.That(_log.PlannerTimes[13], Is.EqualTo(0.69));
        });
    }

    [Test]
    public void ErrorsTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.Errors, Is.Empty);
            var list = new List<(int, int, int, string)> { (3, 13, 33, "Mekk ÚR"), (3, 13, 21, "Adorján"), (0, 0, 0, "Endre"), (3, 23, 53, "Milán"), (3, 69, 69, "Randi") };
            _log.Errors.Clear();
            _log.Errors.AddRange(list);
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.Errors[0], Is.EqualTo((3, 13, 33, "Mekk ÚR")));
            Assert.That(_log.Errors[^1], Is.EqualTo((3, 69, 69, "Randi")));
            Assert.That(_log.Errors[3], Is.EqualTo((3, 23, 53, "Milán")));
        });
    }

    [Test]
    public void EventsTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.Events[0][0], Is.EqualTo((0, 0, "assigned")));
            Assert.That(_log.Events[0][^1], Is.EqualTo((347, 484, "assigned")));
            Assert.That(_log.Events[0][3], Is.EqualTo((32, 60, "finished")));
            Assert.That(_log.Events[^1][0], Is.EqualTo((19, 0, "assigned")));
            Assert.That(_log.Events[^1][3], Is.EqualTo((43, 85, "finished")));
            Assert.That(_log.Events[^1][^1], Is.EqualTo((345, 482, "assigned")));
            Assert.That(_log.Events[3][0], Is.EqualTo((3, 0, "assigned")));
            Assert.That(_log.Events[3][^1], Is.EqualTo((343, 481, "assigned")));
            Assert.That(_log.Events[3][3], Is.EqualTo((42, 62, "finished")));
            _log.Events[0][0] = (13, 13, "Sanyika");
            _log.Events[0][^1] = (13, 13, "Sanyika");
            _log.Events[0][3] = (13, 13, "Sanyika");
            _log.Events[^1][0] = (3, 69, "Randi");
            _log.Events[^1][^1] = (3, 69, "Randi");
            _log.Events[^1][3] = (3, 69, "Randi");
            _log.Events[3][0] = (33, 33, "MEKK ÚR");
            _log.Events[3][^1] = (33, 33, "MEKK ÚR");
            _log.Events[3][3] = (33, 33, "MEKK ÚR");
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.Events[0][0], Is.EqualTo((13, 13, "Sanyika")));
            Assert.That(_log.Events[0][^1], Is.EqualTo((13, 13, "Sanyika")));
            Assert.That(_log.Events[0][3], Is.EqualTo((13, 13, "Sanyika")));
            Assert.That(_log.Events[^1][0], Is.EqualTo((3, 69, "Randi")));
            Assert.That(_log.Events[^1][3], Is.EqualTo((3, 69, "Randi")));
            Assert.That(_log.Events[^1][^1], Is.EqualTo((3, 69, "Randi")));
            Assert.That(_log.Events[3][0], Is.EqualTo((33, 33, "MEKK ÚR")));
            Assert.That(_log.Events[3][^1], Is.EqualTo((33, 33, "MEKK ÚR")));
            Assert.That(_log.Events[3][3], Is.EqualTo((33, 33, "MEKK ÚR")));
        });
    }

    [Test]
    public void TasksTest()
    {
        Assert.Multiple(async () =>
        {
            Assert.That(_log.Tasks[0], Is.EqualTo((0, 21, 6)));
            Assert.That(_log.Tasks[13], Is.EqualTo((13, 26, 22)));
            Assert.That(_log.Tasks[^1], Is.EqualTo((357, 6, 21)));
            _log.Tasks[0] = (3, 13, 33);
            _log.Tasks[13] = (3, 69, 69);
            _log.Tasks[^1] = (3, 69, 90);
            await _logFileDataAccess.SaveAsync("./test_log.json", _log);
            _log = await _logFileDataAccess.LoadAsync("./test_log.json");
            Assert.That(_log.Tasks[0], Is.EqualTo((3, 13, 33)));
            Assert.That(_log.Tasks[13], Is.EqualTo((3, 69, 69)));
            Assert.That(_log.Tasks[^1], Is.EqualTo((3, 69, 90)));
        });


    }

    [Test]
    public void ExceptionTest()
    {
        Assert.ThrowsAsync<JsonException>(async () =>
        {
            _log = await _logFileDataAccess.LoadAsync("../../../../MekkdonaldsWPF/configs/random_20_config.json");
        });
    }
}
