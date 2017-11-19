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

using DotImaging.Primitives2D;
using System;

namespace DotImaging
{
    /// <summary>
    /// Provides basic image extensions for a two-dimensional array.
    /// </summary>
    public static class ArrayImageBasicExtensions
    {
        /// <summary>
        /// Gets image width.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="image">Image.</param>
        /// <returns>The image width.</returns>
        public static int Width<T>(this T[,] image)
        {
            return image.GetLength(1);
        }

        /// <summary>
        /// Gets image height.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="image">Image.</param>
        /// <returns>The image height.</returns>
        public static int Height<T>(this T[,] image)
        {
            return image.GetLength(0);
        }

        /// <summary>
        /// Gets image size.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="image">Image.</param>
        /// <returns>The image size.</returns>
        public static Size Size<T>(this T[,] image)
        {
            return new Size(image.Width(), image.Height());
        }

        /// <summary>
        /// Pins the array and returns the corresponding generic image.
        /// </summary>
        /// <typeparam name="TColor">Color type.</typeparam>
        /// <param name="array">The array to lock.</param>
        /// <returns>The generic image which shares data with the pined array.</returns>
        public static Image<TColor> Lock<TColor>(this TColor[,] array)
            where TColor : struct
        {
            return Image<TColor>.Lock(array);
        }

        /// <summary>
        /// Pins the array and returns the corresponding generic image of a specified portion.
        /// </summary>
        /// <typeparam name="TColor">Color type.</typeparam>
        /// <param name="array">The array to lock.</param>
        /// <param name="area">Working area.</param>
        /// <returns>The generic image which shares data with the pined array.</returns>
        public static Image<TColor> Lock<TColor>(this TColor[,] array, Rectangle area)
          where TColor : struct
        {
            return Image<TColor>.Lock(array).GetSubRect(area);
        }

        /// <summary>
        /// Sets all elements of the array to the default value.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array to clear.</param>
        public static void Clear<T>(this T[,] array)
            where T: struct
        {
            Array.Clear(array, 0, array.Length);
        }

        /// <summary>
        /// Performs deep cloning of the specified array.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array.</param>
        /// <returns>Cloned array.</returns>
        public static T[,] Clone<T>(this T[,] array)
            where T: struct
        {
            return (T[,])array.Clone();
        }

        /// <summary>
        /// Creates new array of the same size as the source array.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array.</param>
        /// <returns>New empty array.</returns>
        public static T[,] CopyBlank<T>(this T[,] array)
        {
            return new T[array.Height(), array.Width()];
        }

        /// <summary>
        /// Gets the element info of the specified array.
        /// </summary>
        /// <typeparam name="TColor">Element type.</typeparam>
        /// <param name="source">Array.</param>
        /// <returns>Array element info.</returns>
        public static ColorInfo ColorInfo<TColor>(this TColor[,] source)
            where TColor: struct
        {
            return DotImaging.ColorInfo.GetInfo<TColor>();
        }

        /// <summary>
        /// Calculates image stride for the specified alignment.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="allignment">Data alignment.</param>
        /// <returns>Image stride.</returns>
        public static int CalculateStride<TImage>(this TImage image, int allignment = 4)
            where TImage: IImage
        {
            int stride = image.Width * image.ColorInfo.Size;

            if (allignment != 0 &&
                stride % allignment != 0)
            {
                stride += (allignment - (stride % allignment));
            }

            return stride;
        }

        /// <summary>
        /// Applies the specified conversion function to each source pixel, producing the destination image.
        /// </summary>
        /// <typeparam name="TSrc">Source element type.</typeparam>
        /// <typeparam name="TDst">Destination element type.</typeparam>
        /// <param name="source">Source array.</param>
        /// <param name="convert">Pixel conversion function.</param>
        /// <returns>Destination array.</returns>
        public static TDst[,] Convert<TSrc, TDst>(this TSrc[,] source, Func<TSrc, TDst> convert)
        {
            Size imSize = source.Size();
            TDst[,] dest = new TDst[imSize.Height, imSize.Width];

            ParallelLauncher.Launch(thread => 
            {
                dest[thread.Y, thread.X] = convert(source[thread.Y, thread.X]);
            }, 
            source.Width(), source.Height());

            return dest;
        }

        /// <summary>
        /// Applies the specified conversion function to each source pixel, producing the destination image.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Source array.</param>
        /// <param name="apply">Pixel conversion function.</param>
        /// <param name="inPlace">
        /// True to apply the function in place, false otherwise.
        /// <para>If true the result image is the same as source image.</para>
        /// </param>
        /// <returns>Destination array.</returns>
        public static T[,] Apply<T>(this T[,] source, Func<T, T> apply, bool inPlace = false)
        {
            Size imSize = source.Size();
            T[,] dest = inPlace ? source : new T[imSize.Height, imSize.Width];

            ParallelLauncher.Launch(thread =>
            {
                dest[thread.Y, thread.X] = apply(source[thread.Y, thread.X]);
            },
            source.Width(), source.Height());

            return dest;
        }

        #region Set value

        /// <summary>
        /// Sets the specified value for each element of the array.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array with value type elements.</param>
        /// <param name="value">Value to set.</param>
        public static void SetValue<T>(this T[,] array, T value)
            where T : struct
        {
            ParallelLauncher.Launch((thread) =>
            {
                array[thread.Y, thread.X] = value;
            },
            array.Width(), array.Height());
        }

        /// <summary>
        /// Sets the specified value for each element of the array.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array with value type elements.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="area">Working area.</param>
        public static void SetValue<T>(this T[,] array, T value, Rectangle area)
            where T : struct
        {
            ParallelLauncher.Launch((thread) =>
            {
                array[thread.Y + area.Y, thread.X + area.X] = value;
            },
            area.Width, area.Height);
        }

        /// <summary>
        /// Sets the specified value for only those element of the array where the mask is true.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array with value type elements.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="area">Working area.</param>
        /// <param name="mask">Mask.</param>
        public static void SetValue<T>(this T[,] array, T value, Rectangle area, bool[,] mask)
        {
            if (array.Size() != mask.Size())
                throw new ArgumentException("Array and mask must have the same size.");

            ParallelLauncher.Launch((thread) =>
            {
                if (mask[thread.Y + area.Y, thread.X + area.X])
                    array[thread.Y + area.Y, thread.X + area.X] = value;
            },
            area.Width, area.Height);
        }

        /// <summary>
        /// Sets the specified value for only those element of the array where the mask is non-zero.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array with value type elements.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="area">Working area.</param>
        /// <param name="mask">Mask.</param>
        public static void SetValue<T>(this T[,] array, T value, Rectangle area, Gray<byte>[,] mask)
        {
            if (array.Size() != mask.Size())
                throw new ArgumentException("Array and mask must have the same size.");

            ParallelLauncher.Launch((thread) =>
            {
                if (mask[thread.Y + area.Y, thread.X + area.X] != 0)
                    array[thread.Y + area.Y, thread.X + area.X] = value;
            },
            area.Width, area.Height);
        }

        /// <summary>
        /// Sets the specified value for only those element of the array where the mask is non-zero.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array with value type elements.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="mask">Mask.</param>
        public static void SetValue<T>(this T[,] array, T value, Gray<byte>[,] mask)
        {
            if (array.Size() != mask.Size())
                throw new ArgumentException("Array and mask must have the same size.");

            ParallelLauncher.Launch((thread) =>
            {
                if (mask[thread.Y, thread.X] != 0)
                    array[thread.Y, thread.X] = value;
            },
            array.Width(), array.Height());
        }

        /// <summary>
        /// Sets the specified value for only those element of the array where the mask is true.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array with value type elements.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="mask">Mask.</param>
        public static void SetValue<T>(this T[,] array, T value, bool[,] mask)
        {
            if (array.Size() != mask.Size())
                throw new ArgumentException("Array and mask must have the same size.");

            ParallelLauncher.Launch((thread) =>
            {
                if (mask[thread.Y, thread.X])
                    array[thread.Y, thread.X] = value;
            },
            array.Width(), array.Height());
        }

        #endregion
    }
}
