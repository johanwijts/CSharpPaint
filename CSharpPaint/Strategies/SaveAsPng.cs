using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace CSharpPaint.Strategies
{
    public class SaveAsPng : IStrategy
    {
        public SaveAsPng(Window window, Canvas canvas, int dpi, string filename)
        {
            this.window = window;
            this.canvas = canvas;
            this.dpi = dpi;
            this.filename = filename;

        }

        private Window window;
        private Canvas canvas;
        private int dpi;
        private string filename;

        public void Execute()
        {
            SaveCanvasToFile(window, canvas, dpi, filename);
        }

        public static void SaveCanvasToFile(Window window, Canvas canvas, int dpi, string filename)
        {
            Size size = new Size(window.Width, window.Height);
            canvas.Measure(size);
            //canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)window.Width, //width
                (int)window.Height, //height
                dpi, //dpi x
                dpi, //dpi y
                PixelFormats.Pbgra32 // pixelformat
                );
            rtb.Render(canvas);

            SaveRTBAsPNGBMP(rtb, filename);
        }

        private static void SaveRTBAsPNGBMP(RenderTargetBitmap bmp, string filename)
        {
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
    }
}
