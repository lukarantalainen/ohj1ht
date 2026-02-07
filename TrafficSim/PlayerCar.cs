using Jypeli;
namespace TrafficSim;

public class PlayerCar : PhysicsObject
{
    private const double SteeringForce = 100;
    public PlayerCar(double width, double height, Image texture, Shape shape) : base(width, height, shape)
    {
        Image = texture;
        LinearDamping = 0.998;
        CanRotate = false;
        base.Position = new Vector(0, -200);
    }

    public void SteerRight(double velocity)
    {
        Push(new Vector(Mass*(velocity+100)*0.5, 0));
    }

    public void SteerLeft(double velocity)
    {
        Push(new Vector(-Mass*(velocity+100)*0.5, 0));
    }
    
}
