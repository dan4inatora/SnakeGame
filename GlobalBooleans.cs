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

namespace AlfaRelease
{
    public static class GlobalBooleans
    {
        public static bool foodFlag = true;
        public static bool isEating = false;
        public static  bool snakeNotBitSelf = true;
        public static bool foodSpawnFlag = true;
        public static bool foodDeSpawnFlag = true;
        public static Point currentCursorPosition;
        public static bool isGameStarted = false;

        public static bool IsGameStarted { get { return isGameStarted; } set { isGameStarted = value; } }
        public static bool FoodFlag { get {return foodFlag; } set {foodFlag = value; } }
        public static bool IsEating { get { return isEating; } set { isEating = value; } }
        public static bool SnakeNotBitSelf { get { return snakeNotBitSelf; } set { snakeNotBitSelf = value; } }
        public static bool FoodSpawnFlag { get { return foodSpawnFlag; } set { foodSpawnFlag = value; } }
        public static bool FoodDeSpawnFlag { get { return foodDeSpawnFlag; } set { foodDeSpawnFlag = value; } }
        public static Point CurrentCursorPosition { get {return currentCursorPosition; } set {currentCursorPosition = value; } }



    }
}
