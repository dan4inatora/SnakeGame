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
    //Class Representing food
    public class Food
    {
        #region FoodDataMembers
        private Ellipse ellipse;
        private double x;
        private double y;
        #endregion

        #region FoodProps
        public ref Ellipse GetEllipse
        {
            get { return ref ellipse; }

        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }


        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        #endregion

        #region FoodCtor
        public Food(Point foodCoords)
        {
            ellipse = new Ellipse();
            ellipse.Fill = Brushes.Red;
            ellipse.Width = 20;
            ellipse.Height = 20;
            X = foodCoords.X;
            Y = foodCoords.Y;

        }

        public Food(Food f)
        {
            this.ellipse = f.GetEllipse;
            this.ellipse.Fill = Brushes.Red;
            ellipse.Width = 20;
            ellipse.Height = 20;
            this.X = f.X;
            this.Y = f.Y;
        }

        public Food():this(new Point(0,0))
        {
        }
        #endregion

        #region FoodMethodSet

        public override string ToString()
        {
            return string.Format("" + this.X + " " + this.Y);
        }
        #endregion
    }
}
