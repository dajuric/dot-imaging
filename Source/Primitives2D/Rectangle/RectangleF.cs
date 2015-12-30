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

namespace DotImaging.Primitives2D
{
    /// <summary>
    /// Stores a set of four floating-point numbers that represent the location and size of a rectangle.
    /// </summary>
    public struct RectangleF
    {
        private float x, y, width, height;

        /// <summary>
        /// Represents a RectangleF structure with its properties left uninitialized.
        /// </summary>
        public static readonly RectangleF Empty;

        /// <summary>
        /// Creates a RectangleF structure with the specified edge locations.
        /// </summary>
        /// <param name="left">The x-coordinate of the upper-left corner of this RectangleF structure.</param>
        /// <param name="top">The y-coordinate of the upper-left corner of this RectangleF structure.</param>
        /// <param name="right">The x-coordinate of the lower-right corner of this RectangleF structure.</param>
        /// <param name="bottom">The y-coordinate of the lower-right corner of this RectangleF structure.</param>
        /// <returns>The new rectangle that this method creates.</returns>
        public static RectangleF FromLTRB(float left, float top,
                           float right, float bottom)
        {
            return new RectangleF(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Creates and returns an enlarged copy of the specified RectangleF structure. 
        /// The copy is enlarged by the specified amount. 
        /// The original RectangleF structure remains unmodified.
        /// </summary>
        /// <param name="rect">The RectangleF with which to start. This rectangle is not modified.</param>
        /// <param name="x">The amount to inflate this rectangle horizontally.</param>
        /// <param name="y">The amount to inflate this rectangle vertically.</param>
        /// <returns>The enlarged rectangle.</returns>
        public static RectangleF Inflate(RectangleF rect, float x, float y)
        {
            RectangleF ir = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            ir.Inflate(x, y);
            return ir;
        }

        /// <summary>
        /// Enlarges this rectangle by the specified amount.
        /// </summary>
        /// <param name="width">The amount to inflate this rectangle horizontally.</param>
        /// <param name="height">The amount to inflate this rectangle vertically.</param>
        public void Inflate(float width, float height)
        {
            Inflate(new SizeF(width, height));
        }

        /// <summary>
        /// Enlarges this rectangle by the specified amount.
        /// </summary>
        /// <param name="size">The amount to inflate this rectangle.</param>
        public void Inflate(SizeF size)
        {
            x -= size.Width;
            y -= size.Height;
            width += size.Width * 2;
            height += size.Height * 2;
        }

        /// <summary>
        /// Returns a third RectangleF structure that represents the intersection of two other RectangleF structures. 
        /// If there is no intersection, an empty Rectangle is returned.
        /// </summary>
        /// <param name="a">A rectangle to intersect.</param>
        /// <param name="b">A rectangle to intersect.</param>
        /// <returns>A rectangle that represents the intersection of a and b.</returns>
        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            // MS.NET returns a non-empty rectangle if the two rectangles
            // touch each other
            if (!a.intersectsWithInclusive(b))
                return Empty;

            return FromLTRB(
                Math.Max(a.Left, b.Left),
                Math.Max(a.Top, b.Top),
                Math.Min(a.Right, b.Right),
                Math.Min(a.Bottom, b.Bottom));
        }

        /// <summary>
        /// Replaces this rectangle with the intersection of itself and the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle with which to intersect.</param>
        public void Intersect(RectangleF rect)
        {
            this = RectangleF.Intersect(this, rect);
        }

        /// <summary>
        /// Determines if this rectangle intersects with <paramref name="rect"/>.
        /// </summary>
        /// <param name="rect">The rectangle to test.</param>
        /// <returns>This method returns true if there is any intersection, otherwise false.</returns>
        public bool IntersectsWith(RectangleF rect)
        {
            return !((Left >= rect.Right) || (Right <= rect.Left) ||
                (Top >= rect.Bottom) || (Bottom <= rect.Top));
        }

        private bool intersectsWithInclusive(RectangleF r)
        {
            return !((Left > r.Right) || (Right < r.Left) ||
                (Top > r.Bottom) || (Bottom < r.Top));
        }

        /// <summary>
        /// Gets a RectangleF structure that contains the union of two RectangleF structures.
        /// </summary>
        /// <param name="a">A rectangle to union.</param>
        /// <param name="b">A rectangle to union.</param>
        /// <returns>A RectangleF structure that bounds the union of the two RectangleF structures.</returns>
        public static RectangleF Union(RectangleF a, RectangleF b)
        {
            return FromLTRB(Math.Min(a.Left, b.Left),
                     Math.Min(a.Top, b.Top),
                     Math.Max(a.Right, b.Right),
                     Math.Max(a.Bottom, b.Bottom));
        }

        /// <summary>
        /// Compares two RectangleF objects. The return value is
        ///	based on the equivalence of the Location and Size 
        ///	properties of the two rectangles.
        /// </summary>
        /// <param name="left">A rectangle to compare.</param>
        /// <param name="right">A rectangle to compare.</param>
        /// <returns>True if two rectangles have the same location and size, false otherwise.</returns>
        public static bool operator ==(RectangleF left, RectangleF right)
        {
            return (left.X == right.X) && (left.Y == right.Y) &&
                                (left.Width == right.Width) && (left.Height == right.Height);
        }

        /// <summary>
        /// Compares two Rectangle objects. The return value is
        ///	based on the equivalence of the Location and Size 
        ///	properties of the two rectangles.
        /// </summary>
        /// <param name="left">A rectangle to compare.</param>
        /// <param name="right">A rectangle to compare.</param>
        /// <returns>True if two rectangles do not have the same location and size, false otherwise.</returns>
        public static bool operator !=(RectangleF left, RectangleF right)
        {
            return (left.X != right.X) || (left.Y != right.Y) ||
                                (left.Width != right.Width) || (left.Height != right.Height);
        }

        /// <summary>
        /// Converts a Rectangle object to a RectangleF.
        /// </summary>
        /// <param name="r">Rectangle.</param>
        /// <returns>A RectangleF representation.</returns>
        public static implicit operator RectangleF(Rectangle r)
        {
            return new RectangleF(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Creates a RectangleF from PointF and SizeF values.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="size">Size.</param>
        public RectangleF(PointF location, SizeF size)
        {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Creates a RectangleF from a specified x,y location and
        ///	width and height values.
        /// </summary>
        /// <param name="x">The x-coordinate of the upper-left corner.</param>
        /// <param name="y">the y-coordinate of the upper-left corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public RectangleF(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Tests whether all numeric properties of this RectangleF have values of zero.
        /// </summary>
        public bool IsEmpty
        {
            get { return (width <= 0 || height <= 0); }
        }

        /// <summary>
        /// Gets the x-coordinate of the left edge of this RectangleF structure.
        /// </summary>
        public float Left
        {
            get { return X; }
        }

        /// <summary>
        /// Gets the x-coordinate that is the sum of X and Width property values of this RectangleF structure.
        /// </summary>
        public float Right
        {
            get { return X + Width; }
        }

        /// <summary>
        /// Gets the y-coordinate of the top edge of this RectangleF structure.
        /// </summary>
        public float Top
        {
            get { return Y; }
        }

        /// <summary>
        /// Gets the y-coordinate that is the sum of the Y and Height property values of this RectangleF structure.
        /// </summary>
        public float Bottom
        {
            get { return Y + Height; }
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of this RectangleF structure.
        /// </summary>
        public PointF Location
        {
            get
            {
                return new PointF(x, y);
            }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this rectangle.
        /// </summary>
        public SizeF Size
        {
            get
            {
                return new SizeF(width, height);
            }
            set
            {
                width = value.Width;
                height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the width of this RectangleF structure.
        /// </summary>
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Gets or sets the height of this RectangleF structure.
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of this RectangleF structure.
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of this RectangleF structure.
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Determines if the specified point is contained within this RectangleF structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the point to test.</param>
        /// <param name="y">The y-coordinate of the point to test.</param>
        /// <returns>This method returns true if the point defined by x and y is contained within this RectangleF structure; otherwise false.</returns>
        public bool Contains(float x, float y)
        {
            return ((x >= Left) && (x < Right) &&
                (y >= Top) && (y < Bottom));
        }

        /// <summary>
        /// Determines if the specified point is contained within this RectangleF structure.
        /// </summary>
        /// <param name="pt">The point to test.</param>
        /// <returns>This method returns true if the point represented by <paramref name="pt"/> is contained within this RectangleF structure; otherwise false.</returns>
        public bool Contains(PointF pt)
        {
            return Contains(pt.X, pt.Y);
        }

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within this RectangleF structure.
        /// </summary>
        /// <param name="rect">The Rectangle to test.</param>
        /// <returns>This method returns true if the rectangular region represented by <paramref name="rect"/> is entirely contained within this RectangleF structure; otherwise false.</returns>
        public bool Contains(RectangleF rect)
        {
            return (rect == Intersect(this, rect));
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        /// <param name="x">The horizontal offset.</param>
        /// <param name="y">The vertical offset.</param>
        public void Offset(float x, float y)
        {
            X += x;
            Y += y;
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        /// <param name="pos">Amount to offset the location.</param>
        public void Offset(PointF pos)
        {
            Offset(pos.X, pos.Y);
        }

        /// <summary>
        /// Tests whether obj is a RectangleF structure with the same location and size of this RectangleF structure.
        /// </summary>
        /// <param name="obj">The System.Object to test.</param>
        /// <returns>This method returns true if obj is a Rectangle structure and its X, Y, Width, and Height properties are equal to the corresponding properties of this RectangleF structure; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is RectangleF))
                return false;

            return (this == (RectangleF)obj);
        }

        /// <summary>
        /// Returns the hash code for this RectangleF structure. For information about the use of hash codes, see System.Object.GetHashCode() .
        /// </summary>
        /// <returns>An integer that represents the hash code for this rectangle.</returns>
        public override int GetHashCode()
        {
            return (int)(x + y + width + height);
        }

        /// <summary>
        /// Converts the attributes of this System.Drawing.Rectangle to a human-readable string.
        /// </summary>
        /// <returns>A string in (x,y,w,h) notation</returns>
        public override string ToString()
        {
            return String.Format("{X={0},Y={1},Width={2},Height={3}}",
                         x, y, width, height);
        }
    }
}
