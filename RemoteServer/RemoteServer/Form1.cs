using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMedia;

namespace RemoteServer
{
    public partial class Form1 : Form, IController
    {
        enum MouseDirection { Left, Right, Top, Bottom }

        private Server server;
        

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

            WinApi.SendMessage(handle, downCode, wParam, lParam); // Mouse button down
            WinApi.SendMessage(handle, upCode, wParam, lParam); // Mouse button up
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

            IntPtr hWnd = WinApi.GetForegroundWindow();
           //SetForegroundWindow(hWnd);
            Console.WriteLine(hWnd);
            WinApi.SendMessage(hWnd, WM_KEYDOWN, new IntPtr(VK_SPACE), IntPtr.Zero);
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            WinApi.SendMessage(hWnd, WM_KEYUP, new IntPtr(VK_SPACE), IntPtr.Zero);
        }

        void AppCommand(WinApi.AppComandCode commandCode)
        {
            this.BeginInvoke((MethodInvoker)delegate () {
                int CommandID = (int)commandCode << 16;
                Console.WriteLine(Process.GetCurrentProcess().MainWindowHandle);
                //SendMessage(Process.GetCurrentProcess().MainWindowHandle, WM_APPCOMMAND, Process.GetCurrentProcess().MainWindowHandle, (IntPtr)CommandID);

                var hWnd = WinApi.GetForegroundWindow();
                WinApi.SendMessage(hWnd, 0, hWnd, (IntPtr)CommandID);
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
            WinApi.LeftMouseClick();
        }

        private void Move(MouseDirection mouseDirection)
        {
            var point = WinApi.GetCursorPosition();
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
            WinApi.SetCursorPos(x, y);
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
            var image = ScreenShot.Capture();
            image.Save(@"C:\tmp\snippetsource.jpg", ImageFormat.Jpeg);
        }

        public void EmitScreenCapture()
        {
            var image = ScreenShot.Capture();
            Bitmap bImage = image;  // Your Bitmap Image
            MemoryStream ms = new MemoryStream();
            bImage.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64
            server.SendImage(SigBase64);
        }

       
    }
}
