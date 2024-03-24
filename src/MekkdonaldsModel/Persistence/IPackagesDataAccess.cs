namespace Mekkdonalds.Persistence;

public interface IPackagesDataAccess
{
    internal Task<List<Package>> LoadAsync(string path, int width, int height);
    // internal Task SaveAsync(string path, List<Package> packages); // not needed for tasks
}
