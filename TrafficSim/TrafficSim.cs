using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        CreateMap();
        CreatePlayer();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    private void CreateMap()
    {
        Level.BackgroundColor = Color.JungleGreen;
        Level.CreateBorders();
        var road = new GameObject(Level.Width*0.8, 2000);
        road.Color = Color.Black;
        Add(road, -3);
    }

    private void CreatePlayer()
    {
        _player = new PhysicsObject(40, 20);
        _player.Color = Color.Red;
        _player.MaxVelocity = 200;
        _player.LinearDamping = 0.95;
        _player.MomentOfInertia = 500;
        _player.AngularDamping = 0.5;
        _player.Angle = Angle.FromDegrees(90);
        Add(_player, 0);
        AddControls();
    }

    private void AddControls()
    {
        Keyboard.Listen(Key.W, ButtonState.Down, MoveCar, "", true);
        Keyboard.Listen(Key.S, ButtonState.Down, MoveCar, "", false);
        Keyboard.Listen(Key.D, ButtonState.Down, RotateCar, "", true);
        Keyboard.Listen(Key.A, ButtonState.Down, RotateCar, "", false);
    }

    private void MoveCar(bool forward)
    {
        Vector facing = Vector.FromLengthAndAngle(500, _player.Angle);
        
        if (forward)
        {
            _player.Push(facing);
        }

        else
        {
            _player.Push(-facing);
        }
    }

    private void RotateCar(bool right)
    {
        if (right)
        {
            _player.ApplyTorque(-1000);
        }
        else
        {
            _player.ApplyTorque(1000);
        }
    }
    
   
}