using Jypeli;
using Jypeli.Assets;
using Jypeli.GameObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace TrafficSim;
using Jypeli;
using Jypeli.Effects;
using Jypeli.Widgets;
using Silk.NET.OpenGL;

public class Progress
{
    private readonly TrafficSim trafficSim;
    
    private readonly DoubleMeter distMeter;
    private readonly DoubleMeter timeMeter;
    
    private readonly PhysicsObject finishLine;
    
    private bool finished = false;
    
    public Progress(TrafficSim trafficSim)
    {
        this.trafficSim = trafficSim;
        timeMeter = new DoubleMeter(0);
        distMeter = new DoubleMeter(0);
        finishLine = new PhysicsObject(TrafficSim.Screen.Width, 20, Shape.Rectangle);
    }

    public void StartGame()
    {
        CreateStartLights();
        CreateProgressBar(Properties.RoadLength);
    }
    
    public void Drive(double velocity)
    {
        if (finished)
        {
            finishLine.Push(new Vector(0, -finishLine.Mass*velocity));
        }
        distMeter.Value+=velocity/1000;
    }
    
    public double StopTimer()
    {
        return timeMeter.Value;
    }

    private static List<GameObject> CreateCircles(GameObject background)
    {
        var circles = new List<GameObject>();
        var color = new Color(27,27, 27);
        const double r = 70;
        var centerCircle = new GameObject(r, r, Shape.Circle)
        {
            Color = color,
            Position = background.Position
        };
        
        var leftCircle = new GameObject(r, r, Shape.Circle)
        {
            Color= color,
            Position = new Vector((centerCircle.Left + background.Left) / 2, background.Y)
        };

        var rightCircle = new GameObject(r, r, Shape.Circle)
        {
            Color = color,
            Position = new Vector((centerCircle.Right + background.Right) / 2, background.Y)
        };

        circles.Add(leftCircle);
        circles.Add(centerCircle);
        circles.Add(rightCircle);

        background.Add(centerCircle);
        background.Add(leftCircle);
        background.Add(rightCircle);

        return circles;
    }

    private void CreateStartLights()
    {
        var background = new GameObject(400, 150, Shape.Rectangle)
        {
            X = 0,
            Top = TrafficSim.Screen.Top - 50,
            Color = Color.Black,
        };

        var root = new GameObject(background.Width + 30, background.Height + 30, Shape.Rectangle)
        {
            Position = background.Position,
            Color = Color.DarkGray
        };

        root.Add(background);
        trafficSim.Add(root);

        var lights = CreateCircles(background);
        CreateCountdown(lights, root);
    }

    private void CreateCountdown(List<GameObject>lights, GameObject root)
    {
        var countdown = new IntMeter(0);
        bool penalty = false;
        trafficSim.Keyboard.Listen(Key.W, ButtonState.Down, delegate 
        { penalty = true;
            trafficSim.MessageDisplay.Add("Penalty: false start");
        }, "");

        var timer = new Timer(1);
        timer.Timeout += delegate () { UpdateLights(countdown, timer, lights, root, penalty); };
        timer.Start();
    }

    private void UpdateLights(IntMeter countdown, Timer timer, List<GameObject> lights, GameObject root, bool penalty)
    {
        if (countdown.Value < 3)
        {
            lights[countdown.Value].Color = Color.Red;
        }

        else if (countdown.Value == 3)
        {
            CreateTimer();
            foreach (var lightObj in lights)
            {
                lightObj.Color = Color.Green;
            }

            if (!penalty)
            {
                trafficSim.AddControls();
            }
        }

        else if (countdown.Value == 4 && !penalty)
        {
            root.Destroy();
            timer.Stop();
        }

        else if (countdown.Value == 5 && penalty)
        {
            root.Destroy();
            timer.Stop();
            trafficSim.AddControls();
        }
        countdown.Value++;
    }

    private void CreateTimer()
    {
        var timer = new Timer
        {
            Interval = 0.01
        };
        timer.Timeout += UpdateTimer;
        timer.Start();
        
        var currentTime = new Label()
        {
            Color = Color.Black,
            TextColor = Color.BrightGreen,
            DecimalPlaces = 2,
            Position = new Vector(-300, 200),
        };
        currentTime.BindTo(timeMeter);
        trafficSim.Add(currentTime);

        var targetTime = new Label()
        {
            Color = Color.Black,
            TextColor = Color.Orange,
            X = currentTime.X,
            Top = currentTime.Bottom,
            Text = Properties.TargetTime.ToString("0,0")
        };

        trafficSim.Add(targetTime);

    }
    
    private void UpdateTimer()
    {
        timeMeter.Value += 0.01;
    }
    
    private void CreateProgressBar(double roadLength)
    {
        distMeter.MaxValue = roadLength;
        distMeter.Changed += delegate(double old, double current) { CheckLimit(old, current, roadLength); };

        var progressBar = new ProgressBar(100, 40);
        progressBar.Angle = Angle.FromDegrees(-90);
        progressBar.BindTo(distMeter);
        progressBar.Position = new Vector(TrafficSim.Screen.Right-100, 100);
        progressBar.BarColor = Color.Red;
        progressBar.Color = Color.Black;
        trafficSim.Add(progressBar);
    }

    private void CheckLimit(double old, double current, double roadLength)
    {
        if (current >= roadLength && !finished)
        {
            CreateFinishLine();
        }
    }

    private void CreateFinishLine()
    {
        finishLine.Color = Color.Black;
        finishLine.Position = new Vector(0, TrafficSim.Screen.Top);
        finishLine.IgnoresCollisionResponse = true;
        finishLine.IgnoresExplosions = true;
        finishLine.IgnoresPhysicsLogics = true;
        trafficSim.Add(finishLine, 3);
            
        finished = true;

        trafficSim.AddCollisionHandler(finishLine, "player", delegate (PhysicsObject a, PhysicsObject b) { trafficSim.EndGame(); }); 
    }
}
