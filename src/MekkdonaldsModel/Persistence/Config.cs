using Mekkdonalds.Persistence.Converters;

using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence;

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
    /// number of robots
    /// </summary>
    public int TeamSize { get; set; }
    /// <summary>
    /// Number of tasks to reveal
    /// </summary>
    public int NumTasksReveal { get; set; }
    /// <summary>
    /// strategy for task assignment
    /// </summary>

    [JsonConverter(typeof(StrategyConverter))]
    public Strategy TaskAssignmentStrategy { get; set; }

    [JsonConstructor]
    public Config() { }
}
