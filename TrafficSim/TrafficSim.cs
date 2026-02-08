using Jypeli;
using Jypeli.Widgets;

namespace TrafficSim;

/// @author lukar
/// @version 16.01.2026
/// <summary>
/// 
/// </summary>
public class TrafficSim : PhysicsGame
{
    
    //private int Lanes = 4; // TODO implement multiple roads 
    public override void Begin()
    {
        IsFullScreen = true;
        ResetGame();
    }
    
    private void ResetGame()
    {
        ClearAll();
        CreateMap();
        var debug = new Debug(this);
    }
    
    private void CreateMap()
    {
        const int playerSize = 200;
        var car = new PlayerCar(playerSize, playerSize,this);
        var roadMap = new RoadMap(this);
        
        var speedOMeter = new SpeedOMeter(roadMap, new Vector(Level.Left+100, Level.Top-100));
        Add(speedOMeter);
        
        var controls = new Controls(car, roadMap);
        
        AddControls(controls);
    }

    private void AddControls(Controls controls)
    {
        Keyboard.Listen(Key.W, ButtonState.Down, controls.Drive, "");
        Keyboard.Listen(Key.S, ButtonState.Down, controls.Brake, "");
        Keyboard.Listen(Key.A, ButtonState.Down, controls.SteerLeft, "");
        Keyboard.Listen(Key.D, ButtonState.Down, controls.SteerRight, "");
        Keyboard.Listen(Key.Up, ButtonState.Down, controls.Drive, "");
        Keyboard.Listen(Key.Down, ButtonState.Down, controls.Brake, "");
        Keyboard.Listen(Key.Left, ButtonState.Down, controls.SteerLeft, "");
        Keyboard.Listen(Key.Right, ButtonState.Down, controls.SteerRight, "");
        Keyboard.Listen(Key.R, ButtonState.Pressed, ResetGame, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    
}