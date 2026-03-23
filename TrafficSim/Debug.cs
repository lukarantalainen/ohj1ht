
using System;

namespace TrafficSim;
using Jypeli;
using Jypeli.Widgets;
public class Debug
{
    private readonly TrafficSim _trafficSim; 
    private readonly Player _player;
    private readonly RoadMap _roadMap;
    private readonly Road _road1;
    private readonly Road _road2;
    private readonly PhysicsObject _borderLeft;
    private readonly PhysicsObject _borderRight;

    private Debug(TrafficSim trafficSim, Player player,  RoadMap roadMap)
    {
        _trafficSim = trafficSim;
        _player = player;
        _roadMap = roadMap;
        _road1 = _roadMap.GetRoad(0);
        _road2 = _roadMap.GetRoad(1);
        _borderLeft = _roadMap.GetBorder(0);
        _borderRight = _roadMap.GetBorder(1);
    }

    public static void Start(TrafficSim trafficSim, Player player, RoadMap roadMap)
    {
        var debug = new Debug(trafficSim, player, roadMap);
        debug.Init();
    }

    private void Init()
    {
        CreateZoomSlider();
        CreateRoadWidthSlider();
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
    
    private void CreateRoadWidthSlider()
    {
        var roadWidth = new IntMeter(200, 1, 2000);
        roadWidth.Changed += ChangeRoadWidth;
        
    
        var roadSlider = new Slider(200, 20);
        roadSlider.Position = new Vector(-500, 500);
        roadSlider.BindTo(roadWidth);
        _trafficSim.Add(roadSlider);
    }

    private void ChangeRoadWidth(int oldValue, int newValue)
    {
        _road1.Width = newValue;
        _road2.Width = newValue;
        MoveBorders();
    }

    private void MoveBorders()
    {
        _borderLeft.Right = _road1.Left;
        _borderRight.Left = _road1.Right;

    }
}