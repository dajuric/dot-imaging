using DotImaging;
using DotImaging.Primitives2D;
using System;

namespace BasicImageOperations
{
    static class Program
    {
        static void Main()
        {
            //load from the Web
            var image = new Uri("http://vignette3.wikia.nocookie.net/disney/images/5/5d/Lena_headey_.jpg")
                            .GetBytes()
                            .DecodeAsColorImage();

            //show image
            image.Show("New Lena"); //to zoom or translate: press and hold Shift and scroll or move your mouse

            image = new Bgr<byte>[480, 640];

            //draw something
            image.Draw(new Rectangle(50, 50, 200, 100), Bgr<byte>.Red, -1);
            image.Draw(new Circle(50, 50, 25), Bgr<byte>.Blue, 5);
            image.Draw(new Box2D(new Point(250, 150), new Size(100, 100), 30), Bgr<byte>.Green, 1);
            image.Draw("Hello world!", Font.Big, new Point(10, 100), Bgr<byte>.White);

            //save and load
            image.Save("out.png");
            ImageIO.LoadColor("out.png").Clone().Show("Saved image", scaleForm: true);

            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();
            UI.CloseAll();
        }
    }
}
