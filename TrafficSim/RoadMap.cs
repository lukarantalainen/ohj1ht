using Jypeli;
using System;

namespace TrafficSim;
using Jypeli.Widgets;
//TODO fix the inheritance

/// <summary>
/// Contains the main road and background of the game 
/// </summary>
public class RoadMap
{
    private readonly TrafficSim _trafficSim;
    
    private readonly Road _road1;
    private readonly Road _road2;
    private readonly PhysicsObject _roadLowerBorder;
    
    private readonly Background _background1;
    private readonly Background _background2;
    private readonly PhysicsObject _backgroundLowerBorder;
    
    private PhysicsObject _borderLeft;
    private PhysicsObject _borderRight;
    
    private const int RoadWidth = 200;
    private const int BorderWidth = 20;
    private static readonly double ScreenWidth = Game.Screen.Width;
    private static readonly double ScreenHeight = Game.Screen.Height;
    public RoadMap(TrafficSim trafficSim)
    {
        _trafficSim = trafficSim;
        var roadTexture = Game.LoadImage("road_texture");
        _road1 = new Road(RoadWidth, ScreenHeight, roadTexture);
        _road2 = new Road(RoadWidth, ScreenHeight, roadTexture);
        _road2.Y+=_road1.Height;
        
        Add(_road1, _road2, trafficSim, -1);
        _roadLowerBorder = CreateLowerBorder(Game.Screen.Bottom - ScreenHeight);
        Add(_roadLowerBorder, trafficSim, -1);
        
        var desertTexture = Game.LoadImage("desert_texture");
        var cactusTexture = Game.LoadImage("cactus_texture");
        
        _background1 = new Background(ScreenWidth, ScreenHeight, Color.JungleGreen);
        _background2 = new Background(ScreenWidth, ScreenHeight, Color.Orange);
        _background2.Y+=_background1.Height;
        
        Add(_background1, _background2, trafficSim, -2);
        _backgroundLowerBorder = CreateLowerBorder(Game.Screen.Bottom - ScreenHeight);
        Add(_backgroundLowerBorder, trafficSim, -2);
        
        AddHandlers(trafficSim);

        CreateRoadBorders(BorderWidth, ScreenHeight, Color.Silver, trafficSim);
        StartVehicleGenerator();

        CreateSlider(trafficSim);
    }

    private void StartVehicleGenerator()
    {
        var timer = new Timer();
        timer.Interval = 1;
        var vehicleGenerator = new VehicleGenerator(_trafficSim, _road1, _road2);
        timer.Timeout += delegate { vehicleGenerator.Generate(); };
        timer.Start();
    }
    

    private static void Add(PhysicsObject obj, TrafficSim parent, int level = 0)
    {
        parent.Add(obj, level);
    }

    private static void Add(Road road1, Road road2, TrafficSim parent, int level = 0)
    {
        parent.Add(road1, level);
        parent.Add(road2, level);
    }

    private static void Add(Background background1, Background background2, TrafficSim parent, int level=0)
    {
        parent.Add(background1, level);
        parent.Add(background2, level);
    }

    private void AddHandlers(TrafficSim parent)
    {
        parent.AddCollisionHandler(_roadLowerBorder, _road1, _road1.MoveRoad);
        parent.AddCollisionHandler(_roadLowerBorder, _road2, _road2.MoveRoad);
        parent.AddCollisionHandler(_backgroundLowerBorder, _background1, _background1.MoveBackground);
        parent.AddCollisionHandler(_backgroundLowerBorder, _background2, _background2.MoveBackground);
    }
    
    /// <summary>
    /// Creates a lower border for the Road objects to bump in to
    /// </summary>
    /// <param name="posY"></param>
    /// <returns></returns>
    private static PhysicsObject CreateLowerBorder(double posY)
    {
        var border = new PhysicsObject(5000, 100);
        border.Top = posY;
        border.IgnoresCollisionResponse = true;
        border.IgnoresGravity = true;
        return border;
    }
    
    /// <summary>
    /// Creates borders to the road and adds to the level
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    /// <param name="parent"></param>
    private void CreateRoadBorders(double width, double height, Color color, TrafficSim parent)
    {
        _borderLeft = new PhysicsObject(width, height);
        _borderLeft.MakeStatic();
        _borderLeft.Right = _road1.Left;
        _borderLeft.Color = color;
        Add(_borderLeft, parent);
        _borderRight = new PhysicsObject(width, height);
        _borderRight.MakeStatic();
        _borderRight.Left = _road1.Right;
        _borderRight.Color = color;
        Add(_borderRight, parent);
    }
    
    /// <summary>
    /// Groups driving methods together
    /// </summary>
    public void Drive()
    {
        const double drivingForce = 1000;
        const double backgroundRatio = 5;
        _road1.SimulateDriving(drivingForce);
        _road2.SimulateDriving(drivingForce);
        _background1.SimulateDriving(drivingForce/backgroundRatio);
        _background2.SimulateDriving(drivingForce/backgroundRatio);
    }
    
    /// <summary>
    /// Groups braking methods together
    /// </summary>
    public void Brake()
    {
        if (GetAbsVelocity() < 500) return;
        _road1.SimulateBraking(5000);
        _road2.SimulateBraking(5000);
        _background1.SimulateBraking();
        _background2.SimulateBraking();
    }

    /// <summary>
    /// Get the perceived velocity of the player's car 
    /// </summary>
    /// <returns></returns>
    public double GetAbsVelocity()
    {
        return (Math.Abs(_road1.Velocity.Magnitude + _road2.Velocity.Magnitude)) / 2;
    }
    
    private void CreateSlider(TrafficSim parent)
    {
        var roadWidth = new  IntMeter(200, 1, 2000);
        roadWidth.Changed += ChangeRoadWidth;
        
    
        var roadSlider = new Slider(200, 20);
        roadSlider.Position = new Vector(-500, 500);
        roadSlider.BindTo(roadWidth);
        parent.Add(roadSlider);
    }

    private void ChangeRoadWidth(int oldValue, int newValue)
    {
        _road1.Width = newValue;
        _road2.Width = newValue;
        MoveBorders();
    }

    private void MoveBorders()
    {
        _borderLeft.Right = _road1.Left;
        _borderRight.Left = _road1.Right;
    }
    
}