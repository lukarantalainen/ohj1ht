using Jypeli;
using System;
using System.Collections.Generic;

namespace TrafficSim;

public class Background : PhysicsObject
{
    private static double _width;
    private static double _height;
    private readonly Image _backgroundItemTexture;
    private new const double MaxVelocity = 1000;

    private Background(double width, double height) : base(width, height) //default
    {
        _height = height;
        _width = width;
        IgnoresGravity = true;
        IgnoresCollisionResponse = true;
        base.MaxVelocity = MaxVelocity;
    }
    
    public Background(double width, double height, Color color) :  this(width, height)
    {
        base.Color = color;
    }
    public Background(double width, double height, Image backgroundTexture, Image backgroundItemTexture) : base(width, height)
    {
        Image = backgroundTexture;
        _backgroundItemTexture = backgroundItemTexture;
    }
    
    public void MoveBackground(PhysicsObject a, PhysicsObject b)
    {
        Y += _height*2;
    }

    public void SimulateDriving()
    {
        Push(new Vector(0, -Mass*100));
    }

    public void SimulateBraking()
    {
        if (Velocity.Y > -20) return;
        Push(new Vector(0, Mass*100));
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