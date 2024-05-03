using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Test;

[TestOf(typeof(Robot))]
public class RobotTests
{
    [Test]
    public void Constructor_SetsPositionAndDirection()
    {
        // Arrange
        Point position = new(1, 2);
        Direction direction = Direction.East;

        // Act
        Robot robot = new(position, direction);

        Assert.Multiple(() =>
        {
            // Assert that position and direction match the robot's current position and direction
            Assert.That(robot.Position, Is.EqualTo(position), "Position should match robot's position");
            Assert.That(robot.Direction, Is.EqualTo(direction), "Direction should match robot's direction");
        });
    }

    [Test]
    public void RemoveTask_ThrowsExceptionWhenNoTask()
    {
        // Arrange
        Robot robot = new(new Point(0, 0), Direction.North);

        // Act & Assert
        Assert.That(() => robot.RemoveTask(), Throws.Exception.TypeOf<System.Exception>());
    }

    [Test]
    public void RemoveTask_RemovesTask()
    {
        // Arrange
        Robot robot = new(new Point(0, 0), Direction.North);
        robot.AddTask(new Point(1, 1));

        // Act
        Package task = robot.RemoveTask();

        Assert.Multiple(() =>
        {
            // Assert that robot has no task and task position is (1,1)
            Assert.That(robot.Task, Is.Null, "Robot should have no task");
            Assert.That(task.Position, Is.EqualTo(new Point(1, 1)), "Task position should be (1,1)");
        });
    }

    [Test]
    public void AddTask_AddsTask()
    {
        // Arrange
        Robot robot = new(new Point(0, 0), Direction.North);
        Point destination = new(1, 1);

        // Act
        robot.AddTask(destination);

        // Assert that robot has a task and destination matches task position
        Assert.That(robot.Task, Is.Not.Null, "Robot should have a task");
        Assert.That(robot.Task.Position, Is.EqualTo(destination), "Destination should match task position");
        
    }

    [Test]
    public void TryStep_MovesRobotForward()
    {
        // Arrange
        Robot robot = new(new Point(0, 0), Direction.East);
        Board board = new(10, 10);

        // Act
        bool success = robot.TryStep(Action.F, board, 0);

        Assert.Multiple(() =>
        {
            // Assert that success is true, and robot's position, direction, and history are as expected
            Assert.That(success, Is.True, "Success should be true");
            Assert.That(robot.Position, Is.EqualTo(new Point(1, 0)), "Position should be (1,0)");
            Assert.That(robot.Direction, Is.EqualTo(Direction.East), "Direction should be East");
            Assert.That(robot.History, Is.EqualTo(new List<Action> { Action.F }), "History should contain only Action.F");
        });
    }

    [Test]
    public void TryStep_RotatesRobotClockwise()
    {
        // Arrange
        Robot robot = new(new Point(0, 0), Direction.North);
        Board board = new(10, 10);

        // Act
        bool success = robot.TryStep(Action.R, board, 0);

        Assert.Multiple(() =>
        {
            // Assert that success is true, and robot's position, direction, and history are as expected
            Assert.That(success, Is.True, "Success should be true");
            Assert.That(robot.Position, Is.EqualTo(new Point(0, 0)), "Position should be (0,0)");
            Assert.That(robot.Direction, Is.EqualTo(Direction.East), "Direction should be East");
            Assert.That(robot.History, Is.EqualTo(new List<Action> { Action.R }), "History should contain only Action.R");
        });
    }
}
