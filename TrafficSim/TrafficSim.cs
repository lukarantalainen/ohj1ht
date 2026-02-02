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
    public IntMeter IntervalMeter;
    public Timer VehicleTimer;
    public override void Begin()
    {
        Level.BackgroundColor = Color.Aqua;
        IntervalMeter = new IntMeter(0);
        VehicleTimer = new Timer();
        VehicleTimer.Interval = 1.0;
        VehicleTimer.Timeout += CreateVehicle;
        VehicleTimer.Start();

        GameObject road = new GameObject(2000, 40);
        Add(road);
        
        ShowInterval();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Enter, ButtonState.Down, SetInterval, "");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    void SetInterval()
    {
        double nextInterval = RandomGen.NextDouble(0.2, 1.0);
        IntervalMeter.Value = Convert.ToInt32(Math.Floor(nextInterval * 100));
        VehicleTimer.Interval = nextInterval;
    }

    void ShowInterval()
    {
        Label text = new Label();
        text.X = Level.Left + 100;
        text.Y = Level.Top - 100;
        text.BindTo(IntervalMeter);
        Add(text);
    }

    void CreateVehicle()
    {
        if (RandomGen.NextInt(1, 10) <= 8)
        {
            Vehicle carObj = new Vehicle();
            PhysicsObject car = carObj.FourDoor();
            car.Position = RandomGen.NextVector(Level.Left, Level.Bottom, Level.Right, Level.Top);
            Add(car);
        }
        else
        {
            Vehicle truckObj = new Vehicle();
            PhysicsObject truck = truckObj.Truck();
            truck.Position = RandomGen.NextVector(Level.Left, Level.Bottom, Level.Right, Level.Top);
            Add(truck);
        }
        

    }

    public static class Colors
    {
        private static Color[] commonCarColors = { Color.Black, Color.White, Color.Gray, Color.Blue, Color.Red};
        private static Color[] rareCarColors = { Color.Silver, Color.Brown, Color.LightBlue, Color.Yellow, Color.BrightGreen };

        public static Color GetCommonCarColor()
        {
            return commonCarColors[RandomGen.NextInt(0, commonCarColors.Length)];
        }

        public static Color GetRareCarColor()
        {
            return rareCarColors[RandomGen.NextInt(0, rareCarColors.Length)];
        }
    }

    public class Vehicle
    {
        public PhysicsObject FourDoor()
        {
            PhysicsObject car = new PhysicsObject(20, 40);
            if (RandomGen.NextInt(1, 10) < 8)
            {
                car.Color = Colors.GetCommonCarColor();
            }
            else
            {
                car.Color = Colors.GetRareCarColor();
            }
            
            return car;
        }

        public PhysicsObject Truck()
        {
            PhysicsObject truck = new PhysicsObject(20, 60);
            truck.Color = RandomGen.NextColor();
            return truck;
        }
        
    }
}