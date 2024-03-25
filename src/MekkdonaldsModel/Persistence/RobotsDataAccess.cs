namespace Mekkdonalds.Persistence;

public class RobotsDataAccess : IRobotsDataAccess
{
    public async Task<List<Robot>> LoadAsync(string path, int width, int height)
    {
        var robots = new List<Robot>();

        using StreamReader sr = new(path);

        _ = sr.ReadLine(); // skip header

        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync() ?? throw new RobotsDataException();

            if (!int.TryParse(line, out var pos) || pos < 0 || pos >= height * width)
            {
                throw new RobotsDataException();
            }

            robots.Add(new Robot(new Point(pos % width, pos / width), Direction.North));
        }

        return robots;
    }
}
