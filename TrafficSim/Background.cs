using Jypeli;
using System;
using System.Collections.Generic;

namespace TrafficSim;

public class Background : PhysicsObject
{
    private static double _width;
    private static double _height;
    private readonly Image _backgroundItemTexture;
    private new const double MaxVelocity = 350;

    private Background(double width, double height, double maxVelocity) : base(width, height) 
    {
        _height = height;
        _width = width;
        IgnoresGravity = true;
        IgnoresCollisionResponse = true;
        IgnoresExplosions = true;
        base.MaxVelocity = maxVelocity;
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
    
    /// <summary>
    /// Used with the two Background objects in a RoadMap
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public void MoveBackground(PhysicsObject a, PhysicsObject b)
    {
        Y += _height*2;
    }

    /// <summary>
    /// Moves the background to simulate driving forward
    /// </summary>
    public void SimulateDriving(double force)
    {
        Push(new Vector(0, -Mass*force));
    }

    /// <summary>
    /// Moves the background to simulate slowing down 
    /// </summary>
    public void SimulateBraking()
    {
        Push(new Vector(0, Mass*100));
    }
    
    /// <summary>
    /// Creates a lower border for the Background objects to bump in to
    /// </summary>
    /// <param name="posY"></param>
    /// <returns></returns>
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