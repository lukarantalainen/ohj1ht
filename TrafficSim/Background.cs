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
    /// <summary>
    /// Constructor that takes a Jypeli.Color as a parameter
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    public Background(double width, double height, Color color) :  this(width, height)
    {
        base.Color = color;
    }
    public Background(double width, double height, Image backgroundTexture, Image backgroundItemTexture) : this(width, height)
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
    public void SimulateDriving()
    {
        Push(new Vector(0, -Mass*100));
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