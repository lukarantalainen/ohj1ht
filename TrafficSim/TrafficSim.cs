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
    }

    public void AddControls()
    {
        Controls.Start(this,  _car,  _roadMap);
    }
    
    public void EndGame(PhysicsObject a, PhysicsObject b)
    {
        //StopAll();
        //var elapsedTime = _progress.StopTimer();
        //Console.WriteLine(elapsedTime);
        //CreateSelectionWindow();
    }

    private void CreateSelectionWindow()
    {
        string[] options = { "Top List", "Restart", "Quit"};
        var endWindow = new MultiSelectWindow("Finished!", options);
        
        endWindow.AddItemHandler(0, delegate{});
        endWindow.AddItemHandler(1, Init);
        endWindow.AddItemHandler(2, ConfirmExit);
        
        Add(endWindow);
    }
    
}