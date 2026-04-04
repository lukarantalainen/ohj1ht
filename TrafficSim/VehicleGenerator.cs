using System;
using System.Collections.Generic;

namespace TrafficSim;
using Jypeli;
using Silk.NET.Maths;

public class VehicleGenerator
{
    private readonly TrafficSim _trafficSim;
    private readonly Map _map;
    private List<Vehicle> _vehicles = [];

    private readonly double x1;
    private readonly double x2;
    private readonly double x3;
    private readonly double x4;
    public VehicleGenerator(TrafficSim trafficSim, Map map, Road road)
    {
        _trafficSim = trafficSim;
        _map = map;

        var road0 = road.GetRoad(0);

        double laneWidth = road0.Width / 5.5;
        x1 = road0.Left+ laneWidth * 0.8;
        x2 = road0.Left + laneWidth*2;
        x3 = road0.Right - laneWidth*0.8;
        x4 = road0.Right - laneWidth*2;

    }
    public void Generate()
    {
        var vehiclesNew = new List<Vehicle>(_vehicles);
        for (int i=0; i<_vehicles.Count; i++)
        {
            if (_vehicles[i].Y > TrafficSim.Screen.Bottom-200)
            {
                vehiclesNew.Add(_vehicles[i]);
            }
            else
            {
                _vehicles[i].Destroy();
            }
        }
        _vehicles = vehiclesNew;

        if (_map.GetVelocity() > 100)
        {
            var vehicleType = (VehicleType)RandomGen.NextInt(0, 2);
            var vehicle = new Vehicle(100, 100, vehicleType);
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
            vehicle.Y = TrafficSim.Screen.Top + 100;
            _vehicles.Add(vehicle);
            _trafficSim.Add(vehicle);
        }
        
        foreach (var v in _vehicles)
        {
            if (v!=null)
            {
                v.Tag = "vehicle";
                v.MoveTo(new Vector(v.X, TrafficSim.Screen.Bottom - 1000), v.PushVelocity+_map.GetVelocity());
            }
            
            
        }
    }
}