using Jypeli;

namespace TrafficSim;

public class Background : PhysicsObject
{
    private static double _height;
    public Background(double width, double height, Image texture) : base(width, height)
    {
        Image = texture;
        _height = height;
        IgnoresGravity = true;
        IgnoresCollisionResponse = true;
        MaxVelocity = 1000;
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
        var border = new PhysicsObject(5000, 1);
        border.Top = posY;
        border.IgnoresCollisionResponse = true;
        border.IgnoresGravity = true;
        return border;
    }
}