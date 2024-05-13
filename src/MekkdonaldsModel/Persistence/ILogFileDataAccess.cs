namespace Mekkdonalds.Persistence;
/// <summary>
/// Interface for the log file data access
/// </summary>
public interface ILogFileDataAccess
{
    /// <summary>
    /// Load method for the log files
    /// </summary>
    /// <param name="path"> Path to the file</param>
    /// <returns>Task that represents the loading operation. The task result contains the log file</returns> 
    Task<LogFile> LoadAsync(string path);
    /// <summary>
    /// Save method for the log files
    /// </summary>
    /// <param name="path">Path to the file</param> 
    /// <param name="logFile">Log file to save</param> 
    /// <returns>Task that represents the saving operation</returns> 
    Task SaveAsync(string path, LogFile logFile);
}
