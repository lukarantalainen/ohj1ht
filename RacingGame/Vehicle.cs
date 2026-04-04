using Jypeli;

namespace RacingGame;

public enum VehicleType
{
    Car,
    Truck,
    Taxi,
}

public enum Direction
{
    Same,
    Opposite,
}

public class Vehicle : PhysicsObject
{
    public double PushVelocity {  get; set; }
    public Direction Direction { get; set; }
    public Vehicle(double width, double height, VehicleType type) : base(width, height)
    {
        IgnoresCollisionResponse = true;
        IgnoresGravity = true;
        IgnoresPhysicsLogics = true;
        switch (type)
        {
            case (VehicleType.Car):
                CreateCar(); break;
            case (VehicleType.Truck):
                CreateTruck(); break;
            case VehicleType.Taxi:
                break;
            default:
                CreateCar(); break;
        }
    }

    private void CreateCar()
    {
        Image = RacingGame.CarImageGreen;
        if (Direction==Direction.Opposite)
        {
            PushVelocity = 1000;
        }
        else
        {
            PushVelocity = -200;
        }

    }

    private void CreateTruck()
    {
        Image = RacingGame.CarImage;

        if (Direction == Direction.Opposite)
        {
            PushVelocity = 800;
        }
        else
        {
            PushVelocity = -100;
        }
        Color = Color.Red;
    }
    private void CreateTaxi()
    {

    }

}

