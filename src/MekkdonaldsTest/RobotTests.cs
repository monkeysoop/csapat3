using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Test;

[TestOf(typeof(Robot))]
public class RobotTests
{
    [Test]
    public void Constructor_SetsPositionAndDirection()
    {
        // Arrange
        Point position = new Point(1, 2);
        Direction direction = Direction.East;

        // Act
        Robot robot = new Robot(position, direction);

        Assert.Multiple(() =>
        {
            // Assert that position and direction match the robot's current position and direction
            Assert.That(position, Is.EqualTo(robot.Position), "Position should match robot's position");
            Assert.That(direction, Is.EqualTo(robot.Direction), "Direction should match robot's direction");
        });
    }

    [Test]
    public void RemoveTask_ThrowsExceptionWhenNoTask()
    {
        // Arrange
        Robot robot = new Robot(new Point(0, 0), Direction.North);

        // Act & Assert
        Assert.That(() => robot.RemoveTask(), Throws.Exception.TypeOf<System.Exception>());
    }

    [Test]
    public void RemoveTask_RemovesTask()
    {
        // Arrange
        Robot robot = new Robot(new Point(0, 0), Direction.North);
        robot.AddTask(new Point(1, 1));

        // Act
        Package task = robot.RemoveTask();

        Assert.Multiple(() =>
        {
            // Assert that robot has no task and task position is (1,1)
            Assert.That(robot.Task, Is.Null, "Robot should have no task");
            Assert.That(new Point(1, 1), Is.EqualTo(task.Position), "Task position should be (1,1)");
        });
    }

    [Test]
    public void AddTask_AddsTask()
    {
        // Arrange
        Robot robot = new Robot(new Point(0, 0), Direction.North);
        Point destination = new Point(1, 1);

        // Act
        robot.AddTask(destination);

        Assert.Multiple(() =>
        {
            // Assert that robot has a task and destination matches task position
            Assert.That(robot.Task, Is.Not.Null, "Robot should have a task");
            Assert.That(destination, Is.EqualTo(robot.Task.Position), "Destination should match task position");
        });
    }

    [Test]
    public void TryStep_MovesRobotForward()
    {
        // Arrange
        Robot robot = new Robot(new Point(0, 0), Direction.East);
        Board board = new Board(10, 10);

        // Act
        bool success = robot.TryStep(Action.F, board, 0);

        Assert.Multiple(() =>
        {
            // Assert that success is true, and robot's position, direction, and history are as expected
            Assert.That(success, Is.True, "Success should be true");
            Assert.That(new Point(1, 0), Is.EqualTo(robot.Position), "Position should be (1,0)");
            Assert.That(Direction.East, Is.EqualTo(robot.Direction), "Direction should be East");
            Assert.That(new List<Action> { Action.F }, Is.EqualTo(robot.History), "History should contain only Action.F");
        });
    }

    [Test]
    public void TryStep_RotatesRobotClockwise()
    {
        // Arrange
        Robot robot = new Robot(new Point(0, 0), Direction.North);
        Board board = new Board(10, 10);

        // Act
        bool success = robot.TryStep(Action.R, board, 0);

        Assert.Multiple(() =>
        {
            // Assert that success is true, and robot's position, direction, and history are as expected
            Assert.That(success, Is.True, "Success should be true");
            Assert.That(new Point(0, 0), Is.EqualTo(robot.Position), "Position should be (0,0)");
            Assert.That(Direction.East, Is.EqualTo(robot.Direction), "Direction should be East");
            Assert.That(new List<Action> { Action.R }, Is.EqualTo(robot.History), "History should contain only Action.R");
        });
    }
}
