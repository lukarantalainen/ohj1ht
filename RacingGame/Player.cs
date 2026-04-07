    using Jypeli;
using Jypeli.Assets;
using Jypeli.Widgets;
namespace RacingGame;

public class Player : PhysicsObject
{
    private readonly RacingGame game;
    private readonly Map map;

    private readonly IntMeter healthMeter = new IntMeter(0);
    private readonly ProgressBar healthBar;

    public Player(RacingGame game, Map map) : base(Properties.CarSize, Properties.CarSize)
    {
        this.game = game;
        this.map = map;

        healthMeter.MaxValue = 100;
        healthMeter.MinValue = 0;
        healthMeter.Value = 100;

        CreatePlayer();
        healthBar = CreateHealthBar(healthMeter);
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
        base.Position = new Vector(0, Game.Screen.Bottom+350);
        game.Add(this, 0);
        game.AddCollisionHandler(this, "vehicle", HandleCollision);
    }

    private void HandleCollision(PhysicsObject colliding, PhysicsObject target)
    {
        target.Destroy();
        healthMeter.Value -= 5;
        game.MediaPlayer.Play("quack");
        map.Slow();
    }

    private static ProgressBar CreateHealthBar(IntMeter healthMeter)
    {
       var healthbar = new ProgressBar(280, 20)
        {
            Image = Game.LoadImage("health_bar_empty"),
            BarImage = Game.LoadImage("health_bar_full"),
        };

        healthbar.BindTo(healthMeter);
        return healthbar;
    }

    public void SteerRight()
    {
        Push(new Vector(Mass*5000, 0));
    }

    public void SteerLeft()
    {
        Push(new Vector(-Mass*5000, 0));
    }

    public ProgressBar GetHealthBar()
    {
        return healthBar;
    }
    
}
