#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2016
// darko.juric2@gmail.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
//using System.Drawing;
using System.IO;
using System.Linq;
using DotImaging;
using DotImaging.Linq;
using DotImaging.Primitives2D;
using System.Runtime.InteropServices;

namespace Test
{
    class Program
    {
        static void TestDrawingFunctions()
        {
            var resourceDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Resources");
            var imgColor = ImageIO.LoadColor(Path.Combine(resourceDir, "testColorBig.jpg")).Clone();

            //imgColor.DrawAnnotation(new Rectangle(10, 10, 500, 500), "Some text.", Font.Big);
            //imgColor.Draw("Some text.", new Font(FontTypes.HERSHEY_PLAIN, 10, 10, 10), new Point(50, 50), Bgr<byte>.Red, 85);
            //imgColor.Draw(new Rectangle(10, 10, 500, 500), Bgr<byte>.Red, -1, 85);
            //imgColor.Draw(new Circle(500, 500, 250), Bgr<byte>.Blue, -1, 128);
            //imgColor.Draw(new Ellipse(new PointF(500, 500), new SizeF(200, 500), 45), Bgr<byte>.Green, 10, 25);
            //imgColor.Draw(new Box2D(new PointF(500, 500), new SizeF(200, 500), 45), Bgr<byte>.Green, 10, 255);
            imgColor.Draw(new Point[] { new Point(10, 10), new Point(500, 10), new Point(500, 600), new Point(25, 10) }, Bgr<byte>.Red, 10, 128);

            imgColor.Show();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        unsafe static void Main()
        {
            TestDrawingFunctions();
            return;

            //test get-rectangle async
            var resourceDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Resources");
            var imgColor = ImageIO.LoadColor(Path.Combine(resourceDir, "testColorBig.jpg")).Clone();

            int i = 0;
            while (true)
            {
                var im = imgColor.Clone<Bgr<byte>>();
                im.Draw(i.ToString(), Font.Big, new Point(45, 45), Bgr<byte>.Red);

                im.GetRectangle(onDrawn: (rect) => Console.WriteLine(rect));
                i++;
            }
            UI.CloseAll();
            return;

            //test Menu
            var selectedIdx = UI.ShowMenu(itemNames: new string[] { "Option A", "Option B" },
                                          actions: new Action[] { () => Console.WriteLine("Option A"), () => Console.WriteLine("Option B") });

            Console.WriteLine("Selected option: {0}", selectedIdx);
            UI.CloseAll();
            return;

            //test image encoding-decoding
            byte[] arr = imgColor.EncodeAsPng();
            var decodedIm = arr.DecodeAsColorImage();
            decodedIm.Save("out.bmp");

            var rImg = imgColor.AsEnumerable().Select(x => x.G).ToArray2D(imgColor.Width(), imgColor.Height());                    
         }

    }
}
