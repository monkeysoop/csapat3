using Mekkdonalds.Persistence.Converters;
using Mekkdonalds.Simulation.Assigner;

using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence;
/// <summary>
/// Configuration for the simulation
/// </summary>
public class Config
{
    /// <summary>
    /// Map file path
    /// </summary>
    public required string MapFile { get; set; }
    /// <summary>
    /// Agent file path
    /// </summary>
    public required string AgentFile { get; set; }
    /// <summary>
    /// Task file path
    /// </summary>
    public required string TaskFile { get; set; }
    /// <summary>
    /// Number of robots
    /// </summary>
    public int TeamSize { get; set; }
    /// <summary>
    /// Number of tasks to reveal
    /// </summary>
    public int NumTasksReveal { get; set; }
    /// <summary>
    /// Strategy for task assignment
    /// </summary>

    [JsonConverter(typeof(StrategyConverter))]
    public Strategy TaskAssignmentStrategy { get; set; }

    [JsonConstructor]
    public Config() { }

    internal Assigner? GetAssigner(Board board, List<Package> tasks, List<Robot> robots) => TaskAssignmentStrategy switch
    {
        Strategy.RoundRobin => new RoundRobinAssigner(board, tasks, robots),
        _ => throw new NotImplementedException()
    };
}
