namespace Mekkdonalds.Persistence;

internal class RobotsDataAccess : IRobotsDataAccess
{
    public async Task<List<Robot>> LoadAsync(string path, int width, int height)
    {
        var robots = new List<Robot>();

        using StreamReader sr = new(path);

        _ = sr.ReadLine(); // skip header

        int id = 1;

        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync() ?? throw new RobotsDataException();

            if (!int.TryParse(line, out var pos) || pos < 0 || pos >= height * width)
            {
                throw new RobotsDataException();
            }

            robots.Add(new Robot(id++, pos % width, pos / height));
        }

        return robots;
    }
}
