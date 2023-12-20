using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Windows.Forms;

namespace RemoteServer
{
    public class ScreenShot
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);




        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        const Int32 CURSOR_SHOWING = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int x;
            public int y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.x, point.y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINT ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct ICONINFO
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


        public static Bitmap CaptureCursor()
        {
            Point cursorPoint;
            if (!GetCursorPos(out cursorPoint))
                return null;

            IntPtr hdc = GetDC(IntPtr.Zero);
            if (hdc == IntPtr.Zero)
                return null;

            IntPtr hdcMem = CreateCompatibleDC(hdc);
            if (hdcMem == IntPtr.Zero)
            {
                ReleaseDC(IntPtr.Zero, hdc);
                return null;
            }

            var size = 256;
            var cursorSize = SystemInformation.CursorSize;
            IntPtr hBitmap = CreateCompatibleBitmap(hdc, size, size);
            if (hBitmap == IntPtr.Zero)
            {
                DeleteDC(hdcMem);
                ReleaseDC(IntPtr.Zero, hdc);
                return null;
            }

            IntPtr hOldBitmap = SelectObject(hdcMem, hBitmap);
            BitBlt(hdcMem, 0, 0, size, size, hdc, cursorPoint.X - size / 2, cursorPoint.Y - size / 2, 0xCC0020);
            SelectObject(hdcMem, hOldBitmap);

            Bitmap bitmap = Image.FromHbitmap(hBitmap);

            DeleteObject(hBitmap);
            DeleteDC(hdcMem);
            ReleaseDC(IntPtr.Zero, hdc);

            return bitmap;
        }

        public static Bitmap Capture2()
        {
            var size = 256;
            // Получаем размеры экрана
            //Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            Rectangle screenBounds = new Rectangle(0, 0, size, size);
            // Создаем объект Bitmap для хранения скриншота
            Bitmap bitmap = new Bitmap(screenBounds.Width, screenBounds.Height, PixelFormat.Format32bppArgb);

            // Создаем объект Graphics для получения скриншота
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
               
                var cursorPosition = GetCursorPosition();
                var x = cursorPosition.X;
                var y = cursorPosition.Y;
                // Копируем изображение экрана в объект Bitmap
                graphics.CopyFromScreen(x - size /2, y - size /2, 0, 0, screenBounds.Size, CopyPixelOperation.SourceCopy);

                // Получаем информацию о курсоре
                CURSORINFO cursorInfo = new CURSORINFO();
                cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);
                if (GetCursorInfo(out cursorInfo))
                {
                    // Если курсор видимый, наложить его изображение на скриншот
                    if (cursorInfo.flags == CURSOR_SHOWING)
                    {
                        POINT cursorPoint;
                        GetCursorPos(out cursorPoint);
                        //ScreenToClient(Handle, ref cursorPoint);

                        // Получаем изображение курсора
                        IntPtr hIcon = CopyIcon(cursorInfo.hCursor);
                        ICONINFO iconInfo;
                        GetIconInfo(hIcon, out iconInfo);

                        // Наложение изображения курсора на скриншот
                        using (Graphics cursorGraphics = Graphics.FromImage(bitmap))
                        {
                            //Point iconPoint = new Point(cursorPoint.x - iconInfo.xHotspot - x/2, cursorPoint.y - iconInfo.yHotspot - y/2);
                            Point iconPoint = new Point(size/2, size/2);
                            cursorGraphics.DrawIcon(Icon.FromHandle(hIcon), new Rectangle(iconPoint, SystemInformation.CursorSize));
                        }
                    }
                }
            }
            return bitmap;
        }
    }
}





