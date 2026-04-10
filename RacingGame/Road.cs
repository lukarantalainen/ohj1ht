using System;
using System.Collections.Generic;
using Jypeli;

namespace RacingGame;

/// <summary>
///     Creates Road objects based on PhysicsObject class
/// </summary>
public class Road
{
    private readonly PhysicsObject borderLeft;
    private readonly PhysicsObject borderRight;
    private readonly PhysicsObject lowerRoad;
    private readonly PhysicsObject upperRoad;

    /// <summary>
    ///     Initializes a road
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="texture"></param>
    /// <param name="game"></param>
    public Road(double width, double height, Image texture, RacingGame game)
    {
        upperRoad = CreateRoad(width, height, new Vector(0, Game.Screen.Height), texture, Properties.MaxVelocity);
        lowerRoad = CreateRoad(width, height, new Vector(0, 0), texture, Properties.MaxVelocity);

        borderLeft = CreateRoadBorder(Color.Silver, Side.Left);
        borderRight = CreateRoadBorder(Color.Silver, Side.Right);

        var lowerBorder = new PhysicsObject(Game.Screen.Width, 1)
        {
            Position = new Vector(0, Game.Screen.Bottom - Game.Screen.Height),
            IgnoresCollisionResponse = true,
            IgnoresGravity = true,
            IgnoresExplosions = true
        };

        Lanes = [-225, -85, 85, 225];

        game.Add(upperRoad, -1);
        game.Add(lowerRoad, -1);
        game.Add(borderLeft, -1);
        game.Add(borderRight, -1);
        game.Add(lowerBorder, -1);

        game.AddCollisionHandler(lowerBorder, upperRoad, Cycle);
        game.AddCollisionHandler(lowerBorder, lowerRoad, Cycle);
    }

    public List<double> Lanes { get; }

    /// <summary>
    ///     Creates a PhysicsObject for the road
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="position"></param>
    /// <param name="image"></param>
    /// <param name="maxVelocity"></param>
    /// <returns></returns>
    private static PhysicsObject CreateRoad(double width, double height, Vector position, Image image,
        double maxVelocity)
    {
        var road = new PhysicsObject(width, height)
        {
            Position = position,
            Image = image,
            LinearDamping = 0.9,
            IgnoresGravity = true,
            IgnoresCollisionResponse = true,
            IgnoresExplosions = true,
            MaxVelocity = maxVelocity
        };
        return road;
    }

    /// <summary>
    ///     Creates borders to the road
    /// </summary>
    /// <param name="color"></param>
    /// <param name="side"></param>
    /// <returns></returns>
    private PhysicsObject CreateRoadBorder(Color color, Side side)
    {
        var border = new PhysicsObject(Properties.RoadBorderWidth, Game.Screen.Height)
        {
            Restitution = 0.5,
            Color = color
        };
        switch (side)
        {
            case Side.Left:
                border.Right = upperRoad.Left;
                break;
            case Side.Right:
                border.Left = upperRoad.Right;
                break;
        }

        border.MakeStatic();

        return border;
    }

    /// <summary>
    ///     Moves the roads to create an illusion
    /// </summary>
    /// <param name="border"></param>
    /// <param name="road"></param>
    private static void Cycle(PhysicsObject border, PhysicsObject road)
    {
        road.Y += Game.Screen.Height;
    }

    /// <summary>
    ///     Handle driving
    /// </summary>
    /// <param name="force"></param>
    public void Drive(double force)
    {
        upperRoad.Push(new Vector(0, -upperRoad.Mass * force));
        lowerRoad.Push(new Vector(0, -lowerRoad.Mass * force));
    }

    /// <summary>
    ///     Handle braking
    /// </summary>
    /// <param name="force"></param>
    public void Brake(double force)
    {
        upperRoad.Push(new Vector(0, upperRoad.Mass * force));
        lowerRoad.Push(new Vector(0, lowerRoad.Mass * force));
    }

    /// <summary>
    ///     Set road max velocity
    /// </summary>
    /// <param name="maxVelocity"></param>
    public void SetMaxVelocity(double maxVelocity)
    {
        upperRoad.MaxVelocity = maxVelocity;
        lowerRoad.MaxVelocity = maxVelocity;
    }

    /// <summary>
    ///     Get road velocity
    /// </summary>
    /// <returns></returns>
    public double GetVelocity()
    {
        return -upperRoad.Velocity.Y;
    }

    /// <summary>
    ///     Get road width
    /// </summary>
    /// <returns></returns>
    public double GetWidth()
    {
        return upperRoad.Width;
    }

    /// <summary>
    ///     Get a road PhysicsObject instance
    /// </summary>
    /// <param name="road"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public PhysicsObject GetRoad(int road)
    {
        return road switch
        {
            0 => upperRoad,
            1 => lowerRoad,
            _ => throw new ArgumentException("Invalid road number. Must be 0 or 1.")
        };
    }

    /// <summary>
    ///     Get road border instance
    /// </summary>
    /// <param name="road"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public PhysicsObject GetBorder(int road)
    {
        return road switch
        {
            0 => borderLeft,
            1 => borderRight,
            _ => throw new ArgumentException("Invalid border number. Must be 0 or 1.")
        };
    }

    private enum Side
    {
        Left,
        Right
    }
}