using Jypeli;


namespace RacingGame;

public class Background : PhysicsObject
{
    private readonly PhysicsObject upperBackground;
    private readonly PhysicsObject lowerBackground;

    public Background(double width, double height, Image image, RacingGame game) : base(width, height)
    {
        upperBackground = CreateBackground(width, height, new Vector(0, 0), image);
        lowerBackground = CreateBackground(width, height, new Vector(0, 0), image);

        var lowerBorder = new PhysicsObject(Game.Screen.Width, 1)
        {
            Position = new Vector(0, Game.Screen.Bottom - Game.Screen.Height),
            IgnoresCollisionResponse = true,
            IgnoresGravity = true,
            IgnoresExplosions = true,
        };

        game.Add(upperBackground, -2);
        game.Add(lowerBackground, -2);
        game.Add(lowerBorder, 2);

        game.AddCollisionHandler(lowerBorder, upperBackground, Cycle);
        game.AddCollisionHandler(lowerBorder, lowerBackground, Cycle);

    }

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
            MaxVelocity = Properties.BGMaxVelocity,

        };
        return background;
    }

    private void Cycle(PhysicsObject border, PhysicsObject background)
    {
        background.Y += Game.Screen.Height
            ;
    }

    public void Drive(double force)
    {
        upperBackground.Push(new Vector(0, -Mass * force));
        lowerBackground.Push(new Vector(0, -Mass * force));
    }

    public void Brake(double force)
    {
        upperBackground.Push(new Vector(0, Mass * force));
        lowerBackground.Push(new Vector(0, Mass * force));
    }

    public void SetMaxVelocity(double maxVelocity)
    {
        upperBackground.MaxVelocity = maxVelocity;
        upperBackground.MaxVelocity = maxVelocity;
    }

    public double GetVelocity()
    {
        return -upperBackground.Velocity.Y;
    }
}