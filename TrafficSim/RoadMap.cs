using Jypeli;

namespace TrafficSim;
using Jypeli.Widgets;
public class RoadMap
{
    private readonly Road _road1;
    private readonly Road _road2;
    public RoadMap(Road road1, Road road2)
    {
        _road1 = road1;
        _road2 = road2;
        _road2.Y+=road1.Height;
    }
    
    public Slider CreateSlider()
    {
        DoubleMeter roadWidth = new  DoubleMeter(200, 1, 1000);
        roadWidth.Changed += ChangeRoadWidth;
    
        Slider roadSlider = new Slider(200, 20);
        roadSlider.BindTo(roadWidth);
        return roadSlider;
    }

    private void ChangeRoadWidth(double oldValue, double newValue)
    {
        _road1.Width = newValue;
        _road2.Width = newValue;
    }

    public void Drive()
    {
        _road1.SimulateDriving(5000);
        _road2.SimulateDriving(5000);
    }
    public void Brake()
    {
        _road1.SimulateBraking(5000);
        _road2.SimulateBraking(5000);
    }

    public double GetVelocity()
    {
        return (_road1.Velocity.Magnitude + _road2.Velocity.Magnitude) / 2;
    }
    
}