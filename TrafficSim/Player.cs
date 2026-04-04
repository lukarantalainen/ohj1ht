    using Jypeli;
using Jypeli.Assets;
namespace TrafficSim;

public class Player : PhysicsObject
{
    private const double PlayerSize = 150;
    private readonly TrafficSim trafficSim;
    private readonly Map map;

    public Player(TrafficSim trafficSim, Map map) : base(PlayerSize, PlayerSize)
    {
        this.trafficSim = trafficSim;
        this.map = map;
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Image = TrafficSim.PlayerTexture;
        base.Shape = TrafficSim.PlayerShape;
        LinearDamping = 0.998;
        Restitution = 1;
        CanRotate = false;
        Mass = 1000;
        Tag = "player";
        base.Position = new Vector(0, Game.Screen.Bottom+200);
        trafficSim.Add(this, 0);
        trafficSim.AddCollisionHandler(this, "vehicle", HandleCollision);
    }

    private void HandleCollision(PhysicsObject colliding, PhysicsObject target)
    {
        target.Y = Game.Screen.Bottom - 1000;
        map.Slow();
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
