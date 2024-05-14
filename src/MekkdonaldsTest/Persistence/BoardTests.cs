namespace Mekkdonalds.Test.Persistence;

[TestOf(typeof(BoardFileDataAccess))]
public class BoardTests
{
    [Test]
    public async Task BoardLoadingTest()
    {
        Board board = await new BoardFileDataAccess().LoadAsync("../../../../MekkdonaldsWPF/maps/random-32-32-20.map");

        Assert.Multiple(() =>
        {
            Assert.That(board.Height, Is.EqualTo(34));
            Assert.That(board.Width, Is.EqualTo(34));
            Assert.That(board.GetValue(0, 0), Is.EqualTo(1));
            Assert.That(board.GetValue(1, 1), Is.EqualTo(0));
            Assert.That(board.GetValue(board.Height - 1, board.Width - 1), Is.EqualTo(1));
            Assert.That(board.GetValue(3, 3), Is.EqualTo(0));
            Assert.That(board.GetValue(14, 13), Is.EqualTo(1));
        });
    }
}