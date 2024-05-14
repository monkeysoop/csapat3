using System.Diagnostics.CodeAnalysis;

namespace Mekkdonalds.Simulation.Assigner;

/// <summary>
/// Assigns packages to robots in a round-robin fashion (the packages are assigned in the order they were added). It is prepared to be used in a multi-threaded environment.
/// </summary>
public class RoundRobinAssigner : Assigner
{
    private new readonly ConcurrentQueue<Package> _packages;

    /// <summary>
    /// Creates a new round-robin assigner
    /// </summary>
    /// <param name="board">Map of the warehouse</param>
    /// <param name="packages">Collection of the packages that will be assigned</param>
    /// <param name="robots">Collection of the robots that are part of the simulation</param>
    public RoundRobinAssigner(Board board, IEnumerable<Package> packages, IEnumerable<Robot> robots) : base(board, packages, robots)
    {
        _packages = new(packages);
    }

    public override bool NoPackage => _packages.IsEmpty;

    public override bool Peek(Robot _, [MaybeNullWhen(false)] out Package package) => _packages.TryPeek(out package);

    public override bool Get(Robot _, [MaybeNullWhen(false)] out Package package) => _packages.TryDequeue(out package);

    public override void Return(Package package) => _packages.Enqueue(package);
}
