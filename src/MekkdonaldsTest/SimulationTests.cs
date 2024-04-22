using Mekkdonalds.Persistence;
using Mekkdonalds.Simulation;
using System.Drawing;
using System.Reflection.Emit;
using System.Text.Json;
using Action = Mekkdonalds.Simulation.Action;
using Path = Mekkdonalds.Simulation.Path;

namespace Mekkdonalds.Test;

public class SimulationTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]

    public void TestPath()
    {
        Path path = new(new List<Action>(), new Point(0, 0));
        Assert.That(path.IsOver, Is.True);
        Assert.That(path[0] is null);
        Assert.That(path[-3] is null);
        Assert.That(path[13] is null);
        Assert.Throws<System.Exception>(() => path.Next());
        path = new(new List<Action>() { Action.F }, new Point(0, 0));
        Assert.That(path.IsOver, Is.False);
        Assert.That(path[0], Is.EqualTo(Action.F));
        Assert.That(path[-3] is null);
        Assert.That(path[13] is null);
        Assert.That(path.Next(), Is.EqualTo(Action.F));
        Assert.That(path.IsOver, Is.True);
        Assert.Throws<System.Exception>(() => path.Next());
        path = new(new List<Action>() { Action.F, Action.C }, new Point(0, 0));
        Assert.That(path.IsOver, Is.False);
        Assert.That(path[0], Is.EqualTo(Action.F));
        Assert.That(path[1], Is.EqualTo(Action.C));
        Assert.That(path[-3] is null);
        Assert.That(path[13] is null);
        Assert.That(path.Next(), Is.EqualTo(Action.F));
        Assert.That(path.IsOver, Is.False);
        Assert.That(path.Next(), Is.EqualTo(Action.C));
        Assert.That(path.IsOver, Is.True);
        Assert.Throws<System.Exception>(() => path.Next());
        path = new(new List<Action>() { Action.F, Action.C, Action.R, Action.T, Action.B, Action.F }, new Point(0, 0));
        Assert.That(path.IsOver, Is.False);
        Assert.That(path[0], Is.EqualTo(Action.F));
        Assert.That(path[1], Is.EqualTo(Action.C));
        Assert.That(path[5], Is.EqualTo(Action.F));
        Assert.That(path[-3] is null);
        Assert.That(path[13] is null);
        Assert.That(path.Next(), Is.EqualTo(Action.F));
        Assert.That(path.IsOver, Is.False);
        Assert.That(path.Next(), Is.EqualTo(Action.C));
        Assert.That(path.IsOver, Is.False);
        Assert.That(path.Next(), Is.EqualTo(Action.R));
        Assert.That(path.IsOver, Is.False);
        Assert.That(path.Next(), Is.EqualTo(Action.T));
        Assert.That(path.IsOver, Is.False);
        Assert.That(path.Next(), Is.EqualTo(Action.B));
        Assert.That(path.IsOver, Is.False);
        Assert.That(path.Next(), Is.EqualTo(Action.F));
        Assert.That(path.IsOver, Is.True);
        Assert.Throws<System.Exception>(() => path.Next());
    }
}
