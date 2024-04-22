using Mekkdonalds.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using Mekkdonalds.Exception;

namespace Mekkdonalds.Simulation.Tests
{
    [TestFixture]
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

            // Assert
            Assert.That(position, Is.EqualTo(robot.Position));
            Assert.That(direction, Is.EqualTo(robot.Direction));
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

            // Assert
            Assert.That(robot.Task, Is.Null);
            Assert.That(new Point(1, 1), Is.EqualTo(task.Position));
        }

        [Test]
        public void AddTask_AddsTask()
        {
            // Arrange
            Robot robot = new Robot(new Point(0, 0), Direction.North);
            Point destination = new Point(1, 1);

            // Act
            robot.AddTask(destination);

            // Assert
            Assert.That(robot.Task, Is.Not.Null);
            Assert.That(destination, Is.EqualTo(robot.Task.Position));
        }

        [Test]
        public void TryStep_MovesRobotForward()
        {
            // Arrange
            Robot robot = new Robot(new Point(0, 0), Direction.East);
            Board board = new Board(10, 10);

            // Act
            bool success = robot.TryStep(Action.F, board, 0);

            // Assert
            Assert.That(success, Is.True);
            Assert.That(new Point(1, 0), Is.EqualTo(robot.Position));
            Assert.That(Direction.East, Is.EqualTo(robot.Direction));
            Assert.That(new List<Action> { Action.F }, Is.EqualTo(robot.History));
        }

        [Test]
        public void TryStep_RotatesRobotClockwise()
        {
            // Arrange
            Robot robot = new Robot(new Point(0, 0), Direction.North);
            Board board = new Board(10, 10);

            // Act
            bool success = robot.TryStep(Action.R, board, 0);

            // Assert
            Assert.That(success, Is.True);
            Assert.That(new Point(0, 0), Is.EqualTo(robot.Position));
            Assert.That(Direction.East, Is.EqualTo(robot.Direction));
            Assert.That(new List<Action> { Action.R }, Is.EqualTo(robot.History));
        }

        // Add more tests for other actions and scenarios
    }
}
