#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014 
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
