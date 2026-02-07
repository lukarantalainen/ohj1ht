namespace TrafficSim;
using Jypeli;
public class RoadMap
{
    private Road _road1;
    private Road _road2;
    public RoadMap(Road road1, Road road2)
    {
        _road1 = road1;
        _road2 = road2;
        _road2.X = road1.Width;
        _road2.Y = road1.Height;

    }

    public void Drive()
    {
        _road1.SimulateDriving(500);
        _road2.SimulateDriving(500);
    }
    public void Brake()
    {
        _road1.SimulateBraking(5000);
        _road2.SimulateBraking(5000);
    }
    
}