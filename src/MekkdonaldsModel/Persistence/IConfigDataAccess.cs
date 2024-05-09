namespace Mekkdonalds.Persistence;

public interface IConfigDataAccess
{
    /// <summary>
    /// Load method for the config
    /// </summary>
    /// <param name="path">path to the file</param> 
    /// <returns>task that represents the loading operation. The task result contains the config</returns> 
    public Task<Config> LoadAsync(string path);
}
