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

using System.Runtime.InteropServices;

namespace DotImaging
{
    /// <summary>
    /// Represents Bgra color type of type <typeparam name="T">depth</typeparam>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Bgra<T> : IColor4<T>
        where T: struct
    {
        /// <summary>
        /// Creates new Bgra color.
        /// </summary>
        /// <param name="b">Blue</param>
        /// <param name="g">Green</param>
        /// <param name="r">Red</param>
        /// <param name="a">Alpha (transparency).</param>
        public Bgra(T b, T g, T r, T a)
        {
            this.B = b;
            this.G = g;
            this.R = r;
            this.A = a;
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
        /// Gets or sets the alpha component.
        /// </summary>
        public T A;

        /// <summary>
        /// Gets the string color representation.
        /// </summary>
        /// <returns>String color representation.</returns>
        public override string ToString()
        {
            return string.Format("B: {0}, G: {1}, R: {2}, A: {3}", B, G, R, A);
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
        /// Gets the index of the alpha component.
        /// </summary>
        public const int IdxA = 3;

        /// <summary>
        /// Converts 8-bit Bgra to 8-bit Bgr color.
        /// </summary>
        /// <param name="bgra">Source color.</param>
        /// <param name="bgr">Destination color.</param>
        public static void Convert(Bgra<byte> bgra, ref Bgr<byte> bgr)
        {
            bgr.B = bgra.B;
            bgr.G = bgra.G;
            bgr.R = bgra.R;
        }
    }
}
