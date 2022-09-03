using System.Windows;
using System.Windows.Forms;

namespace InternetSwitcher {

    static internal class ScreenResolution {

        public static int scaleOfScreen = 100 * Screen.PrimaryScreen.Bounds.Width / rWidth;


        public static int rWidth => (int)SystemParameters.PrimaryScreenWidth;

        public static int rHeight => (int)SystemParameters.PrimaryScreenHeight;


        public static int wWidth => Screen.PrimaryScreen.Bounds.Width;

        public static int wHeight => Screen.PrimaryScreen.Bounds.Height;

    }
}