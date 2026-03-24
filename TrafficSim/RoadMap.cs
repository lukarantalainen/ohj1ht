using Jypeli;
using System;

namespace TrafficSim;
using Jypeli.Widgets;

/// <summary>
/// Contains the main road and background of the game 
/// </summary>
public class RoadMap
{
    private readonly TrafficSim _trafficSim;
    private readonly Progress _progress;
    
    private readonly Road _road2;
    private readonly Road _road1;
    
    private readonly Background _background1;
    private readonly Background _background2;
    
    private readonly PhysicsObject _borderLeft;
    private readonly PhysicsObject _borderRight;
    
    private readonly double _screenWidth = Game.Screen.Width;
    private readonly double _screenHeight = Game.Screen.Height;
    
    private const double MaxVelocity = 1500;
    private readonly Image _roadTexture = Game.LoadImage("road_texture");
    //private readonly Image _desertTexture = Game.LoadImage("desert_texture");
    //private readonly Image _cactusTexture = Game.LoadImage("cactus_texture");
    
    public RoadMap(TrafficSim trafficSim, Progress progress)
    {
        _trafficSim = trafficSim;
        _progress = progress;
        _road1 = CreateRoad(new Vector(0, 0));
        _road2 = CreateRoad(new Vector(0, _screenHeight));
        _borderLeft = CreateRoadBorder(_road1, Color.Silver, 'l');
        _borderRight = CreateRoadBorder(_road1, Color.Silver, 'r');
        _background1 = CreateBackground(new Vector(0, 0));
        _background2 = CreateBackground(new Vector(0, _screenHeight));
        InitializeMap();
    }

    private void InitializeMap()
    {
        StartVehicleGenerator();
    }

    private Road CreateRoad(Vector position)
    {
        const int roadWidth = 200;
        var road = new Road(roadWidth, _screenHeight*2, _roadTexture, MaxVelocity);
        road.Position = position;
        _trafficSim.Add(road, -1);
        PhysicsObject lowerBorder = CreateLowerBorder(Game.Screen.Bottom - _screenHeight);
        _trafficSim.Add(lowerBorder, -1);
        _trafficSim.AddCollisionHandler(lowerBorder, road, road.Cycle);
        return road;
    }
    
    private PhysicsObject CreateRoadBorder(Road anchor, Color color, char side)
    {
        double borderWidth = 20;
        double borderHeight = _screenHeight; 
        PhysicsObject border = new PhysicsObject(borderWidth, borderHeight);
        border.MakeStatic();
        border.Right = _road1.Left;
        border.Color = color;
        switch (side)
        {
            case 'l':
                border.Right = _road1.Left;
                break;
                case 'r':
                border.Left = _road1.Right;
                break;
        }
        _trafficSim.Add(border);
        return border;
    }

    private Background CreateBackground(Vector position)
    {
        Background background = new Background(_screenWidth, _screenHeight, Color.LightCyan, MaxVelocity);
        background.Position = position;
        
        _trafficSim.Add(background, -2);
        PhysicsObject lowerBorder = CreateLowerBorder(Game.Screen.Bottom - _screenHeight);
        _trafficSim.Add(lowerBorder, -2);
        
        _trafficSim.AddCollisionHandler(lowerBorder, background, background.Cycle);
        return background;
    }

    private void StartVehicleGenerator()
    {
        var timer = new Timer();
        timer.Interval = 1;
        var vehicleGenerator = new VehicleGenerator(_trafficSim, _road1, _road2);
        timer.Timeout += delegate { vehicleGenerator.Generate(GetAbsVelocity()); };
        timer.Start();
    }
    
    private static PhysicsObject CreateLowerBorder(double posY)
    {
        var border = new PhysicsObject(5000, 100);
        border.Top = posY;
        border.IgnoresCollisionResponse = true;
        border.IgnoresGravity = true;
        return border;
    }
    
    public void Drive()
    {
        const double drivingForce = 1000;
        const double backgroundRatio = 5;
        _progress.SimulateDriving(drivingForce);
        _road1.Drive(drivingForce);
        _road2.Drive(drivingForce);
        _background1.Drive(drivingForce/backgroundRatio);
        _background2.Drive(drivingForce/backgroundRatio);

    }

    public void Brake()
    {
        if (GetAbsVelocity() < 500) return;
        _road1.Brake(5000);
        _road2.Brake(5000);
        _background1.Brake();
        _background2.Brake();
    }
    
    public double GetAbsVelocity()
    {
        return Math.Abs(_road1.Velocity.Magnitude + _road2.Velocity.Magnitude) / 2;
    }

    public Road GetRoad(int road)
    {
        switch (road)
        {
            case 0: return _road1;
            case 1: return _road2;
            default: return _road1;
        }
    }

    public PhysicsObject GetBorder(int border)
    {
        switch (border)
        {
            case 0: return _borderLeft;
            case 1: return _borderRight;
            default: return _borderLeft;
        }
    }
}