using System;
using System.Collections.Generic;

namespace TrafficSim;
using Jypeli;
using Silk.NET.Maths;

public class VehicleGenerator
{
    private readonly TrafficSim _trafficSim;
    private readonly Map _map;
    private readonly Road _road1;
    private readonly Road _road2;
    private List<GameObject> _vehicles = [];

    private readonly double x1;
    private readonly double x2;
    private readonly double x3;
    private readonly double x4;
    public VehicleGenerator(TrafficSim trafficSim, Map map, Road road1, Road road2)
    {
        _trafficSim = trafficSim;
        _map = map;
        _road1 = road1;
        _road2 = road2;
        double laneWidth = _road1.Width / 5.5;
        x1 = _road1.Left+laneWidth*0.8;
        x2 = _road1.Left + laneWidth*2;
        x3 = _road1.Right - laneWidth*0.8;
        x4 = _road1.Right - laneWidth*2;

    }
    public void Generate()
    {
        var vehiclesNew = new List<GameObject>(_vehicles);
        for (int i=0; i<_vehicles.Count; i++)
        {
            if (_vehicles[i].Y > Game.Screen.Bottom-200)
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
            var vehicle = new Vehicle(100, 200, vehicleType);
            var lane =  RandomGen.NextInt(0, 4);
            switch(lane)
            {
                case 0:
                    vehicle.X = x1;
                    break;
                case 1:
                    vehicle.X = x2;
                    break;
                case 2:
                    vehicle.X = x3;
                    break;
                case 3:
                    vehicle.X = x4;
                    break;
            }
            vehicle.Y = Game.Screen.Top + 100;
            _vehicles.Add(vehicle);
            _trafficSim.Add(vehicle);
        }
        
        foreach (var v in _vehicles)
        {
            if (v!=null)
            {
                v.Tag = "vehicle";
                v.MoveTo(new Vector(v.X, Game.Screen.Bottom - 1000), _map.GetVelocity());
            }
            
            
        }
    }
}