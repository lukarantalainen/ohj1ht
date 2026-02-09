using Jypeli;
namespace TrafficSim;

public class PlayerCar : PhysicsObject
{
    private const double PlayerSize = 200;

    public PlayerCar(TrafficSim parent) : base(PlayerSize, PlayerSize)
    {
        var carTexture = Game.LoadImage("car_texture");
        Image = carTexture;
        base.Shape = Shape.FromImage(carTexture);
        LinearDamping = 0.998;
        Restitution = 0.5;
        CanRotate = false;
        base.Position = new Vector(0, -200);
        parent.Add(this, 0);
    }

    public void SteerRight(double velocity)
    {
        Push(new Vector(Mass*(velocity), 0));
    }

    public void SteerLeft(double velocity)
    {
        Push(new Vector(-Mass*(velocity), 0));
    }
    
}
