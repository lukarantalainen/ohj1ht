using Jypeli;

namespace RacingGame;

public class Background : PhysicsObject
{
    private readonly PhysicsObject lowerBackground;
    private readonly PhysicsObject upperBackground;

    /// <summary>
    ///     Constructs the whole background object that's shown on screen
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="image"></param>
    /// <param name="game"></param>
    public Background(double width, double height, Image image, RacingGame game) : base(width, height)
    {
        upperBackground = CreateBackground(width, height, new Vector(0, 0), image);
        lowerBackground = CreateBackground(width, height, new Vector(0, 0), image);

        var lowerBorder = new PhysicsObject(Game.Screen.Width, 1)
        {
            Position = new Vector(0, Game.Screen.Bottom - Game.Screen.Height),
            IgnoresCollisionResponse = true,
            IgnoresGravity = true,
            IgnoresExplosions = true
        };

        game.Add(upperBackground, -2);
        game.Add(lowerBackground, -2);
        game.Add(lowerBorder, 2);

        game.AddCollisionHandler(lowerBorder, upperBackground, Cycle);
        game.AddCollisionHandler(lowerBorder, lowerBackground, Cycle);
    }

    /// <summary>
    ///     Creates a PhysicsObject
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="position"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    private static PhysicsObject CreateBackground(double width, double height, Vector position, Image image)
    {
        var background = new PhysicsObject(width, height)
        {
            Image = image,
            LinearDamping = 0.75,
            Position = position,
            IgnoresCollisionResponse = true,
            IgnoresGravity = true,
            IgnoresExplosions = true,
            MaxVelocity = Properties.BgMaxVelocity
        };
        return background;
    }

    /// <summary>
    ///     Moves the backgrounds to create an illusion
    /// </summary>
    /// <param name="border"></param>
    /// <param name="background"></param>
    private void Cycle(PhysicsObject border, PhysicsObject background)
    {
        background.Y += Game.Screen.Height
            ;
    }

    /// <summary>
    ///     Pushes the background as you drive
    /// </summary>
    /// <param name="force"></param>
    public void Drive(double force)
    {
        upperBackground.Push(new Vector(0, -Mass * force));
        lowerBackground.Push(new Vector(0, -Mass * force));
    }

    /// <summary>
    ///     Slows down the background
    /// </summary>
    /// <param name="force"></param>
    public void Brake(double force)
    {
        upperBackground.Push(new Vector(0, Mass * force));
        lowerBackground.Push(new Vector(0, Mass * force));
    }

    /// <summary>
    ///     Sets maximum velocity for the backgrounds
    /// </summary>
    /// <param name="maxVelocity"></param>
    public void SetMaxVelocity(double maxVelocity)
    {
        upperBackground.MaxVelocity = maxVelocity;
        upperBackground.MaxVelocity = maxVelocity;
    }

    /// <summary>
    ///     Gets background velocity
    /// </summary>
    /// <returns></returns>
    public double GetVelocity()
    {
        return -upperBackground.Velocity.Y;
    }
}