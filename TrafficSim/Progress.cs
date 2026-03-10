namespace TrafficSim;
using Jypeli;
using Jypeli.Widgets;
public class Progress
{
    private TrafficSim _trafficSim;
    private DoubleMeter _progressBar;

    Progress(TrafficSim trafficSim, double trackLength)
    {
        _trafficSim = trafficSim;
        _progressBar = new DoubleMeter(0);
    }

    private void CreateProgress()
    {
        
    }
    
    
}