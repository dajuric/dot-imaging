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
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DotImaging
{
    /// <summary>
    /// Provides conversion extension methods between generic image and <see cref="System.Windows.Media.Imaging.BitmapSource"/>.
    /// </summary>
    public static class BitmpSourceConversionExtensions
    {
        #region Conversion from BitmapSource

        /// <summary>
        /// Converts the specified bitmap into the managed array.
        /// </summary>
        /// <typeparam name="TColor">Color type.</typeparam>
        /// <param name="bmpSource">Bitmap source.</param>
        /// <returns>Managed array.</returns>
        public static TColor[,] ToArray<TColor>(this BitmapSource bmpSource)
            where TColor : struct, IColor<byte>
        {
            if (bmpSource.Format.BitsPerPixel != ColorInfo.GetInfo<TColor>().Size * 8)
                throw new NotSupportedException("Color size and bitmap-source pixel format must match.");

            TColor[,] im = new TColor[bmpSource.PixelHeight, bmpSource.PixelWidth];

            using (var uIm = im.Lock())
            {
                bmpSource.CopyPixels(Int32Rect.Empty, uIm.ImageData, uIm.Stride * uIm.Height, uIm.Stride);
            }

            return im;
        }

        #endregion

        #region Conversion to BitmapSource

        /// <summary>
        /// Converts the specified managed array to the corresponding bitmap source.
        /// </summary>
        /// <param name="image">Managed array.</param>
        /// <returns>Bitmap source.</returns>
        public static BitmapSource ToBitmapSource(this Bgr<byte>[,] image)
        {
            BitmapSource bmpSource = null;
            using (var uImg = image.Lock())
            {
                bmpSource = BitmapSource.Create(uImg.Width, uImg.Height, 96, 96,
                                                PixelFormats.Bgr24, null,
                                                uImg.ImageData, uImg.Stride * uImg.Height, uImg.Stride);
            }

            return bmpSource;
        }

        /// <summary>
        /// Converts the specified managed array to the corresponding bitmap source.
        /// </summary>
        /// <param name="image">Managed array.</param>
        /// <returns>Bitmap source.</returns>
        public static BitmapSource ToBitmapSource(this Bgra<byte>[,] image)
        {
            BitmapSource bmpSource = null;
            using (var uImg = image.Lock())
            {
                bmpSource = BitmapSource.Create(uImg.Width, uImg.Height, 96, 96,
                                                PixelFormats.Bgr32, null,
                                                uImg.ImageData, uImg.Stride * uImg.Height, uImg.Stride);
            }

            return bmpSource;
        }

        /// <summary>
        /// Converts the specified managed array to the corresponding bitmap source.
        /// </summary>
        /// <param name="image">Managed array.</param>
        /// <returns>Bitmap source.</returns>
        public static BitmapSource ToBitmapSource(this Gray<byte>[,] image)
        {
            BitmapSource bmpSource = null;
            using (var uImg = image.Lock())
            {
                bmpSource = BitmapSource.Create(uImg.Width, uImg.Height, 96, 96,
                                                PixelFormats.Gray8, BitmapPalettes.Gray256,
                                                uImg.ImageData, uImg.Stride * uImg.Height, uImg.Stride);
            }

            return bmpSource;
        }

        /// <summary>
        /// Converts the specified managed array to the corresponding bitmap source.
        /// </summary>
        /// <param name="image">Managed array.</param>
        /// <returns>Bitmap source.</returns>
        public static BitmapSource ToBitmapSource(this Gray<short>[,] image)
        {
            BitmapSource bmpSource = null;
            using (var uImg = image.Lock())
            {
                bmpSource = BitmapSource.Create(uImg.Width, uImg.Height, 96, 96,
                                                PixelFormats.Gray16, BitmapPalettes.Gray256,
                                                uImg.ImageData, uImg.Stride * uImg.Height, uImg.Stride);
            }

            return bmpSource;
        }

        #endregion
    }
}
