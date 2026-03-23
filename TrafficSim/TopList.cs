using Jypeli.Widgets;

namespace TrafficSim;
using Jypeli;

public class TopList
{
    private ScoreList _topList;

    TopList()
    {
        _topList = new ScoreList(10, true, 0);
    }

  
}