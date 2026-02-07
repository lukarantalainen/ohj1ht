using Jypeli;
namespace TrafficSim;

public class Road : PhysicsObject
{
    private readonly double _screenHeight;
    public Road(double width, double height, Image texture) : base(width, height)
    {
        _screenHeight = height;
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
        if (Velocity.Y > 0) return;
        base.Push(new Vector(0, Mass * force));
    }

    public void MoveRoad(PhysicsObject border, PhysicsObject self)
    {
        Y = _screenHeight;
    }
}