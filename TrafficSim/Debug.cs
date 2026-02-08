namespace TrafficSim;
using Jypeli;
using Jypeli.Widgets;
public class Debug
{
    private readonly TrafficSim _parent; 
    public Debug(TrafficSim parent)
    {
        _parent = parent;
        CreateZoomSlider();
    }

    private void CreateZoomSlider()
    {
        var zoomMeter = new  DoubleMeter(0, -5, 5);
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