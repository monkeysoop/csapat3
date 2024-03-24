namespace Mekkdonalds.Persistence;

public class PackagesDataAccess : IPackagesDataAccess
{
    public async Task<List<Package>> LoadAsync(string path, int width, int height)
    {
        var packages = new List<Package>();

        using var sr = new StreamReader(path);

        _ = await sr.ReadLineAsync();

        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync() ?? throw new PackagesDataException();

            if (!int.TryParse(line, out var pos) || pos < 0 || pos >= height * width)
            {
                throw new PackagesDataException();
            }

            packages.Add(new Package(pos % width, pos / height));
        }

        return packages;
    }
}
