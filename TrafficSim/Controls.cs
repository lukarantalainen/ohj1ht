namespace RacingGame;

using Jypeli;

public class Controls
{
    private readonly RacingGame game;
    private readonly Player car;
    private readonly Map map;
    private readonly Progress progress;

    private Controls(RacingGame game, Player car, Map map, Progress progress)
    {
        this.game = game;
        this.car = car;
        this.map = map;
        this.progress = progress;
    }

    public static void Start(RacingGame game, Player car, Map map, Progress progress)
    {
        var controls = new Controls(game, car, map, progress);
        controls.AddControls();
    }

    private void Drive()
    {
        map.Drive();
        progress.Drive(map.GetVelocity());
    }

    private void Brake()
    {
        map.Brake();
        progress.Drive(map.GetVelocity());
    }

    private void DriveIdle()
    {
        map.DriveIdle();
        progress.Drive(map.GetVelocity());
    }

    private void SteerLeft()
    {
        car.SteerLeft();
    }

    private void SteerRight()
    {
        car.SteerRight();
    }

    private void AddControls()
    {
        game.Keyboard.Listen(Key.W, ButtonState.Down, Drive, "");
        game.Keyboard.Listen(Key.W, ButtonState.Up, DriveIdle, "");
        game.Keyboard.Listen(Key.S, ButtonState.Down, Brake, "");
        game.Keyboard.Listen(Key.A, ButtonState.Down, SteerLeft, "");
        game.Keyboard.Listen(Key.D, ButtonState.Down, SteerRight, "");
        game.Keyboard.Listen(Key.Up, ButtonState.Down, Drive, "");
        game.Keyboard.Listen(Key.Up, ButtonState.Up, DriveIdle, "");
        game.Keyboard.Listen(Key.Down, ButtonState.Down, Brake, "");
        game.Keyboard.Listen(Key.Left, ButtonState.Down, SteerLeft, "");
        game.Keyboard.Listen(Key.Right, ButtonState.Down, SteerRight, "");
    }
}