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

using System;
using System.Drawing;

namespace DotImaging
{
    /// <summary>
    /// Flip image direction. 
    /// They can be used as bit flags.
    /// </summary>
    [Flags]
    public enum FlipDirection
    {
        /// <summary>
        /// No flipping.
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Horizontal flipping.
        /// </summary>
        Horizontal = 0x1,
        /// <summary>
        /// Vertical flipping
        /// </summary>
        Vertical = 0x2,
        /// <summary>
        /// All flipping (horizontal + vertical).
        /// </summary>
        All = 0x3
    }

    /// <summary>
    /// Contains extension methods for image flipping.
    /// </summary>
    public static class ImageFlipping
    {
        /// <summary>
        /// Flips an input image horizontally / vertically / both directions / or none (data copy).
        /// </summary>
        /// <typeparam name="TColor">Color type.</typeparam>
        /// <param name="source">Input image.</param>
        /// <param name="flipDirection">Flip direction.</param>
        /// <returns>Returns flipped image.</returns>
        public static TColor[,] FlipImage<TColor>(this TColor[,] source, FlipDirection flipDirection)
            where TColor: struct
        {
            TColor[,] dest = source.CopyBlank();
            var sourceArea = new Rectangle(0, 0, source.Width(), source.Height());
            var destinationOffset = new Point();
            source.FlipImage(sourceArea, dest, destinationOffset, flipDirection);

            return dest;
        }

        /// <summary>
        /// Flips an input image horizontally / vertically / both directions / or none (data copy).
        /// </summary>
        /// <typeparam name="TColor">Color type.</typeparam>
        /// <param name="source">Input image.</param>
        /// <param name="sourceArea">Source area.</param>
        /// <param name="destination">Destination image.</param>
        /// <param name="destinationOffset">Destination image offset.</param>
        /// <param name="flipDirection">Flip direction.</param>
        public static void FlipImage<TColor>(this TColor[,] source, Rectangle sourceArea, TColor[,] destination, Point destinationOffset, FlipDirection flipDirection)
        {
            int startDstRow = 0; int vDirection = 1;
            int startDstCol = 0; int hDirection = 1;

            if ((flipDirection & FlipDirection.Vertical) != 0)
            {
                startDstRow = (destinationOffset.Y + sourceArea.Height) - 1; vDirection = -1;
            }
            if ((flipDirection & FlipDirection.Horizontal) != 0)
            {
                startDstCol = (destinationOffset.X + sourceArea.Width) - 1; hDirection = -1;
            }

            for (int srcRow = 0, dstRow = startDstRow; srcRow < sourceArea.Bottom; srcRow++, dstRow += vDirection)
            {
                for (int srcCol = 0, dstCol = startDstCol; srcCol < sourceArea.Right; srcCol++, dstCol += hDirection)
                {
                    destination[dstRow, dstCol] = source[srcRow, srcCol];
                }
            }
        }
    }
}
