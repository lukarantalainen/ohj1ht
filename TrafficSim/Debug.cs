
using System;

namespace TrafficSim;
using Jypeli;
using Jypeli.Widgets;
public class Debug
{
    private TrafficSim _trafficSim; 
    private Player _player;
    private Debug(TrafficSim trafficSim,  Player player)
    {
        _player = player;
        _trafficSim = trafficSim;
    }

    public static void Start(TrafficSim trafficSim, Player player)
    {
        var debug = new Debug(trafficSim, player);
        debug.Init();

    }

    private void Init()
    {
        CreateZoomSlider();
        CreatePlayerPosition();
    }

    private void CreateZoomSlider()
    {
        var zoomMeter = new  DoubleMeter(0, -0.99, 1);
        zoomMeter.Changed += ZoomLevel;

        var zoomSlider = new Slider(200, 20);
        zoomSlider.BindTo(zoomMeter);
        _trafficSim.Add(zoomSlider);
    }

    private void ZoomLevel(double oldValue, double newValue)
    {
        _trafficSim.Camera.ZoomFactor = 1+newValue;
    }

    private void CreatePlayerPosition()
    {
        var label = new Label();
        label.Color = Color.Green;
        label.Text = "Player Position";
        label.Position = new Vector(-300, 100);
        _trafficSim.Add(label, 2);
        
        var timer = new  Timer();
        timer.Interval = 0.5;
        timer.Timeout += delegate { UpdatePos(label); };
        timer.Start();
    }

    private void UpdatePos(Label label)
    {
        label.Text = _player.Position.ToString();
    }
}