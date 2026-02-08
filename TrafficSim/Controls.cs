namespace TrafficSim;

public class Controls
{
    private readonly PlayerCar _car;
    private readonly RoadMap _roadMap;

    public Controls(PlayerCar car,  RoadMap roadMap)
    {
        _car = car;
        _roadMap = roadMap;
    }

    public void Drive()
    {
        _roadMap.Drive();
    }

    public void Brake()
    {
        _roadMap.Brake();
    }

    public void SteerLeft()
    {
        _car.SteerLeft(_roadMap.GetAbsVelocity());
    }

    public void SteerRight()
    {
        _car.SteerRight(_roadMap.GetAbsVelocity());
    }
}