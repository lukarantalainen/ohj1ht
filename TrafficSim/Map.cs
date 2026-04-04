using Jypeli;
using System;

namespace TrafficSim;
using Jypeli.Widgets;

public class Map
{
    private readonly TrafficSim trafficSim;
    private readonly Road road;
    private readonly Background background;
    
    private const double DrivingForce = 500; 

    public Map(TrafficSim trafficSim)
    {
        Properties.MaxVelocity = 1500;
        Properties.BGMaxVelocity = 600;
        this.trafficSim = trafficSim;

        road = new Road(600, Game.Screen.Height * 2, TrafficSim.RoadTexture, trafficSim);

        background = new Background(Game.Screen.Width, Game.Screen.Height*2, TrafficSim.DesertTexture, trafficSim);

        StartVehicleGenerator();
    }

    private void StartVehicleGenerator()
    {
        var timer = new Timer
        {
            Interval = 1
        };
        var vehicleGenerator = new VehicleGenerator(trafficSim, this, road);
        timer.Timeout += vehicleGenerator.Generate;
        timer.Start();
    }

    private void UnSlow(Timer timer)
    {
        trafficSim.MessageDisplay.Add("unslow");
        timer.Stop();
        road.SetMaxVelocity(Properties.MaxVelocity);
        background.SetMaxVelocity(Properties.BGMaxVelocity);
    }
    
    public double GetVelocity()
    {
        return road.GetVelocity();
    }

    public double GetBGVelocity()
    {
        return background.GetVelocity(); 
    }

    public void Drive()
    {
        const double ratio = 2.5;
        var force = DrivingForce;
        road.Drive(force);
        background.Drive(force / ratio);
    }

    public void DriveIdle()
    {
        if (GetVelocity() > 300 && GetBGVelocity() > 200)
        {
            road.Brake(300);
            background.Brake(300 / 2.5);
        }
    }

    public void Brake()
    {
        const double ratio = 2.5;
        double force = 1000;
        double velocity = GetVelocity();
        double bgVelocity = GetBGVelocity();

        if (velocity > 300)
        {
            road.Brake(force);
        }

        if (bgVelocity > 200)
        {
            background.Brake(force / ratio);
        }
    }

    public void Slow()
    {
        Timer timer = new()
        {
            Interval = 1
        };
        timer.Timeout += delegate { UnSlow(timer); };
        timer.Start();
        road.SetMaxVelocity(300);
        background.SetMaxVelocity(200);
    }

    public PhysicsObject GetRoad(int num)
    {
        return road.GetRoad(num);
    }

    public PhysicsObject GetBorder(int num)
    {
        return road.GetBorder(num);
    }

}