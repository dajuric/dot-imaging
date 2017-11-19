#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2018
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

using DotImaging;
using DotImaging.Primitives2D;
using System;
using System.IO;

namespace Capture
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Press ESC to stop playing");

            //var reader = new CameraCapture(0); //capture from camera
            //(reader as CameraCapture).FrameSize = new Size(640, 480);

            var reader = new FileCapture(Path.Combine(getResourceDir(), "Welcome.mp4")); //capture from video
            //var reader = new ImageDirectoryCapture(Path.Combine(getResourceDir(), "Sequence"), "*.jpg");
            reader.Open();
    
            Bgr<byte>[,] frame = null;
            do
            {
                reader.ReadTo(ref frame);
                if (frame == null)
                    break;

                frame.Show(scaleForm: true);
            }
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape));

            reader.Dispose();
            UI.CloseAll();
        }

        private static string getResourceDir()
        {
            return Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, "Resources");
        }
    }
}
