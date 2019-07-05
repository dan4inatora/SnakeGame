using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace AlfaRelease
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        Point startingMousePos;
        double stepSize = 10; // Minimum required distance between 2 poins of the mouse needed to move the snake
        Game game;
        
       

        public MainWindow()
        {
            InitializeComponent();
            Snake snake = new Snake();
            game = new Game(ref canvas, ref snake);
            InitializeComponent();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var newMousePos = e.GetPosition(canvas);
            GlobalBooleans.CurrentCursorPosition = new Point(newMousePos.X, newMousePos.Y);

            if (game.ObstacleCollided(GlobalBooleans.CurrentCursorPosition))
            {
                game.ResetGame(txtRules, ScoreLbl);
            }
            if (!this.canvas.IsMouseOver && GlobalBooleans.isGameStarted) // checks if cursor is out of canvas
            {
                game.ResetGame(txtRules, ScoreLbl);
            }

            if (DistanceBetween2Points(newMousePos, startingMousePos) > Math.Pow(stepSize, 2)) // Check if the distance is far enough
            {
                DecideSnakeDirection(ref newMousePos, ref startingMousePos);

                game.MoveSnakeBy1Step(startingMousePos);

                TrimSnake();
            }
        }

        double DistanceBetween2Points(Point p1, Point p2) // The square of the distance between two points (avoids to calculate square root)
        {
            var distanceX = p1.X - p2.X;
            var distanceY = p1.Y - p2.Y;
            return Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2);
        }

        private void DecideSnakeDirection(ref Point currentPoint, ref Point oldPoint)
        {
            var distanceX = currentPoint.X - oldPoint.X;
            var distanceY = currentPoint.Y - oldPoint.Y;

            if (Math.Abs(distanceX) > Math.Abs(distanceY)) // Test in which direction the snake is going
                startingMousePos.X += Math.Sign(distanceX) * stepSize;
            else
                startingMousePos.Y += Math.Sign(distanceY) * stepSize;
        }

        private void TrimSnake()
        {
            if (game.SnakeLength() > 10)
                game.TrimSnakeTailBy1();
        }

        private void btnEasy_Click(object sender, RoutedEventArgs e)
        {
            game.ResetGame(txtRules, ScoreLbl);
            game.SetGame(txtRules, ScoreLbl);
            SetCursorPos(200, 200);
            GlobalBooleans.isGameStarted = true;
            game.EasyGame(txtRules, ScoreLbl);
        }

        private void btnHard_Click(object sender, RoutedEventArgs e)
        {
            game.ResetGame(txtRules, ScoreLbl);
            game.SetGame(txtRules, ScoreLbl);
            SetCursorPos(200, 200);
            GlobalBooleans.isGameStarted = true;
            game.HardGame(txtRules, ScoreLbl);
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            txtRules.Visibility = Visibility.Visible;
            txtRules.Text = "Basic Rules:" + Environment.NewLine;
            txtRules.AppendText("1. Do not hit the walls" + Environment.NewLine);
            txtRules.AppendText("2. Avoid eating yourself" + Environment.NewLine);
            txtRules.AppendText("3. Avoid hitting obstacles" + Environment.NewLine);
            txtRules.AppendText("4. Try getting as much food" + Environment.NewLine);
            txtRules.AppendText("5. Set Difficulty with the buttons above" + Environment.NewLine);
            txtRules.AppendText("6. Enjoy" + Environment.NewLine);
            canvas.Visibility = Visibility.Hidden;
        }
    }
}
