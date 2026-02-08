using Jypeli;
namespace TrafficSim;

/// <summary>
/// Creates Road objects based on PhysicsObject class
/// </summary>
public class Road : PhysicsObject
{
    /// <summary>
    /// Constructs a Road object
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="texture"></param>
    public Road(double width, double height, Image texture) : base(width, height)
    {
        Image = texture;
        IgnoresGravity = true;
        IgnoresCollisionResponse = true;
    }

    /// <summary>
    /// Moves the road to simulate driving forward
    /// </summary>
    /// <param name="force"></param>
    public void SimulateDriving(double force)
    {
        base.Push(new Vector(0, -Mass*force));
    }

    /// <summary>
    /// Moves the background to simulate slowing down 
    /// </summary>
    /// <param name="force"></param>
    public void SimulateBraking(double force)
    {
        if (Velocity.Y > -100) return;
        base.Push(new Vector(0, Mass*force));
    }

    /// <summary>
    /// Used with the two Background objects in a RoadMap
    /// </summary>
    /// <param name="border"></param>
    /// <param name="self"></param>
    public void MoveRoad(PhysicsObject border, PhysicsObject self)
    {
        Y += Height*2;
    }
    
    
}