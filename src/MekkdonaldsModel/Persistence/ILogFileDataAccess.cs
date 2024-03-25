
namespace Mekkdonalds.Persistence
{
    internal interface ILogFileDataAccess
    {
        public Task<LogFile> Load(string path);

    }
}
