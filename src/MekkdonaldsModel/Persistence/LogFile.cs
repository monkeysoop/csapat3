using Mekkdonalds.Persistence.Converters;

using System.Text.Json.Serialization;

using Action = Mekkdonalds.Simulation.Action;

namespace Mekkdonalds.Persistence;

public class LogFile
{
    public required string ActionModel { get; set; }

    [JsonConverter(typeof(YesNoConverter))]
    public bool AllValid { get; set; }

    public int TeamSize { get; set; }

    [JsonConverter(typeof(StartPosConverter))]
    public required List<(Point, Direction)> Start { get; init; }

    public int NumTaskFinished { get; set; }

    public int SumOfCost { get; set; }

    public int Makespan { get; set; }

    [JsonConverter(typeof(PathConverter))]
    public required List<List<Action>> ActualPaths { get; init; }

    [JsonConverter(typeof(PathConverter))]
    public required List<List<Action>> PlannerPaths { get; init; }

    public required List<double> PlannerTimes { get; init; }

    [JsonConverter(typeof(ErrorConverter))]
    public required List<(int, int, int, string)> Errors { get; init; }

    [JsonConverter(typeof(EventConverter))]
    public required List<List<(int, int, string)>> Events { get; init; }

    [JsonConverter(typeof(TaskConverter))]
    public required List<(int, int, int)> Tasks { get; init; }

    [JsonConstructor]
    public LogFile() { }

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
