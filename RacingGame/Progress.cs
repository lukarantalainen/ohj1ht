using System.Collections.Generic;

namespace RacingGame;

using Jypeli;
using Jypeli.Widgets;

public class Progress
{
    private readonly RacingGame game;

    private readonly DoubleMeter distMeter = new(0);
    private readonly DoubleMeter timeMeter = new(0);

    private readonly PhysicsObject finishLine = new(Properties.RoadWidth, 35, Shape.Rectangle)
    {
        Image = RacingGame.Finishline,
    };
    
    private bool finished = false;

    public Progress(RacingGame game)
    {
        this.game = game;
    }

    public void StartGame()
    {
        CreateStartLights();
        CreateProgressBar(Properties.RoadLength);
    }
    
    public void Drive(double velocity)
    {
        if (finished)
        {
            finishLine.Push(new Vector(0, -finishLine.Mass*velocity));
        }
        distMeter.Value+=velocity/1000;
    }
    
    public double StopTimer()
    {
        return timeMeter.Value;
    }

    private static List<GameObject> CreateCircles(GameObject background)
    {
        var circles = new List<GameObject>();
        var color = new Color(27,27, 27);
        const double r = 70;
        var centerCircle = new GameObject(r, r, Shape.Circle)
        {
            Color = color,
            Position = background.Position
        };
        
        var leftCircle = new GameObject(r, r, Shape.Circle)
        {
            Color= color,
            Position = new Vector((centerCircle.Left + background.Left) / 2, background.Y)
        };

        var rightCircle = new GameObject(r, r, Shape.Circle)
        {
            Color = color,
            Position = new Vector((centerCircle.Right + background.Right) / 2, background.Y)
        };

        circles.Add(leftCircle);
        circles.Add(centerCircle);
        circles.Add(rightCircle);

        background.Add(centerCircle);
        background.Add(leftCircle);
        background.Add(rightCircle);

        return circles;
    }

    private void CreateStartLights()
    {
        var background = new GameObject(400, 150, Shape.Rectangle)
        {
            X = 0,
            Top = Game.Screen.Top - 50,
            Color = Color.Black,
        };

        var root = new GameObject(background.Width + 10, background.Height + 10, Shape.Rectangle)
        {
            Position = background.Position,
            Color = Color.LightGray
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

        root.Add(holderLeft);
        root.Add(holderRight);
        root.Add(background);
        game.Add(root);
        

        var lights = CreateCircles(background);
        CreateCountdown(lights, root);
    }

    private void CreateCountdown(List<GameObject>lights, GameObject root)
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
            game.AddControls();
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
        
        var currentTime = new Label()
        {
            Color = Color.Black,
            TextColor = Color.BrightGreen,
            DecimalPlaces = 2,
            Position = new Vector(-300, 200),
        };
        currentTime.BindTo(timeMeter);
        game.Add(currentTime);

        var targetTime = new Label()
        {
            Color = Color.Black,
            TextColor = Color.Orange,
            X = currentTime.X,
            Top = currentTime.Bottom,
            Text = Properties.TargetTime.ToString("0,0")
        };

        game.Add(targetTime);

    }
    
    private void UpdateTimer()
    {
        timeMeter.Value += 0.01;
    }
    
    private void CreateProgressBar(double roadLength)
    {
        distMeter.MaxValue = roadLength;
        distMeter.Changed += delegate(double old, double current) { CheckLimit(old, current, roadLength); };

        var progressBar = new ProgressBar(100, 40) {
            Angle = Angle.FromDegrees(-90),
            Position = new Vector(Game.Screen.Right - 100, 100),
            BarColor = Color.Red,
            Color = Color.Black,
        };
        progressBar.BindTo(distMeter);
        game.Add(progressBar);
    }

    private void CheckLimit(double old, double current, double roadLength)
    {
        if (current >= roadLength && !finished)
        {
            CreateFinishLine();
        }
    }

    private void CreateFinishLine()
    {
        finishLine.Color = Color.Black;
        finishLine.Position = new Vector(0, Game.Screen.Top);
        finishLine.IgnoresCollisionResponse = true;
        finishLine.IgnoresExplosions = true;
        finishLine.IgnoresPhysicsLogics = true;
        game.Add(finishLine, 3);
            
        finished = true;

        game.AddCollisionHandler(finishLine, "player", delegate (PhysicsObject a, PhysicsObject b) { game.End(); }); 
    }
}
