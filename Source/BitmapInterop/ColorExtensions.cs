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
using System.Drawing;

namespace DotImaging
{
    /// <summary>
    /// Contains color format conversion extensions.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Gets System.Drawing.Color from Bgr8 color.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="opacity">Opacity. If color has 4 channels opacity is discarded.</param>
        /// <returns>System.Drawing.Color</returns>
        public static System.Drawing.Color ToColor(this Gray<byte> color, byte opacity = Byte.MaxValue)
        {
            return Color.FromArgb(opacity, color.Intensity, color.Intensity, color.Intensity);
        }

        /// <summary>
        /// Gets System.Drawing.Color from Bgr8 color.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="opacity">Opacity. If color has 4 channels opacity is discarded.</param>
        /// <returns>System.Drawing.Color</returns>
        public static System.Drawing.Color ToColor(this Bgr<byte> color, byte opacity = Byte.MaxValue)
        {
            return Color.FromArgb(opacity, color.R, color.G, color.B);
        }

        /// <summary>
        /// Gets System.Drawing.Color from Bgra8 color.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <returns>System.Drawing.Color</returns>
        public static System.Drawing.Color ToColor(this Bgra<byte> color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts (casts) the color into 32-bit BGR color.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <returns>Bgr representation.</returns>
        public static Bgr<byte> ToBgr(this System.Drawing.Color color)
        {
            return new Bgr<byte> { B = color.B, G = color.G, R = color.R };
        }
    }
}
