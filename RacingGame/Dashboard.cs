using Jypeli;

namespace RacingGame;

public class Dashboard
{
    private readonly RacingGame game;
    private readonly Player player;
    private readonly Progress progress;

    /// <summary>
    ///     Creates the UI
    /// </summary>
    /// <param name="game"></param>
    /// <param name="player"></param>
    /// <param name="progress"></param>
    public Dashboard(RacingGame game, Player player, Progress progress)
    {
        this.game = game;
        this.player = player;
        this.progress = progress;

        Init();
    }

    /// <summary>
    /// Initializes the UI
    /// </summary>
    private void Init()
    {
        var background = CreateBackground();

        var topBar = CreateTopBar();
        game.Add(topBar, 2);

        var healthBar = player.GetHealthBar();
        healthBar.Left = background.Left + 10;
        healthBar.Top = background.Top - 10;

        var progressBar = progress.GetProgressBar();
        progressBar.Right = background.Right - 10;
        progressBar.Y = background.Y;

        game.Add(progressBar);

        game.Add(healthBar, 2);

        game.Add(background, 2);
    }

    /// <summary>
    ///     Creates a background
    /// </summary>
    /// <returns></returns>
    private static GameObject CreateBackground()
    {
        var background = new GameObject(Properties.RoadWidth + 2 * Properties.RoadBorderWidth, 125, Shape.Rectangle)
        {
            X = 0,
            Bottom = Game.Screen.Bottom,
            Color = Color.Black
        };
        return background;
    }

    /// <summary>
    ///     Creates a bar on the top containing the timers
    /// </summary>
    /// <returns></returns>
    private GameObject CreateTopBar()
    {
        var topBar = new GameObject(Properties.RoadWidth + 2 * Properties.RoadBorderWidth, 40)
        {
            Color = Color.Black,
            Top = Game.Screen.Top
        };

        var timeLabel = progress.GetTimeLabel();
        timeLabel.Position = new Vector(topBar.X - 30, topBar.Y);
        topBar.Add(timeLabel);
        var targetTimeLabel = progress.GetTargetTimeLabel();
        targetTimeLabel.Position = new Vector(topBar.X + 30, topBar.Y);
        topBar.Add(targetTimeLabel);

        return topBar;
    }
}