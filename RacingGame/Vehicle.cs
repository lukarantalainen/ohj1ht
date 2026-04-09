using Jypeli;

namespace RacingGame;

public enum VehicleType
{
    Car,
    Truck,
    Taxi,
}

public enum Side
{
    Left,
    Right,
}

public class Vehicle : PhysicsObject
{
    public double PushVelocity {  get; set; }
    public Side Side { get; set; }
    public Vehicle(double width, double height, VehicleType type) : base(width, height)
    {
        IgnoresCollisionResponse = true;
        IgnoresGravity = true;
        IgnoresPhysicsLogics = true;
        LinearDamping = 0.9;
        Mass = 1000;

        switch (type)
        {
            case (VehicleType.Car):
                CreateCar(); break;
            case (VehicleType.Truck):
                CreateTruck(); break;
            case VehicleType.Taxi:
                CreateTaxi();  break;
            default:
                CreateCar(); break;
        }
    }

    private void CreateCar()
    {
        Image = RacingGame.CarImageGreen;
        Shape = RacingGame.CarShape;
        PushVelocity = 500;


    }

    private void CreateTruck()
    {
        Image = RacingGame.PlayerImage;
        Shape = RacingGame.CarShape;
        PushVelocity = 300;
    }

    private void CreateTaxi()
    {
        Image = RacingGame.TaxiImage;
        Shape = RacingGame.CarShape;
        PushVelocity = 500;
    }

}

