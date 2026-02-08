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
    private int Lanes = 4; // TODO implement multiple roads 
    
    private static readonly Image DesertTexture = LoadImage("desert_texture");
    private static readonly Image CactusTexture = LoadImage("cactus_texture");

    private double _screenWidth;
    private double _screenHeight;
    private const int PlayerSize = 200;
    private const int RoadWidth = 200;
    
    public override void Begin()
    {
        IsFullScreen = true;
        ResetGame();
    }
    
    private void ResetGame()
    {
        ClearAll();
        _screenWidth =  Screen.Width;
        _screenHeight = Screen.Height;
        CreateMap();
        CreateZoomSlider();
    }
    
    private void CreateMap()
    {
        
        var car = CreateCar();
        
        var roads = CreateRoads();
        var backgrounds = CreateBackgrounds();
        
        var roadMap = CreateRoad(roads[0], roads[1], backgrounds[0], backgrounds[1]);
        var controls = new Controls(car, roadMap);
        
        AddControls(controls);
    }
    private PlayerCar CreateCar()
    {
        var car = new PlayerCar(PlayerSize, PlayerSize, CarTexture, CarShape);
        Add(car, 0);
        return car;
    }

    private Road[] CreateRoads()
    {
        var roadHeight = _screenHeight;
        var road1 = new Road(RoadWidth, roadHeight, RoadTexture);
        var road2 = new Road(RoadWidth, roadHeight, RoadTexture);
        Add(road1, -1);
        Add(road2, -1);
        return [road1, road2];
    }

    private Background[] CreateBackgrounds()
    {
        //var background1 = new Background(Screen.Width, _screenHeight, DesertTexture, CactusTexture);
        //var background2 = new Background(Screen.Width, _screenHeight, DesertTexture, CactusTexture);
        var background1 = new Background(_screenWidth,  _screenHeight, Color.JungleGreen);
        var background2 = new Background(_screenWidth,  _screenHeight, Color.JungleGreen);

        Add(background1, -2);
        Add(background2, -2);
        return [background1, background2];
    }
    
    private RoadMap CreateRoad(Road road1, Road road2, Background background1, Background background2)
    {
        const int borderWidth = 20;
        var roadMap = new RoadMap(road1, road2, background1, background2);
        var slider =  roadMap.CreateSlider();
        Add(slider);
        
        foreach (var b in roadMap.CreateRoadBorders(borderWidth, _screenHeight, Color.Silver))
        {
            Add(b, -1);
        }
        
        var roadLowerBorder = Road.CreateLowerBorder(Screen.Bottom-_screenHeight);
        Add(roadLowerBorder, -1);
        var backgroundLowerBorder = Background.CreateLowerBorder(Screen.Bottom-_screenHeight);
        Add(backgroundLowerBorder, -2);
        
        AddCollisionHandler(roadLowerBorder, road1, road1.MoveRoad);
        AddCollisionHandler(roadLowerBorder, road2, road2.MoveRoad);
        AddCollisionHandler(backgroundLowerBorder, background1, background1.MoveBackground);
        AddCollisionHandler(backgroundLowerBorder, background2, background2.MoveBackground);
        
        var speedOMeter = new SpeedOMeter(roadMap, new Vector(Level.Left+100, Level.Top-100));
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
        Keyboard.Listen(Key.R, ButtonState.Pressed, ResetGame, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    private void CreateZoomSlider()
    {
        var zoomMeter = new  DoubleMeter(0, -5, 5);
        zoomMeter.Changed += ZoomLevel;

        var zoomSlider = new Slider(200, 20);
        zoomSlider.BindTo(zoomMeter);
        Add(zoomSlider);
    }

    private void ZoomLevel(double oldValue, double newValue)
    {
        Camera.ZoomFactor = 1+newValue;
    }
}