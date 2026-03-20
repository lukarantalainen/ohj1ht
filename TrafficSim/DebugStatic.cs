namespace TrafficSim;
using Jypeli;
using Jypeli.Widgets;
public static class DebugStatic
{
    
    public static void CreateSlider(TrafficSim parent, Road road1, Road road2, PhysicsObject borderLeft, PhysicsObject borderRight)
    {
        var roadWidth = new IntMeter(200, 1, 2000);
        roadWidth.Changed += delegate(int oldVal, int newVal) {ChangeRoadWidth(oldVal, newVal, road1, road2, borderLeft, borderRight);};
        
    
        var roadSlider = new Slider(200, 20);
        roadSlider.Position = new Vector(-500, 500);
        roadSlider.BindTo(roadWidth);
        parent.Add(roadSlider);
    }

    private static void ChangeRoadWidth(int oldValue, int newValue, Road road1, Road road2, PhysicsObject borderLeft, PhysicsObject borderRight)
    {
        road1.Width = newValue;
        road2.Width = newValue;
        MoveBorders(road1, road2, borderLeft, borderRight);
    }

    private static void MoveBorders(Road road1, Road road2, PhysicsObject borderLeft, PhysicsObject borderRight)
    {
        borderLeft.Right = road1.Left;
        borderRight.Left = road1.Right;

    }
}