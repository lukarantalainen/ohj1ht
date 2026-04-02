using Jypeli;
namespace TrafficSim;

/// <summary>
/// Creates Road objects based on PhysicsObject class
/// </summary>
public class Road : PhysicsObject
{
    public Road(double width, double height, Image texture, double maxVelocity) : base(width, height)
    {
        Image = texture;
        Restitution = 0.8;
        IgnoresGravity = true;
        IgnoresCollisionResponse = true;
        IgnoresExplosions = true;
        IgnoresCollisionResponse = true;
        MaxVelocity = maxVelocity;
    }
    
    public void Drive(double force)
    {
        Push(new Vector(0, -Mass*force));
    }

    public void Brake(double force)
    {

        Push(new Vector(0, Mass*force));
    }
    
    public void Cycle(PhysicsObject border, PhysicsObject self)
    {
        Bottom = Game.Screen.Top;
    }
}