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
        public static Bitmap Capture()
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
               
                var cursorPosition = WinApi.GetCursorPosition();
                var x = cursorPosition.X;
                var y = cursorPosition.Y;
                // Копируем изображение экрана в объект Bitmap
                graphics.CopyFromScreen(x - size /2, y - size /2, 0, 0, screenBounds.Size, CopyPixelOperation.SourceCopy);

                // Получаем информацию о курсоре
                WinApi.CURSORINFO cursorInfo = new WinApi.CURSORINFO();
                cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);
                if (WinApi.GetCursorInfo(out cursorInfo))
                {
                    // Если курсор видимый, наложить его изображение на скриншот
                    if (cursorInfo.flags == WinApi.CURSOR_SHOWING)
                    {
                        WinApi.POINT cursorPoint;
                        WinApi.GetCursorPos(out cursorPoint);
                        //ScreenToClient(Handle, ref cursorPoint);

                        // Получаем изображение курсора
                        IntPtr hIcon = WinApi.CopyIcon(cursorInfo.hCursor);
                        WinApi.ICONINFO iconInfo;
                        WinApi.GetIconInfo(hIcon, out iconInfo);

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