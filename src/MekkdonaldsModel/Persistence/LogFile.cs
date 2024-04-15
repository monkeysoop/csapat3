using Mekkdonalds.Persistence.Converters;
using System.Text.Json;
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
    public required List<(Point, Direction)> Start { get; set; }

    public int NumTaskFinished { get; set; }
    public int SumOfCost { get; set; }
    public int Makespan { get; set; }
    [JsonConverter(typeof(PathConverter))]
    public required List<List<Action>> ActualPaths { get; set; }
    [JsonConverter(typeof(PathConverter))]
    public required List<List<Action>> PlannerPaths { get; set; }
    public required List<double> PlannerTimes { get; set; }
    [JsonConverter(typeof(ErrorConverter))]
    public required List<(int, int, int, string)> Errors { get; set; }
    [JsonConverter(typeof(EventConverter))]
    public required List<List<(int, int, string)>> Events { get; set; }
    [JsonConverter(typeof(TaskConverter))]
    public required List<(int, int, int)> Tasks { get; set; }


    [JsonConstructor]
    public LogFile() { }
}
