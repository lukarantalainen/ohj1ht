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
    private Player _car;
    private RoadMap _roadMap;
    private Progress _progress;
    public override void Begin()
    {
        IsFullScreen = true;
        ResetGame();
    }
    
    public void ResetGame()
    {
        ClearAll();
        CreateMap();
        Debug.Start(this, _car);
    }
    
    private void CreateMap()
    {
        _progress = new Progress(this, _roadMap, 6000);
        _car = new Player(this);
        _roadMap = new RoadMap(this, _progress);
        
        var speedOMeter = new SpeedOMeter(_roadMap, new Vector(Level.Left+100, Level.Top-100));
        Add(speedOMeter);

    }

    public void AddControls()
    {
        Controls.Start(_car, _roadMap, this);
        
    }
}