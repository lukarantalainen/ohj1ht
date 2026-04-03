using Jypeli;

namespace TrafficSim;

public enum VehicleType
{
    Car,
    Truck,
    Taxi,
}

public class Vehicle : PhysicsObject
{
    public double PushVelocity {  get; set; }
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
        Width = 100;
        Height = 100;
        Image = TrafficSim.CarTexture;
        PushVelocity = 1000;
        Color = Color.Blue;
    }

    private void CreateTruck()
    {
        Width = 100;
        Height = 100;
        Image = TrafficSim.CarTexture;
        PushVelocity = 800;
        Color = Color.Red;
    }
    private void CreateTaxi()
    {

    }

}

