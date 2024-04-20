using Mekkdonalds.Persistence;
using Mekkdonalds.Simulation;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Drawing;

namespace MekkdonaldsTest.Persistence;

public class BoardTests
{
    private Board board;

    [SetUp]
    public void Setup()
    {
        board = new Board(10, 10); // Create a new 10x10 board for testing
    }

    [Test]
    public void TestBoardInitialization()
    {
        // Assert that the board dimensions are correct
        Assert.That(board.Height, Is.EqualTo(12));
        Assert.That(board.Width, Is.EqualTo(12));

        // Assert that all cells are initially empty
        for (int y = 1; y < 11; y++)
        {
            for (int x = 1; x < 11; x++)
            {
                Assert.That(board.GetValue(x, y), Is.EqualTo(Board.EMPTY));
            }
        }
    }

    [Test]
    public void TestSetValue()
    {
        // Set a value on the board
        board.SetValue(5, 5, Board.WALL);

        // Assert that the value was set correctly
        Assert.That(board.GetValue(5, 5), Is.EqualTo(Board.WALL));

        // Assert that setting an invalid value throws an exception
        Assert.Throws<ArgumentOutOfRangeException>(() => board.SetValue(5, 5, 2));
    }

    [Test]
    public void TestMoveRobot()
    {
        // Place a robot on the board
        Point initialPosition = new Point(5, 5);
        Point nextPosition = new Point(6, 5);
        board.SetValue(initialPosition.X, initialPosition.Y, Board.OCCUPIED);

        // Move the robot
        bool moveSuccessful = board.TryMoveRobot(initialPosition, nextPosition);

        // Assert that the move was successful
        ClassicAssert.IsTrue(moveSuccessful);
        Assert.That(board.GetValue(nextPosition.X, nextPosition.Y), Is.EqualTo(Board.OCCUPIED));
        Assert.That(board.GetValue(initialPosition.X, initialPosition.Y), Is.EqualTo(Board.EMPTY));
    }

    [Test]
    public void TestSetSearchedIfEmpty()
    {
        // Set an empty cell as searched
        Point emptyPosition = new Point(5, 5);
        bool setSearchedSuccessful = board.SetSearchedIfEmpty(emptyPosition);

        // Assert that the cell was marked as searched
        ClassicAssert.IsTrue(setSearchedSuccessful);

        // Check if the same cell cannot be marked as searched again
        bool isSearched = board.SetSearchedIfEmpty(emptyPosition);
        ClassicAssert.IsFalse(isSearched);
    }

    [Test]
    public void TestClearMask()
    {
        // Mark some cells as searched
        board.SetSearchedIfEmpty(new Point(5, 5));
        board.SetSearchedIfEmpty(new Point(6, 6));

        // Clear the search mask
        board.ClearMask();

        // Assert that all cells can be marked as searched again
        for (int y = 1; y < 11; y++)
        {
            for (int x = 1; x < 11; x++)
            {
                ClassicAssert.IsTrue(board.SetSearchedIfEmpty(new Point(x, y)));
            }
        }
    }

    [Test]
    public void TestSetValueOutOfBounds()
    {
        // Assert that setting a value outside the board bounds throws an exception
        Assert.Throws<ArgumentOutOfRangeException>(() => board.SetValue(-1, 5, Board.WALL));
        Assert.Throws<ArgumentOutOfRangeException>(() => board.SetValue(5, -1, Board.WALL));
        Assert.Throws<ArgumentOutOfRangeException>(() => board.SetValue(board.Width, 5, Board.WALL));
        Assert.Throws<ArgumentOutOfRangeException>(() => board.SetValue(5, board.Height, Board.WALL));
    }

    [Test]
    public void TestGetValueOutOfBounds()
    {
        // Assert that getting a value outside the board bounds throws an exception
        Assert.Throws<ArgumentOutOfRangeException>(() => board.GetValue(-1, 5));
        Assert.Throws<ArgumentOutOfRangeException>(() => board.GetValue(5, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => board.GetValue(board.Width, 5));
        Assert.Throws<ArgumentOutOfRangeException>(() => board.GetValue(5, board.Height));
    }
}