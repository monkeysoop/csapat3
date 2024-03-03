namespace Mekkdonalds.Simulation;

internal class Wall : IMapObject
{
    private static Wall? _instnace;

    internal static Wall Instance
    {
        get => _instnace ??= new();
    }

    private Wall() { }
}
