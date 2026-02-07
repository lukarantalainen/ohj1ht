using Jypeli;

namespace TrafficSim;

public class Vehicle : PhysicsObject
{
    private new const int Width = 200;
    private new const int Height = 200;

    
    public enum VehicleType
    {
        Car,
        Truck,
        Taxi,
        
    }

    public Vehicle() : base(Width, Height)
    {
        
    }
    public Vehicle(double width, double height, double topSpeed, Color color) : base(width, height)
    {
        
    }

    public Vehicle(double width, double height, double topSpeed, Image texture, Shape shape) : base(width, height,
        shape)
    {
        Image = texture;
    }
}

public class Taxi : Vehicle
{
    public Taxi() : base(200, 200, 200, Color.Yellow)
    {
    }
}

public class Truck :  Vehicle
{
    public Truck() : base(200, 300, 120, Color.Red)
    {
    }
}

public class Car : Vehicle
{
    public Car(double width, double height, double topSpeed, Image texture, Shape shape) : base(200, 200, 200, texture,
        shape)
    {
        
    }
}