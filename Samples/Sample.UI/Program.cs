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
using DotImaging.Linq;
using System.Linq;
using System;
using System.Threading;
using System.IO;

namespace UIDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";runtime/win10-x64/"); //only needed if projects are directly referenced

            //select color
            Bgr<byte>[,] image = new Bgr<byte>[480, 640];
            Hsv<byte> color = UI.PickColor(Bgr<byte>.Red).ToHsv();

            //select mask
            Gray<byte>[,] mask = image.GetMask();
            if (mask.AsEnumerable().Sum(x => x.Intensity) == 0) //if the mask is empty
                mask.SetValue<Gray<byte>>(Byte.MaxValue);

            //increase saturation incrementally
            for (int s = 0; s <= Byte.MaxValue; s++)
            {
                color.S = (byte)s;
                image.SetValue<Bgr<byte>>(color.ToBgr(), mask);

                image.Show(scaleForm: true);
                ((double)s / Byte.MaxValue).Progress(message: "Changing saturation");

                Thread.Sleep(50);
            }

            //save last image
            string fileName = UI.SaveImage();
            if (fileName != null) image.Save(fileName);

            //close all
            UI.CloseAll();
        }
    }
}
