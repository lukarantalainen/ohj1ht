using Jypeli;
using System;

namespace TrafficSim;
using Jypeli.Widgets;
public class RoadMap
{
    private readonly Road _road1;
    private readonly Road _road2;
    private readonly Background _background1;
    private readonly Background _background2;
    private PhysicsObject borderLeft;
    private PhysicsObject borderRight;
    public RoadMap(Road road1, Road road2, Background background1, Background background2)
    {
        _road1 = road1;
        _road2 = road2;
        _road2.Y+=road1.Height;
        _background1 = background1;
        _background2 = background2;
        _background2.Y+=background1.Height;
    }
    
    

    public void Drive()
    {
        _road1.SimulateDriving(5000);
        _road2.SimulateDriving(5000);
        _background1.SimulateDriving();
        _background2.SimulateDriving();
    }
    public void Brake()
    {
        if (GetAbsVelocity() < 500) return;
        _road1.SimulateBraking(5000);
        _road2.SimulateBraking(5000);
        _background1.SimulateBraking();
        _background2.SimulateBraking();
    }

    public double GetAbsVelocity()
    {
        return (Math.Abs(_road1.Velocity.Magnitude + _road2.Velocity.Magnitude)) / 2;
    }
    
    public Slider CreateSlider()
    {
        IntMeter roadWidth = new  IntMeter(200, 1, 2000);
        roadWidth.Changed += ChangeRoadWidth;
        
    
        Slider roadSlider = new Slider(200, 20);
        roadSlider.Position = new Vector(-500, 500);
        roadSlider.BindTo(roadWidth);
        return roadSlider;
    }

    private void ChangeRoadWidth(int oldValue, int newValue)
    {
        _road1.Width = newValue;
        _road2.Width = newValue;
        MoveBorders();
    }

    public PhysicsObject[] CreateRoadBorders(double width, double height, Color color)
    {
        borderLeft = new PhysicsObject(width, height);
        borderLeft.MakeStatic();
        borderLeft.Right = _road1.Left;
        borderLeft.Color = color;
        borderRight = new PhysicsObject(width, height);
        borderRight.MakeStatic();
        borderRight.Left = _road1.Right;
        borderRight.Color = color;
        return [borderRight, borderLeft];
    }

    private void MoveBorders()
    {
        borderLeft.Right = _road1.Left;
        borderRight.Left = _road1.Right;
    }
    
}