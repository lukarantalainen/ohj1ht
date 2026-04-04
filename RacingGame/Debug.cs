
using System;
using System.Collections;
namespace RacingGame;
using Jypeli;
using Jypeli.Widgets;
public class Debug
{
    private readonly RacingGame game; 
    private readonly Player player;
    private readonly Map map;
    private readonly PhysicsObject road;
    private readonly PhysicsObject road2;
    private readonly PhysicsObject borderLeft;
    private readonly PhysicsObject borderRight;

    private double Y = Game.Screen.Top - 100;
    private Debug(RacingGame game, Player player, Map map)
    {
        this.game = game;
        this.player = player;
        this.map = map;
        road = this.map.GetRoad(0);
        road2 = this.map.GetRoad(1);
        borderLeft = this.map.GetBorder(0);
        borderRight = this.map.GetBorder(1);

        game.Mouse.Listen(MouseButton.Left, ButtonState.Pressed, ShowClickPosition, "");
    }

    private void ShowClickPosition()
    {
        game.MessageDisplay.Add(game.Mouse.PositionOnWorld.ToString());
    }
    
    public static void Start(RacingGame game, Player player, Map map)
    {
        var debug = new Debug(game, player, map);
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
        slider.Y = Y;
        slider.Color = RandomGen.NextColor();
        Y -= 100;
        game.Add(slider);
        label.Position = new Vector(slider.X, slider.Y+50);
        game.Add(label);
    }

    private void AddToScreen(Label display, Label label)
    {
        display.Position = new Vector(Game.Screen.Left + 100, Y);
        display.Color = RandomGen.NextColor();
        Y -= 100;
        game.Add(display);
        label.Position = new Vector(display.X, display.Y+50);
        game.Add(label);
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
        game.Camera.ZoomFactor = 1+newValue;
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
        meter.Value = map.GetVelocity();

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
        label.Text = player.Position.ToString();
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
        road.Width = newValue;
        road2.Width = newValue;
        MoveBorders();
    }

    private void MoveBorders()
    {
        borderLeft.Right = road.Left;
        borderRight.Left = road.Right;

    }
}