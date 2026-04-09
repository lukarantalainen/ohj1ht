using System;
using System.Collections.Generic;

namespace RacingGame;
using Jypeli;
using Jypeli.Assets;
using Silk.NET.Maths;
using System.Reflection.Emit;

public class VehicleGenerator
{
    private readonly RacingGame game;
    private readonly Road road;

    private int previousLane = -1;

    private readonly List<Vehicle> vehicles = new();

    public VehicleGenerator(RacingGame game, Road road)
    {
        this.game = game;
        this.road = road;

        var timer = new Timer(1);
        timer.Timeout += delegate { Generate(timer); };
        timer.Start();

        var lowerBorder = new PhysicsObject(Game.Screen.Width, 1)
        {
            Position = new Vector(0, Game.Screen.Bottom - 500),
        };

        var upperBorder = new PhysicsObject(Game.Screen.Width, 1)
        {
            Position = new Vector(0, Game.Screen.Top + 6000)
        };
        game.Add(lowerBorder);
        game.Add(upperBorder);

        game.AddCollisionHandler(lowerBorder, "vehicle", CollisionHandler.DestroyTarget);
        game.AddCollisionHandler(upperBorder, "vehicle", CollisionHandler.DestroyTarget);

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

    public void Update()
    {
        foreach (var vehicle in vehicles)
        {
            if (vehicle.Side == Side.Left)
            {
                vehicle.Push(new Vector(0, -(vehicle.Mass * vehicle.PushVelocity + vehicle.Mass * road.GetVelocity())));
            }
            else if (vehicle.Side == Side.Right)
            vehicle.Push(new Vector(0, vehicle.Mass*vehicle.PushVelocity-vehicle.Mass*road.GetVelocity()));   
        }
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
            CanRotate = false,
        };

        if (lane == 0 || lane == 1)
        {
            vehicle.Side = Side.Left;
            vehicle.Angle = Angle.FromDegrees(180);
        } else
        {
            vehicle.Side = Side.Right;
        }

        previousLane = lane;
        
        vehicles.Add(vehicle);
        game.Add(vehicle);
    }
}