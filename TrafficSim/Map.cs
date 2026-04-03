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
    
    
    private const double DrivingForce = 100; 

    private enum Side
    {
        Left,
        Right,
    }
    
    public Map(TrafficSim trafficSim, Progress progress)
    {
        Properties.MaxVelocity = 1500;
        Properties.BGMaxVelocity = 600;
        _trafficSim = trafficSim;
        _progress = progress;
        _road1 = CreateRoad(new Vector(0, 0));
        _road2 = CreateRoad(new Vector(0, _screenHeight));
        _borderLeft = CreateRoadBorder(Color.Silver, Side.Left);
        _borderRight = CreateRoadBorder(Color.Silver, Side.Right);
        _background1 = CreateBackground(new Vector(0, 0));
        _background2 = CreateBackground(new Vector(0, _screenHeight));
        StartVehicleGenerator();
    }

    private Road CreateRoad(Vector position)
    {
        const int roadWidth = 600;
        var road = new Road(roadWidth, _screenHeight * 2, TrafficSim.RoadTexture, Properties.MaxVelocity)
        {
            Position = position
        };
        _trafficSim.Add(road, -1);
        var lowerBorder = CreateLowerBorder(Game.Screen.Bottom - _screenHeight);
        _trafficSim.Add(lowerBorder, -1);
        _trafficSim.AddCollisionHandler(lowerBorder, road, road.Cycle);
        return road;
    }
    
    private PhysicsObject CreateRoadBorder(Color color, Side side)
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
        var background = new Background(_screenWidth, _screenHeight * 2, TrafficSim.DesertTexture, Properties.BGMaxVelocity)
        {
            Position = position
        };

        _trafficSim.Add(background, -2);
        var lowerBorder = CreateLowerBorder(Game.Screen.Bottom - _screenHeight);
        _trafficSim.Add(lowerBorder, -2);
        
        _trafficSim.AddCollisionHandler(lowerBorder, background, background.Cycle);
        return background;
    }

    private void StartVehicleGenerator()
    {
        var timer = new Timer
        {
            Interval = 1
        };
        var vehicleGenerator = new VehicleGenerator(_trafficSim, this, _road1, _road2);
        timer.Timeout += vehicleGenerator.Generate;
        timer.Start();
    }
    
    private static PhysicsObject CreateLowerBorder(double posY)
    {
        var border = new PhysicsObject(5000, 1)
        {
            Top = posY,
            IgnoresCollisionResponse = true,
            IgnoresGravity = true
        };
        return border;
    }
    
    public void Drive()
    {
        const double ratio = 2.5;
        var force = DrivingForce;
        _progress.Drive(GetVelocity()/100);
        _road1.Drive(force);
        _road2.Drive(force);
        _background1.Drive(force/ratio);
        _background2.Drive(force/ratio);
    }

    public void DriveIdle()
    {
        if (GetVelocity() > 300 && GetBGVelocity() > 200)
        {
            _road1.Brake(300);
            _road2.Brake(300);
            _background1.Brake(300 / 2.5);
            _background2.Brake(300 / 2.5);
        }
        _progress.Drive(GetVelocity());
    }

    public void Brake()
    {
        const double ratio = 2.5;
        double force = 1000;
        double velocity = GetVelocity();
        double bgVelocity = GetBGVelocity();

        if (velocity>300)
        {
            _road1.Brake(force);
            _road2.Brake(force);
        }

        if (bgVelocity > 200)
        {
            _background1.Brake(force/ratio);
            _background2.Brake(force/ratio);
        }
            
        _progress.Drive(GetVelocity());
    }

    public void Slow()
    {
        Timer timer = new()
        {
            Interval = 1
        };
        timer.Timeout += delegate { UnSlow(timer); };
        timer.Start();
       // Brake();
        _road1.MaxVelocity = 300;
        _road2.MaxVelocity = 300;
        _background1.MaxVelocity = 200;
        _background2.MaxVelocity = 200;
    }

    private void UnSlow(Timer timer)
    {
        _trafficSim.MessageDisplay.Add("unslow");
        timer.Stop();
        _road1.MaxVelocity = Properties.MaxVelocity;
        _road2.MaxVelocity = Properties.MaxVelocity;
        _background1.MaxVelocity = Properties.BGMaxVelocity;
        _background2.MaxVelocity = Properties.BGMaxVelocity;
    }
    
    public double GetVelocity()
    {
        return -_road1.Velocity.Y;
    }

    public double GetBGVelocity()
    {
        return _background1.Velocity.Y; 
    }

    public Road GetRoad(int road)
    {
        return road switch
        {
            0 => _road1,
            1 => _road2,
            _ => _road1,
        };
    }

    public PhysicsObject GetBorder(int border)
    {
        return border switch
        {
            0 => _borderLeft,
            1 => _borderRight,
            _ => _borderLeft,
        };
    }
}