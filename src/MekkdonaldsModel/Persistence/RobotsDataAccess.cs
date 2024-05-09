namespace Mekkdonalds.Persistence;

public class RobotsDataAccess : IRobotsDataAccess
{
    /// <summary>
    /// loads the robots from the file
    /// </summary>
    /// <param name="path"> path to the file</param>
    /// <param name="width"> width of the board</param>
    /// <param name="height"> height of the board</param>
    /// <returns>a task that represents the loading operation. The task result contains the list of robots</returns> 
    /// <exception cref="RobotsDataException">thrown when the data is invalid</exception> 
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
