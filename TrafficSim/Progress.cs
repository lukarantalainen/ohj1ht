using System;
using Jypeli.GameObjects;
using Jypeli;
using Jypeli.Assets;

namespace TrafficSim;
using Jypeli;
using Jypeli.Widgets;
public class Progress
{
    private readonly TrafficSim _trafficSim;
    private readonly RoadMap _roadMap;
    private double _roadLength;
    private DoubleMeter _distMeter;
    private PhysicsObject _finishLine;
    private bool _finished;
    private Timer _startTimer;

    public Progress(TrafficSim trafficSim, RoadMap roadMap, double roadLength)
    {
        _trafficSim = trafficSim;
        _roadMap = roadMap;
        _roadLength = new DoubleMeter(roadLength);
        CreateProgressBar();
        AddStartTimer();
    }

    private void AddStartTimer()
    {
        _startTimer = new Timer();
        _startTimer.Interval = 1;
        _startTimer.Timeout += delegate {Countdown(_startTimer.CurrentTime);};
        _startTimer.Start();
    }

    private void Countdown(double time)
    {
        if (Math.Round(time) == 4)
        {
            _startTimer.Stop();
        }
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

        _trafficSim.AddCollisionHandler(_finishLine, "player", _roadMap.EndGame);
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