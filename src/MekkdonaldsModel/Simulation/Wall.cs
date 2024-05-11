﻿namespace Mekkdonalds.Simulation;

/// <summary>
/// Wall in the simulation
/// </summary> 
public sealed class Wall
{
    /// <summary>
    /// Position of the wall
    /// </summary>
    public Point Position { get; }

    /// <summary>
    /// Creates a new wall
    /// </summary>
    /// <param name="x">Coordinate of the wall</param> 
    public Wall(Point x)
    {
        Position = x;
    }

    /// <summary>
    /// Creates a new wall
    /// </summary>
    /// <param name="x">X coordinate of the wall</param> 
    /// <param name="y">Y coordinate of the wall</param> 
    public Wall(int x, int y) : this(new(x, y)) { }
}
