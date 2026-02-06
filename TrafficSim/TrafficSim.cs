using System;
using Jypeli;


namespace TrafficSim;

/// @author lukar
/// @version 16.01.2026
/// <summary>
/// 
/// </summary>
public class TrafficSim : PhysicsGame
{
    private Player _player;
    private static readonly Image CarTexture = LoadImage("car_texture");
    private static readonly Shape CarShape = Shape.FromImage(CarTexture);
    
    private Road _topRoad;
    private Road _bottomRoad;
    private static readonly Image RoadTexture = LoadImage("road_texture");
    private PhysicsObject _lowerBorder;
    
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
        CreatePlayer();
        AddControls();
        CreateMap();
        CreateDebugLabel();
        
    }
    
    private void CreatePlayer()
    {
        _player = new Player(PlayerSize, PlayerSize, CarTexture, CarShape);
        Add(_player, 0);
    }
    
    private void AddControls()
    {
        Keyboard.Listen(Key.W, ButtonState.Down, Drive, "");
        Keyboard.Listen(Key.S, ButtonState.Down, Brake, "");
        Keyboard.Listen(Key.A, ButtonState.Down, _player.SteerLeft, "");
        Keyboard.Listen(Key.D, ButtonState.Down, _player.SteerRight, "");
        Keyboard.Listen(Key.Up, ButtonState.Down, Drive, "");
        Keyboard.Listen(Key.Down, ButtonState.Down, Brake, "");
        Keyboard.Listen(Key.Left, ButtonState.Down, _player.SteerLeft, "");
        Keyboard.Listen(Key.Right, ButtonState.Down, _player.SteerRight, "");
        Keyboard.Listen(Key.R, ButtonState.Pressed, InitializeGame, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    
    private void Drive()
    {
        _bottomRoad.SimulateDriving(500);
        _topRoad.SimulateDriving(500);
    }
    private void Brake()
    {
        if (_bottomRoad.Velocity.Y > 0) return;
        _bottomRoad.SimulateBraking(500);
        _topRoad.SimulateBraking(500);
    }
    private void CreateMap()
    {
        CreateLowerBorder();
        CreateRoads();
    }

    private void CreateLowerBorder()
    {
        Level.BackgroundColor = Color.JungleGreen;
        _lowerBorder = new PhysicsObject(Level.Width, 1);
        _lowerBorder.Y = -(Screen.Height-200);
        Add(_lowerBorder, -3);
    }

    private void CreateRoads()
    {
        _bottomRoad = new Road(RoadWidth, Screen.Height, RoadTexture);
        _topRoad = new Road(RoadWidth, Screen.Height, RoadTexture);
        
        _topRoad.Y = _bottomRoad.Y+_bottomRoad.Height-200;
        
        Add(_bottomRoad, -3);
        Add(_topRoad, -3);
        AddCollisionHandler(_lowerBorder, _bottomRoad, RoadCycle);
        AddCollisionHandler(_lowerBorder, _topRoad, RoadCycle);
    }
    
    private static void RoadCycle(PhysicsObject target, PhysicsObject road)
    {
        road.Y = 3000;
    }
    
    private void CreateDebugLabel()
    {
        _debugLabel = new Label();
        _debugLabel.Position = new Vector(Level.Left+100, Level.Top-100);
        Add(_debugLabel);
        _debugLabel.Text = "Test";
        Timer speedMeter = new Timer();
        speedMeter.Interval = 0.01;
        speedMeter.Timeout += UpdateLabel;
        speedMeter.Start();
    }

    private void UpdateLabel()
    {
        _debugLabel.Text = _topRoad.Velocity.Magnitude.ToString();

    }
    
}