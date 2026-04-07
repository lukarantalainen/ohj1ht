using Jypeli;
using Jypeli.Effects;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
namespace RacingGame;

/// <summary>
/// Creates Road objects based on PhysicsObject class
/// </summary>
public class Road
{
    private readonly PhysicsObject upperRoad;
    private readonly PhysicsObject lowerRoad;

    private readonly PhysicsObject borderLeft;
    private readonly PhysicsObject borderRight;

    public List<double> Lanes { get; }
    private enum Side
    {
        Left,
        Right,
    }

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
            IgnoresExplosions = true,
        };

        Lanes = [ -225, -85, 85, 225 ];

        game.Add(upperRoad, -1);
        game.Add(lowerRoad, -1);
        game.Add(borderLeft, -1);
        game.Add(borderRight, -1);
        game.Add(lowerBorder, -1);

        game.AddCollisionHandler(lowerBorder, upperRoad, Cycle);
        game.AddCollisionHandler(lowerBorder, lowerRoad, Cycle);
    }


    private static PhysicsObject CreateRoad(double width, double height, Vector position, Image image, double maxVelocity)
    {
        var road = new PhysicsObject(width, height)
        {
            Position = position,
            Image = image,
            Restitution = 0.8,
            LinearDamping = 0.75,
            IgnoresGravity = true,
            IgnoresCollisionResponse = true,
            IgnoresExplosions = true,
            MaxVelocity = maxVelocity
        };
        return road;
    }

    
    private PhysicsObject CreateRoadBorder(Color color, Side side)
    {
        var border = new PhysicsObject(Properties.RoadBorderWidth, Game.Screen.Height)
        {
            Restitution = 0,
            Color = color,
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

    private static void Cycle(PhysicsObject border, PhysicsObject road)
    {
        road.Y += Game.Screen.Height;
    }

    public void Drive(double force)
    {
        upperRoad.Push(new Vector(0, -upperRoad.Mass*force));
        lowerRoad.Push(new Vector(0, -lowerRoad.Mass * force));
    }

    public void Brake(double force)
    {

        upperRoad.Push(new Vector(0, upperRoad.Mass * force));
        lowerRoad.Push(new Vector(0, lowerRoad.Mass * force));
    }

    public void SetMaxVelocity(double maxVelocity)
    {
        upperRoad.MaxVelocity = maxVelocity;
        lowerRoad.MaxVelocity = maxVelocity;
    }

    public double GetVelocity()
    {
        return -upperRoad.Velocity.Y;
    }

    public double GetWidth()
    {
        return upperRoad.Width;
    }

    public PhysicsObject GetRoad(int road)
    {
        return road switch
        {
            0 => upperRoad,
            1 => lowerRoad,
            _ => throw new ArgumentException("Invalid road number. Must be 0 or 1.")
        };
    }

    public PhysicsObject GetBorder(int road)
    {
        return road switch
        {
            0 => borderLeft,
            1 => borderRight,
            _ => throw new ArgumentException("Invalid border number. Must be 0 or 1.")
        };
    }
}