using Jypeli;
using Jypeli.Widgets;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using Jypeli.Assets;

namespace TrafficSim;

/// @author lukar
/// @version 16.01.2026
/// <summary>
/// 
/// </summary>
public class TrafficSim : PhysicsGame
{
    private Progress _progress;
    private Player _car;
    private Map _map;

    public static readonly Image CarTexture = Game.LoadImage("car_texture");
    public static readonly Shape CarShape = Shape.FromImage(CarTexture);
    public static readonly Image RoadTexture = Game.LoadImage("road_texture");

    public static readonly Image DesertTexture = Game.LoadImage("desert_texture");
    //public static readonly Image _cactusTexture = Game.LoadImage("cactus_texture");

    private ScoreList _topList;
    
    public override void Begin()
    {
        //IsFullScreen = true;
        Init();
    }
    
    private void Init()
    {
        ClearAll();
        _topList = new ScoreList(10, true, 30);
        _topList = DataStorage.TryLoad<ScoreList>(_topList, "scores.xml");
        Keyboard.Listen(Key.R, ButtonState.Pressed, Init, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        _progress = new Progress(this);
        _map = new Map(this, _progress);
        _car = new Player(this, _map);
        
        Debug.Start(this, _car, _map);
        MessageDisplay.Add("Press SPACE to begin!");
        Keyboard.Listen(Key.Space, ButtonState.Down, _progress.StartGame, "");
        
    }

    public void AddControls()
    {
        Keyboard.Clear();
        Keyboard.Listen(Key.R, ButtonState.Pressed, Init, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Controls.Start(this,  _car,  _map); 
    }
    
    public void EndGame(PhysicsObject a, PhysicsObject b)
    {
        IsPaused = true;
        var elapsedTime = _progress.StopTimer();
        CreateSelectionWindow(elapsedTime);
        RemoveCollisionHandlers();
    }

    private void ShowTopList(double time)
    {
        var window = new HighScoreWindow(
            "Top List", "Your time was %p! Enter a name:",
            _topList, time);
        window.Closed += delegate (Window sender) { SaveScores(sender, time); };
        Add(window);

    }
    
    private void SaveScores(Window sender,double time)
    {
        DataStorage.Save<ScoreList>(_topList, "scores.xml");
        CreateSelectionWindow(time);
    }
    
    private void CreateSelectionWindow(double finishTime=0)
    {
        string[] options = ["Top List", "Restart", "Quit"];
        var endWindow = new MultiSelectWindow($"Finished in! {finishTime}", options);
        
        endWindow.AddItemHandler(0, delegate { ShowTopList(finishTime); });
        endWindow.AddItemHandler(1, Init);
        endWindow.AddItemHandler(2, Exit);
        
        Add(endWindow);
    }
    
    
    
}