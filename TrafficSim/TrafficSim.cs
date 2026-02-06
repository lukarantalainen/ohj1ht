using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using SixLabors.ImageSharp.Formats.Webp;

namespace TrafficSim;

/// @author lukar
/// @version 16.01.2026
/// <summary>
/// 
/// </summary>
public class TrafficSim : PhysicsGame
{
    private PhysicsObject _player;
    private PhysicsObject _road1;
    private PhysicsObject _road2;
    private Label _debugLabel;
    private PhysicsObject _lowerBorder;
    private Image road_texture = LoadImage("road_texture");
    public override void Begin()
    {
        CreatePlayer();
        AddControls();
        CreateMap();
        _road1 = CreateRoad(Color.Black, 2000);
        _road2 = CreateRoad(Color.Blue, 2000);
        _road2.Y = _road1.Y + _road1.Height;
        Add(_road1);
        Add(_road2);
        
        CreateDebugLabel();

        AddCollisionHandler(_lowerBorder, _road1, RoadCycle);
        AddCollisionHandler(_lowerBorder, _road2, RoadCycle);
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    
    private void CreateDebugLabel()
    {
        _debugLabel = new Label();
        _debugLabel.Position = new Vector(Level.Left+100, Level.Top-100);
        Add(_debugLabel);
    }

    private void CreateMap()
    {
        Level.BackgroundColor = Color.JungleGreen;
        _lowerBorder = new PhysicsObject(Level.Width, 1);
        _lowerBorder.Y = -2000;
        Add(_lowerBorder, -3);
    }
    
    private PhysicsObject CreateRoad(Color color, double height)
    {
        PhysicsObject road = new PhysicsObject(Level.Width*0.8, 2000);
        road.Image = road_texture;
        road.IgnoresGravity = true;
        road.IgnoresCollisionResponse = true;

        road.MaxVelocity = 200;
        
        return road;
    }
    
    private static void RoadCycle(PhysicsObject target, PhysicsObject road)
    {
        road.Y += 2000;
    }
    private void CreatePlayer()
    {
        _player = new PhysicsObject(40, 20);
        _player.Color = Color.Red;
        _player.AngularDamping = 0.95;
        _player.Angle = Angle.FromDegrees(90);
        Add(_player, 0);
    }
    
    private void AddControls()
    {
        Keyboard.Listen(Key.W, ButtonState.Down, Drive, "");
        Keyboard.Listen(Key.S, ButtonState.Down, Brake, "");
        Keyboard.Listen(Key.D, ButtonState.Down, RotateCar, "", true);
        Keyboard.Listen(Key.A, ButtonState.Down, RotateCar, "", false);
        Keyboard.Listen(Key.R, ButtonState.Pressed, ResetPos, "");
    }

    private void ResetPos()
    {
        _road1.Position = new Vector(0, 0);
        _road2.Position = new Vector(0, 50);
    }
    private void MoveRoads(double force)
    {
        _road1.Push(new Vector(0, _road1.Mass*force));
        _road2.Push(new Vector(0, _road2.Mass*force));
    }
    
    private void Drive()
    {
        _debugLabel.Text = "Accelerating";
        MoveRoads(-100);
    }

    private void Brake()
    {
        if (_road1.Velocity.Y < 0)
        {
            _debugLabel.Text = "Braking";
            MoveRoads(100);
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