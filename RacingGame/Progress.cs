using System.Collections.Generic;

namespace RacingGame;

using Jypeli;
using Jypeli.Widgets;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

public class Progress
{
    private readonly RacingGame game;

    private readonly DoubleMeter distMeter = new(0);
    private readonly DoubleMeter distPercentage = new(0);
    private readonly DoubleMeter timeMeter = new(0);
    private readonly DoubleMeter timeLeftMeter = new(Properties.TargetTime);

    private readonly Label timeLabel;
    private readonly Label targetTimeLabel;
    private ProgressBar progressBar;

    private bool finished = false;

    public Progress(RacingGame game)
    {
        this.game = game;

        timeLabel = CreateTimeLabel();
        targetTimeLabel = CreateTargetTimeLabel();
        progressBar = CreateProgressBar();
    }

    public void Start()
    {
        CreateStartLights();
    }

    private Label CreateTimeLabel()
    {
        var label = new Label()
        {
            Width = 20,
            Color = Color.Black,
            TextColor = Color.BrightGreen,
            DecimalPlaces = 2,
        };
        label.BindTo(timeMeter);
        return label;
    }

    private Label CreateTargetTimeLabel()
    {
        var label = new Label()
        {
            Width = 20,
            Color = Color.Black,
            TextColor = Color.Orange,
            Text = Properties.TargetTime.ToString("0")
        };
        label.BindTo(timeLeftMeter);
        return label;
    }

    private ProgressBar CreateProgressBar()
    {
        distMeter.MaxValue = Properties.RoadLength;
        distMeter.UpperLimit += AddFinishLine;

        progressBar = new ProgressBar(100, 50)
        {
            Angle = -Angle.RightAngle,
            Position = new Vector(Game.Screen.Right - 100, 100),
            BarColor = Color.BrightGreen,
            Color = Color.Orange,
        };
        progressBar.BindTo(distMeter);

        var progressLabel = new Label()
        {
            Position = progressBar.Position,
            TextColor = Color.White,
            DecimalPlaces = 0,
        };
        progressLabel.BindTo(distPercentage);
        progressBar.Add(progressLabel);

        return progressBar;
    }

    private void AddFinishLine()
    {
        if (!finished)
        {
            finished = true;
            var finishLine = new PhysicsObject(Properties.RoadWidth, 35, Shape.Rectangle)
            {
                Image = RacingGame.Finishline,
                Position = new Vector(0, Game.Screen.Top),
                IgnoresCollisionResponse = true,
                IgnoresExplosions = true,
                IgnoresPhysicsLogics = true,
            };

            game.AddCollisionHandler(finishLine, "player", delegate (PhysicsObject a, PhysicsObject b) { game.End(false); });
            game.Add(finishLine, 3);

            finishLine.MoveTo(new Vector(finishLine.X, Game.Screen.Bottom), Properties.MaxVelocity);
        }
    }

    private static GameObject CreateStartLightBackground()
    {
        var background = new GameObject(400, 150, Shape.Rectangle)
        {
            X = 0,
            Top = Game.Screen.Top - 50,
            Color = Color.Black,
        };

        var holderLeft = new GameObject(10, 200, Shape.Rectangle)
        {
            X = background.Left + 50,
            Bottom = background.Top,
            Color = Color.Black
        };

        var holderRight = new GameObject(10, 200, Shape.Rectangle)
        {
            X = background.Right - 50,
            Bottom = background.Top,
            Color = Color.Black
        };

        background.Add(holderLeft);
        background.Add(holderRight);
        return background;
    }


    private void CreateStartLights()
    {
        var background = CreateStartLightBackground();

        var lights = new List<GameObject>();
        var color = new Color(27, 27, 27);
        const double r = 70;
        double gap = (background.Width - (3 * r)) / 4;

        for (int i = 1; i < 4; i++)
        {
            var light = new GameObject(r, r, Shape.Circle)
            {
                Color = color,
                Position = new Vector(background.Left + i * gap + (2*i-1)*r/2, background.Y)
            };
            lights.Add(light);
            background.Add(light);
        }

        var root = new GameObject(background.Width + 10, background.Height + 10, Shape.Rectangle)
        {
            Position = background.Position,
            Color = Color.LightGray
        };

        root.Add(background);
        game.Add(root);

        StartCountdown(lights, root);
    }

    private void StartCountdown(List<GameObject>lights, GameObject root)
    {
        var countdown = new IntMeter(0);
        bool penalty = false;
        game.Keyboard.Listen(Key.W, ButtonState.Down, delegate 
        { penalty = true;
            game.MessageDisplay.Add("Penalty: false start");
        }, "");

        var timer = new Timer(1);
        timer.Timeout += delegate () { UpdateLights(countdown, timer, lights, root, penalty); };
        timer.Start();
    }

    private void UpdateLights(IntMeter countdown, Timer timer, List<GameObject> lights, GameObject root, bool penalty)
    {
        if (countdown.Value==0)
        {
            RacingGame.StartSound.Play();
        }

        if (countdown.Value < 3)
        {
            lights[countdown.Value].Color = Color.Red;
        }

        else if (countdown.Value == 3)
        {
            CreateTimer();
            foreach (var lightObj in lights)
            {
                lightObj.Color = Color.Green;
            }

            if (!penalty)
            {
                game.AddControls();
                game.Start();
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
            game.Start();
        }
        
        countdown.Value++;
    }

    private void CreateTimer()
    {
        var timer = new Timer
        {
            Interval = 0.01
        };
        timer.Timeout += UpdateTimer;
        timer.Start();

        var timeLeft = new Timer(0.01);
        {
            timeLeft.Timeout += delegate { timeLeftMeter.Value -= 0.01; };
        }
        timeLeft.Start();
    }
    
    private void UpdateTimer()
    {
        timeMeter.Value += 0.01;
    }

    public void Drive(double velocity)
    {
        distMeter.Value += velocity / 1000;
        distPercentage.Value = distMeter.Value / Properties.RoadLength*100;
    }

    public double StopTimer()
    {
        return timeMeter.Value;
    }

    public Label GetTimeLabel()
    {

        return timeLabel;
    }

    public Label GetTargetTimeLabel()
    {
        return targetTimeLabel;
    }

    public ProgressBar GetProgressBar()
    {
        return progressBar;
    }
}
