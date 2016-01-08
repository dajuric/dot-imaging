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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        unsafe static void Main()
        {
            var resourceDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Resources");
            var imgColor = ImageIO.LoadColor(Path.Combine(resourceDir, "testColorBig.jpg")).Clone();

            byte[] arr = imgColor.EncodeAsPng();
            var decodedIm = arr.DecodeAsColorImage();
            decodedIm.Save("out.bmp");

            var rImg = imgColor.AsEnumerable().Select(x => x.G).ToArray2D(imgColor.Width(), imgColor.Height());                    
         }

    }
}
