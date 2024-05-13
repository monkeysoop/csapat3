namespace Mekkdonalds.Simulation;

public class Logger
{
    private readonly LogFile _logFile;
    private readonly string _fileName;

    public Logger(string mapName)
    {
        _logFile = LogFile.New;

        if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");

        _fileName = $"logs/{mapName}_{DateTime.Now:yyyy-MM-dd-HH-mm}_log.json";
    }

    /// <summary>
    /// Sets the action model
    /// </summary>
    /// <param name="model">Name of the action model</param>
    public void SetActionModel(string model) => _logFile.ActionModel = model;

    /// <summary>
    /// Sets the team size
    /// </summary>
    /// <param name="size"></param>
    public void SetTeamSize(int size) => _logFile.TeamSize = size;

    /// <summary>
    /// Logs the start positions and directions of the robots
    /// </summary>
    /// <param name="robots">Collection of the robots</param>
    public void LogStarts(IEnumerable<Robot> robots)
    {
        if (_logFile.Start.Count > 0) throw new InvalidOperationException("Starts have already been set");

        _logFile.Start.AddRange(robots.Select(r => (r.Position, r.Direction)));
        _logFile.TeamSize = _logFile.Start.Count;
        _logFile.Events.AddRange(robots.Select(r => new List<(int, int, string)>()));
        _logFile.PlannerPaths.AddRange(robots.Select(r => new List<Action>()));
        _logFile.ActualPaths.AddRange(robots.Select(r => new List<Action>()));
    }

    /// <summary>
    /// Logs the paths actually taken by the robots based on their history (clears the ActualPaths list)
    /// </summary>
    /// <param name="robots">List of robots</param>
    public void LogActualPaths(IEnumerable<Robot> robots)
    {
        foreach (var robot in robots)
        {
            _logFile.ActualPaths[robot.ID - 1].Clear();
            _logFile.ActualPaths[robot.ID - 1].AddRange(robot.History);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iD"></param>
    /// <param name="action"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void LogActualPath(int iD, Action action)
    {
        _logFile.ActualPaths[iD - 1].Add(action);
    }

    /// <summary>
    /// Logs the planned path of the currently assigned task
    /// </summary>
    /// <param name="robotID">Identifier of the robot</param>
    /// <param name="plannedPath">Planned path</param>
    public void LogPlannerPaths(int robotID, Path plannedPath) => _logFile.PlannerPaths[robotID - 1].AddRange(plannedPath.PlannedPath);

    /// <summary>
    /// Logs a single step of the planned path of the currently assigned task
    /// </summary>
    /// <param name="robotID">Identifier of the robot</param>
    /// <param name="step">The step to be logged</param>
    public void LogPlannerPaths(int robotID, Action step) => _logFile.PlannerPaths[robotID - 1].Add(step);

    /// <summary>
    /// Logs the time taken by the planner to assign to each robot
    /// </summary>
    /// <param name="time">The time elapsed</param>
    public void LogTime(double time) => _logFile.PlannerTimes.Add(time);

    /// <summary>
    /// Logs an error
    /// </summary>
    /// <param name="robot1">ID of the first robot in the incident</param>
    /// <param name="robot2">ID of the second robot in the incident</param>
    /// <param name="time">When it occurred</param>
    /// <param name="eventName">Type of the event</param>
    public void LogError(int robot1, int robot2, int time, string eventName)
    {
        _logFile.AllValid = false;
        _logFile.Errors.Add((robot1 - 1, robot2 - 1, time, eventName));
    }

    /// <summary>
    /// Logs an error that doesn't involve robots
    /// </summary>
    /// <param name="time">When it occurred</param>
    /// <param name="eventName">Type of the event</param>
    public void LogError(int time, string eventName) => LogError(0, 0, time, eventName);

    /// <summary>
    /// Logs the assignment of a task to a robot
    /// </summary>
    /// <param name="robotID">ID of the robot the task got assigned to</param>
    /// <param name="taskID">ID of the task</param>
    /// <param name="when">The number of the simulation step it occurred</param>
    public void LogAssignment(int robotID, int taskID, int when) => LogEvent(robotID, taskID, when, "assigned");

    /// <summary>
    /// Logs the completion of a task by a robot
    /// </summary>
    /// <param name="robotID">ID of the robot the task got assigned to</param>
    /// <param name="taskID">ID of the task</param>
    /// <param name="when">The number of the simulation step it occurred</param>
    public void LogFinish(int robotID, int taskID, int when)
    {
        _logFile.NumTaskFinished++;
        LogEvent(robotID, taskID, when, "finished");
    }

    /// <summary>
    /// Logs a single task
    /// </summary>
    /// <param name="package">Task to log</param>
    public void LogTask(Package package) => _logFile.Tasks.Add((_logFile.Tasks.Count, package.Position.Y - 1, package.Position.X - 1));

    /// <summary>
    /// Logs a collection of tasks
    /// </summary>
    /// <param name="tasks">Collection of tasks</param>
    public void LogTasks(IEnumerable<Package> tasks) => _logFile.Tasks.AddRange(tasks.Select((t, i) => (i, t.Position.Y - 1, t.Position.X - 1)));

    public void LogReplayLength(int length) => _logFile.Makespan = length;

    /// <summary>
    /// Saves the logged data to a log file
    /// </summary>
    /// <param name="access">The type data access to use</param>
    /// <returns>A Task that represents the asynchronous save operation</returns>
    public async Task SaveAsync(ILogFileDataAccess access) => await access.SaveAsync(_fileName, _logFile);

    /// <summary>
    /// Creates a deep copy of the log file
    /// </summary>
    /// <returns>A log file equivalent to the one used by the logger</returns>
    public LogFile GetLogFile()
    {
        LogFile l = LogFile.New;

        l.ActualPaths.AddRange(_logFile.ActualPaths);
        l.AllValid = _logFile.AllValid;
        l.Errors.AddRange(_logFile.Errors);
        l.Events.AddRange(_logFile.Events);
        l.Makespan = _logFile.Makespan;
        l.PlannerPaths.AddRange(_logFile.PlannerPaths);
        l.PlannerTimes.AddRange(_logFile.PlannerTimes);
        l.Start.AddRange(_logFile.Start);
        l.SumOfCost = _logFile.SumOfCost;
        l.NumTaskFinished = _logFile.NumTaskFinished;
        l.TeamSize = _logFile.TeamSize;
        l.Tasks.AddRange(_logFile.Tasks);
        l.ActionModel = _logFile.ActionModel;

        return l;
    }

    private void LogEvent(int robotID, int taskID, int when, string eventName) => _logFile.Events[robotID - 1].Add((taskID - 1, when, eventName));
}
