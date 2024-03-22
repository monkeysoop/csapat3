namespace Mekkdonalds.Persistence;

internal interface IPackagesDataAccess
{
    internal Task<List<Package>> LoadAsync(string path);
    internal Task SaveAsync(string path, List<Package> packages);
}
