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

using System.Runtime.InteropServices;

namespace DotImaging
{
    /// <summary>
    /// Represents gray color of type <typeparam name="T">color depth</typeparam>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Gray<T> : IColor<T>
        where T: unmanaged
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
