namespace Mekkdonalds.Test;

[TestOf(typeof(Package))]
public class PackageTests
{
    [Test]
    public void TestPackageConstructor()
    {
        // Arrange
        int x = 3;
        int y = 7;
        Point position = new Point(5, 10);

        // Act
        Package package = new Package(x, y);
        Package package2 = new Package(position);

        // Assert
        Assert.Multiple(() =>
        {
            // Assert that the Point (x, y) is equal to package.Position
            Assert.That(new Point(x, y), Is.EqualTo(package.Position), "Point (x, y) should match package position");

            // Assert that the ID 1 is equal to package.ID
            Assert.That(1, Is.EqualTo(package.ID), "ID 1 should match package ID");

            // Assert that the position is equal to package2.Position
            Assert.That(position, Is.EqualTo(package2.Position), "Position should match package2 position");

            // Assert that the ID 2 is equal to package2.ID
            Assert.That(2, Is.EqualTo(package2.ID), "ID 2 should match package2 ID");
        });

    }

    [TestCase(0, 0)]
    [TestCase(-5, 10)]
    [TestCase(20, -15)]
    public void TestPackageConstructorWithValidCoordinates(int x, int y)
    {
        // Act
        Package package = new Package(x, y);

        // Assert
        Assert.That(new Point(x, y), Is.EqualTo(package.Position));
    }

    [Test]
    public void TestPackagePositionImmutable()
    {
        // Arrange
        Package package = new Package(5, 10);
        Point initialPosition = package.Position;

        // Act
        // Attempt to modify the Position property (megint readonlyxddd)

        // Assert
        Assert.That(initialPosition, Is.EqualTo(package.Position));
    }

    [Test]
    public void TestPackageIDsAreUnique()
    {
        // Arrange
        int numPackages = 10;
        Package[] packages = new Package[numPackages];

        // Act
        for (int i = 0; i < numPackages; i++)
        {
            packages[i] = new Package(i, i);
        }

        // Assert
        for (int i = 0; i < numPackages; i++)
        {
            for (int j = i + 1; j < numPackages; j++)
            {
                Assert.That(packages[i].ID, Is.Not.EqualTo(packages[j].ID));
            }
        }
    }
}
