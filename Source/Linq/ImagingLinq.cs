#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2015
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
using System.Collections.Generic;
using System.Linq;

namespace DotImaging.Linq
{
    /// <summary>
    /// Contains 2D array Linq extensions
    /// </summary>
    static partial class ImageLinqExtensions
    {
        /// <summary>
        /// Converts the specified collection to a 2D array determined by the specified size.
        /// <para>The number of elements must match the number of elements in the collection.</para>
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="size">Array size</param>
        /// <returns>2D array</returns>
        public static T[,] ToArray2D<T>(this IEnumerable<T> collection, Size size)
        {
            return collection.ToArray2D(size.Width, size.Height);
        }

        /// <summary>
        /// Converts the specified collection to a 2D array determined by the specified size.
        /// <para>The number of elements must match the number of elements in the collection.</para>
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="width">Array width</param>
        /// <param name="height">Array height</param>
        /// <returns>2D array</returns>
        public static T[,] ToArray2D<T>(this IEnumerable<T> collection, int width, int height)
        {
            T[,] dest = new T[height, width];

            int row = 0, col = 0;
            foreach (var item in collection)
            {
                dest[row, col] = item;

                col++;
                if (col >= width)
                {
                    col = 0;
                    row++;
                }
            }

            return dest;
        }

        /// <summary>
        /// Gets the 2x CPU number of slices which width is equal to array width.
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="array">Array</param>
        /// <returns>Array slices</returns>
        public static IEnumerable<Slice2D<T>> SliceUniform<T>(this T[,] array)
        {
            int sliceCount = Environment.ProcessorCount * 2;
            int sliceHeight = array.Height() / sliceCount;

            int y = 0;
            Rectangle area = Rectangle.Empty;

            for (int sliceIdx = 0; sliceIdx < (sliceCount-1); sliceIdx++)
            {
                area = new Rectangle(0, y, array.Width(), sliceHeight);
                y += sliceHeight;

                yield return new Slice2D<T>(array, area);
            }

            //last slice
            area = new Rectangle(0, y, array.Width(), array.Height() - y);
            yield return new Slice2D<T>(array, area);
        }

        /// <summary>
        /// Creates a parallel-ordered query of the array element collection.
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="array">Array</param>
        /// <returns>Parallel-ordered query</returns>
        public static ParallelQuery<T> AsParallelOrdered<T>(this T[,] array) //slow
        {
            //return array.AsGrid().AsParallel().SelectMany(x => x.Array.AsEnumerable(x.Area));
            return array.AsEnumerable().AsParallel().AsOrdered();
        }

        /// <summary>
        /// Represents the specified array row as enumerable collection.
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="array">Array</param>
        /// <param name="row">Row index</param>
        /// <returns>Row elements</returns>
        public static IEnumerable<T> Row<T>(this T[,] array, int row)
        {
            var width = array.Width();

            for (int col = 0; col < width; col++)
            {
                yield return array[row, col];
            }
        }

        /// <summary>
        /// Represents the specified array column as enumerable collection.
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="array">Array</param>
        /// <param name="column">Column index</param>
        /// <returns>Column elements</returns>
        public static IEnumerable<T> Column<T>(this T[,] array, int column)
        {
            var height = array.Height();

            for (int row = 0; row < height; row++)
            {
                yield return array[row, column];
            }
        }

        /// <summary>
        /// Represents the specified array portion as enumerable collection.
        /// <para>The scan path is from top-left -> bottom-right border.</para>
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="array">Array</param>
        /// <param name="area">Array portion</param>
        /// <returns>Element collection</returns>
        public static IEnumerable<T> AsEnumerable<T>(this T[,] array, Rectangle area)
        {
            for (int row = area.Top; row < area.Bottom; row++)
            {
                for (int col = area.Left; col < area.Right; col++)
                {
                    yield return array[row, col];
                }
            }
        }

        /// <summary>
        /// Represents the specified array as enumerable collection.
        /// <para>The scan path is from top-left -> bottom-right border.</para>
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">Array</param>
        /// <returns>Element collection</returns>
        public static IEnumerable<T> AsEnumerable<T>(this T[,] array)
        {
            var area = new Rectangle(0, 0, array.Width(), array.Height());
            return array.AsEnumerable(area);
        }
    }
}
