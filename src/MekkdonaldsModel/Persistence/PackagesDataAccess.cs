namespace Mekkdonalds.Persistence;

public class PackagesDataAccess : IPackagesDataAccess
{
    /// <summary>
    /// loads the packages from a file
    /// </summary>
    /// <param name="path">path to the file</param> 
    /// <param name="width">width of the board</param> 
    /// <param name="height">height of the board</param> 
    /// <returns>a task that represents the loading operation. The task result contains the list of packages</returns> 
    /// <exception cref="PackagesDataException">thrown when the data is invalid</exception> 
    public async Task<List<Package>> LoadAsync(string path, int width, int height)
    {
        List<Package> packages = [];

        using StreamReader sr = new(path);

        _ = await sr.ReadLineAsync();

        while (!sr.EndOfStream)
        {
            string line = await sr.ReadLineAsync() ?? throw new PackagesDataException();

            if (!int.TryParse(line, out var pos) || pos < 0 || pos >= height * width)
            {
                throw new PackagesDataException();
            }

            packages.Add(new Package(((pos % width) + 1), ((pos / width) + 1)));
        }

        return packages;
    }
}
