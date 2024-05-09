namespace Mekkdonalds.Persistence;

public interface IPackagesDataAccess
{
    /// <summary>
    /// Load method to override
    /// </summary>
    /// <param name="path">path to the file</param> 
    /// <param name="width">width of the board</param> 
    /// <param name="height">height of the board</param> 
    /// <returns>task that represents the loading operation. The task result contains the list of packages</returns> 
    internal Task<List<Package>> LoadAsync(string path, int width, int height);
    // internal Task SaveAsync(string path, List<Package> packages); // not needed for tasks
}
