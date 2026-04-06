using System;
using System.Collections.Generic;

namespace RacingGame;
using Jypeli;
using Jypeli.Assets;
using Silk.NET.Maths;

public class VehicleGenerator
{
    private readonly RacingGame game;
    private readonly Road road;

    public VehicleGenerator(RacingGame game, Road road)
    {
        this.game = game;
        this.road = road;

    }

    public static void Start(RacingGame game, Road road)
    {
        var generator = new VehicleGenerator(game, road);
        {
            var timer = new Timer(1);
            timer.Timeout += delegate { generator.Generate(timer);  };
            timer.Start();

            var lowerBorder = new PhysicsObject(Game.Screen.Width, 1)
            {
                Position = new Vector(0, Game.Screen.Bottom - 500),
            };
            game.Add(lowerBorder);

            game.AddCollisionHandler(lowerBorder, "vehicle", CollisionHandler.DestroyTarget);
        }
    }

    private void Generate(Timer timer)
    {
        timer.Interval = RandomGen.NextDouble(0.5, 1.5);
        int lane = RandomGen.NextInt(0, 4);

        var vehicle = new Vehicle(Properties.CarSize, Properties.CarSize, (VehicleType)RandomGen.NextInt(0, 2))
        {
            Position = new Vector(road.Lanes[lane], Game.Screen.Top + 200),
            Tag = "vehicle",
        };

        if (lane == 0 || lane == 1)
        {
            vehicle.MoveTo(new Vector(vehicle.X, Game.Screen.Bottom - 1000), vehicle.PushVelocity+road.GetVelocity());
            vehicle.Angle = Angle.FromDegrees(180);
        } else
        {
            vehicle.MoveTo(new Vector(vehicle.X, Game.Screen.Bottom - 1000), road.GetVelocity()+200);
        }
        
        game.Add(vehicle);
    }
}