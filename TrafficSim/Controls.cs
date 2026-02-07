using System.IO;

namespace TrafficSim;

public class Controls
{
    private Car _car;
    private RoadMap _RoadMap;
    

    public Controls(Car car,  RoadMap roadMap)
    {
        _car = car;
        _RoadMap = roadMap;
    }

    public void Drive()
    {
        _RoadMap.Drive();
    }

    public void Brake()
    {
        _RoadMap.Brake();
    }

    public void SteerLeft()
    {
        _car.SteerLeft();
    }

    public void SteerRight()
    {
        _car.SteerRight();
    }
}