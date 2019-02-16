using DotImaging;
using System.Drawing;
using System;
using System.IO;

namespace BasicImageOperations
{
    static class Program
    {
        static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";runtimes/win10-x64/"); //only needed if projects are directly referenced

            //load from the Web
            var image = new Uri("http://vignette3.wikia.nocookie.net/disney/images/5/5d/Lena_headey_.jpg")
                            .GetBytes()
                            .DecodeAsColorImage();

            //show image
            image.Show("New Lena");

            //draw something
            image.DrawRectangle(new Rectangle(50, 50, 200, 100), Bgr<byte>.Red, -1);
            image.DrawText("Hello world!", DotImaging.Font.Big, new Point(10, 100), Bgr<byte>.White);

            //save and load
            image.Save("out.png");
            ImageIO.LoadColor("out.png").ShowDialog("Saved image", autoSize: true);
        }
    }
}
