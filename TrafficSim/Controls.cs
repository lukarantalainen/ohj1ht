using System.IO;

namespace TrafficSim;

public class Controls
{
    private PlayerCar _car;
    private RoadMap _RoadMap;

    public Controls(PlayerCar car,  RoadMap roadMap)
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
        _car.SteerLeft(_RoadMap.GetVelocity());
    }

    public void SteerRight()
    {
        _car.SteerRight(_RoadMap.GetVelocity());
    }
}