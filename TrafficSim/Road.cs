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

    public void SimulateDriving(double force)
    {
        base.Push(new Vector(0, -Mass*force));
    }

    public void SimulateBraking(double force)
    {
        if (Velocity.Y > -100) return;
        base.Push(new Vector(0, Mass*force));
        
    }

    public void MoveRoad(PhysicsObject border, PhysicsObject self)
    {
        Y += Height*2;
    }
    
    public static PhysicsObject CreateLowerBorder(double posY)
    {
        var border = new PhysicsObject(5000, 100);
        border.Top = posY;
        border.IgnoresCollisionResponse = true;
        border.IgnoresGravity = true;
        return border;
    }
}