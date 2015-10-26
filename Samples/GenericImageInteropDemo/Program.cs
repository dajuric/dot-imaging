#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2015
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericImageInteropDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var img = new Bgr<byte>[480, 640];

            //***********************************************************************************************************************************************************************
            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("********* TColor[,] <=> Image<> conversions (built-in) ****************"); Console.ResetColor();
            //to Image<>
            Image<Bgr<byte>> lockedImg = img.Lock();
            //from Image<>
            var arr = lockedImg.Clone();

            //***********************************************************************************************************************************************************************
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("********* Image<,> <=> OpenCV conversions (built-in) ****************"); Console.ResetColor();
            //to IplImage
            IplImage iplImage;
            using (var uImg = img.Lock())
            {
                iplImage = uImg.AsCvIplImage(); //data is shared
            }
            //from IplImage
            var imgFromIpl = iplImage.AsImage(); 

            //***********************************************************************************************************************************************************************
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("*********** Image<,> <=> Bitmap conversions (BitmapInterop) ****************"); Console.ResetColor();
            //to Bitmap
            var bmp = img.ToBitmap();
            //from Bitmap
            var imgFromBmp = bmp.ToArray();
        }
    }
}
