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
        _car = new Player(this);
        var roadMap = new RoadMap(this);
        
        var speedOMeter = new SpeedOMeter(roadMap, new Vector(Level.Left+100, Level.Top-100));
        Add(speedOMeter);

        Controls.Start(_car, roadMap, this);
    }
}