using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMedia;

namespace RemoteServer
{
    public partial class Form1 : Form, IController
    {
        enum MouseDirection { Left, Right, Top, Bottom }


        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        private Server server;

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

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


        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick()
        {
            var point = GetCursorPosition();
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

        public Form1()
        {
            InitializeComponent();
        }

        private static void MouseClick(int x, int y, IntPtr handle) //handle for the browser window
        {
            IntPtr lParam = (IntPtr)((y << 16) | x); // The coordinates
            IntPtr wParam = IntPtr.Zero; // Additional parameters for the click (e.g. Ctrl)

            const uint downCode = 0x201; // Left click down code
            const uint upCode = 0x202; // Left click up code

            SendMessage(handle, downCode, wParam, lParam); // Mouse button down
            SendMessage(handle, upCode, wParam, lParam); // Mouse button up
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            moveTest();
        }

        private void btSpace_Click(object sender, EventArgs e)
        {
            start_Vid_Click();
        }

        private async void start_Vid_Click()
        {
            
            await Task.Delay(TimeSpan.FromSeconds(3));
            const uint WM_KEYDOWN = 0x0100;
            const uint WM_KEYUP = 0x0101;
            const int VK_SPACE = 0x20;

            IntPtr hWnd = GetForegroundWindow();
           //SetForegroundWindow(hWnd);
            Console.WriteLine(hWnd);
            SendMessage(hWnd, WM_KEYDOWN, new IntPtr(VK_SPACE), IntPtr.Zero);
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            SendMessage(hWnd, WM_KEYUP, new IntPtr(VK_SPACE), IntPtr.Zero);
        }

        void AppCommand(AppComandCode commandCode)
        {
            this.BeginInvoke((MethodInvoker)delegate () {
                int CommandID = (int)commandCode << 16;
                Console.WriteLine(Process.GetCurrentProcess().MainWindowHandle);
                //SendMessage(Process.GetCurrentProcess().MainWindowHandle, WM_APPCOMMAND, Process.GetCurrentProcess().MainWindowHandle, (IntPtr)CommandID);

                var hWnd = GetForegroundWindow();
                SendMessage(hWnd, 0, hWnd, (IntPtr)CommandID);
            });
        }

        private void btServer_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                server = new Server(this);
                server.start();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.stop();
        }

        public void VolumeUp()
        {
            throw new NotImplementedException();
        }

        public void VolumeDown()
        {
            throw new NotImplementedException();
        }

        public void Prevoius()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void PlayPause()
        {
            throw new NotImplementedException();
        }

        public void MoveLeft()
        {
            Move(MouseDirection.Left);
        }

        public void MoveTop()
        {
            Move(MouseDirection.Top);
        }


        public void MoveRight()
        {
            Move(MouseDirection.Right);
        }

        public void MoveBottom()
        {
            Move(MouseDirection.Bottom);
        }

        public void Click()
        {
            LeftMouseClick();
        }

        private void Move(MouseDirection mouseDirection)
        {
            var point = GetCursorPosition();
            int x = point.X;
            int y = point.Y;
            int pixels = 10;
            switch (mouseDirection)
            {
                case MouseDirection.Left:
                    x -= pixels;
                    break;
                case MouseDirection.Top:
                    y -= pixels;
                    break;
                case MouseDirection.Right:
                    x += pixels;
                    break;
                case MouseDirection.Bottom:
                    y += pixels;
                    break;

            }    
            Console.WriteLine($"{x} {y}");
            SetCursorPos(x, y);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            moveTest();
        }

        private async void moveTest()
        {
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                MoveRight();
            }
        }

        private void btCapture_Click(object sender, EventArgs e)
        {
            //var image = ScreenCapture.Capture();
            var image = ScreenShot.Capture2();
            Bitmap bImage = image;  // Your Bitmap Image
            System.IO.MemoryStream ms = new MemoryStream();
            bImage.Save(ms, ImageFormat.Jpeg);
            image.Save(@"C:\tmp\snippetsource.jpg", ImageFormat.Jpeg);
        }

        public void EmitScreenCapture()
        {
            var image = ScreenShot.Capture2();
            Bitmap bImage = image;  // Your Bitmap Image
            System.IO.MemoryStream ms = new MemoryStream();
            bImage.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64
            server.SendImage(SigBase64);
            // image.Save(@"C:\tmp\snippetsource.jpg", ImageFormat.Jpeg);
        }

       
    }
}
