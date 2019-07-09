#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2019
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

using System.Drawing;
using System;

namespace DotImaging
{
    /// <summary>
    /// Provides color conversion extension methods.
    /// </summary>
    public static class ColorConversionExtensions
    {
        #region Gray color conversion

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="grayIm">Source image.</param>
        /// <returns>image with converted color.</returns>
        public static Bgr<byte>[,] ToBgr(this Gray<byte>[,] grayIm)
        {
            return grayIm.Convert<Gray<byte>, Bgr<byte>>(Gray<byte>.Convert);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="grayIm">Source image.</param>
        /// <param name="area">Working area.</param>
        /// <returns>image with converted color.</returns>
        public static Bgr<byte>[,] ToBgr(this Gray<byte>[,] grayIm, Rectangle area)
        {
            return grayIm.Convert<Gray<byte>, Bgr<byte>>(Gray<byte>.Convert, area);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="grayIm">Source image.</param>
        /// <returns>image with converted color.</returns>
        public static Bgra<byte>[,] ToBgra(this Gray<byte>[,] grayIm)
        {
            return grayIm.Convert<Gray<byte>, Bgra<byte>>(Gray<byte>.Convert);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="grayIm">Source image.</param>
        /// <param name="area">Working area.</param>
        /// <returns>image with converted color.</returns>
        public static Bgra<byte>[,] ToBgra(this Gray<byte>[,] grayIm, Rectangle area)
        {
            return grayIm.Convert<Gray<byte>, Bgra<byte>>(Gray<byte>.Convert, area);
        }

        #endregion

        #region Bgr color conversion

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <returns>image with converted color.</returns>
        public static Bgra<byte>[,] ToBgra(this Bgr<byte>[,] image)
        {
            return image.Convert<Bgr<byte>, Bgra<byte>>(Bgr<byte>.Convert);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <param name="area">Working area.</param>
        /// <returns>image with converted color.</returns>
        public static Bgra<byte>[,] ToBgra(this Bgr<byte>[,] image, Rectangle area)
        {
            return image.Convert<Bgr<byte>, Bgra<byte>>(Bgr<byte>.Convert, area);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <returns>image with converted color.</returns>
        public static Gray<byte>[,] ToGray(this Bgr<byte>[,] image)
        {
            return image.Convert<Bgr<byte>, Gray<byte>>(Bgr<byte>.Convert);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <param name="area">Working area.</param>
        /// <returns>image with converted color.</returns>
        public static Gray<byte>[,] ToGray(this Bgr<byte>[,] image, Rectangle area)
        {
            return image.Convert<Bgr<byte>, Gray<byte>>(Bgr<byte>.Convert, area);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <returns>image with converted color.</returns>
        public static Hsv<byte>[,] ToHsv(this Bgr<byte>[,] image)
        {
            return image.Convert<Bgr<byte>, Hsv<byte>>(Bgr<byte>.Convert);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <param name="area">Working area.</param>
        /// <returns>image with converted color.</returns>
        public static Hsv<byte>[,] ToHsv(this Bgr<byte>[,] image, Rectangle area)
        {
            return image.Convert<Bgr<byte>, Hsv<byte>>(Bgr<byte>.Convert, area);
        }

        #endregion

        #region Rgb color conversion

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <returns>image with converted color.</returns>
        public static Bgr<byte>[,] ToBgr(this Rgb<byte>[,] image)
        {
            return image.Convert<Rgb<byte>, Bgr<byte>>(Rgb<byte>.Convert);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <param name="area">Working area.</param>
        /// <returns>image with converted color.</returns>
        public static Bgr<byte>[,] ToBgr(this Rgb<byte>[,] image, Rectangle area)
        {
            return image.Convert<Rgb<byte>, Bgr<byte>>(Rgb<byte>.Convert, area);
        }

        #endregion

        #region Bgra color conversion

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <returns>Image with converted color.</returns>
        public static Bgr<byte>[,] ToBgr(this Bgra<byte>[,] image)
        {
            return image.Convert<Bgra<byte>, Bgr<byte>>(Bgra<byte>.Convert);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <param name="area">Working area.</param>
        /// <returns>Image with converted color.</returns>
        public static Bgr<byte>[,] ToBgr(this Bgra<byte>[,] image, Rectangle area)
        {
            return image.Convert<Bgra<byte>, Bgr<byte>>(Bgra<byte>.Convert, area);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <returns>Image with converted color.</returns>
        public static Gray<byte>[,] ToGray(this Bgra<byte>[,] image)
        {
            return image.Convert<Bgra<byte>, Gray<byte>>(Bgra<byte>.Convert);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <param name="area">Working area.</param>
        /// <returns>Image with converted color.</returns>
        public static Gray<byte>[,] ToGray(this Bgra<byte>[,] image, Rectangle area)
        {
            return image.Convert<Bgra<byte>, Gray<byte>>(Bgra<byte>.Convert, area);
        }
        #endregion

        #region Hsv color conversion

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <returns>image with converted color.</returns>
        public static Bgr<byte>[,] ToBgr(this Hsv<byte>[,] image)
        {
            return image.Convert<Hsv<byte>, Bgr<byte>>(Hsv<byte>.Convert);
        }

        /// <summary>
        /// Converts the source color to the destination color.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <param name="area">Working area.</param>
        /// <returns>image with converted color.</returns>
        public static Bgr<byte>[,] ToGray(this Hsv<byte>[,] image, Rectangle area)
        {
            return image.Convert<Hsv<byte>, Bgr<byte>>(Hsv<byte>.Convert, area);
        }

        #endregion



        #region Unmanaged image and array cloning (ToBgr(), ToBgra(), ToGray())

        /// <summary>
        /// Converts the specified image into Bgr managed image.
        /// </summary>
        /// <param name="image">Bgr, Bgra or Gray type image.</param>
        /// <returns>Bgr image or null if conversion can not be performed.</returns>
        public static Bgr<byte>[,] ToBgr(this IImage image)
        {
            if (image is Image<Bgra<byte>>)
            {
                return (image as Image<Bgra<byte>>).Clone().ToBgr();
            }
            else if (image is Image<Bgr<byte>>)
            {
                return (image as Image<Bgr<byte>>).Clone();
            }
            else if (image is Image<Gray<byte>>)
            {
                return (image as Image<Gray<byte>>).Clone().ToBgr();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the specified image into Bgra managed image.
        /// </summary>
        /// <param name="image">Bgr, Bgra or Gray type image.</param>
        /// <returns>Bgra image or null if conversion can not be performed.</returns>
        public static Bgra<byte>[,] ToBgra(this IImage image)
        {
            if (image is Image<Bgra<byte>>)
            {
                return (image as Image<Bgra<byte>>).Clone();
            }
            else if (image is Image<Bgr<byte>>)
            {
                return (image as Image<Bgr<byte>>).Clone().ToBgra();
            }
            else if (image is Image<Gray<byte>>)
            {
                return (image as Image<Gray<byte>>).Clone().ToBgra();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the specified image into gray managed image.
        /// </summary>
        /// <param name="image">Bgr, Bgra or Gray type image.</param>
        /// <returns>Gray image or null if conversion can not be performed.</returns>
        public static Gray<byte>[,] ToGray(this IImage image)
        {
            if (image is Image<Bgra<byte>>)
            {
                return (image as Image<Bgra<byte>>).ToGray();
            }
            else if (image is Image<Bgr<byte>>)
            {
                return (image as Image<Bgr<byte>>).Clone().ToGray();
            }
            else if (image is Image<Gray<byte>>)
            {
                return (image as Image<Gray<byte>>).Clone();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the specified 2D array into Bgr managed image.
        /// <para>If the specified array is the Bgr managed image, the source is returned.</para>
        /// </summary>
        /// <param name="array2D">Bgr, Bgra or Gray type bitmap.</param>
        /// <returns>Bgr image or null if conversion can not be performed.</returns>
        public static Bgr<byte>[,] ToBgr(this Array array2D)
        {
            if (array2D is Bgra<byte>[,])
            {
                return ((Bgra<byte>[,])array2D).ToBgr();
            }
            else if (array2D is Bgr<byte>[,])
            {
                return ((Bgr<byte>[,])array2D);
            }
            else if (array2D is Gray<byte>[,])
            {
                return ((Gray<byte>[,])array2D).ToBgr();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the specified 2D array into Bgra managed image.
        /// <para>If the specified array is the Bgra managed image, the source is returned.</para>
        /// </summary>
        /// <param name="array2D">Bgra, Bgr or Gray type bitmap.</param>
        /// <returns>Bgra image or null if conversion can not be performed.</returns>
        public static Bgra<byte>[,] ToBgra(this Array array2D)
        {
            if (array2D is Bgra<byte>[,])
            {
                return (Bgra<byte>[,])array2D;
            }
            else if (array2D is Bgr<byte>[,])
            {
                return ((Bgr<byte>[,])array2D).ToBgra();
            }
            else if (array2D is Gray<byte>[,])
            {
                return ((Gray<byte>[,])array2D).ToBgra();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the specified 2D array into gray managed image.
        /// <para>If the specified array is the gray managed image, the source is returned.</para>
        /// </summary>
        /// <param name="array2D">Bgra, Bgr or Gray type bitmap.</param>
        /// <returns>Gray image or null if conversion can not be performed.</returns>
        public static Gray<byte>[,] ToGray(this Array array2D)
        {
            if (array2D is Bgra<byte>[,])
            {
                return ((Bgra<byte>[,])array2D).ToGray();
            }
            else if (array2D is Bgr<byte>[,])
            {
                return ((Bgr<byte>[,])array2D).ToGray();
            }
            else if (array2D is Gray<byte>[,])
            {
                return (Gray<byte>[,])array2D;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
