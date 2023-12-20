using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace RemoteServer
{
    public class WinApi
    {

        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;


        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);



        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        public const Int32 CURSOR_SHOWING = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.x, point.y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINT ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            public bool fIcon;
            public Int32 xHotspot;
            public Int32 yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }



        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick()
        {
            var point = WinApi.GetCursorPosition();
            int x = point.X;
            int y = point.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        public enum AppComandCode : uint
        {
            BASS_BOOST = 20,
            BASS_DOWN = 19,
            BASS_UP = 21,
            BROWSER_BACKWARD = 1,
            BROWSER_FAVORITES = 6,
            BROWSER_FORWARD = 2,
            BROWSER_HOME = 7,
            BROWSER_REFRESH = 3,
            BROWSER_SEARCH = 5,
            BROWSER_STOP = 4,
            LAUNCH_APP1 = 17,
            LAUNCH_APP2 = 18,
            LAUNCH_MAIL = 15,
            LAUNCH_MEDIA_SELECT = 16,
            MEDIA_NEXTTRACK = 11,
            MEDIA_PLAY_PAUSE = 14,
            MEDIA_PREVIOUSTRACK = 12,
            MEDIA_STOP = 13,
            TREBLE_DOWN = 22,
            TREBLE_UP = 23,
            VOLUME_DOWN = 9,
            VOLUME_MUTE = 8,
            VOLUME_UP = 10,
            MICROPHONE_VOLUME_MUTE = 24,
            MICROPHONE_VOLUME_DOWN = 25,
            MICROPHONE_VOLUME_UP = 26,
            CLOSE = 31,
            COPY = 36,
            CORRECTION_LIST = 45,
            CUT = 37,
            DICTATE_OR_COMMAND_CONTROL_TOGGLE = 43,
            FIND = 28,
            FORWARD_MAIL = 40,
            HELP = 27,
            MEDIA_CHANNEL_DOWN = 52,
            MEDIA_CHANNEL_UP = 51,
            MEDIA_FASTFORWARD = 49,
            MEDIA_PAUSE = 47,
            MEDIA_PLAY = 46,
            MEDIA_RECORD = 48,
            MEDIA_REWIND = 50,
            MIC_ON_OFF_TOGGLE = 44,
            NEW = 29,
            OPEN = 30,
            PASTE = 38,
            PRINT = 33,
            REDO = 35,
            REPLY_TO_MAIL = 39,
            SAVE = 32,
            SEND_MAIL = 41,
            SPELL_CHECK = 42,
            UNDO = 34,
            DELETE = 53,
            DWM_FLIP3D = 54,
            DOWN_CODE = 201, // Left click down code
            UP_CODE = 202, // Left click up code
        }
    }

    
}
