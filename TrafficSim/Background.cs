using Jypeli;
using System;
using System.Collections.Generic;

namespace TrafficSim;

public class Background : PhysicsObject
{
    private readonly PhysicsObject upperBackground;
    private readonly PhysicsObject lowerBackground;

    public Background(double width, double height, Image image, TrafficSim trafficSim) : base(width, height) 
    {
        upperBackground = CreateBackground(width, height, new Vector(0, 0), image);
        lowerBackground = CreateBackground(width, height, new Vector(0, 0), image);

        var lowerBorder = new PhysicsObject(Game.Screen.Width, 1)
        {
            Position = new Vector(0, Game.Screen.Bottom - Game.Screen.Height),
            IgnoresCollisionResponse = true,
            IgnoresGravity = true,
            IgnoresExplosions = true,
        };

        trafficSim.Add(upperBackground, -2);
        trafficSim.Add(lowerBackground, -2);
        trafficSim.Add(lowerBorder, 2);

        trafficSim.AddCollisionHandler(lowerBorder, upperBackground, Cycle);
        trafficSim.AddCollisionHandler(lowerBorder, lowerBackground, Cycle);

    }

    private static PhysicsObject CreateBackground(double width, double height, Vector position, Image image)
    {
        var background = new PhysicsObject(width, height)
        {
            Image = image,
            Position = position,
            IgnoresCollisionResponse = true,
            IgnoresGravity = true,
            IgnoresExplosions = true,
            MaxVelocity = Properties.BGMaxVelocity,

        };
        return background;
    }
    
    private void Cycle(PhysicsObject border, PhysicsObject background)
    {
        background.Y += Game.Screen.Height;
    }

    public void Drive(double force)
    {
        upperBackground.Push(new Vector(0, -Mass*force));
        lowerBackground.Push(new Vector(0, -Mass * force));
    }
 
    public void Brake(double force)
    {
        upperBackground.Push(new Vector(0, Mass*force));
        lowerBackground.Push(new Vector(0, Mass * force));
    }

    public void SetMaxVelocity(double maxVelocity)
    {
        upperBackground.MaxVelocity = maxVelocity;
        upperBackground.MaxVelocity = maxVelocity;
    }

    public double GetVelocity()
    {
        return -upperBackground.Velocity.Y;
    }
}