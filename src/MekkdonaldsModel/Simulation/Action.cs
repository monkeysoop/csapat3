namespace Mekkdonalds.Simulation;
/// <summary>
/// Actions that can be taken by a robot
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
/// Extension methods for the <see cref="Action"/> enum
/// </summary>
public static class ActionMethods
{
    /// <summary>
    /// Get the opposite action of an action
    /// </summary>
    /// <param name="action"> The action to reverse</param>
    /// <returns>The opposite action</returns>
    public static Action Reverse(this Action action) => action switch
    {
        Action.F => Action.B,
        Action.R => Action.C,
        Action.C => Action.R,
        _ => action
    };
}
