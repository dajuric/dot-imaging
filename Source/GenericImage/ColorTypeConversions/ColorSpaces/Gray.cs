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

using System.Runtime.InteropServices;

namespace DotImaging
{
    /// <summary>
    /// Represents gray color of type <typeparam name="T">color depth</typeparam>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Gray<T> : IColor<T>
        where T: struct
    {
        /// <summary>
        /// Creates new gray color.
        /// </summary>
        /// <param name="intensity">Intensity.</param>
        public Gray(T intensity)
        {
            this.Intensity = intensity;
        }

        /// <summary>
        /// Gets or sets the intensity.
        /// </summary>
        public T Intensity;

        /// <summary>
        /// Converts gray structure to <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="gray">Gray color.</param>
        /// <returns>Intensity.</returns>
        public static implicit operator T(Gray<T> gray)
        {
            return gray.Intensity;
        }

        /// <summary>
        /// Converts intensity of type <see cref="System.Double"/> to Gray color.
        /// </summary>
        /// <param name="intensity">Intensity.</param>
        /// <returns>Gray color.</returns>
        public static implicit operator Gray<T>(T intensity)
        {
            return new Gray<T>(intensity);
        }

        /// <summary>
        /// Gets the string color representation.
        /// </summary>
        /// <returns>String color representation.</returns>
        public override string ToString()
        {
            return string.Format("{0}", Intensity);
        }

        /// <summary>
        /// Converts 8-bit gray intensity to the 8-bit Bgr color.
        /// </summary>
        /// <param name="gray">Source color.</param>
        /// <param name="bgr">Destination color.</param>
        public static void Convert(Gray<T> gray, ref Bgr<T> bgr)
        {
            bgr.B = gray.Intensity;
            bgr.G = gray.Intensity;
            bgr.R = gray.Intensity;
        }

        /// <summary>
        /// Converts 8-bit gray intensity to the 8-bit Bgra color.
        /// </summary>
        /// <param name="gray">Source color.</param>
        /// <param name="bgra">Destination color.</param>
        public static void Convert(Gray<byte> gray, ref Bgra<byte> bgra)
        {
            bgra.B = gray.Intensity;
            bgra.G = gray.Intensity;
            bgra.R = gray.Intensity;
            bgra.A = System.Byte.MaxValue;
        }
    }
}
