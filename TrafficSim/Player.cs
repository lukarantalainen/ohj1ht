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
        Image = TrafficSim.PlayerTexture;
        base.Shape = TrafficSim.PlayerShape;
        LinearDamping = 0.998;
        Restitution = 0;
        CanRotate = false;
        Mass = 1000;
        Tag = "player";
        base.Position = new Vector(0, Game.Screen.Bottom+200);
        _trafficSim.Add(this, 0);
        _trafficSim.AddCollisionHandler(this, "vehicle", HandleCollision);
    }

    private void HandleCollision(PhysicsObject colliding, PhysicsObject target)
    {
        target.Y = Game.Screen.Bottom - 1000;
        TrafficSim.testSound.Play();
        _map.Slow();
    }

    public void SteerRight()
    {
        Push(new Vector(Mass*5000, 0));
    }

    public void SteerLeft()
    {
        Push(new Vector(-Mass*5000, 0));
    }
    
}
