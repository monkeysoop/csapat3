namespace Mekkdonalds.Persistence;
/// <summary>
/// Interface for the robots data access
/// </summary>
public interface IRobotsDataAccess
{
    /// <summary>
    /// Load method to override
    /// </summary>
    /// <param name="path">Path to the file</param> 
    /// <param name="width">Width of the board</param> 
    /// <param name="height">Height of the board</param> 
    /// <returns>Task that represents the loading operation. The task result contains the list of robots</returns> 
    internal Task<List<Robot>> LoadAsync(string path, int width, int height);
    // internal Task SaveAsync(string path, List<Robot> packages); - do we need this?
}

