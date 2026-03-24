using Jypeli;
using System;
using System.Collections.Generic;
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
    public override void Begin()
    {
        IsFullScreen = true;
        
        ResetGame();
    }
    
    public void ResetGame()
    {
        ClearAll();
        CreateMap();
        Keyboard.Listen(Key.R, ButtonState.Pressed, ResetGame, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Debug.Start(this, _car,  _roadMap);
    }
    
    private void CreateMap()
    {
        _progress = new Progress(this, 6000);
        _car = new Player(this);
        _roadMap = new RoadMap(this, _progress);
        
        var speedOMeter = new SpeedOMeter(_roadMap, new Vector(Level.Left+100, Level.Top-100));
        Add(speedOMeter);
    }

    public void AddControls()
    {
        Controls.Start(_car, _roadMap, this);
    }
    
    public void EndGame(PhysicsObject a, PhysicsObject b)
    {
        _progress.StopTimer();
        IsPaused = true;
        CreateSelectionWindow();
    }

    private void CreateSelectionWindow()
    {
        string[] options = { "Top List", "Restart", "Quit"};
        MultiSelectWindow endWindow = new MultiSelectWindow("Finished!", options);
        
        endWindow.AddItemHandler(0, delegate{});
        endWindow.AddItemHandler(1, ResetGame);
        endWindow.AddItemHandler(2, ConfirmExit);
        
        Add(endWindow);
    }
    
}