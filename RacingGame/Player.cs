    using Jypeli;
using Jypeli.Assets;
using Jypeli.Widgets;
namespace RacingGame;

public class Player : PhysicsObject
{
    private readonly RacingGame game;
    private readonly Map map;

    private readonly IntMeter healthMeter;
    private readonly ProgressBar healthBar;

    /// <summary>
    /// Creates the player
    /// </summary>
    /// <param name="game"></param>
    /// <param name="map"></param>
    public Player(RacingGame game, Map map) : base(Properties.CarSize, Properties.CarSize)
    {
        this.game = game;
        this.map = map;

        healthMeter = new IntMeter(Properties.PlayerHealth)
        {
            MaxValue = Properties.PlayerHealth,
            MinValue = 0,
        };
        healthMeter.LowerLimit += HealthEnd;

        CreatePlayer();
        healthBar = CreateHealthBar(healthMeter);
    }

    /// <summary>
    /// Do when out of health
    /// </summary>
    private void HealthEnd()
    {
        game.End(true);
    }

    /// <summary>
    /// Creates the player
    /// </summary>
    private void CreatePlayer()
    {
        Image = RacingGame.PlayerImage;
        base.Shape = RacingGame.CarShape;
        LinearDamping = 0.8;
        Restitution = 1;
        CanRotate = false;
        Mass = 1000;
        Tag = "player";
        base.Position = new Vector(0, Game.Screen.Bottom+350);
        game.Add(this, 0);
        game.AddCollisionHandler(this, "vehicle", HandleCollision);
    }

    /// <summary>
    /// Handles collision to other vehicles
    /// </summary>
    /// <param name="colliding"></param>
    /// <param name="target"></param>
    private void HandleCollision(PhysicsObject colliding, PhysicsObject target)
    {
        target.Destroy();
        healthMeter.Value -= Properties.DamageFromCar;
        game.MediaPlayer.Play("quack");
        map.Slow();
    }

    /// <summary>
    /// Creates a health bar
    /// </summary>
    /// <param name="healthMeter"></param>
    /// <returns></returns>
    private static ProgressBar CreateHealthBar(IntMeter healthMeter)
    {
       var healthbar = new ProgressBar(280, 20)
        {
            Image = Game.LoadImage("health-bar-empty"),
            BarImage = Game.LoadImage("health-bar-full"),
        };

        healthbar.BindTo(healthMeter);
        return healthbar;
    }

    /// <summary>
    /// Move to the right
    /// </summary>
    public void SteerRight()
    {
        Push(new Vector(Mass*1000, 0));
    }

    /// <summary>
    /// Move to the left
    /// </summary>
    public void SteerLeft()
    {
        Push(new Vector(-Mass*1000, 0));
    }

    /// <summary>
    /// Get the health bar instance
    /// </summary>
    /// <returns></returns>
    public ProgressBar GetHealthBar()
    {
        return healthBar;
    }
    
}
