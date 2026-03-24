using System;
using Jypeli.Widgets;
using Jypeli;
namespace TrafficSim;


public class TopList : EasyHighScore
{
    private readonly TrafficSim _trafficSim;

    public TopList(TrafficSim trafficSim)
    {
        _trafficSim = trafficSim;
    }
    public void ShowTopList(double time)
    {
        EnterAndShow(time);
        HighScoreWindow.Closed += delegate { _trafficSim.CreateSelectionWindow(time); };
    }
}