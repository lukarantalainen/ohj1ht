using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Jypeli;

namespace RacingGame
{
    public class Dashboard
    {
        private readonly RacingGame game;
        private readonly Progress progress;

        public Dashboard(RacingGame game, Player player, Progress progress)
        {
            this.game = game;
            this.progress = progress;
            var background = CreateBackground();

            var topBar = CreateTopBar();
            game.Add(topBar, 2);

            var healthBar = player.GetHealthBar();
            healthBar.Left = background.Left + 10;
            healthBar.Top = background.Top - 10;

            var timeLabel = progress.GetTimeLabel();
            timeLabel.Position = new Vector(topBar.X-30, topBar.Y);
            var targetTimeLabel = progress.GetTargetTimeLabel();
            targetTimeLabel.Position = new Vector(topBar.X + 30, topBar.Y);


            var progressBar = progress.GetProgressBar();
            progressBar.Right = background.Right - 10;
            progressBar.Y = background.Y;

            game.Add(progressBar);

            game.Add(timeLabel, 3);
            game.Add(targetTimeLabel, 3);

            game.Add(healthBar, 2);

            game.Add(background, 2);
        }


        private static GameObject CreateBackground()
        {
            var background = new GameObject(Properties.RoadWidth+2*Properties.RoadBorderWidth, 125, Shape.Rectangle)
            {
                X = 0,
                Bottom = Game.Screen.Bottom,
                Color = Color.Black,
            };
            return background;
        }

        private static GameObject CreateTopBar()
        {
            var topBar = new GameObject(Properties.RoadWidth + 2 * Properties.RoadBorderWidth, 40)
            {
                Color = Color.Black,
                Top = Game.Screen.Top,

            };
            return topBar;
        }


    }    
}
