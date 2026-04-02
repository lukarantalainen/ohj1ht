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
    private readonly TrafficSim _trafficSim;
    
    private readonly DoubleMeter _distMeter;
    private readonly DoubleMeter _timeMeter;
    
    private readonly PhysicsObject _finishLine;
    
    private const double RoadLength = 3000;
    
    private bool _started = false;
    private bool _finished = false;
    
    public Progress(TrafficSim trafficSim)
    {
        _trafficSim = trafficSim;
        _timeMeter = new DoubleMeter(0);
        _distMeter = new DoubleMeter(0);
        _finishLine = new PhysicsObject(Game.Screen.Width, 20, Shape.Rectangle);
    }

    public void StartGame()
    {
        if (_started) return;
        _trafficSim.MessageDisplay.Clear();
        CreateStartLights();
        CreateProgressBar(RoadLength);
        _started = true;
    }
    
    public void Drive(double velocity)
    {
        if (_finished)
        {
            _finishLine.Push(new Vector(0, -_finishLine.Mass*velocity));
        }
        _distMeter.Value+=velocity/100;
    }
    
    public double StopTimer()
    {
        return _timeMeter.Value;
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
            Top = Game.Screen.Top - 50,
            Color = Color.Black,
        };

        var root = new GameObject(background.Width + 30, background.Height + 30, Shape.Rectangle)
        {
            Position = background.Position,
            Color = Color.DarkGray
        };

        root.Add(background);
        _trafficSim.Add(root);

        var lights = CreateCircles(background);
        CreateStartTimer(lights, root);
    }

    private void CreateStartTimer(List<GameObject>lights, GameObject root)
    {
        var countdown = new IntMeter(0);
        bool penalty = false;
        _trafficSim.Keyboard.Listen(Key.W, ButtonState.Down, delegate 
        { penalty = true;
            _trafficSim.MessageDisplay.Add("Penalty: false start");
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
            CreateStartTimer();
            foreach (var lightObj in lights)
            {
                lightObj.Color = Color.Green;
            }

            if (!penalty)
            {
                _trafficSim.AddControls();
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
            _trafficSim.AddControls();
        }
        countdown.Value++;
    }

    private void CreateStartTimer()
    {
        var timer = new Timer();
        timer.Interval = 0.1;
        timer.Timeout += UpdateTimer;
        timer.Start();
        
        var currentTime = new Label();
        currentTime.BindTo(_timeMeter);
        currentTime.Color = Color.Black;
        currentTime.TextColor = Color.BrightGreen;
        currentTime.Position = new Vector(-300, 200);
        _trafficSim.Add(currentTime);
    }
    
    private void UpdateTimer()
    {
        _timeMeter.Value += 0.1;
    }
    
    private void CreateProgressBar(double roadLength)
    {
        _distMeter.MaxValue = roadLength;
        _distMeter.Changed += delegate(double old, double current) { CheckLimit(old, current, roadLength); };

        var progressBar = new ProgressBar(100, 40);
        progressBar.Angle = Angle.FromDegrees(-90);
        progressBar.BindTo(_distMeter);
        progressBar.Position = new Vector(Game.Screen.Right-100, 100);
        progressBar.BarColor = Color.Red;
        progressBar.Color = Color.Black;
        _trafficSim.Add(progressBar);
    }

    private void CheckLimit(double old, double current, double roadLength)
    {
        if (current >= roadLength && !_finished)
        {
            CreateFinishLine();
        }
    }

    private void CreateFinishLine()
    {
        _finishLine.Color = Color.Black;
        _finishLine.Position = new Vector(0, Game.Screen.Top);
        _finishLine.IgnoresCollisionResponse = true;
        _finishLine.IgnoresExplosions = true;
        _finishLine.IgnoresPhysicsLogics = true;
        _trafficSim.Add(_finishLine, 3);
            
        _finished = true;

        _trafficSim.AddCollisionHandler(_finishLine, "player", _trafficSim.EndGame);
    }
}
