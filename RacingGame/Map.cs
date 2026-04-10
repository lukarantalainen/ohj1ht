using Jypeli;
using System;

namespace RacingGame;
using Jypeli.Widgets;

public class Map
{
    private readonly RacingGame game;
    private readonly Road road;
    private readonly Background background;
    
    private const double DrivingForce = 1500; 

    /// <summary>
    /// Initializes the map
    /// </summary>
    /// <param name="game"></param>
    public Map(RacingGame game)
    {
        Properties.MaxVelocity = 1500;
        Properties.BGMaxVelocity = 600;
        this.game = game;

        road = new Road(Properties.RoadWidth, Game.Screen.Height * 2, RacingGame.RoadImage, game);

        background = new Background(Game.Screen.Width, Game.Screen.Height * 2, RacingGame.DesertImage, game);
    }
    
    /// <summary>
    /// Get current velocity
    /// </summary>
    /// <returns></returns>
    public double GetVelocity()
    {
        return road.GetVelocity();
    }

    /// <summary>
    /// Get current background velocity (background velocity is lower than road velocity to create a better illusion)
    /// </summary>
    /// <returns></returns>
    public double GetBGVelocity()
    {
        return background.GetVelocity(); 
    }

    /// <summary>
    /// Get the road object
    /// </summary>
    /// <returns></returns>
    public Road GetRoad()
    {
        return road;
    }
    
    /// <summary>
    /// Handle driving
    /// </summary>
    public void Drive()
    {
        const double ratio = 2.5;
        var force = DrivingForce;
        road.Drive(force);
        background.Drive(force / ratio);
    }

    /// <summary>
    /// Handle braking
    /// </summary>
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

    /// <summary>
    /// Slow on collision
    /// </summary>
    public void Slow()
    {
        Timer timer = new()
        {
            Interval = 1
        };
        timer.Timeout += delegate { UnSlow(timer); };
        timer.Start();
        road.SetMaxVelocity(700);
        background.SetMaxVelocity(400);
    }

    /// <summary>
    /// Cancel slow effects
    /// </summary>
    /// <param name="timer"></param>
    private void UnSlow(Timer timer)
    {
        timer.Stop();
        road.SetMaxVelocity(Properties.MaxVelocity);
        background.SetMaxVelocity(Properties.BGMaxVelocity);
    }

    /// <summary>
    /// Get a specific road
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public PhysicsObject GetRoadPhysicsObject(int num)
    {
        return road.GetRoad(num);
    }

    /// <summary>
    /// Get a specific border
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public PhysicsObject GetBorder(int num)
    {
        return road.GetBorder(num);
    }

}