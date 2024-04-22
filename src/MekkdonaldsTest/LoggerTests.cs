using Mekkdonalds.Persistence;
using Mekkdonalds.Simulation;

using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Test;

internal class LoggerTests
{
    private const string MAPNAME = "test";
    private Logger _logger;
    private readonly Action[] _actions = [Action.F, Action.C, Action.W, Action.R];

    private Action RandomAction => _actions[Random.Shared.Next(_actions.Length)];

    [SetUp]
    public void Setup()
    {
        _logger = new Logger(MAPNAME);
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
        Assert.That(
            _logger.GetLogFile().Start,
            Is.EqualTo(rs.Select(robot => (robot.Position, robot.Direction)))
        );
    }

    [Test]
    public void SetActionModelTest()
    {
        _logger.SetActionModel("test");
        Assert.That(_logger.GetLogFile().ActionModel, Is.EqualTo("test"));
    }

    [Test]
    public void SetTeamSizeTest()
    {
        var size = Random.Shared.Next(1, 2000);
        _logger.SetTeamSize(size);
        Assert.That(_logger.GetLogFile().TeamSize, Is.EqualTo(size));
    }

    [Test]
    public void LogActualPathsTest()
    {
        var rs = new List<Robot>();

        rs.AddRange(Enumerable.Range(0, 10).Select(_ => new Robot(
            new(Random.Shared.Next(30), Random.Shared.Next(30)),
            (Direction)Random.Shared.Next(4)
        )));

        rs.ForEach(robot =>
            Enumerable.Range(0, Random.Shared.Next(5, 20)).ToList().ForEach(_ =>
                robot.Step(RandomAction)
            )
        );

        _logger.LogActualPaths(rs);

        Assert.That(
            _logger.GetLogFile().ActualPaths,
            Is.EquivalentTo(rs.Select(robot => robot.History))
        );
    }

    [Test]
    public void LogPlannerPathsTest()
    {
        var count = Random.Shared.Next(1, 2000);
        var rs = new List<Robot>();
        rs.AddRange(Enumerable.Repeat(new Robot(new(0,0), Direction.East), count));
        _logger.LogStarts(rs); // this will create the necessary lists

        var paths = rs.Select(_ => new Simulation.Path([.. Enumerable.Range(0, Random.Shared.Next(20)).Select(x => RandomAction)], default)).ToList();

        for (var i = 0; i < count; i++)
        {
            _logger.LogPlannerPaths(i+1, paths[i]);
        }

        Assert.That(
            _logger.GetLogFile().PlannerPaths,
            Is.EqualTo(paths.Select(p => p.PlannedPath))
        );
    }

    [Test]
    public void LogTimeTest()
    {
        var times = Enumerable.Range(0, Random.Shared.Next(1, 2000)).Select(_ => Random.Shared.NextDouble() / 2).ToList();

        times.ForEach(_logger.LogTime);

        Assert.That(
            _logger.GetLogFile().PlannerTimes,
            Is.EqualTo(times)
        );
    }

    [Test]
    public void LogErrorTest()
    {
        Assert.Multiple(() =>
        {
            
            var count = Random.Shared.Next(1, 2000);
            List<(int R1, int R2, int T, string msg)> errors = Enumerable.Range(0, count).Select(_ => (Random.Shared.Next(1, 2000), Random.Shared.Next(1, 2000), Random.Shared.Next(1, 2000), RandomAction.ToString())).ToList();

            errors.ForEach(e => _logger.LogError(e.R1, e.R2, e.T, e.msg));

            errors = errors.Select(e => (e.R1 - 1, e.R2 - 1, e.T, e.msg)).ToList();

            Assert.That(
                _logger.GetLogFile().Errors,
                Is.EqualTo(errors)
            );

            count = Random.Shared.Next(1, 2000);

            List<(int T, string msg)> errors2 = Enumerable.Range(0, count).Select(_ => (Random.Shared.Next(1, 2000), RandomAction.ToString())).ToList();

            errors2.ForEach(e => _logger.LogError(e.T, e.msg));

            Assert.That(
                _logger.GetLogFile().Errors,
                Is.EqualTo(Enumerable.Concat(errors, errors2.Select(e => (-1, -1, e.T, e.msg))))
            );
        });
    }

    [Test]
    public void LogAssignmentTest()
    {
        var count = Random.Shared.Next(1, 2000);

        var rs = new List<Robot>();
        rs.AddRange(Enumerable.Repeat(new Robot(new(0, 0), Direction.East), count));
        _logger.LogStarts(rs); // this will create the necessary lists

        List<(int R, int T, int W)> assignments = Enumerable.Range(0, count).Select(_ => (Random.Shared.Next(1, count), Random.Shared.Next(1, 2000), Random.Shared.Next(1, 2000))).ToList();

        assignments.ForEach(a => _logger.LogAssignment(a.R, a.T, a.W));

        Assert.That(
            _logger.GetLogFile().Events,
            Is.EqualTo(
                Enumerable.Range(1, count).Select(i =>
                    assignments
                    .Where(t => t.R == i)
                    .Select(x => (x.T - 1, x.W, "assigned")
            ))
        ));
    }

    [Test]
    public void LogFinishTest()
    {
        var count = Random.Shared.Next(1, 2000);

        var rs = new List<Robot>();
        rs.AddRange(Enumerable.Repeat(new Robot(new(0, 0), Direction.East), count));
        _logger.LogStarts(rs); // this will create the necessary lists

        List<(int R, int T, int W)> finishes = Enumerable.Range(1, count).Select(_ => (Random.Shared.Next(1, count), Random.Shared.Next(1, 2000), Random.Shared.Next(1, 2000))).ToList();

        finishes.ForEach(f => _logger.LogFinish(f.R, f.T, f.W));

        Assert.That(
            _logger.GetLogFile().Events,
            Is.EqualTo(
                Enumerable.Range(1, count).Select(i =>
                    finishes
                    .Where(t => t.R == i)
                    .Select(x => (x.T - 1, x.W, "finished")
            ))
        ));
    }

    [Test]
    public void LogTasksTest()
    {
        var count = Random.Shared.Next(1, 2000);

        var tasks = Enumerable.Range(1, count).Select(_ => new Package(new(Random.Shared.Next(50), Random.Shared.Next(50)))).ToList(); // needs to be a list (otherwise Random.Shared.Next will be called again)

        _logger.LogTasks(tasks);

        Assert.That(
            _logger.GetLogFile().Tasks,
            Is.EquivalentTo(tasks.Select((t, i) => (i, t.Position.Y - 1, t.Position.X - 1))
        ));
    }

    [Test]
    public void LogReplayLengthTest()
    {
        var length = Random.Shared.Next(1, 2000);
        _logger.LogReplayLength(length);
        Assert.That(_logger.GetLogFile().Makespan, Is.EqualTo(length));
    }

    [Test]
    public void LogAllValidTest()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_logger.GetLogFile().AllValid, Is.True);
            _logger.LogError(1, "test");
            Assert.That(_logger.GetLogFile().AllValid, Is.False);
        });
    }

    [Test]
    public void SaveAsyncTest()
    {
        Assert.That(Directory.Exists("./logs"));
        Directory.EnumerateFiles("./logs").ToList().ForEach(File.Delete);
        Assert.MultipleAsync(async () =>
        {
            Assert.That(Directory.GetFiles("./logs"), Is.Empty);
            await _logger.SaveAsync(new LogFileDataAccess());
            Assert.That(Directory.GetFiles("./logs"), Has.Length.EqualTo(1));
            var fileName = Directory.GetFiles("./logs")[0];
            Assert.That(File.Exists(fileName));
            Assert.That(System.IO.Path.GetExtension(fileName), Is.EqualTo("json"));
            Assert.That(fileName, Does.StartWith(MAPNAME));
        });
    }
}
