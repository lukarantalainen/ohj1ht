using System.Security.AccessControl;
using System.Threading;

namespace TrafficSim;
using Jypeli;

public class Controls
{
    private readonly Player _car;
    private readonly RoadMap _roadMap;
    private readonly TrafficSim _trafficSim;

    private Controls(Player car,  RoadMap roadMap, TrafficSim trafficSim)
    {
        _car = car;
        _roadMap = roadMap;
        _trafficSim = trafficSim;
    }

    public static void Start(Player car,  RoadMap roadMap, TrafficSim trafficSim)
    {
        var controls = new Controls(car, roadMap, trafficSim);
        controls.AddControls();
    }

    private void Drive()
    {
        _roadMap.Drive();
    }

    private void Brake()
    {
        _roadMap.Brake();
    }

    private void SteerLeft()
    {
        _car.SteerLeft(_roadMap.GetAbsVelocity());
    }

    private void SteerRight()
    {
        _car.SteerRight(_roadMap.GetAbsVelocity());
    }
    
    private void AddControls()
    {
        _trafficSim.Keyboard.Listen(Key.W, ButtonState.Down, Drive, "");
        _trafficSim.Keyboard.Listen(Key.S, ButtonState.Down, Brake, "");
        _trafficSim.Keyboard.Listen(Key.A, ButtonState.Down, SteerLeft, "");
        _trafficSim.Keyboard.Listen(Key.D, ButtonState.Down, SteerRight, "");
        _trafficSim.Keyboard.Listen(Key.Up, ButtonState.Down, Drive, "");
        _trafficSim.Keyboard.Listen(Key.Down, ButtonState.Down, Brake, "");
        _trafficSim.Keyboard.Listen(Key.Left, ButtonState.Down, SteerLeft, "");
        _trafficSim.Keyboard.Listen(Key.Right, ButtonState.Down, SteerRight, "");
        _trafficSim.Keyboard.Listen(Key.R, ButtonState.Pressed, _trafficSim.ResetGame, "");
        _trafficSim.Keyboard.Listen(Key.Escape, ButtonState.Pressed, _trafficSim.ConfirmExit, "Lopeta peli");
    }
}