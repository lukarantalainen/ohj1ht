using Jypeli;
namespace TrafficSim;

public class Car : PhysicsObject
{
    private const double SteeringForce = 1000;
    public Car(double width, double height, Image texture, Shape shape) : base(width, height, shape)
    {
        Image = texture;
        LinearDamping = 0.998;
        CanRotate = false;
        base.Position = new Vector(0, -200);
    }

    public void SteerRight()
    {
        Push(new Vector(Mass*SteeringForce, 0));
    }

    public void SteerLeft()
    {
        Push(new Vector(-Mass*SteeringForce, 0));
    }
    
}
