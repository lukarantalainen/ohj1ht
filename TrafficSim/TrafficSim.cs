using System;
using System.IO;
using Jypeli;


namespace TrafficSim;

/// @author lukar
/// @version 16.01.2026
/// <summary>
/// 
/// </summary>
public class TrafficSim : PhysicsGame
{
    
    private static readonly Image CarTexture = LoadImage("car_texture");
    private static readonly Shape CarShape = Shape.FromImage(CarTexture);
    
    private static readonly Image RoadTexture = LoadImage("road_texture");
    
    private Label _debugLabel;
    
    private const int RoadWidth = 200;
    private const int PlayerSize = 200;
    public override void Begin()
    {
        IsFullScreen = true;
        InitializeGame();
    }
    
    private void InitializeGame()
    {
        ClearAll();

        CreateMap();
        CreateDebugLabel();
    }
    
    private void CreateMap()
    {
        var roadMap = CreateRoads();
        var car = CreateCar();
        Controls controls = new Controls(car, roadMap);
        AddControls(controls);
    }

    private RoadMap CreateRoads()
    {
        double screenHeight = Screen.Height;
        Road road1 = new Road(RoadWidth, screenHeight+200, RoadTexture);
        Road road2 = new Road(RoadWidth, screenHeight, RoadTexture);
        
        Add(road1);
        Add(road2);
        
        PhysicsObject lowerBorder = CreateLowerBorder();
        Add(lowerBorder);
        
        AddCollisionHandler(lowerBorder, road1, road1.MoveRoad);
        AddCollisionHandler(lowerBorder, road2, road2.MoveRoad);
        
        RoadMap roadMap = new RoadMap(road1, road2);
        
        return roadMap;
    }
    
    private PhysicsObject CreateLowerBorder()
    {
        Level.BackgroundColor = Color.JungleGreen;
        PhysicsObject lowerBorder = new PhysicsObject(Level.Width, 1);
        lowerBorder.Color = Color.HotPink;
        lowerBorder.Y = Screen.Bottom-Screen.Height;
        return lowerBorder;
    }

    private Car CreateCar()
    {
        var car = new Car(PlayerSize, PlayerSize, CarTexture, CarShape);
        Add(car);
        return car;
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
        Keyboard.Listen(Key.R, ButtonState.Pressed, InitializeGame, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    
    
    private void CreateDebugLabel()
    {
        _debugLabel = new Label();
        _debugLabel.Position = new Vector(Level.Left+100, Level.Top-100);
        Add(_debugLabel);
        _debugLabel.Text = "Test";
    }
    
}