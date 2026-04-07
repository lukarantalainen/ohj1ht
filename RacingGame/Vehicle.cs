using Jypeli;

namespace RacingGame;

public enum VehicleType
{
    Car,
    Truck,
    Taxi,
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
        Color = Color.Blue;
        PushVelocity = 500;


    }

    private void CreateTruck()
    {
        Color = Color.Red;
        PushVelocity = 300;
    }

    private void CreateTaxi()
    {

    }

}

