using System;
using System.Runtime.InteropServices;

namespace InternetSwitcher {

    static internal class EdgeSupport {
        public enum DWMWA {

            DWMWA_WINDOW_CORNER_PREFERENCE = 33
        }

        public enum DWMWCP {

            DWMWCP_DEFAULT = 0,
            DWMWCP_DONOTROUND = 1,
            DWMWCP_ROUND = 2,
            DWMWCP_ROUNDSMALL = 3
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern long DwmSetWindowAttribute(IntPtr hwnd, DWMWA attribute, ref DWMWCP pvAttribute, uint cbAttribute);
    }
}
