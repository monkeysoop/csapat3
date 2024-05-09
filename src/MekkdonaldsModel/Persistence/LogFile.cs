using Mekkdonalds.Persistence.Converters;

using System.Text.Json.Serialization;

using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Persistence;

public class LogFile
{
    /// <summary>
    /// action model used in the replay
    /// </summary>
    public required string ActionModel { get; set; }

    /// <summary>
    /// whether all the actions are valid
    /// </summary>
    [JsonConverter(typeof(YesNoConverter))]
    public bool AllValid { get; set; }

    /// <summary>
    /// team size
    /// </summary>
    public int TeamSize { get; set; }
    /// <summary>
    /// Start positions and directions of the robots
    /// </summary>

    [JsonConverter(typeof(StartPosConverter))]
    public required List<(Point, Direction)> Start { get; init; }

    /// <summary>
    /// number of tasks finished
    /// </summary>
    public int NumTaskFinished { get; set; }

    /// <summary>
    /// total cost of the actions
    /// </summary>
    public int SumOfCost { get; set; }

    /// <summary>
    /// length of the replay
    /// </summary>
    public int Makespan { get; set; }

    /// <summary>
    /// Paths actually taken by the robots
    /// </summary>
    [JsonConverter(typeof(PathConverter))]
    public required List<List<Action>> ActualPaths { get; init; }
    /// <summary>
    /// Paths planned by the planner
    /// </summary>
    [JsonConverter(typeof(PathConverter))]
    public required List<List<Action>> PlannerPaths { get; init; }
    /// <summary>
    /// times taken by the planner to plan the paths
    /// </summary>
    public required List<double> PlannerTimes { get; init; }
    /// <summary>
    /// errors encountered during the replay
    /// </summary>
    [JsonConverter(typeof(ErrorConverter))]
    public required List<(int, int, int, string)> Errors { get; init; }
    /// <summary>
    /// Events that occurred during the replay
    /// </summary>

    [JsonConverter(typeof(EventConverter))]
    public required List<List<(int, int, string)>> Events { get; init; }
    /// <summary>
    /// Tasks that need to be completed
    /// </summary>

    [JsonConverter(typeof(TaskConverter))]
    public required List<(int, int, int)> Tasks { get; init; }

    [JsonConstructor]
    public LogFile() { }

    /// <summary>
    /// Creates an empty log file
    /// </summary>
    public static LogFile New => new()
    {
        ActionModel = "MAPF_T",
        AllValid = true,
        TeamSize = 0,
        Start = [],
        NumTaskFinished = 0,
        SumOfCost = 0,
        Makespan = 0,
        ActualPaths = [],
        PlannerPaths = [],
        PlannerTimes = [],
        Errors = [],
        Events = [],
        Tasks = []
    };
}
