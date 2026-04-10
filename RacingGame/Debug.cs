
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

    /// <summary>
    /// Gets the objects needed for debugging
    /// </summary>
    /// <param name="game"></param>
    /// <param name="player"></param>
    /// <param name="map"></param>
    private Debug(RacingGame game, Player player, Map map)
    {
        this.game = game;
        this.player = player;
        this.map = map;
        road = this.map.GetRoadPhysicsObject(0);
        road2 = this.map.GetRoadPhysicsObject(1);
        borderLeft = this.map.GetBorder(0);
        borderRight = this.map.GetBorder(1);

        game.Mouse.Listen(MouseButton.Left, ButtonState.Pressed, ShowClickPosition, "");
    }

    /// <summary>
    /// Shows where you clicked
    /// </summary>
    private void ShowClickPosition()
    {
        game.MessageDisplay.Add(game.Mouse.PositionOnWorld.ToString());
    }
    
    /// <summary>
    /// Starts the debug display
    /// </summary>
    /// <param name="game"></param>
    /// <param name="player"></param>
    /// <param name="map"></param>
    public static void Start(RacingGame game, Player player, Map map)
    {
        var debug = new Debug(game, player, map);
        debug.CreateZoomSlider();
        debug.CreatePlayerPosition();
        debug.CreateSpeedOMeter();
    }
    
    /// <summary>
    /// Add a slider to the debug display
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="label"></param>
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

    /// <summary>
    /// Add a label to the debug display
    /// </summary>
    /// <param name="display"></param>
    /// <param name="label"></param>
    private void AddToScreen(Label display, Label label)
    {
        display.Position = new Vector(Game.Screen.Left + 100, Y);
        display.Color = RandomGen.NextColor();
        Y -= 100;
        game.Add(display);
        label.Position = new Vector(display.X, display.Y+50);
        game.Add(label);
    }

    /// <summary>
    /// Create a slider for zooming the map
    /// </summary>
    private void CreateZoomSlider()
    {
        var zoomMeter = new  DoubleMeter(0, -0.99, 1);
        zoomMeter.Changed += ZoomLevel;

        var zoomSlider = new Slider(200, 20);
        zoomSlider.BindTo(zoomMeter);
        AddToScreen(zoomSlider, new Label("Zoom slider"));
    }

    /// <summary>
    /// Change camera zoom level
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private void ZoomLevel(double oldValue, double newValue)
    {
        game.Camera.ZoomFactor = 1+newValue;
    }
    
    /// <summary>
    /// Create a speedometer
    /// </summary>
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
    
    /// <summary>
    /// Update the speedometer
    /// </summary>
    /// <param name="meter"></param>
    private void UpdateSpeedOMeter(DoubleMeter meter)
    {
        meter.Value = map.GetVelocity();

    }

    /// <summary>
    /// Show player position
    /// </summary>
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

    /// <summary>
    /// Update player position
    /// </summary>
    /// <param name="label"></param>
    private void UpdatePos(Label label)
    {
        label.Text = player.Position.ToString();
    }
    

}