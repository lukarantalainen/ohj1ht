
using System;
using System.Collections;
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

    private double _debugDisplayPosY = Game.Screen.Top -100;
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
        trafficSim.DebugEnabled = true;
        var debug = new Debug(trafficSim, player, roadMap);
        debug.Init();
    }
    
    private void Init()
    {
        CreateZoomSlider();
        CreateRoadWidthSlider();
        CreatePlayerPosition();
        CreateSpeedOMeter();
    }
    
    private void AddToScreen(Slider slider, Label label)
    {
        slider.Left = Game.Screen.Left + 20;
        slider.Y = _debugDisplayPosY;
        slider.Color = RandomGen.NextColor();
        _debugDisplayPosY -= 100;
        _trafficSim.Add(slider);
        label.Position = new Vector(slider.X, slider.Y+50);
        _trafficSim.Add(label);
    }

    private void AddToScreen(Label display, Label label)
    {
        display.Position = new Vector(Game.Screen.Left + 100, _debugDisplayPosY);
        display.Color = RandomGen.NextColor();
        _debugDisplayPosY -= 100;
        _trafficSim.Add(display);
        label.Position = new Vector(display.X, display.Y+50);
        _trafficSim.Add(label);
    }

    private void CreateZoomSlider()
    {
        var zoomMeter = new  DoubleMeter(0, -0.99, 1);
        zoomMeter.Changed += ZoomLevel;

        var zoomSlider = new Slider(200, 20);
        zoomSlider.BindTo(zoomMeter);
        AddToScreen(zoomSlider, new Label("Zoom slider"));
    }

    private void ZoomLevel(double oldValue, double newValue)
    {
        _trafficSim.Camera.ZoomFactor = 1+newValue;
    }
    
    private void CreateSpeedOMeter()
    {
        DoubleMeter meter = new DoubleMeter(0);
        var timer = new Timer();
        timer.Interval = 0.5;
        timer.Timeout += delegate {UpdateSpeedOMeter(meter);};
        timer.Start();
        Label label = new Label();
        label.BindTo(meter);
        AddToScreen(label, new  Label("Speed"));
    }
    
    private void UpdateSpeedOMeter(DoubleMeter meter)
    {
        meter.Value = _roadMap.GetAbsVelocity();

    }

    private void CreatePlayerPosition()
    {
        var label = new Label();
        label.Text = "Player Position";
        label.Position = new Vector(-300, 100);
        AddToScreen(label, new Label("Player Position"));
        
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
        AddToScreen(roadSlider, new Label("Road width"));
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