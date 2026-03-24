using Jypeli;
using Jypeli.Widgets;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    private RoadMap _roadMap;
    private TopList _topList;
    public bool DebugEnabled = false;
    public override void Begin()
    {
        IsFullScreen = true;
        Init();
    }
    
    private void Init()
    {
        ClearAll();
        Keyboard.Listen(Key.R, ButtonState.Pressed, Init, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        _progress = new Progress(this, 6000);
        _car = new Player(this);
        _roadMap = new RoadMap(this, _progress);

        _topList = new TopList(this);
        
        Debug.Start(this, _car, _roadMap);
        MessageDisplay.Add("Press SPACE to begin!");
        Keyboard.Listen(Key.Space, ButtonState.Down, _progress.StartGame, "");
        
    }

    public void AddControls()
    {
        Controls.Start(this,  _car,  _roadMap); 
    }
    
    public void EndGame(PhysicsObject a, PhysicsObject b)
    {
        IsPaused = true;
        var elapsedTime = _progress.StopTimer();
        CreateSelectionWindow(elapsedTime);
        RemoveCollisionHandlers();
    }
    
    public void CreateSelectionWindow(double finishTime)
    {
        string[] options = { "Top List", "Restart", "Quit"};
        var endWindow = new MultiSelectWindow($"Finished in! {finishTime}", options);
        
        endWindow.AddItemHandler(0, delegate { _topList.ShowTopList(finishTime); });
        endWindow.AddItemHandler(1, Init);
        endWindow.AddItemHandler(2, Exit);
        
        Add(endWindow);
    }
    
}