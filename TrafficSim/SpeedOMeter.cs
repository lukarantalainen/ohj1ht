namespace TrafficSim;
using Jypeli;

public class SpeedOMeter : Label
{
    public SpeedOMeter(RoadMap roadMap, Vector position)
    {
        base.Text = "0";
        base.Position = position;
        
        var speedOMeterTimer = new Timer();
        speedOMeterTimer.Interval = 0.5;
        speedOMeterTimer.Timeout += delegate {UpdateSpeedOMeter(this, roadMap);};
        speedOMeterTimer.Start();
    }
    
    private void UpdateSpeedOMeter(Label speedOMeter, RoadMap roadMap)
    {
        base.Text = roadMap.GetAbsVelocity().ToString("F1");
        
    }
}