namespace Mekkdonalds.Test;

[TestOf(typeof(Board))]
public class BoardTests
{
    private Board board;

    [SetUp]
    public void Setup()
    {
        board = new Board(10, 10); // Create a new 10x10 board for testing
    }

    [Test]
    public void BoardInitializationTest()
    {
        Assert.Multiple(() =>
        {
            // Assert that the board dimensions are correct
            Assert.That(board.Height, Is.EqualTo(12));
            Assert.That(board.Width, Is.EqualTo(12));
        });

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
    public void SetValueTest()
    {
        // Set a value on the board
        board.SetValue(5, 5, Board.WALL);

        // Assert that the value was set correctly
        Assert.That(board.GetValue(5, 5), Is.EqualTo(Board.WALL));

        // Assert that setting an invalid value throws an exception
        Assert.That(() => board.SetValue(5, 5, 2), Throws.Exception
            .With.Message.EqualTo("invalid value2"));
    }


    [Test]
    public void MoveRobotTest()
    {
        // Place a robot on the board
        Point initialPosition = new(5, 5);
        Point nextPosition = new(6, 5);
        board.SetRobotMaskValue(initialPosition.X, initialPosition.Y, Board.OCCUPIED);

        Assert.Multiple(() =>
        {
            // Assert that the move was successful
            Assert.That(board.TryMoveRobot(initialPosition, nextPosition), Is.True, "The move should be successful");
            Assert.That(board.GetRobotMaskValue(nextPosition.X, nextPosition.Y), Is.EqualTo(Board.OCCUPIED), "Robot should be at the next position");
            Assert.That(board.GetRobotMaskValue(initialPosition.X, initialPosition.Y), Is.EqualTo(Board.EMPTY), "Initial position should be empty after the move");
        });

    }

    [Test]
    public void MoveRobotToOccupiedSpaceTest()
    {
        Point initialPosition = new(5, 5);
        Point nextPosition = new(6, 5);
        Point occupiedPosition = new(6, 5);

        // Place a robot on the board
        board.SetRobotMaskValue(initialPosition.X, initialPosition.Y, Board.OCCUPIED);

        // Set the next position as occupied
        board.SetRobotMaskValue(occupiedPosition.X, occupiedPosition.Y, Board.OCCUPIED);

        Assert.Multiple(() =>
        {
            Assert.That(board.TryMoveRobot(initialPosition, nextPosition), Is.False);
            Assert.That(board.GetRobotMaskValue(initialPosition.X, initialPosition.Y), Is.EqualTo(Board.OCCUPIED), "Robot position should remain occupied at initial position");
            Assert.That(board.GetRobotMaskValue(occupiedPosition.X, occupiedPosition.Y), Is.EqualTo(Board.OCCUPIED), "Occupied position should remain occupied");
        });
    }

    [Test]
    public void SetValueOutOfBoundsTest()
    {
        // Assert that setting a value outside the board bounds throws an exception with the correct message
        Assert.That(() => board.SetValue(-1, 5, Board.WALL), Throws.Exception
            .With.Message.EqualTo("invalid position{X=-1,Y=5}"));
        Assert.That(() => board.SetValue(5, -1, Board.WALL), Throws.Exception
            .With.Message.EqualTo("invalid position{X=5,Y=-1}"));
        Assert.That(() => board.SetValue(board.Width, 5, Board.WALL), Throws.Exception
            .With.Message.EqualTo($"invalid position{{X={board.Width},Y=5}}"));
        Assert.That(() => board.SetValue(5, board.Height, Board.WALL), Throws.Exception
            .With.Message.EqualTo($"invalid position{{X=5,Y={board.Height}}}"));
    }

    [Test]
    public void GetValueOutOfBoundsTest()
    {
        // Assert that getting a value outside the board bounds throws an exception with the correct message
        Assert.That(() => board.GetValue(-1, 5), Throws.Exception
            .With.Message.EqualTo("invalid position{X=-1,Y=5}"));
        Assert.That(() => board.GetValue(5, -1), Throws.Exception
            .With.Message.EqualTo("invalid position{X=5,Y=-1}"));
        Assert.That(() => board.GetValue(board.Width, 5), Throws.Exception
            .With.Message.EqualTo($"invalid position{{X={board.Width},Y=5}}"));
        Assert.That(() => board.GetValue(5, board.Height), Throws.Exception
            .With.Message.EqualTo($"invalid position{{X=5,Y={board.Height}}}"));
    }



    [Test]
    public void AddBorderTest()
    {
        // Arrange
        int height = 5;
        int width = 10;
        Board board = new(height, width);

        // Check if the border cells are set to WALL
        for (int y = 0; y < board.Height; y++)
        {
            for (int x = 0; x < board.Width; x++)
            {
                if (x == 0 || x == board.Width - 1 || y == 0 || y == board.Height - 1)
                {
                    Assert.That(board.GetValue(x, y), Is.EqualTo(Board.WALL));
                }
            }
        }

        // Check if the inner cells are set to EMPTY
        for (int y = 1; y < board.Height - 1; y++)
        {
            for (int x = 1; x < board.Width - 1; x++)
            {
                Assert.That(board.GetValue(x, y), Is.EqualTo(Board.EMPTY));
            }
        }
    }
}