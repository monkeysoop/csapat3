namespace Mekkdonalds.Persistence;
/// <summary>
/// Data access for the robots
/// </summary>
public class RobotsDataAccess : IRobotsDataAccess
{
    /// <summary>
    /// Loads the robots from the file
    /// </summary>
    /// <param name="path"> Path to the file</param>
    /// <param name="width"> Width of the board</param>
    /// <param name="height"> Height of the board</param>
    /// <returns>A task that represents the loading operation. The task result contains the list of robots</returns> 
    /// <exception cref="RobotsDataException">Thrown when the data is invalid</exception> 
    public async Task<List<Robot>> LoadAsync(string path, int width, int height)
    {
        List<Robot> robots = [];

        using StreamReader sr = new(path);

        _ = sr.ReadLine(); // skip header

        while (!sr.EndOfStream)
        {
            string line = await sr.ReadLineAsync() ?? throw new RobotsDataException();

            if (!int.TryParse(line, out var pos) || pos < 0 || pos >= height * width)
            {
                throw new RobotsDataException();
            }

            robots.Add(new Robot(new Point(((pos % width) + 1), ((pos / width) + 1)), Direction.North));
        }

        return robots;
    }
}
