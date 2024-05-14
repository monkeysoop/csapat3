namespace Mekkdonalds.Persistence;
/// <summary>
/// Interface for the config data access
/// </summary>
public interface IConfigDataAccess
{
    /// <summary>
    /// Load method for the config
    /// </summary>
    /// <param name="path">Path to the file</param> 
    /// <returns>Task that represents the loading operation. The task result contains the config</returns> 
    public Task<Config> LoadAsync(string path);
}
