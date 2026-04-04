using System.Security.AccessControl;
using System.Threading;

namespace TrafficSim;
using Jypeli;

public class Controls
{
    private readonly TrafficSim trafficSim;
    private readonly Player car;
    private readonly Map map;
    private readonly Progress progress;

    private Controls(TrafficSim trafficSim, Player car,  Map map, Progress progress)
    {
        this.trafficSim = trafficSim;
        this.car = car;
        this.map = map;
        this.progress = progress;
    }

    public static void Start(TrafficSim trafficSim, Player car,  Map map, Progress progress)
    {
        var controls = new Controls(trafficSim, car, map, progress);
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
        trafficSim.Keyboard.Listen(Key.W, ButtonState.Down, Drive, "");
        trafficSim.Keyboard.Listen(Key.W, ButtonState.Up, DriveIdle, "");
        trafficSim.Keyboard.Listen(Key.S, ButtonState.Down, Brake, "");
        trafficSim.Keyboard.Listen(Key.A, ButtonState.Down, SteerLeft, "");
        trafficSim.Keyboard.Listen(Key.D, ButtonState.Down, SteerRight, "");
        trafficSim.Keyboard.Listen(Key.Up, ButtonState.Down, Drive, "");
        trafficSim.Keyboard.Listen(Key.Up, ButtonState.Up, DriveIdle, "");
        trafficSim.Keyboard.Listen(Key.Down, ButtonState.Down, Brake, "");
        trafficSim.Keyboard.Listen(Key.Left, ButtonState.Down, SteerLeft, "");
        trafficSim.Keyboard.Listen(Key.Right, ButtonState.Down, SteerRight, "");
    }
}