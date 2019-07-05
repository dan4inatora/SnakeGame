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

namespace AlfaRelease
{
    public class SnakePlayingField
    {
        private Canvas canvas;
        private Snake snake;
        private Queue<Food> foodholder = new Queue<Food>();
        private List<Obstacle> obstacles = new List<Obstacle>();
        public ref Snake Snake { get {return ref snake; } }

        public ref Canvas Canvas { get {return ref canvas; } }
        public ref List<Obstacle> Obstacles { get {return ref obstacles; } }
        public int FoodCount { get {return foodholder.Count; } }
        public Queue<Food> Food { get {return foodholder; } }


        public SnakePlayingField(ref Canvas c, ref Snake s)
        {
            canvas = c;
            snake = s;
        }

        public void SetSnakeinCanvas()
        {
            canvas.Children.Add(snake.getPolyline);
        }

        public void SetFoodObjinCanvas(ref Food food)
        {
            Canvas.SetTop(food.GetEllipse, food.Y);
            Canvas.SetLeft(food.GetEllipse, food.X);
            canvas.Children.Add(food.GetEllipse);
            foodholder.Enqueue(food);
        }

        public void RemoveFoodObjFromCanvas(Food food)
        {
            canvas.Children.Remove(foodholder.Peek().GetEllipse);
            foodholder.Dequeue();          
        }

        public void MoveSnakeBy1Step(Point step)
        {
            snake.AddPointToHead(step);
        }

        public int SnakeLength()
        {
            return snake.getPointsCount;
        }

        public PointCollection GetSnakePoints()
        {
            return snake.GetPointCollection();
        }

        public bool HasSnakeBittenItself(Point location)
        {
            return snake.HasBittenSelf(location);
        }

        public void TrimSnakeTailBy1()
        {
            snake.RemovePointFromTail();
        }

        public void ClearSnakePoints()
        {
            snake.ClearSnakeBody();
        }

        public void DeleteSnakeFromCanvas()
        {
            canvas.Children.Remove(snake.getPolyline);
        }

        public void SetObstacleonCanvas(ref Obstacle obstacle)
        {
            Canvas.SetLeft(obstacle.Rect, obstacle.LeftCoords);
            Canvas.SetTop(obstacle.Rect, obstacle.TopCoords);
            canvas.Children.Add(obstacle.Rect);
            obstacles.Add(obstacle);
        }

        public void RemoveObstacleFromCanvas(Obstacle obstacle)
        {
            if (obstacles.Contains(obstacle))
                obstacles.Remove(obstacle);
            canvas.Children.Remove(obstacle.Rect);
        }

        public int ObstacleCount()
        {
            return obstacles.Count;
        }
    }
}
