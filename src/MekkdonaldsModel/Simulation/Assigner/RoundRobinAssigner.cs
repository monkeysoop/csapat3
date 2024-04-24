using System.Diagnostics.CodeAnalysis;

namespace Mekkdonalds.Simulation.Assigner;

public class RoundRobinAssigner(Board board, IEnumerable<Package> packages, IEnumerable<Robot> robots) : Assigner(board, packages, robots)
{
    private new readonly ConcurrentQueue<Package> _packages = new(packages);

    public override bool NoPackage => _packages.IsEmpty;

    public override bool Peek([MaybeNullWhen(false)] out Package package) => _packages.TryPeek(out package);

    public override bool Get([MaybeNullWhen(false)] out Package package) => _packages.TryDequeue(out package);

    public override void Return(Package package) => _packages.Enqueue(package);
}
