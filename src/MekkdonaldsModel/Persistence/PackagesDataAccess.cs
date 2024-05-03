namespace Mekkdonalds.Persistence;

public class PackagesDataAccess : IPackagesDataAccess
{
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
