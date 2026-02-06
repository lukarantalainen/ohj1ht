using Jypeli;
namespace TrafficSim;

public class Road : PhysicsObject
{
    public Road(double width, double height, Image texture) : base(width, height)
    {
        Image = texture;
        IgnoresGravity = true;
        IgnoresCollisionResponse = true;
    }
    
    public Road(double width, double height, Color color) : base(width, height)
    {
        base.Color = color;
        IgnoresGravity = true;
        IgnoresCollisionResponse = true;
    }

    public void SimulateDriving(double force)
    {
        base.Push(new Vector(0, -Mass*force));
    }

    public void SimulateBraking(double force)
    {
        base.Push(new Vector(0, Mass*force));
    }
}