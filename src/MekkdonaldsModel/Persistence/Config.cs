﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekkdonalds.Persistence;

public class Config
{
    public required string MapFile { get; set; }
    public required string AgentFile { get; set; }
    public required string TaskFile { get; set; }

    public int TeamSize { get; set; }
    public int NumTasksReveal { get; set; }

    [JsonConverter(typeof(StrategyConverter))]
    public Strategy TaskAssignmentStrategy { get; set; }

    [JsonConstructor]
    public Config() { }
}
