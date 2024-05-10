namespace Mekkdonalds.Simulation;
/// <summary>
/// Enum for the actions the robot can take
/// </summary>
public enum Action
{
    F,
    R,
    C,
    W,
    T,
    B
}
/// <summary>
/// Methods for the actions
/// </summary>
public static class ActionMethods
{
    /// <summary>
    /// Reverses the action
    /// </summary>
    /// <param name="action"> The action to reverse</param>
    /// <returns> The opposite action</returns>
    public static Action Reverse(this Action action) => action switch
    {
        Action.F => Action.B,
        Action.R => Action.C,
        Action.C => Action.R,
        _ => action
    };
}
