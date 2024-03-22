namespace Mekkdonalds.Persistence;

internal interface IRobotsDataAccess
{
    internal Task<List<Robot>> LoadAsync(string path, int width, int height);
    // internal Task SaveAsync(string path, List<Robot> packages); - do we need this?
}

