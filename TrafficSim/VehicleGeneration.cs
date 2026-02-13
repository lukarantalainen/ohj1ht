using System;
using System.Collections.Generic;

namespace TrafficSim;
using Jypeli;
public class VehicleGenerator
{
    private readonly TrafficSim _trafficSim;
    private readonly Road _road1;
    private readonly Road _road2;
    private readonly List<PhysicsObject> _vehicles = new ();
    public VehicleGenerator(TrafficSim trafficSim, Road road1, Road road2)
    {
        _trafficSim = trafficSim;
        _road1 = road1;
        _road2 = road2;
    }
    public void Generate()
    {
        var lane1 = (_road1.Left+_road1.X)/2;
        var lane2 = (_road2.Right+_road2.X)/2;
        
        var vehicle = new Vehicle(50, 100);
        var lane =  RandomGen.NextInt(0, 2);
        vehicle.Position = (lane==0) ? new Vector(lane1, Game.Screen.Top+200) :  new Vector(lane2, Game.Screen.Top+200);
        _vehicles.Add(vehicle);
        
        foreach (var v in _vehicles)
        {
            v.Tag = "vehicle";
            v.MoveTo(new Vector(v.X, Game.Screen.Bottom-200), Math.Abs(_road1.Velocity.Y));
            if (v.Y < Game.Screen.Bottom)
            {
                v.Destroy();
            }
        }
        _trafficSim.Add(vehicle);
    }

}