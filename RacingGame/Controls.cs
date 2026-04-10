using Jypeli;

namespace RacingGame;

public class Controls
{
    private readonly Player car;
    private readonly RacingGame game;
    private readonly Map map;
    private readonly Progress progress;

    /// <summary>
    ///     Initializes the class variables
    /// </summary>
    /// <param name="game"></param>
    /// <param name="car"></param>
    /// <param name="map"></param>
    /// <param name="progress"></param>
    private Controls(RacingGame game, Player car, Map map, Progress progress)
    {
        this.game = game;
        this.car = car;
        this.map = map;
        this.progress = progress;
    }

    /// <summary>
    ///     Adds controls to the game
    /// </summary>
    /// <param name="game"></param>
    /// <param name="car"></param>
    /// <param name="map"></param>
    /// <param name="progress"></param>
    public static void Start(RacingGame game, Player car, Map map, Progress progress)
    {
        var controls = new Controls(game, car, map, progress);
        controls.AddControls();
    }

    /// <summary>
    ///     Handles driving
    /// </summary>
    private void Drive()
    {
        map.Drive();
        progress.Drive(map.GetVelocity());
    }


    /// <summary>
    ///     Handles braking
    /// </summary>
    private void Brake()
    {
        map.Brake();
        progress.Drive(map.GetVelocity());
    }

    /// <summary>
    ///     Handles progress when the car is moving but not accelerating
    /// </summary>
    private void DriveIdle()
    {
        //map.DriveIdle();
        progress.Drive(map.GetVelocity());
    }

    /// <summary>
    ///     Move player to the left
    /// </summary>
    private void SteerLeft()
    {
        car.SteerLeft();
    }

    /// <summary>
    ///     Move player to the right
    /// </summary>
    private void SteerRight()
    {
        car.SteerRight();
    }

    /// <summary>
    ///     Add the controls to the game
    /// </summary>
    private void AddControls()
    {
        game.Keyboard.Listen(Key.W, ButtonState.Down, Drive, "");
        game.Keyboard.Listen(Key.W, ButtonState.Up, DriveIdle, "");
        game.Keyboard.Listen(Key.S, ButtonState.Down, Brake, "");
        game.Keyboard.Listen(Key.A, ButtonState.Down, SteerLeft, "");
        game.Keyboard.Listen(Key.D, ButtonState.Down, SteerRight, "");
        //game.Keyboard.Listen(Key.Up, ButtonState.Down, Drive, "");
        //game.Keyboard.Listen(Key.Up, ButtonState.Up, DriveIdle, "");
        //game.Keyboard.Listen(Key.Down, ButtonState.Down, Brake, "");
        //game.Keyboard.Listen(Key.Left, ButtonState.Down, SteerLeft, "");
        //game.Keyboard.Listen(Key.Right, ButtonState.Down, SteerRight, "");
    }
}