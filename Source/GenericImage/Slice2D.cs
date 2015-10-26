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

namespace DotImaging
{
    /// <summary>
    /// A wrapper for 2D sub-array.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public class Slice2D<T>
    {
        /// <summary>
        /// Creates a new grid from the specified array.
        /// </summary>
        /// <param name="array">Array.</param>
        public Slice2D(T[,] array)
        {
            this.Array = array;
            this.Area = new Rectangle(0, 0, array.Width(), array.Height());
        }

        /// <summary>
        /// Creates a new grid from the specified array.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="area">Sub-array area.</param>
        public Slice2D(T[,] array, Rectangle area)
        {
            this.Array = array;
            this.Area = area;
        }

        /// <summary>
        /// Gets the original array.
        /// </summary>
        public T[,] Array { get; private set; }

        /// <summary>
        /// Gets the working array area.
        /// </summary>
        public Rectangle Area { get; private set; }

        /// <summary>
        /// Gets or sets the specified value.
        /// </summary>
        /// <param name="y">Offset from the upper-left y area location.</param>
        /// <param name="x">Offset from the upper-left x area location.</param>
        /// <returns>Value.</returns>
        public T this[int y, int x]
        {
            get { return Array[Area.Y + y, Area.X + x]; }
            set { Array[Area.Y + y, Area.X + x] = value; }
        }

        /// <summary>
        /// Converts the array into grid representation.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <returns>Grid.</returns>
        public static implicit operator Slice2D<T>(T[,] array)
        {
            return new Slice2D<T>(array);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">Other object</param>
        /// <returns>True if two objects are equal, false otherwise.</returns>
        public override bool Equals(object other)
        {
            var otherObj = other as Slice2D<T>;
            if (otherObj == null)
                return false;

            if(this.Array.Equals(otherObj.Array) && this.Area.Equals(otherObj.Area))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
