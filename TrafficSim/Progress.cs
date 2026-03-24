using System;
using System.Globalization;
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
    private readonly double _roadLength;
    private DoubleMeter _distMeter;
    private PhysicsObject _finishLine;
    private bool _finished;
    private Timer _startTimer;
    private DoubleMeter _timeMeter;
    private Label _display;
    public Progress(TrafficSim trafficSim, double roadLength)
    {
        _trafficSim = trafficSim;
        _roadLength = new DoubleMeter(roadLength);
        CreateProgressBar();
        AddCountdown();
    }

    private void AddCountdown()
    {
        _timeMeter = new DoubleMeter(3.9);
        
        _startTimer = new Timer(3);
        
        _startTimer.Interval = 0.1;
        _startTimer.Timeout += Countdown;
        _startTimer.Start();

        _display = new Label(100, 50, "3");
        _display.Color = Color.Orange;
        _display.TextColor = Color.GreenYellow;
        _display.DecimalPlaces = 0;
        _display.BindTo(_timeMeter);
        _display.Position = new Vector(0, 100);
        _trafficSim.Add(_display);
    }

    private void Countdown()
    {
        _timeMeter.Value -= 0.1;
        if (_timeMeter.Value <=0)
        {
            _display.Unbind();
            _trafficSim.AddControls();
            _display.Text = "Go!";
            StartTimer();

        }

        if (_timeMeter.Value <= -2)
        {
            _display.Destroy();
            _startTimer.Stop();
        }
    }

    private void StartTimer()
    {
        DoubleMeter timeMeter = new DoubleMeter(0);
        Timer timer = new Timer();
        timer.Interval = 0.1;
        timer.Timeout += delegate {UpdateTimer(timeMeter);};

        Label currentTime = new Label();
        currentTime.BindTo(timeMeter);
    }

    public void StopTimer()
    {
        
    }

    private void UpdateTimer(DoubleMeter timeMeter)
    {
        timeMeter.Value += 0.1;
    }

    private void CreateProgressBar()
    {
        _distMeter = new DoubleMeter(0);
        _distMeter.MaxValue = _roadLength;
        _distMeter.Changed += CheckLimit;

        ProgressBar progressBar = new ProgressBar(100, 40);
        progressBar.Angle = Angle.FromDegrees(-90);
        progressBar.BindTo(_distMeter);
        progressBar.Position = new Vector(Game.Screen.Right-100, 100);
        progressBar.BarColor = Color.Red;
        progressBar.Color = Color.Black;
        _trafficSim.Add(progressBar);
    }

    private void CheckLimit(double last, double current)
    {
        if (current >= _roadLength && !_finished)
        {
            CreateFinishLine();
        }
    }

    private void CreateFinishLine()
    {
        _finishLine = new PhysicsObject(Game.Screen.Width, 20, Shape.Rectangle);
        _finishLine.Color = Color.Black;
        _finishLine.Position = new Vector(Game.Screen.Left, Game.Screen.Top-100);
        _finishLine.IgnoresCollisionResponse = true;
        _finishLine.IgnoresExplosions = true;
        _finishLine.IgnoresPhysicsLogics = true;
        _trafficSim.Add(_finishLine, 3);
            
        _finished = true;

        _trafficSim.AddCollisionHandler(_finishLine, "player", _trafficSim.EndGame);
    }

    public void Stop()
    {
        _finishLine.Stop();
    }

    public void SimulateDriving(double force)
    {
        if (_finished)
        {
            _finishLine.Push(new Vector(0, -_finishLine.Mass*force));
        }
        _distMeter.Value+=10;
    }

    public void SimulateBraking()
    {
        if (_finished)
        {
            _finishLine.Stop();
        }
    }
}