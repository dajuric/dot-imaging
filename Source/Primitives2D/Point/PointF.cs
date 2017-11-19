#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2018
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

//---------------------- original-----------------------
// Author:
//   Mike Kestner (mkestner@speakeasy.net)
//
// Copyright (C) 2001 Mike Kestner
// Copyright (C) 2004, 2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Globalization;

namespace DotImaging.Primitives2D
{
    /// <summary>
    /// Represents the 2D (X, Y) pair.
    /// </summary>
    public struct PointF
    {
        private float x, y;

        /// <summary>
        /// An uninitialized PointF structure.
        /// </summary>
        public static readonly PointF Empty;

        /// <summary>
        /// Translates a PointF by the positive of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>PointF structure.</returns>
        public static PointF Add(PointF pt, SizeF sz)
        {
            return new PointF(pt.X + sz.Width, pt.Y + sz.Height);
        }

        /// <summary>
        /// Translates a PointF by the negative of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>PointF structure.</returns>
        public static PointF Subtract(PointF pt, SizeF sz)
        {
            return new PointF(pt.X - sz.Width, pt.Y - sz.Height);
        }

        /// <summary>
        /// Translates a PointF by the positive of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>PointF structure.</returns>
        public static PointF Add(PointF pt, Size sz)
        {
            return new PointF(pt.X + sz.Width, pt.Y + sz.Height);
        }

        /// <summary>
        /// Translates a PointF by the negative of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>PointF structure.</returns>
        public static PointF Subtract(PointF pt, Size sz)
        {
            return new PointF(pt.X - sz.Width, pt.Y - sz.Height);
        }

        /// <summary>
        /// Translates a PointF by the positive of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>Translated point.</returns>
        public static PointF operator +(PointF pt, SizeF sz)
        {
            return Add(pt, sz);
        }

        /// <summary>
        /// Translates a PointF by the negative of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>Translated point.</returns>
        public static PointF operator -(PointF pt, SizeF sz)
        {
            return Subtract(pt, sz);
        }

        /// <summary>
        /// Translates a PointF by the positive of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>Translated point.</returns>
        public static PointF operator +(PointF pt, Size sz)
        {
            return Add(pt, sz);
        }

        /// <summary>
        /// Translates a PointF by the negative of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>Translated point.</returns>
        public static PointF operator -(PointF pt, Size sz)
        {
            return Subtract(pt, sz);
        }

        /// <summary>
        /// Compares two PointF objects. The return value is
        ///	based on the equivalence of the X and Y properties 
        ///	of the two points.
        /// </summary>
        /// <param name="left">First structure.</param>
        /// <param name="right">Second structure.</param>
        /// <returns>PointF structure.</returns>
        public static bool operator ==(PointF left, PointF right)
        {
            return ((left.X == right.X) && (left.Y == right.Y));
        }

        /// <summary>
        /// Compares two PointF objects. The return value is
        ///	based on the equivalence of the X and Y properties 
        ///	of the two points.
        /// </summary>
        /// <param name="left">First structure.</param>
        /// <param name="right">Second structure.</param>
        /// <returns>PointF structure.</returns>
        public static bool operator !=(PointF left, PointF right)
        {
            return ((left.X != right.X) || (left.Y != right.Y));
        }

        /// <summary>
        /// Creates a PointF from a specified x,y coordinate pair.
        /// </summary>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        public PointF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Indicates if both X and Y are zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((x == 0.0) && (y == 0.0));
            }
        }

        /// <summary>
        /// Gets or sets X value.
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets Y value.
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Checks equivalence of this PointF and another object.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns>True if the provided object is equal to this structure, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PointF))
                return false;

            return (this == (PointF)obj);
        }

        /// <summary>
        /// Calculates a hash value of the object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return (int)x ^ (int)y;
        }

        /// <summary>
        /// Formats the structure as a string in coordinate notation.
        /// </summary>
        /// <returns>Structure represented as a string.</returns>
        public override string ToString()
        {
            return String.Format("{{X={0}, Y={1}}}", x.ToString(CultureInfo.CurrentCulture), 
                                                     y.ToString(CultureInfo.CurrentCulture));
        }
    }
}