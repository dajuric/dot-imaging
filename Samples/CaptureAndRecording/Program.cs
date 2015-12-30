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

using DotImaging;
using System;
using System.IO;

namespace CaptureAndRecording
{
    static class Program
    {
        static void Main()
        {
           var reader = new CameraCapture(0); //capture from camera
           //reader = new FileCapture(Path.Combine(getResourceDir(), "Welcome.mp4"));
           reader.Open();

           var writer = new VideoWriter(@"output.avi", reader.FrameSize, /*reader.FrameRate does not work Cameras*/ 30); //TODO: bug: FPS does not work for cameras
           writer.Open();

            Bgr<byte>[,] frame = null;
            do
            {
                reader.ReadTo(ref frame);
                if (frame == null)
                    break;

                using (var uFrame = frame.Lock())
                { writer.Write(uFrame); }

                frame.Show(scaleForm: true);
            }
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape));

            reader.Dispose();
            writer.Dispose();
        }

        private static string getResourceDir()
        {
            var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent;
            if (directoryInfo != null)
                return Path.Combine(directoryInfo.FullName, "Resources");

            return null;
        }
    }
}
