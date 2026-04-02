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
        //Image = TrafficSim.CarTexture;
        Color = Color.Blue;
        Shape = Shape.Rectangle;
    }

    private void CreateTruck()
    {
        Color = Color.Red;
        Shape = Shape.Rectangle;
        //base.Shape = TrafficSim.TruckShape;
    }
    private void CreateTaxi()
    {

    }

}

