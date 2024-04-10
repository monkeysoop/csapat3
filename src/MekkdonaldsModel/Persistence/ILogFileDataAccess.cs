namespace Mekkdonalds.Persistence;

public interface ILogFileDataAccess
{
    Task<LogFile> Load(string path);
    Task Save(string path, LogFile logFile);
}
