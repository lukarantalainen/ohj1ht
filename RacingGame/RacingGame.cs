using Jypeli;
using Jypeli.Widgets;

namespace RacingGame;

/// @author lukar
/// @version 16.01.2026
/// <summary>
/// </summary>
public struct Properties
{
    public static double MaxVelocity { get; set; }
    public static double BgMaxVelocity { get; set; }
    public const double RoadWidth = 600;
    public const double RoadBorderWidth = 20;

    public const double RoadLength = 1000;

    public const double CarSize = 150;

    public const double TargetTime = RoadLength / 30;

    public const int PlayerHealth = 100;
    public const int DamageFromCar = 10;
}

public class RacingGame : PhysicsGame
{
    public static readonly Image PlayerImage = LoadImage("player-texture");

    public static readonly Image CarImage = LoadImage("car-texture");
    public static readonly Image CarImageGreen = LoadImage("sports-car-green");

    public static readonly Image CarShapeImage = LoadImage("rectangle-car-shape");
    public static readonly Shape CarShape = Shape.FromImage(CarShapeImage);

    public static readonly Image TaxiImage = LoadImage("taxi-texture");

    public static readonly Image RoadImage = LoadImage("road-texture");
    public static readonly Image DesertImage = LoadImage("desert-texture");

    public static readonly Image Finishline = LoadImage("finishline");

    public static readonly SoundEffect StartSound = LoadSoundEffect("start-countdown");
    //https://freesound.org/people/Pastew/sounds/813525/

    public static readonly SoundEffect Quack = LoadSoundEffect("quack");
    private Map map;
    private Player player;
    private Progress progress;

    //public static readonly Image _cactusTexture = LoadImage("cactus-texture");


    private ScoreList topList;
    private VehicleGenerator vehicleGenerator;

    public override void Begin()
    {
        //IsFullScreen = true;
        Init();
    }

    /// <summary>
    ///     Initializes the game
    /// </summary>
    private void Init()
    {
        ClearAll();
        CreateTopList();

        Keyboard.Listen(Key.R, ButtonState.Pressed, Init, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        map = new Map(this);
        player = new Player(this, map);
        progress = new Progress(this);
        _ = new Dashboard(this, player, progress);

        var startLabel = new Label(300, 100)
        {
            Text = "Press SPACE to begin!",
            Color = Color.Black,
            TextColor = Color.YellowGreen
        };
        Add(startLabel);

        Keyboard.Listen(Key.Space, ButtonState.Down, delegate
        {
            startLabel.Destroy();
            StartGame();
        }, "");

        //Debug.Start(this, player, map);
    }

    /// <summary>
    ///     Creates a toplist
    /// </summary>
    private void CreateTopList()
    {
        topList = DataStorage.TryLoad(topList, "scores.xml");
        topList = new ScoreList(10, true, Properties.TargetTime);
    }

    /// <summary>
    ///     Starts the countdown
    /// </summary>
    private void StartGame()
    {
        Keyboard.Clear();
        MessageDisplay.Clear();
        progress.Start();
    }

    /// <summary>
    ///     Shows the name input window
    /// </summary>
    /// <param name="time"></param>
    private void ShowTopList(double time)
    {
        var window = new HighScoreWindow(
            "Top List", $"Your time was {time:0.00}! Enter a name:",
            topList, time)
        {
            Color = Color.FromHexCode("fffdcc")
        };
        window.List.ScoreFormat = "{0:0.00}";
        window.Closed += delegate(Window sender) { SaveScores(sender, time); };
        Add(window);
    }

    /// <summary>
    ///     Saves the toplist scores to a file
    /// </summary>
    /// <param name="_"></param>
    /// <param name="time"></param>
    private void SaveScores(Window _, double time)
    {
        DataStorage.Save(topList, "scores.xml");
        CreateSelectionWindow(false, time);
    }

    /// <summary>
    ///     Creates the seletion window at the end of a game
    /// </summary>
    /// <param name="failed"></param>
    /// <param name="time"></param>
    private void CreateSelectionWindow(bool failed, double time)
    {
        if (failed)
        {
            string[] options = ["Restart", "Quit"];
            var endWindow = new MultiSelectWindow("Failed: you crashed too many times.", options);
            endWindow.AddItemHandler(0, Init);
            endWindow.AddItemHandler(1, Exit);
            Add(endWindow);
        }

        else
        {
            string[] options = ["Top List", "Restart", "Quit"];
            var endWindow = new MultiSelectWindow($"Finished in {time:0.00}!", options);
            endWindow.AddItemHandler(0, delegate { ShowTopList(time); });
            endWindow.AddItemHandler(1, Init);
            endWindow.AddItemHandler(2, Exit);
            Add(endWindow);
        }
    }

    /// <summary>
    ///     Adds all controls
    /// </summary>
    public void AddControls()
    {
        Keyboard.Clear();
        Keyboard.Listen(Key.R, ButtonState.Pressed, Init, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Controls.Start(this, player, map, progress);
    }

    /// <summary>
    ///     Starts the game
    /// </summary>
    public void Start()
    {
        AddControls();
        vehicleGenerator = new VehicleGenerator(this, map.GetRoad());
    }

    /// <summary>
    ///     Ends the game
    /// </summary>
    /// <param name="failed"></param>
    public void End(bool failed)
    {
        IsPaused = true;
        RemoveCollisionHandlers();

        var time = progress.StopTimer();
        if (time > Properties.TargetTime)
        {
            MessageDisplay.Clear();
            MessageDisplay.Add($"Too slow! Your time was {time:0.00}. Try again!");
        }

        CreateSelectionWindow(failed, time);
    }

    /// <summary>
    ///     Updates vehicle positions
    /// </summary>
    /// <param name="time"></param>
    protected override void Update(Time time)
    {
        var totalSeconds = time.SinceLastUpdate.TotalSeconds;
        if (PhysicsEnabled) Engine.Update(totalSeconds);

        base.Update(time);
        Joints.Update(time);
        vehicleGenerator?.Update();
    }
}