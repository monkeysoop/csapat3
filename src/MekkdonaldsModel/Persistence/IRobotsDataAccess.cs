using Mekkdonalds.Simulation;

namespace MekkdonaldsModel.Persistence
{
    internal interface IRobotsDataAccess
    {
        internal Task<List<Robot>> LoadAsync(string path);
        internal Task SaveAsync(string path, List<Robot> packages);
    }
}

