using NUnit.Framework;
using System.Drawing;

namespace Mekkdonalds.Simulation.Tests
{
    [TestFixture]
    public class WallTests
    {
        [Test]
        public void TestWallConstructorWithPoint()
        {
            // Arrange
            int x = 5;
            int y = 10;
            Point position = new Point(x, y);

            // Act
            Wall wall = new Wall(position);

            // Assert
            Assert.That(position, Is.EqualTo(wall.Position));
        }

        [Test]
        public void TestWallConstructorWithIntegers()
        {
            // Arrange
            int x = 3;
            int y = 7;

            // Act
            Wall wall = new Wall(x, y);

            // Assert
            Assert.That(new Point(x, y), Is.EqualTo(wall.Position));
        }

        [TestCase(0, 0)]
        [TestCase(-5, 10)]
        [TestCase(20, -15)]
        public void TestWallConstructorWithValidCoordinates(int x, int y)
        {
            // Act
            Wall wall = new Wall(x, y);

            // Assert
            Assert.That(new Point(x, y), Is.EqualTo(wall.Position));
        }

        [Test]
        public void TestWallPositionImmutable()
        {
            // Arrange
            Wall wall = new Wall(5, 10);
            Point initialPosition = wall.Position;

            // Act
            // Attempt to modify the Position property (readonlyXD)

            // Assert
            Assert.That(initialPosition, Is.EqualTo(wall.Position));
        }
    }
}
