using System.Security.AccessControl;
using System.Threading;

namespace TrafficSim;
using Jypeli;

public class Controls
{
    private readonly Player _car;
    private readonly Map _map;
    private readonly TrafficSim _trafficSim;

    private Controls(TrafficSim trafficSim, Player car,  Map map)
    {
        _car = car;
        _map = map;
        _trafficSim = trafficSim;
    }

    public static void Start(TrafficSim trafficSim, Player car,  Map map)
    {
        var controls = new Controls(trafficSim, car, map);
        controls.AddControls();
    }

    private void Drive()
    {
        _map.Drive();
    }

    private void Brake()
    {
        _map.Brake();
    }

    private void DriveIdle()
    {
        _map.DriveIdle();
    }

    private void SteerLeft()
    {
        _car.SteerLeft();
    }

    private void SteerRight()
    {
        _car.SteerRight();
    }
    
    private void AddControls()
    {
        _trafficSim.Keyboard.Listen(Key.W, ButtonState.Down, Drive, "");
        _trafficSim.Keyboard.Listen(Key.W, ButtonState.Up, DriveIdle, "");
        _trafficSim.Keyboard.Listen(Key.S, ButtonState.Down, Brake, "");
        _trafficSim.Keyboard.Listen(Key.A, ButtonState.Down, SteerLeft, "");
        _trafficSim.Keyboard.Listen(Key.D, ButtonState.Down, SteerRight, "");
        _trafficSim.Keyboard.Listen(Key.Up, ButtonState.Down, Drive, "");
        _trafficSim.Keyboard.Listen(Key.Up, ButtonState.Released, DriveIdle, "");
        _trafficSim.Keyboard.Listen(Key.Down, ButtonState.Down, Brake, "");
        _trafficSim.Keyboard.Listen(Key.Left, ButtonState.Down, SteerLeft, "");
        _trafficSim.Keyboard.Listen(Key.Right, ButtonState.Down, SteerRight, "");
    }
}