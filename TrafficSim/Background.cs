using Jypeli;
using System;
using System.Collections.Generic;

namespace TrafficSim;

public class Background : PhysicsObject
{
    private static double _height;
    private readonly Image _backgroundItemTexture;

    private Background(double width, double height, double maxVelocity) : base(width, height) 
    {
        _height = height*2;
        IgnoresGravity = true;
        IgnoresCollisionResponse = true;
        IgnoresExplosions = true;
        MaxVelocity = maxVelocity;
    }

    public Background(double width, double height, Color color, double maxVelocity) :  this(width, height, maxVelocity)
    {
        base.Color = color;
    }
    
    public Background(double width, double height, Image backgroundTexture, double maxVelocity) : this(width, height, maxVelocity)
    {
        Image = backgroundTexture;
    }
    public Background(double width, double height, Image backgroundTexture, Image backgroundItemTexture, double maxVelocity) : this(width, height, maxVelocity)
    {
        Image = backgroundTexture;
        _backgroundItemTexture = backgroundItemTexture;
    }
    
    public void Cycle(PhysicsObject a, PhysicsObject b)
    {
        Y += _height;
    }

    public void Drive(double force)
    {
        Push(new Vector(0, -Mass*force));
    }

    public void Brake(double force)
    {
        Push(new Vector(0, Mass*force));
    }
    public static PhysicsObject CreateLowerBorder(double posY)
    {
        var border = new PhysicsObject(6000, 100);
        border.Color = Color.Black;
        border.Top = posY;
        border.IgnoresCollisionResponse = true;
        border.IgnoresGravity = true;
        return border;
    }
}