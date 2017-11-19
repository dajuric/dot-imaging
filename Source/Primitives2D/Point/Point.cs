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
    public struct Point
    {
        private int x, y;

        /// <summary>
        /// An uninitialized Point structure.
        /// </summary>
        public static readonly Point Empty;

        /// <summary>
        /// Produces a Point structure from a PointF structure by
        ///	taking the ceiling of the X and Y properties.
        /// </summary>
        /// <param name="value">Floating-point coordinate pair.</param>
        /// <returns>Integer coordinate pair.</returns>
        public static Point Ceiling(PointF value)
        {
            int x, y;
            checked
            {
                x = (int)Math.Ceiling(value.X);
                y = (int)Math.Ceiling(value.Y);
            }

            return new Point(x, y);
        }

        /// <summary>
        /// Produces a Point structure from a PointF structure by
        ///	rounding the X and Y properties.
        /// </summary>
        /// <param name="value">Floating-point coordinate pair.</param>
        /// <returns>Integer coordinate pair.</returns>
        public static Point Round(PointF value)
        {
            int x, y;
            checked
            {
                x = (int)Math.Round(value.X);
                y = (int)Math.Round(value.Y);
            }

            return new Point(x, y);
        }

        /// <summary>
        /// Produces a Point structure from a PointF structure by
        ///	truncating the X and Y properties.
        /// </summary>
        /// <param name="value">Floating-point coordinate pair.</param>
        /// <returns>Integer coordinate pair.</returns>
        public static Point Truncate(PointF value)
        {
            int x, y;
            checked
            {
                x = (int)value.X;
                y = (int)value.Y;
            }

            return new Point(x, y);
        }

        /// <summary>
        /// Translates a Point by the positive of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>PointF structure.</returns>
        public static Point Add(Point pt, Size sz)
        {
            return new Point(pt.X + sz.Width, pt.Y + sz.Height);
        }

        /// <summary>
        /// Translates a Point by the negative of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>PointF structure.</returns>
        public static Point Subtract(Point pt, Size sz)
        {
            return new Point(pt.X - sz.Width, pt.Y - sz.Height);
        }

        /// <summary>
        /// Translates a Point by the positive of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>Translated point.</returns>
        public static Point operator +(Point pt, Size sz)
        {
            return Add(pt, sz);
        }

        /// <summary>
        /// Translates a Point by the negative of a specified size.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="sz">Offset.</param>
        /// <returns>Translated point.</returns>
        public static Point operator -(Point pt, Size sz)
        {
            return Subtract(pt, sz);
        }

        /// <summary>
        /// Compares two Point objects. The return value is
        ///	based on the equivalence of the X and Y properties 
        ///	of the two points.
        /// </summary>
        /// <param name="left">First structure.</param>
        /// <param name="right">Second structure.</param>
        /// <returns>Point structure.</returns>
        public static bool operator ==(Point left, Point right)
        {
            return ((left.X == right.X) && (left.Y == right.Y));
        }

        /// <summary>
        /// Compares two Point objects. The return value is
        ///	based on the equivalence of the X and Y properties 
        ///	of the two points.
        /// </summary>
        /// <param name="left">First structure.</param>
        /// <param name="right">Second structure.</param>
        /// <returns>Point structure.</returns>
        public static bool operator !=(Point left, Point right)
        {
            return ((left.X != right.X) || (left.Y != right.Y));
        }

        /// <summary>
        /// Returns a Size based on the Coordinates of a given 
        ///	Point. Requires explicit cast.
        /// </summary>
        /// <param name="p">Point.</param>
        public static explicit operator Size(Point p)
        {
            return new Size(p.X, p.Y);
        }

        /// <summary>
        /// Creates a PointF based on the coordinates of a given 
        ///	Point. No explicit cast is required.
        /// </summary>
        /// <param name="p">Point.</param>
        public static implicit operator PointF(Point p)
        {
            return new PointF(p.X, p.Y);
        }

        /// <summary>
        /// Creates a Point from an integer which holds the Y
        ///	coordinate in the high order 16 bits and the X
        ///	coordinate in the low order 16 bits.
        /// </summary>
        /// <param name="dw">An integer-packed point.</param>
        public Point(int dw)
        {
            y = dw >> 16;
            x = dw & 0xffff;
        }

        /// <summary>
        /// Creates a Point from a Size value.
        /// </summary>
        /// <param name="sz">Size.</param>
        public Point(Size sz)
        {
            x = sz.Width;
            y = sz.Height;
        }

        /// <summary>
        /// Creates a PointF from a specified x,y coordinate pair.
        /// </summary>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        public Point(int x, int y)
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
                return ((x == 0) && (y == 0));
            }
        }

        /// <summary>
        /// Gets or sets X value.
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets Y value.
        /// </summary>
        public int Y
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
            if (!(obj is Point))
                return false;

            return (this == (Point)obj);
        }

        /// <summary>
        /// Calculates a hash value of the object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return x ^ y;
        }

        /// <summary>
        /// Moves the Point a specified distance.
        /// </summary>
        /// <param name="dx">Horizontal offset.</param>
        /// <param name="dy">Vertical offset.</param>
        public void Offset(int dx, int dy)
        {
            x += dx;
            y += dy;
        }

        /// <summary>
        /// Formats the structure as a string in coordinate notation.
        /// </summary>
        /// <returns>Structure represented as a string.</returns>
        public override string ToString()
        {
            return string.Format("{{X={0}, Y={1}}}", x, y);
        }
    }
}
