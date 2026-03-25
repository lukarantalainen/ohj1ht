    using Jypeli;
using Jypeli.Assets;
namespace TrafficSim;

public class Player : PhysicsObject
{
    private const double PlayerSize = 200;
    private readonly TrafficSim _trafficSim;
    private readonly Map _map;

    public Player(TrafficSim trafficSim, Map map) : base(PlayerSize, PlayerSize)
    {
        _trafficSim = trafficSim;
        _map = map;
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        var carTexture = Game.LoadImage("car_texture");
        Image = carTexture;
        base.Shape = Shape.FromImage(carTexture);   
        LinearDamping = 0.998;
        Restitution = 0;
        CanRotate = false;
        Mass = 1000;
        Tag = "player";
        base.Position = new Vector(0, -200);
        _trafficSim.Add(this, 0);
        _trafficSim.AddCollisionHandler(this, "vehicle", HandleCollision);
    }

    private void HandleCollision(PhysicsObject colliding, PhysicsObject target)
    {
        var e = new Explosion( 50 );
        e.Position = target.Position;
        target.Destroy();
        _trafficSim.Add(e);
        _map.Slow();
        
    }

    public void SteerRight()
    {
        Push(new Vector(Mass*1000, 0));
    }

    public void SteerLeft()
    {
        Push(new Vector(-Mass*1000, 0));
    }
    
}
