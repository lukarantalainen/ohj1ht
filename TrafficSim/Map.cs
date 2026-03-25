using Jypeli;
using System;

namespace TrafficSim;
using Jypeli.Widgets;

public class Map
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
    private double _drivingForce = 1000; 
    
    private readonly Image _roadTexture = Game.LoadImage("road_texture");
    private readonly Image _desertTexture = Game.LoadImage("desert_texture");
    //private readonly Image _cactusTexture = Game.LoadImage("cactus_texture");

    private enum Side
    {
        Left,
        Right,
    }
    
    public Map(TrafficSim trafficSim, Progress progress)
    {
        _trafficSim = trafficSim;
        _progress = progress;
        _road1 = CreateRoad(new Vector(0, 0));
        _road2 = CreateRoad(new Vector(0, _screenHeight));
        _borderLeft = CreateRoadBorder(_road1, Color.Silver, Side.Left);
        _borderRight = CreateRoadBorder(_road1, Color.Silver, Side.Right);
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
        var lowerBorder = CreateLowerBorder(Game.Screen.Bottom - _screenHeight*1.5);
        _trafficSim.Add(lowerBorder, -1);
        _trafficSim.AddCollisionHandler(lowerBorder, road, road.Cycle);
        return road;
    }
    
    private PhysicsObject CreateRoadBorder(Road anchor, Color color, Side side)
    {
        const double borderWidth = 20;
        var border = new PhysicsObject(borderWidth, _screenHeight);
        border.MakeStatic();
        border.Right = _road1.Left;
        border.Color = color;
        switch (side)
        {
            case Side.Left:
                border.Right = _road1.Left;
                break;
            case Side.Right:
            border.Left = _road1.Right;
            break;
        }
        _trafficSim.Add(border);
        return border;
    }

    private Background CreateBackground(Vector position)
    {
        var background = new Background(_screenWidth, _screenHeight, _desertTexture, MaxVelocity);
        background.Position = position;
        
        _trafficSim.Add(background, -2);
        var lowerBorder = CreateLowerBorder(Game.Screen.Bottom - _screenHeight*1.5);
        _trafficSim.Add(lowerBorder, -2);
        
        _trafficSim.AddCollisionHandler(lowerBorder, background, background.Cycle);
        return background;
    }

    private void StartVehicleGenerator()
    {
        var timer = new Timer();
        timer.Interval = 1;
        var vehicleGenerator = new VehicleGenerator(_trafficSim, this, _road1, _road2);
        timer.Timeout += delegate { vehicleGenerator.Generate(); };
        timer.Start();
    }
    
    private static PhysicsObject CreateLowerBorder(double posY)
    {
        var border = new PhysicsObject(5000, 1);
        border.Top = posY;
        border.IgnoresCollisionResponse = true;
        border.IgnoresGravity = true;
        return border;
    }
    
    public void Drive()
    {
        const double ratio = 2.5;
        var force = _drivingForce;
        _progress.SimulateDriving(force);
        _road1.Drive(force);
        _road2.Drive(force);
        _background1.Drive(force/ratio);
        _background2.Drive(force/ratio);
    }

    public void DriveIdle()
    {
        _progress.SimulateDriving(GetVelocity());
    }

    public void Brake()
    {
        const double ratio = 2.5;
        double force = _drivingForce;
        _road1.Brake(force);
        _road2.Brake(force);
        _background1.Brake(force / ratio);
        _background2.Brake(force / ratio);
    }

    public void Slow()
    {
        _trafficSim.MessageDisplay.Add("slow");
        const double ratio = 3;
        _drivingForce /= ratio;
        Timer timer = new Timer();
        timer.Interval = 5;
        timer.Timeout += delegate { UnSlow(timer, ratio); };
        _road1.Hit(new Vector(0, GetVelocity()-300));
        _road2.Hit(new Vector(0, GetVelocity()-300));
    }

    private void UnSlow(Timer timer, double ratio)
    {
        timer.Stop();
        _drivingForce /= ratio;
    }
    
    public double GetVelocity()
    {
        return -_road1.Velocity.Y;
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