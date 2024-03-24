namespace Mekkdonalds.Persistence;

public interface IConfigDataAccess
{
    public Task<Config> Load(string path);
}
