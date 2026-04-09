using System;
using System.Collections.Generic;

namespace RacingGame;
using Jypeli;
using Jypeli.Assets;
using Silk.NET.Maths;

public class VehicleGenerator
{
    private readonly RacingGame game;
    private readonly Road road;

    private int previousLane = -1;

    public VehicleGenerator(RacingGame game, Road road)
    {
        this.game = game;
        this.road = road;

        
    }

    public static void Start(RacingGame game, Road road)
    {
        var generator = new VehicleGenerator(game, road);
        {
            var timer = new Timer(1);
            timer.Timeout += delegate { generator.Generate(timer);  };
            timer.Start();

            var lowerBorder = new PhysicsObject(Game.Screen.Width, 1)
            {
                Position = new Vector(0, Game.Screen.Bottom - 500),
            };
            game.Add(lowerBorder);

            game.AddCollisionHandler(lowerBorder, "vehicle", CollisionHandler.DestroyTarget);
        }
    }

    private static VehicleType GetRandomVehicle()
    {
        int number = RandomGen.NextInt(0, 20);

        if (number <= 1)
        {
            return VehicleType.Taxi;
        }
        else if (number > 1 && number <= 7)
        {
            return VehicleType.Truck;
        } else if (number > 7 && number < 20)
        {
            return VehicleType.Car;
        }

        return VehicleType.Car;
    }

    private void Generate(Timer timer)
    {
        timer.Interval = RandomGen.NextDouble(0.5, 1.5);
        int lane = RandomGen.NextInt(0, 4);
        while (previousLane == lane)
        {
            lane = RandomGen.NextInt(0, 4);
        }

        var vehicle = new Vehicle(Properties.CarSize, Properties.CarSize, GetRandomVehicle())
        {
            Position = new Vector(road.Lanes[lane], Game.Screen.Top + 200),
            Tag = "vehicle",
        };

        if (lane == 0 || lane == 1)
        {
            vehicle.MoveTo(new Vector(vehicle.X, Game.Screen.Bottom - 1000), vehicle.PushVelocity+road.GetVelocity());
            vehicle.Angle = Angle.FromDegrees(180);
        } else
        {
            vehicle.MoveTo(new Vector(vehicle.X, Game.Screen.Bottom - 1000), 200);
        }

        previousLane = lane;
        
        game.Add(vehicle);
    }
}