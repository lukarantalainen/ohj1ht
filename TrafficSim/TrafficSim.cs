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
    
    private static readonly Image CarTexture = LoadImage("car_texture");
    private static readonly Shape CarShape = Shape.FromImage(CarTexture);
    
    private static readonly Image RoadTexture = LoadImage("road_texture");
    
    public override void Begin()
    {
        IsFullScreen = true;
        InitializeGame();
    }
    
    private void InitializeGame()
    {
        ClearAll();
        CreateMap();
    }
    
    private void CreateMap()
    {
        var car = CreateCar();
        var roadMap = CreateRoad();
        Controls controls = new Controls(car, roadMap);
        AddControls(controls);
    }
    private PlayerCar CreateCar()
    {
        const int playerSize = 200;
        var car = new PlayerCar(playerSize, playerSize, CarTexture, CarShape);
        Add(car);
        return car;
    }
    private RoadMap CreateRoad()
    {
        int roadWidth = 200;
        double roadHeight = Screen.Height*1.5;
        Road road1 = new Road(roadWidth, roadHeight, RoadTexture);
        Road road2 = new Road(roadWidth, roadHeight, RoadTexture);
        
        Add(road1, -1);
        Add(road2, -1);

        PhysicsObject lowerBorder = Road.CreateLowerBorder(Screen.Bottom-roadHeight);
        Add(lowerBorder, -1);
        
        AddCollisionHandler(lowerBorder, road1, road1.MoveRoad);
        AddCollisionHandler(lowerBorder, road2, road2.MoveRoad);
        RoadMap roadMap = new RoadMap(road1, road2);
        Add(roadMap.CreateSlider());

        SpeedOMeter speedOMeter = new SpeedOMeter(roadMap, new Vector(Level.Left+100, Level.Top-100));
        Add(speedOMeter);
        
        return roadMap;
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
    

    
}