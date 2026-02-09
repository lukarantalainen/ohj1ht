using System.Security.AccessControl;
using System.Threading;

namespace TrafficSim;
using Jypeli;

public class Controls
{
    private readonly PlayerCar _car;
    private readonly RoadMap _roadMap;
    private readonly TrafficSim _parent;

    private Controls(PlayerCar car,  RoadMap roadMap, TrafficSim parent)
    {
        _car = car;
        _roadMap = roadMap;
        _parent = parent;
    }

    public static void Start(PlayerCar car,  RoadMap roadMap, TrafficSim parent)
    {
        var controls = new Controls(car, roadMap, parent);
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
        _parent.Keyboard.Listen(Key.W, ButtonState.Down, Drive, "");
        _parent.Keyboard.Listen(Key.S, ButtonState.Down, Brake, "");
        _parent.Keyboard.Listen(Key.A, ButtonState.Down, SteerLeft, "");
        _parent.Keyboard.Listen(Key.D, ButtonState.Down, SteerRight, "");
        _parent.Keyboard.Listen(Key.Up, ButtonState.Down, Drive, "");
        _parent.Keyboard.Listen(Key.Down, ButtonState.Down, Brake, "");
        _parent.Keyboard.Listen(Key.Left, ButtonState.Down, SteerLeft, "");
        _parent.Keyboard.Listen(Key.Right, ButtonState.Down, SteerRight, "");
        _parent.Keyboard.Listen(Key.R, ButtonState.Pressed, _parent.ResetGame, "");
        _parent.Keyboard.Listen(Key.Escape, ButtonState.Pressed, _parent.ConfirmExit, "Lopeta peli");
    }
}