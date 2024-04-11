namespace Mekkdonalds.Simulation;

public enum Action
{
    F,
    R,
    C,
    W,
    T,
    B
}

public static class ActionMethods
{
    public static Action Reverse(this Action action) => action switch
    {
        Action.F => Action.B,
        Action.R => Action.C,
        Action.C => Action.R,
        _ => action
    };
}
