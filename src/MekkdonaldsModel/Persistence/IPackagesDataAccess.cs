namespace Mekkdonalds.Persistence;
/// <summary>
/// Interface for the packages data access
/// </summary>
public interface IPackagesDataAccess
{
    /// <summary>
    /// Load method to override
    /// </summary>
    /// <param name="path">Path to the file</param> 
    /// <param name="width">Width of the board</param> 
    /// <param name="height">Height of the board</param> 
    /// <returns>Task that represents the loading operation. The task result contains the list of packages</returns> 
    internal Task<List<Package>> LoadAsync(string path, int width, int height);
    // internal Task SaveAsync(string path, List<Package> packages); // not needed for tasks
}
