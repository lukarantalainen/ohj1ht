using System;

namespace TrafficSim;
using Jypeli;
public class VehicleGenerator
{
    private readonly Road _parent;
    public VehicleGenerator(Road parent)
    {
        _parent = parent;
    }
    public void Generate()
    {
        var leftLaneX = (_parent.Left+_parent.X)/2;
        var rightLaneX = (_parent.Right+_parent.X)/2;
        
        var vehicle = new Vehicle(50, 100);
        var lane =  RandomGen.NextInt(0, 2);
        vehicle.Position = (lane==0) ? new Vector(leftLaneX, _parent.Top) :  new Vector(rightLaneX, _parent.Top);
        _parent.Add(vehicle);
    }
}