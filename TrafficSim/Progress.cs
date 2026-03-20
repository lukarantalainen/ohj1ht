using Jypeli.GameObjects;

namespace TrafficSim;
using Jypeli;
using Jypeli.Widgets;
public class Progress
{
    private readonly TrafficSim _trafficSim;
    private readonly double _trackLength;
    private DoubleMeter _distMeter;
    private PhysicsObject _finishLine;
    private bool _finished;

    public Progress(TrafficSim trafficSim, double trackLength)
    {
        _trafficSim = trafficSim;
        _trackLength = trackLength;
        CreateProgressBar();
    }

    private void CreateProgressBar()
    {
        _distMeter = new DoubleMeter(0);
        _distMeter.MaxValue = _trackLength;
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
        if (current >= _trackLength && !_finished)
        {
            _finishLine = new PhysicsObject(Game.Screen.Width, 20, Shape.Rectangle);
            _finishLine.Color = Color.Black;
            _finishLine.Position = new Vector(Game.Screen.Left, Game.Screen.Top-100);
            _finishLine.IgnoresCollisionResponse = true;
            _finishLine.IgnoresExplosions = true;
            _finishLine.IgnoresPhysicsLogics = true;
            _trafficSim.Add(_finishLine, 3);
            _finished = true;
        }
    }

    public void SimulateDriving(double force)
    {
        if (_finished)
        {
            _finishLine.Push(new Vector(0, -_finishLine.Mass*force));
        }
        _distMeter.Value+=10;
    }

    public void SimulateBraking(double force)
    {
        if (_finished && _finishLine.Velocity.Y < 30)
        {
            _finishLine.Push(new Vector(0, _finishLine.Mass * force));
        }
    }
}