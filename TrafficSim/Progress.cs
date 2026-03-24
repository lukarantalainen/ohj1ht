using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using Jypeli.GameObjects;
using Jypeli;
using Jypeli.Assets;

namespace TrafficSim;
using Jypeli;
using Jypeli.Widgets;
public class Progress
{
    private readonly TrafficSim _trafficSim;
    
    private readonly DoubleMeter _distMeter;
    private readonly DoubleMeter _timeMeter;
    
    private readonly PhysicsObject _finishLine;
    
    private readonly double _roadLength;
    
    private bool _started;
    private bool _finished;
    
    public Progress(TrafficSim trafficSim, double roadLength)
    {
        _trafficSim = trafficSim;
        _roadLength = roadLength;
        _timeMeter = new DoubleMeter(0);
        _distMeter = new DoubleMeter(0);
        _finishLine = new PhysicsObject(Game.Screen.Width, 20, Shape.Rectangle);
    }

    public void StartGame()
    {
        if (_started) return;
        _trafficSim.MessageDisplay.Clear();
        CreateStartCountdown();
        CreateProgressBar(_roadLength);
        _started = true;
    }
    
    public void SimulateDriving(double force)
    {
        if (_finished)
        {
            _finishLine.Push(new Vector(0, -_finishLine.Mass*force));
        }
        _distMeter.Value+=10;
    }
    
    public double StopTimer()
    {
        return _timeMeter.Value;
    }

    private void CreateStartCountdown()
    {
        var countdown = new DoubleMeter(3);
        
        var display = new Label(100, 50, "3");
        display.Color = Color.Orange;
        display.TextColor = Color.GreenYellow;
        display.DecimalPlaces = 1;
        display.BindTo(countdown);
        display.Position = new Vector(0, 100);
        _trafficSim.Add(display);
        
        var starttimer = new Timer(3);
        
        starttimer.Interval = 0.1;
        starttimer.Timeout += delegate { Countdown(display, countdown, starttimer); };
        starttimer.Start();
    }

    private void Countdown(Label display, DoubleMeter countdown,  Timer starttimer)
    {
        countdown.Value -= 0.1;
        if (countdown.Value <=0.1)
        {
            starttimer.Stop();
            display.Unbind();
            _trafficSim.AddControls();
            display.Destroy();
            CreateStartTimer();
        }
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