using Jypeli;
using Jypeli.Assets;
namespace TrafficSim;

public class Player : PhysicsObject
{
    private const double PlayerSize = 200;

    public Player(TrafficSim trafficSim) : base(PlayerSize, PlayerSize)
    {
        var carTexture = Game.LoadImage("car_texture");
        Image = carTexture;
        base.Shape = Shape.FromImage(carTexture);
        LinearDamping = 0.998;
        Restitution = 0;
        CanRotate = false;
        Mass = 100000;
        base.Position = new Vector(0, -200);
        trafficSim.Add(this, 0);
        trafficSim.AddCollisionHandler(this, "vehicle", CollisionHandler.DestroyTarget);
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
