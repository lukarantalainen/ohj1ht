
namespace TrafficSim;
using Jypeli;
using Jypeli.Widgets;
public class Debug
{
    private TrafficSim _parent; 
    private Debug(TrafficSim parent)
    {
        _parent = parent;
    }

    public static void Start(TrafficSim parent)
    {
        var debug = new Debug(parent);
        debug.CreateZoomSlider();

    }

    private void CreateZoomSlider()
    {
        var zoomMeter = new  DoubleMeter(0, -0.99, 1);
        zoomMeter.Changed += ZoomLevel;

        var zoomSlider = new Slider(200, 20);
        zoomSlider.BindTo(zoomMeter);
        _parent.Add(zoomSlider);
    }

    private void ZoomLevel(double oldValue, double newValue)
    {
        _parent.Camera.ZoomFactor = 1+newValue;
    }
}