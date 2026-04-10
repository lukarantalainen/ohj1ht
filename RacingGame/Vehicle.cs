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

    /// <summary>
    /// Initialize a vehicle
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="type"></param>
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
                CreateSuperCar(); break;
            case VehicleType.Taxi:
                CreateTaxi();  break;
            default:
                CreateCar(); break;
        }
    }

    /// <summary>
    /// Create a car
    /// </summary>
    private void CreateCar()
    {
        Image = RacingGame.CarImageGreen;
        Shape = RacingGame.CarShape;
        PushVelocity = 500;


    }

    /// <summary>
    /// Create a supercar
    /// </summary>
    private void CreateSuperCar()
    {
        Image = RacingGame.PlayerImage;
        Shape = RacingGame.CarShape;
        PushVelocity = 300;
    }

    /// <summary>
    /// Create a taxi
    /// </summary>
    private void CreateTaxi()
    {
        Image = RacingGame.TaxiImage;
        Shape = RacingGame.CarShape;
        PushVelocity = 500;
    }

}

