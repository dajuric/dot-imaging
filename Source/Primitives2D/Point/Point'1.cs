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

namespace DotImaging.Primitives2D
{
    /// <summary>
    /// Represents the 2D (X, Y) pair.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public struct Point<T>
        where T : struct
    {
        /// <summary>
        /// Creates a new point.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Point(T x, T y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets X coordinate.
        /// </summary>
        public T X;
        
        /// <summary>
        /// Gets or sets Y coordinate.
        /// </summary>
        public T Y;

        /// <summary>
        /// Determines whether the provided object is equal to the current object.
        /// </summary>
        /// <param name="obj">Other object to compare with.</param>
        /// <returns>True if the two objects are equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Point<T> == false)
                return false;

            var pt = (Point<T>)obj;
            return this.X.Equals(pt.X) && this.Y.Equals(pt.Y);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        /// <summary>
        /// Gets the string representation of the object.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return string.Format("{{X={0}, Y={1}}}", X, Y);
        }
    }
}
