namespace Mekkdonalds.Persistence;

public interface ILogFileDataAccess
{
    /// <summary>
    /// Load method for the log files
    /// </summary>
    /// <param name="path"> path to the file</param>
    /// <returns>task that represents the loading operation. The task result contains the log file</returns> 
    Task<LogFile> LoadAsync(string path);
    /// <summary>
    /// save method for the log files
    /// </summary>
    /// <param name="path">path to the file</param> 
    /// <param name="logFile">log file to save</param> 
    /// <returns>task that represents the saving operation</returns> 
    Task SaveAsync(string path, LogFile logFile);
}
