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

using DotImaging.Primitives2D;

namespace DotImaging
{
    /// <summary>
    /// Element conversion delegate.
    /// </summary>
    /// <typeparam name="TSrc">Source element type.</typeparam>
    /// <typeparam name="TDst">Destination element type.</typeparam>
    /// <param name="source">Source element.</param>
    /// <param name="destination">Destination element.</param>
    public delegate void Convert<TSrc, TDst>(TSrc source, ref TDst destination)
        where TSrc : struct
        where TDst : struct;

    /// <summary>
    /// Provides generic element conversion extensions.
    /// </summary>
    public static class ConvertExtensions
    {
        /// <summary>
        /// Converts each element of the array by using the specified conversion function.
        /// </summary>
        /// <typeparam name="TSrc">Source element type.</typeparam>
        /// <typeparam name="TDst">Destination element type.</typeparam>
        /// <param name="source">Source array.</param>
        /// <param name="convert">Conversion function.</param>
        /// <returns>Destination array.</returns>
        public static TDst[,] Convert<TSrc, TDst>(this TSrc[,] source, Convert<TSrc, TDst> convert)
            where TSrc : struct
            where TDst : struct
        {
            var area = new Rectangle(0, 0, source.GetLength(1), source.GetLength(0));
            return source.Convert<TSrc, TDst>(convert, area);
        }

        /// <summary>
        /// Converts each element of the array by using the specified conversion function.
        /// </summary>
        /// <typeparam name="TSrc">Source element type.</typeparam>
        /// <typeparam name="TDst">Destination element type.</typeparam>
        /// <param name="source">Source array.</param>
        /// <param name="convert">Conversion function.</param>
        /// <param name="area">Working area.</param>
        public static TDst[,] Convert<TSrc, TDst>(this TSrc[,] source, Convert<TSrc, TDst> convert, Rectangle area)
            where TSrc : struct
            where TDst : struct
        {
            TDst[,] destination = new TDst[area.Height, area.Width];
            var offset = area.Location;

            ParallelLauncher.Launch(thread =>
            {
                convert(source[thread.Y + offset.Y, thread.X + offset.X], ref destination[thread.Y, thread.X]);
            },
            area.Width, area.Height);

            return destination;
        }
    }
}
