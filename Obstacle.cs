using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlfaRelease
{
    public class Obstacle
    {
        private Rectangle rect;
        private int leftCoords;
        private int topCoords;

        public int LeftCoords { get { return leftCoords; } }
        public int TopCoords { get { return topCoords; } }
        public ref Rectangle Rect { get { return ref rect; } }

        public Obstacle(int width, int height, int left, int top)
        {
            rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Width = width;
            rect.Height = height;
            rect.StrokeThickness = 10;
            leftCoords = left;
            topCoords = top;
        }
    }
}
