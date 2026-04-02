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
    public Vehicle(double size, VehicleType type) : base(size, size)
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
        Image = TrafficSim.CarTexture;
    }

    private void CreateTruck()
    {
        Image = TrafficSim.TruckTexture;

        Angle = Angle.FromDegrees(-90);
    }
    private void CreateTaxi()
    {

    }

}

