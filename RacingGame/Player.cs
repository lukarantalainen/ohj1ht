    using Jypeli;
using Jypeli.Assets;
namespace RacingGame;

public class Player : PhysicsObject
{
    private readonly RacingGame game;
    private readonly Map map;

    public Player(RacingGame game, Map map) : base(Properties.CarSize, Properties.CarSize)
    {
        this.game = game;
        this.map = map;
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Image = RacingGame.PlayerImage;
        base.Shape = RacingGame.PlayerShape;
        LinearDamping = 0.998;
        Restitution = 1;
        CanRotate = false;
        Mass = 1000;
        Tag = "player";
        base.Position = new Vector(0, Game.Screen.Bottom+200);
        game.Add(this, 0);
        game.AddCollisionHandler(this, "vehicle", HandleCollision);
    }

    private void HandleCollision(PhysicsObject colliding, PhysicsObject target)
    {
        target.Destroy();
        game.MediaPlayer.Play("quack");
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
