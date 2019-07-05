using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AlfaRelease
{

    public class Game
    {
        private SnakePlayingField canvasContainer;
        private int score = 0;

        public ref Snake Snake { get { return ref canvasContainer.Snake; } }
        public ref List<Obstacle> Obstacles { get { return ref canvasContainer.Obstacles; } }
        public int FoodCount { get { return canvasContainer.FoodCount; } }
        public Queue<Food> Food { get { return canvasContainer.Food; } }

        public Game(ref Canvas c, ref Snake s)
        {
            canvasContainer = new SnakePlayingField(ref c, ref s);
        }

        public void SetFoodObjinCanvas(ref Food food)
        {
            canvasContainer.SetFoodObjinCanvas(ref food);
        }

        public void RemoveFoodObjFromCanvas(Food food)
        {
            canvasContainer.RemoveFoodObjFromCanvas(food);
        }

        public void SetSnakeinCanvas()
        {
            canvasContainer.SetSnakeinCanvas();
        }

        public void MoveSnakeBy1Step(Point step)
        {
            canvasContainer.MoveSnakeBy1Step(step);
        }

        public int SnakeLength()
        {
            return canvasContainer.SnakeLength();
        }

        public PointCollection GetSnakePoints()
        {
            return canvasContainer.GetSnakePoints();
        }

        private bool HasSnakeBittenItself(Point location)
        {
            return canvasContainer.HasSnakeBittenItself(location);
        }

        public void TrimSnakeTailBy1()
        {
            canvasContainer.TrimSnakeTailBy1();
        }

        private void SetObstacleonCanvas(ref Obstacle obstacle)
        {
            canvasContainer.SetObstacleonCanvas(ref obstacle);
        }

        private void RemoveObstacleFromCanvas(Obstacle obstacle)
        {
            canvasContainer.RemoveObstacleFromCanvas(obstacle);
        }

        private int ObstacleCount()
        {
            return canvasContainer.ObstacleCount();
        }

        #region SetResetGame
        public void ResetGame(TextBox txtBox, Label label)
        {
            ResetFlags();
            ResetVisivility(txtBox);
            ResetText(label, txtBox);
            score = 0;
            Food.Clear();
            ClearObstacles();
            ClearSnake();
        }

        private void ResetFlags()
        {
            GlobalBooleans.isEating = false;
            GlobalBooleans.FoodSpawnFlag = false;
            GlobalBooleans.FoodDeSpawnFlag = false;
            GlobalBooleans.FoodFlag = false;
            GlobalBooleans.isGameStarted = false;
        }

        private void ResetVisivility(TextBox txtBox)
        {
            txtBox.Visibility = Visibility.Hidden;
            canvasContainer.Canvas.Visibility = Visibility.Hidden;
        }

        private void ResetText(Label l, TextBox txtBox)
        {
            l.Content = "Game Ended Score is : " + score;
            txtBox.Text = "";
        }

        public void SetGame(TextBox textBox, Label label)
        {
            SetFlags();
            SetVisibility(textBox);
            SetText(label, textBox);
            canvasContainer.SetSnakeinCanvas();
        }

        private void SetFlags()
        {
            GlobalBooleans.isEating = false;
            GlobalBooleans.FoodSpawnFlag = true;
            GlobalBooleans.FoodDeSpawnFlag = true;
            GlobalBooleans.FoodFlag = true;
        }

        private void SetVisibility(TextBox textBox)
        {
            canvasContainer.Canvas.Visibility = Visibility.Visible;
            textBox.Visibility = Visibility.Hidden;
        }

        private void SetText(Label label, TextBox textBox)
        {
            label.Content = "Score is : " + score;
            textBox.Text = "";
        }
        #endregion

        #region ColisonDetection
        private void ClearObstacles()
        {
            for (int i = 0; i < ObstacleCount(); i++)
            {
                canvasContainer.RemoveObstacleFromCanvas(Obstacles[i]);
            }
            Obstacles.Clear();
        }

        private void ClearSnake()
        {
            canvasContainer.DeleteSnakeFromCanvas();
            canvasContainer.ClearSnakePoints();
        }


        //set to private after
        public bool ObstacleCollided(Point location)
        {
            bool flag = false;
            for (int i = 0; i < ObstacleCount(); i++)
            {
                if (CheckifPointinRectangle(Obstacles[i].Rect, location))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private async void IsSnakeCurrentlyEating(Label l)
        {
            while (GlobalBooleans.FoodFlag)
            {
                await Task.Delay(50);
                if (FoodCount > 0)
                {
                    var temp = Food.Peek();
                    if (CheckifPointinEllipse(temp.GetEllipse, GlobalBooleans.CurrentCursorPosition))
                    {
                        GlobalBooleans.IsEating = true;
                        EatFood(temp);
                        SetScore(l);
                    }
                }
                else
                    GlobalBooleans.isEating = false;
            }
        }

        private void EatFood(Food food)
        {
            RemoveFoodObjFromCanvas(food);
            MoveSnakeBy1Step(new Point(GlobalBooleans.CurrentCursorPosition.X, GlobalBooleans.CurrentCursorPosition.Y));
        }

        private void SetScore(Label label)
        {
            score++;
            label.Content = "Score : " + score;
        }

        private async void SnakeColision(TextBox textBox, Label label)
        {
            // async see if the snake has eaten itself and if so end the game
            while (GlobalBooleans.snakeNotBitSelf)
            {
                await Task.Delay(10);
                if (!GlobalBooleans.IsEating)
                {
                    if (HasSnakeBittenItself(GlobalBooleans.currentCursorPosition))
                    {
                        ResetGame(textBox, label);

                    }

                }
            }
        } 
        #endregion

        #region FoodSpawnDeSpawnHandler
        public void FoodHandler(int spawnDelay, int despawnDelay)
        {
            FoodSpawnHandler(spawnDelay);
            FoodDespawnHandler(despawnDelay);
        }

        private void FoodSpawnHandler(int spawnDelay) // Checks if the point is trying to spawn on an Invalid object and if so randomises the food's position
        {
            canvasContainer.Canvas.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(async () =>
            {
                Point testPoint;
                while (GlobalBooleans.FoodSpawnFlag)
                {
                    await Task.Delay(spawnDelay);
                    Food food;
                    testPoint = CreateRandomPointInCanvas();
                    while (ObstacleCollided(testPoint))
                    {
                        testPoint = CreateRandomPointInCanvas();
                    }
                    food = new Food(testPoint);
                    SetFoodObjinCanvas(ref food);
                }
            }));
        }
        private Point CreateRandomPointInCanvas()
        {
            Random rand = new Random();
            double X = rand.Next((int)canvasContainer.Canvas.ActualWidth - 10);
            double Y = rand.Next((int)canvasContainer.Canvas.ActualHeight - 50);
            Point testPoint = new Point(X, Y);
            return testPoint;
        }

        private void FoodDespawnHandler(int deSpawnDelay)
        {
            canvasContainer.Canvas.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(async () => //async deletes food from canvas
            {
                while (GlobalBooleans.FoodDeSpawnFlag)
                {
                    await Task.Delay(deSpawnDelay);
                    if (FoodCount > 0)
                    {
                        RemoveFoodObjFromCanvas(Food.Peek());
                    }
                }
            }));
        } 
        #endregion

        #region GameTypes
        public void EasyGame(TextBox textBox, Label label)
        {

            SetEasyGameObstacles();
            //SetGame(textBox, label);
            FoodHandler(9500, 4500);
            IsSnakeCurrentlyEating(label);
            SnakeColision(textBox, label);
        }

        private void SetEasyGameObstacles()
        {
            Obstacle obstacle1 = new Obstacle(15, 130, 300, 0);
            SetObstacleonCanvas(ref obstacle1);
            Obstacle obstacle2 = new Obstacle(15, 130, 450, 0);
            SetObstacleonCanvas(ref obstacle2);
            Obstacle obstacle3 = new Obstacle(150, 15, 0, 170);
            SetObstacleonCanvas(ref obstacle3);
            Obstacle obstacle4 = new Obstacle(150, 15, 627, 170);
            SetObstacleonCanvas(ref obstacle4);
            Obstacle obstacle5 = new Obstacle(15, 130, 300, 233);
            SetObstacleonCanvas(ref obstacle5);
            Obstacle obstacle6 = new Obstacle(15, 130, 450, 233);
            SetObstacleonCanvas(ref obstacle6);
        }

        public void HardGame(TextBox textBox, Label label)
        {
            //SetGame(textBox, label);
            SetHardGameObstacles();
            FoodHandler(5500, 3500);
            IsSnakeCurrentlyEating(label);
            SnakeColision(textBox, label);
        }

        private void SetHardGameObstacles()
        {
            Obstacle obstacle1 = new Obstacle(15, 90, 80, 40);
            SetObstacleonCanvas(ref obstacle1);
            Obstacle obstacle2 = new Obstacle(15, 90, 280, 40);
            SetObstacleonCanvas(ref obstacle2);
            Obstacle obstacle3 = new Obstacle(15, 90, 480, 40);
            SetObstacleonCanvas(ref obstacle3);
            Obstacle obstacle4 = new Obstacle(15, 90, 680, 40);
            SetObstacleonCanvas(ref obstacle4);
            Obstacle obstacle5 = new Obstacle(15, 90, 80, 220);
            SetObstacleonCanvas(ref obstacle5);
            Obstacle obstacle6 = new Obstacle(15, 90, 480, 220);
            SetObstacleonCanvas(ref obstacle6);
            Obstacle obstacle7 = new Obstacle(15, 90, 680, 220);
            SetObstacleonCanvas(ref obstacle7);
            Obstacle obstacle8 = new Obstacle(130, 15, 120, 170);
            SetObstacleonCanvas(ref obstacle8);
            Obstacle obstacle9 = new Obstacle(130, 15, 320, 170);
            SetObstacleonCanvas(ref obstacle9);
            Obstacle obstacle10 = new Obstacle(130, 15, 520, 170);
            SetObstacleonCanvas(ref obstacle10);
        } 
        #endregion

        #region HelperFunctions
        private bool CheckifPointinEllipse(Ellipse ellipse, Point location) // method that tests if apoint is in an ellipse
        {
            Point ellipseCentre = EllipseCentre(ellipse);
            double _xRadius = ellipse.Width / 2;
            double _yRadius = ellipse.Height / 2;
            Point normalized = new Point(location.X - ellipseCentre.X,
                                         location.Y - ellipseCentre.Y);

            if (_xRadius <= 0.0 || _yRadius <= 0.0)
                return false;

            return CheckEllipseEquasion(normalized, _xRadius, _yRadius);

        }

        private Point EllipseCentre(Ellipse ellipse)
        {
            Point ellipseCentre = new Point(
                  Canvas.GetLeft(ellipse) + (ellipse.Width / 2),
                  Canvas.GetTop(ellipse) + (ellipse.Height / 2));

            return ellipseCentre;
        }

        public bool CheckifPointinRectangle(Rectangle rectangle, Point location) // method that tests if apoint is in an ellipse
        {
            Point rectCentre = RectangleCentre(rectangle);
            double _xRadius = rectangle.Width / 2;
            double _yRadius = rectangle.Height / 2;
            Point normalized = new Point(location.X - rectCentre.X,
                                         location.Y - rectCentre.Y);

            if (_xRadius <= 0.0 || _yRadius <= 0.0)
                return false;

            return CheckEllipseEquasion(normalized, _xRadius, _yRadius);

        }

        private Point RectangleCentre(Rectangle rect)
        {
            Point rectCentre = new Point(
                  Canvas.GetLeft(rect) + (rect.Width / 2),
                  Canvas.GetTop(rect) + (rect.Height / 2));

            return rectCentre;
        }


        private bool CheckEllipseEquasion(Point normalized, double _xRadius, double _yRadius)
        {
            /* This is a more general form of the circle equation
             *
             * X^2/a^2 + Y^2/b^2 <= 1
             */
            return ((double)(Math.Pow(normalized.X, 2)) / (Math.Pow(_xRadius, 2))) + ((double)(Math.Pow(normalized.Y, 2)) / (Math.Pow(_yRadius, 2))) <= 1.0;
        } 
        #endregion

    }
}
