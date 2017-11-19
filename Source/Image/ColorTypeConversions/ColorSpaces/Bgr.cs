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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotImaging
{
      /// <summary>
    /// Represents Bgr color type of type <typeparam name="T">color depth</typeparam>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Bgr<T> : IColor3<T>
        where T : struct
    {
        /// <summary>
        /// Creates new Bgr color.
        /// </summary>
        /// <param name="b">Blue</param>
        /// <param name="g">Green</param>
        /// <param name="r">Red</param>
        public Bgr(T b, T g, T r)
        {
            this.B = b;
            this.G = g;
            this.R = r;
        }

        /// <summary>
        /// Gets or sets the blue component.
        /// </summary>
        public T B;
        /// <summary>
        /// Gets or sets the green component.
        /// </summary>
        public T G;
        /// <summary>
        /// Gets or sets the red component.
        /// </summary>
        public T R;

        /// <summary>
        /// Gets the string color representation.
        /// </summary>
        /// <returns>String color representation.</returns>
        public override string ToString()
        {
            return string.Format("B: {0}, G: {1}, R: {2}", B, G, R);
        }

        /// <summary>
        /// Gets the index of the blue component.
        /// </summary>
        public const int IdxB = 0;
        /// <summary>
        /// Gets the index of the green component.
        /// </summary>
        public const int IdxG = 1;
        /// <summary>
        /// Gets the index of the red component.
        /// </summary>
        public const int IdxR = 2;

        /// <summary>
        /// Gets the 8-bit red color.
        /// </summary>
        public static Bgr<byte> Red { get { return new Bgr<byte> { B = 0, G = 0, R = byte.MaxValue }; } }
        /// <summary>
        /// Gets the 8-bit blue color.
        /// </summary>
        public static Bgr<byte> Blue { get { return new Bgr<byte> { B = byte.MaxValue, G = 0, R = 0 }; } }
        /// <summary>
        /// Gets the 8-bit green color.
        /// </summary>
        public static Bgr<byte> Green { get { return new Bgr<byte> { B = 0, G = byte.MaxValue, R = 0 }; } }
        /// <summary>
        /// Gets the 8-bit black color.
        /// </summary>
        public static Bgr<byte> Black { get { return new Bgr<byte> { B = 0, G = 0, R = 0 }; } }
        /// <summary>
        /// Gets the 8-bit white color.
        /// </summary>
        public static Bgr<byte> White { get { return new Bgr<byte> { B = byte.MaxValue, G = byte.MaxValue, R = byte.MaxValue }; } }

        /// <summary>
        /// Converts 8-bit Bgr to 8-bit gray intensity. 
        /// </summary>
        /// <param name="bgr">Source color.</param>
        /// <param name="gray">Destination color.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Convert(Bgr<byte> bgr, ref Gray<byte> gray)
        {
            int val = ((bgr.R << 1) +           //2 * red
                       (bgr.G << 2) + bgr.G +  //5 * green
                        bgr.B                   //1 * blue

                      ) >> 3;                   //divide by 8

            gray.Intensity = (byte)val;
        }

        /// <summary>
        /// Converts 8-bit Bgr to 8-bit Bgra. 
        /// </summary>
        /// <param name="bgr">Source color.</param>
        /// <param name="bgra">Destination color.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Convert(Bgr<byte> bgr, ref Bgra<byte> bgra)
        {
            bgra.B = bgr.B;
            bgra.G = bgr.G;
            bgra.R = bgr.R;
            bgra.A = System.Byte.MaxValue;
        }

        /// <summary>
        /// Converts 8-bit Bgr to 8-bit Bgra. 
        /// </summary>
        /// <param name="bgr">Source color.</param>
        /// <param name="bgra">Destination color.</param>
        /// <param name="opacity">Opacity.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Convert(Bgr<byte> bgr, ref Bgra<byte> bgra, byte opacity = System.Byte.MaxValue)
        {
            bgra.B = bgr.B;
            bgra.G = bgr.G;
            bgra.R = bgr.R;
            bgra.A = opacity;
        }

        /// <summary>
        /// Converts 8-bit Bgr to 8-bit Hsv color. Value range for 8-bit HSv color is  [0..180].
        /// </summary>
        /// <param name="bgr">Source color.</param>
        /// <param name="hsv">Destination color.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Convert(Bgr<byte> bgr, ref Hsv<byte> hsv)
        {
            byte rgbMin, rgbMax;

            rgbMin = bgr.R < bgr.G ? (bgr.R < bgr.B ? bgr.R : bgr.B) : (bgr.G < bgr.B ? bgr.G : bgr.B);
            rgbMax = bgr.R > bgr.G ? (bgr.R > bgr.B ? bgr.R : bgr.B) : (bgr.G > bgr.B ? bgr.G : bgr.B);

            hsv.V = rgbMax;
            if (hsv.V == 0)
            {
                hsv.H = 0;
                hsv.S = 0;
                return;
            }

            hsv.S = (byte)(255 * (rgbMax - rgbMin) / rgbMax);
            if (hsv.S == 0)
            {
                hsv.H = 0;
                return;
            }

            int hue = 0;
            if (rgbMax == bgr.R)
            {
                hue = 0 + 60 * (bgr.G - bgr.B) / (rgbMax - rgbMin);
                if (hue < 0)
                    hue += 360;
            }
            else if (rgbMax == bgr.G)
            {
                hue = 120 + 60 * (bgr.B - bgr.R) / (rgbMax - rgbMin);
            }
            else //rgbMax == bgr.B
            {
                hue = 240 + 60 * (bgr.R - bgr.G) / (rgbMax - rgbMin);
            }

            hsv.H = (byte)(hue / 2); //scale [0-360] . [0-180] (only needed for byte!)

            //Debug.Assert(hue >= 0 && hue <= 360);
        }
    }

    /// <summary>
    /// Provides extension conversion methods for 8-bit Bgr type.
    /// </summary>
    public static class BgrColorConversionExtensions
    {
        /// <summary>
        /// Converts 8-bit Bgr color to 8-bit gray.
        /// </summary>
        /// <param name="bgr">8-bit Bgr color.</param>
        /// <returns>8-bit gray</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Gray<byte> ToGray(this Bgr<byte> bgr)
        {
            Gray<byte> gray = default(Gray<byte>);
            Bgr<byte>.Convert(bgr, ref gray);
            return gray;
        }

        /// <summary>
        /// Converts 8-bit Bgr color to 8-bit Hsv color.
        /// </summary>
        /// <param name="bgr">8-bit Bgr color.</param>
        /// <returns>8-bit Hsv color.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Hsv<byte> ToHsv(this Bgr<byte> bgr)
        {
            Hsv<byte> hsv = default(Hsv<byte>);
            Bgr<byte>.Convert(bgr, ref hsv);
            return hsv;
        }
    }

    /// <summary>
    /// Represents 8-bit Bgr color type.
    /// <para>Its usage should be restricted only for unsafe pixel manipulation.</para>
    /// </summary>
    public struct Bgr8
    {
        /// <summary>
        /// Gets or sets the blue component.
        /// </summary>
        public byte B;
        /// <summary>
        /// Gets or sets the green component.
        /// </summary>
        public byte G;
        /// <summary>
        /// Gets or sets the red component.
        /// </summary>
        public byte R;
    }
}
