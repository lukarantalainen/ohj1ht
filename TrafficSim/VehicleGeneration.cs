using System;
using System.Collections.Generic;

namespace TrafficSim;
using Jypeli;
public class VehicleGenerator
{
    private readonly TrafficSim _trafficSim;
    private readonly Map _map;
    private readonly Road _road1;
    private readonly Road _road2;
    private readonly List<GameObject> _vehicles = [];

    private readonly double x1;
    private readonly double x2;
    public VehicleGenerator(TrafficSim trafficSim, Map map, Road road1, Road road2)
    {
        _trafficSim = trafficSim;
        _map = map;
        _road1 = road1;
        _road2 = road2;
        x1 = (_road1.Left+_road1.X)/2;
        x2 = (_road2.Right + _road2.X) / 2;
    }
    public void Generate()
    {
        foreach (var v in _vehicles)
        {
            if (v.Y < Game.Screen.Bottom-200)
            {
                v.Destroy();
            }
        }

        if (_map.GetVelocity() > 100)
        {
            var vehicle = new Vehicle(100, VehicleType.Truck);
            var lane =  RandomGen.NextInt(0, 2);
            vehicle.Position = (lane==0) ? new Vector(x1, Game.Screen.Top+200) : new Vector(x2, Game.Screen.Top+200);
            _vehicles.Add(vehicle);
            _trafficSim.Add(vehicle);
        }
        
        foreach (var v in _vehicles)
        {
            v.Tag = "vehicle";
            v.MoveTo(new Vector(v.X, Game.Screen.Bottom-1000), _map.GetVelocity());
            
        }
    }
}