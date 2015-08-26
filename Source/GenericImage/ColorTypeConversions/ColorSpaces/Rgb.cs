#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2015
// darko.juric2@gmail.com
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
// 
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see <https://www.gnu.org/licenses/lgpl.txt>.
//
#endregion

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotImaging
{
    /// <summary>
    /// Represents Rgb color type of type <typeparam name="T">color depth</typeparam>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rgb<T> : IColor3<T>
        where T : struct
    {
        /// <summary>
        /// Creates new Rgb color.
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        public Rgb(T r, T g, T b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }
    
        /// <summary>
        /// Gets or sets the red component.
        /// </summary>
        public T R;
        /// <summary>
        /// Gets or sets the green component.
        /// </summary>
        public T G;
        /// <summary>
        /// Gets or sets the blue component.
        /// </summary>
        public T B;

        /// <summary>
        /// Gets the string color representation.
        /// </summary>
        /// <returns>String color representation.</returns>
        public override string ToString()
        {
            return string.Format("R: {0}, G: {1}, B: {2}", R, G, B);
        }

        /// <summary>
        /// Gets the index of the red component.
        /// </summary>
        public const int IdxR = 0;
        /// <summary>
        /// Gets the index of the green component.
        /// </summary>
        public const int IdxG = 1;
        /// <summary>
        /// Gets the index of the blue component.
        /// </summary>
        public const int IdxB = 2;

        /// <summary>
        /// Converts 8-bit Rgb to 8-bit Bgr. 
        /// </summary>
        /// <param name="Rgb">Source color.</param>
        /// <param name="Bgr">Destination color.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Convert(Rgb<byte> rgb, ref Bgr<byte> bgr)
        {
            rgb.R = bgr.R;
            rgb.G = bgr.G;
            rgb.B = bgr.B;
        }
    }

    /// <summary>
    /// Represents 8-bit Rgb color type.
    /// <para>Its usage should be restricted only for unsafe pixel manipulation.</para>
    /// </summary>
    public struct Rgb8
    {
        /// <summary>
        /// Gets or sets the red component.
        /// </summary>
        public byte R;
        /// <summary>
        /// Gets or sets the green component.
        /// </summary>
        public byte G;
        /// <summary>
        /// Gets or sets the blue component.
        /// </summary>
        public byte B;
    }
}