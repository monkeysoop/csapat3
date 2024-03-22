namespace Mekkdonalds.Persistence;

internal class RobotsDataAccess : IRobotsDataAccess
{
    public async Task<List<Robot>> LoadAsync(string path, int width, int height)
    {
        var robots = new List<Robot>();

        using StreamReader reader = new(path);

        _ = reader.ReadLine(); // skip header

        int id = 1;

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync() ?? throw new RobotsDataException();

            if (!int.TryParse(line, out var pos) || pos < 0 || pos >= height * width)
            {
                throw new RobotsDataException();
            }

            robots.Add(new Robot(id++, pos / height, pos % width));
        }

        return robots;
    }
}
