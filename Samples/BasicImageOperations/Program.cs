using DotImaging;
using DotImaging.Primitives2D;

namespace BasicImageOperations
{
    static class Program
    {
        static void Main()
        {
            //create a managed image
            var image = new Bgr<byte>[480, 640];

            //draw something
            image.Draw(new Rectangle(50, 50, 200, 100), Bgr<byte>.Red, -1);
            image.Draw(new Circle(50, 50, 25), Bgr<byte>.Blue, 5);
            image.Draw(new Box2D(new Point(250, 150), new Size(100, 100), 30), Bgr<byte>.Green, 1);
            image.Draw("Hello world!", Font.Big, new Point(10, 100), Bgr<byte>.White);

            //save and load
            image.Save("out.png");
            ImageIO.LoadColor("out.png").Clone().Show(scaleForm: true);
        }
    }
}
