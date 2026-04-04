using System;
using System.Collections.Generic;

namespace RacingGame;
using Jypeli;
using Silk.NET.Maths;

public class VehicleGenerator
{
    private readonly RacingGame game;
    private readonly Map map;
    private List<Vehicle> vehicles = [];

    private readonly double x1;
    private readonly double x2;
    private readonly double x3;
    private readonly double x4;
    public VehicleGenerator(RacingGame game, Map map, Road road)
    {
        this.game = game;
        this.map = map;

        var road0 = road.GetRoad(0);

        x1 = -225;
        x2 = -85;
        x3 = 85;
        x4 = 225;

    }
    public void Generate()
    {
        var vehiclesNew = new List<Vehicle>(vehicles);
        for (int i=0; i<vehicles.Count; i++)
        {
            if (vehicles[i].Y > Game.Screen.Bottom-200)
            {
                vehiclesNew.Add(vehicles[i]);
            }
            else
            {
                vehicles[i].Destroy();
            }
        }
        vehicles = vehiclesNew;

        if (map.GetVelocity() > 100)
        {
            var vehicleType = (VehicleType)RandomGen.NextInt(0, 2);
            var vehicle = new Vehicle(Properties.CarSize, Properties.CarSize, vehicleType);
            var lane =  RandomGen.NextInt(0, 4);
            switch(lane)
            {
                case 0:
                    vehicle.Direction = Direction.Opposite;
                    vehicle.X = x1;
                    vehicle.Angle = Angle.FromDegrees(180);
                    break;
                case 1:
                    vehicle.Direction = Direction.Opposite;
                    vehicle.X = x2;
                    vehicle.Angle = Angle.FromDegrees(180);
                    break;
                case 2:
                    vehicle.Direction = Direction.Same;
                    vehicle.X = x3;
                    break;
                case 3:
                    vehicle.Direction = Direction.Same;
                    vehicle.X = x4;
                    break;
            }
            vehicle.Y = Game.Screen.Top + 100;
            vehicles.Add(vehicle);
            game.Add(vehicle);
        }
        
        foreach (var v in vehicles)
        {
            if (v!=null)
            {
                v.Tag = "vehicle";
                v.MoveTo(new Vector(v.X, Game.Screen.Bottom - 1000), v.PushVelocity+map.GetVelocity());
            }
            
            
        }
    }
}