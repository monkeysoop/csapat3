namespace Mekkdonalds.Persistence;

public interface IConfigDataAccess
{
    public Task<Config> LoadAsync(string path);
}
