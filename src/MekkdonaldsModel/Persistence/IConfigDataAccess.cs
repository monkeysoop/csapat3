namespace Mekkdonalds.Persistence;

internal interface IConfigDataAccess
{
    public Task<Config> Load(string path);
}
