using Jypeli;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RacingGame;

/// @author lukar
/// @version 16.01.2026
/// <summary>
/// 
/// </summary>
/// 

public struct Properties
{
    public static double MaxVelocity { get; set; }
    public static double BGMaxVelocity { get; set; }
    public const double RoadWidth = 600;
    public const double RoadBorderWidth = 20;

    public const double RoadLength = 1000;

    public const double CarSize = 150;

    public const double TargetTime = RoadLength / 30;

    public const int PlayerHealth = 100;
    public const int DamageFromCar = 10;

}

public struct CarColors
{
    public static Color red = Color.FromHexCode("ff000");

}

public class RacingGame : PhysicsGame
{
    private Progress progress;
    private Player player;
    private Map map;
    private VehicleGenerator vehicleGenerator;

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

    //public static readonly Image _cactusTexture = LoadImage("cactus-texture");


    private ScoreList topList;

    public override void Begin()
    {
        //IsFullScreen = true;
        Init();
    }

    private void Init()
    {
        ClearAll();
        CreateTopList();
        
        Keyboard.Listen(Key.R, ButtonState.Pressed, Init, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        map = new Map(this);
        player = new Player(this, map);
        progress = new Progress(this);
        var dashboard = new Dashboard(this, player, progress);



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

        Debug.Start(this, player, map);
    }

    private void CreateTopList()
    {
        topList = DataStorage.TryLoad<ScoreList>(topList, "scores.xml");
        topList = new ScoreList(10, true, Properties.TargetTime);
    }

    private void StartGame()
    {
        Keyboard.Clear();
        MessageDisplay.Clear();
        progress.Start();
    }

    
    

    private void ShowTopList(double time)
    {
        var window = new HighScoreWindow(
            "Top List", $"Your time was {time:0.00}! Enter a name:",
            topList, time)
        {
            Color = Color.FromHexCode("fffdcc")
        };
        window.List.ScoreFormat = "{0:0.00}";
        window.Closed += delegate (Window sender) { SaveScores(sender, time); };
        Add(window);
    }

    private void SaveScores(Window _, double time)
    {
        DataStorage.Save<ScoreList>(topList, "scores.xml");
        CreateSelectionWindow(false, time);
    }

    private void CreateSelectionWindow(bool failed, double time)
    {
        
        if (failed)
        {
            string[] options = ["Restart", "Quit"];
            var endWindow = new MultiSelectWindow($"Failed: you crashed too many times.", options);
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

    public void AddControls()
    {
        Keyboard.Clear();
        Keyboard.Listen(Key.R, ButtonState.Pressed, Init, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Controls.Start(this, player, map, progress);
    }

    public void Start()
    {
        AddControls();
        
        vehicleGenerator = new VehicleGenerator(this, map.GetRoad());
    }

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

    protected override void Update(Time time)
    {
        double totalSeconds = time.SinceLastUpdate.TotalSeconds;
        if (PhysicsEnabled)
        {
            Engine.Update(totalSeconds);
        }

        base.Update(time);
        Joints.Update(time);
        if (vehicleGenerator != null)
        {
            vehicleGenerator.Update();
        }
    }

}