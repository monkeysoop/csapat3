namespace Mekkdonalds.Persistence;

public interface ILogFileDataAccess
{
    Task<LogFile> LoadAsync(string path);
    Task SaveAsync(string path, LogFile logFile);
}
