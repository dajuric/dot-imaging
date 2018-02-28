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
using DotImaging;
using System.Collections.Generic;
using System.IO;

namespace MultipleCameraCapture
{
    static class Program
    {
        static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";runtimes/win10-x64/"); //only needed if projects are directly referenced

            Console.WriteLine("Press ESC to stop playing");

            List<CameraCapture> captures = new List<CameraCapture>();

            var cameraCount = CameraCapture.CameraCount;
            if (cameraCount == 0)
            {
                Console.WriteLine("No camera device is present.");
                return;
            }

            //initialize
            for (int camIdx = 0; camIdx < cameraCount; camIdx++)
            {
                var cap = new CameraCapture(camIdx);
                cap.Open();

                captures.Add(cap);
            }

            //grab frames
            Bgr<byte>[][,] frames = new Bgr<byte>[cameraCount][,];
            do
            {
                for (int camIdx = 0; camIdx < cameraCount; camIdx++)
                {
                    captures[camIdx].ReadTo(ref frames[camIdx]);
                    if (frames[camIdx] == null)
                        break;

                    frames[camIdx].Show(String.Format("Camera {0}", camIdx), scaleForm: false);
                }
            }
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape));

            //dispose
            captures.ForEach(x => x.Dispose());
            UI.CloseAll();
        }
    }
}
