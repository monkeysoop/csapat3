using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence;

public class LogFile
{
    public required string ActionModel { get; set; }

    public bool AllValid { get; set; }
    public int TeamSize { get; set; }
    public required List<(Point, Direction)> Start { get; set; }

    public int NumTaskFinished { get; set; }
    public int SumOfCost { get; set; }
    public int Makespan { get; set; }

    public required List<string> ActualPaths { get; set; }
    public required List<string> PlannerPaths { get; set; }
    public required List<double> PlannerTimes { get; set; }
    public required List<(int, int, int, string)> Errors { get; set; }
    public required List<List<(int, int, string)>> Events { get; set; }
    public required List<(int, int, int)> Tasks { get; set; }



    [JsonConstructor]
    public LogFile() { }
}
