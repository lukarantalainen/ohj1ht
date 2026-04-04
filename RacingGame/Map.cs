using Jypeli;
using System;

namespace RacingGame;
using Jypeli.Widgets;

public class Map
{
    private readonly RacingGame game;
    private readonly Road road;
    private readonly Background background;
    
    private const double DrivingForce = 500; 

    public Map(RacingGame game)
    {
        Properties.MaxVelocity = 1500;
        Properties.BGMaxVelocity = 600;
        this.game = game;

        road = new Road(Properties.RoadWidth, Game.Screen.Height * 2, RacingGame.RoadImage, game);

        background = new Background(Game.Screen.Width, Game.Screen.Height*2, RacingGame.DesertImage, game);

        StartVehicleGenerator();
    }

    private void StartVehicleGenerator()
    {
        var timer = new Timer
        {
            Interval = 1
        };
        var vehicleGenerator = new VehicleGenerator(game, this, road);
        timer.Timeout += vehicleGenerator.Generate;
        timer.Start();
    }

    private void UnSlow(Timer timer)
    {
        game.MessageDisplay.Add("unslow");
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