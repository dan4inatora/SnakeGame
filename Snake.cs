using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlfaRelease
{
    //class Representing snake
    public class Snake
    {
        #region SnakeDataMembers
        private Polyline polyline;

        #endregion

        #region SnakeProperties
        public ref Polyline getPolyline { get { return ref polyline; } }
        public int getPointsCount { get { return polyline.Points.Count; } }
        #endregion

        #region SnakeCtor
        public Snake()
        {
            polyline = new Polyline();
            polyline.Stroke = Brushes.Green;
            polyline.StrokeThickness = 13;
            polyline.Points = new PointCollection();


        }
        #endregion

        #region SnakeMethods
        public PointCollection GetPointCollection()
        {
            return polyline.Points;
        }

        public void SetPointCollection(PointCollection p)
        {
            this.polyline.Points = p;
        }

        private void SetPointColection(PointCollection points)
        {
            polyline.Points = points;
        }

        public void AddPointToHead(Point p)
        {
            polyline.Points.Add(p);
        }

        public void RemovePointFromTail()
        {
            polyline.Points.RemoveAt(0);
        }

        public bool HasBittenSelf(Point location)
        {
            for (int i = getPointsCount - 2; i > 1; i--)
            {
                if (IsCursorOnSnakePoint(GetPointCollection()[i], location))
                {
                    return true;
                }
            }
            return false;
        }

        public void ClearSnakeBody()
        {
            GetPointCollection().Clear();
        }

        private static bool IsCursorOnSnakePoint(Point snakepoint, Point cursor)
        {
            //cursor is somewhere around 13 height and 10 width of the point
            double _xRadius = 10 / 2;
            double _yRadius = 13 / 2;

            return cursor.X <= snakepoint.X + _xRadius &&
                   cursor.X >= snakepoint.X - _xRadius &&
                   cursor.Y <= snakepoint.Y + _yRadius &&
                   cursor.Y >= snakepoint.Y - _yRadius;
        }
        #endregion
    }
}
