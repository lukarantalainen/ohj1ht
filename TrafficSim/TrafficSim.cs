using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using SixLabors.ImageSharp.Formats.Webp;

namespace TrafficSim;

/// @author gr313129
/// @version 16.01.2026
/// <summary>
/// 
/// </summary>
public class TrafficSim : PhysicsGame
{
    private PhysicsObject _player;
    public override void Begin()
    {
        GameObject road = new GameObject(Level.Height, Level.Width * 0.8, Shape.Rectangle);
        road.Color = Color.Black;
        Add(road, -3);
        
        CreatePlayer();
        AddControls();
        

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    
    }

    private void CreatePlayer()
    {
        _player = new PhysicsObject(40, 20,  Shape.Rectangle);
        _player.Color = Color.Green;
        Add(_player);
    }

    private void AddControls()
    {
        Keyboard.Listen(Key.W, ButtonState.Down, MovePlayer, "", new Vector(0, 100));
        Keyboard.Listen(Key.S, ButtonState.Down, MovePlayer, "", new Vector(0, -100));
        Keyboard.Listen(Key.A, ButtonState.Down, MovePlayer, "", new Vector(-100, 0));
        Keyboard.Listen(Key.D, ButtonState.Down, MovePlayer, "", new Vector(100, 0));
    }

    private void MovePlayer(Vector direction)
    {
        _player.Push(direction);
    }

    private void MovePlayer(Angle angle)
    {
        
    }
}