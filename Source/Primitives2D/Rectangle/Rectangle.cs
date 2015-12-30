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
    /// Stores a set of four integer numbers that represent the location and size of a rectangle.
    /// </summary>
    public struct Rectangle
    {
        private int x, y, width, height;

        /// <summary>
        /// Represents a Rectangle structure with its properties left uninitialized.
        /// </summary>
        public static readonly Rectangle Empty;

        /// <summary>
        /// Creates a Rectangle structure with the specified edge locations.
        /// </summary>
        /// <param name="left">The x-coordinate of the upper-left corner of this Rectangle structure.</param>
        /// <param name="top">The y-coordinate of the upper-left corner of this Rectangle structure.</param>
        /// <param name="right">The x-coordinate of the lower-right corner of this Rectangle structure.</param>
        /// <param name="bottom">The y-coordinate of the lower-right corner of this Rectangle structure.</param>
        /// <returns>The new rectangle that this method creates.</returns>
        public static Rectangle FromLTRB(int left, int top,
                          int right, int bottom)
        {
            return new Rectangle(left, top, right - left,
                          bottom - top);
        }

        /// <summary>
        /// Creates and returns an enlarged copy of the specified Rectangle structure. 
        /// The copy is enlarged by the specified amount. 
        /// The original Rectangle structure remains unmodified.
        /// </summary>
        /// <param name="rect">The Rectangle with which to start. This rectangle is not modified.</param>
        /// <param name="x">The amount to inflate this rectangle horizontally.</param>
        /// <param name="y">The amount to inflate this rectangle vertically.</param>
        /// <returns>The enlarged rectangle.</returns>
        public static Rectangle Inflate(Rectangle rect, int x, int y)
        {
            Rectangle r = new Rectangle(rect.Location, rect.Size);
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Enlarges this rectangle by the specified amount.
        /// </summary>
        /// <param name="width">The amount to inflate this rectangle horizontally.</param>
        /// <param name="height">The amount to inflate this rectangle vertically.</param>
        public void Inflate(int width, int height)
        {
            Inflate(new Size(width, height));
        }

        /// <summary>
        /// Enlarges this rectangle by the specified amount.
        /// </summary>
        /// <param name="size">The amount to inflate this rectangle.</param>
        public void Inflate(Size size)
        {
            x -= size.Width;
            y -= size.Height;
            Width += size.Width * 2;
            Height += size.Height * 2;
        }

        /// <summary>
        /// Returns a third Rectangle structure that represents the intersection of two other Rectangle structures. 
        /// If there is no intersection, an empty Rectangle is returned.
        /// </summary>
        /// <param name="a">A rectangle to intersect.</param>
        /// <param name="b">A rectangle to intersect.</param>
        /// <returns>A rectangle that represents the intersection of a and b.</returns>
        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            // MS.NET returns a non-empty rectangle if the two rectangles
            // touch each other
            if (!a.intersectsWithInclusive(b))
                return Empty;

            return Rectangle.FromLTRB(
                Math.Max(a.Left, b.Left),
                Math.Max(a.Top, b.Top),
                Math.Min(a.Right, b.Right),
                Math.Min(a.Bottom, b.Bottom));
        }

        /// <summary>
        /// Replaces this rectangle with the intersection of itself and the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle with which to intersect.</param>
        public void Intersect(Rectangle rect)
        {
            this = Rectangle.Intersect(this, rect);
        }

        /// <summary>
        /// Determines if this rectangle intersects with <paramref name="rect"/>.
        /// </summary>
        /// <param name="rect">The rectangle to test.</param>
        /// <returns>This method returns true if there is any intersection, otherwise false.</returns>
        public bool IntersectsWith(Rectangle rect)
        {
            return !((Left >= rect.Right) || (Right <= rect.Left) ||
                (Top >= rect.Bottom) || (Bottom <= rect.Top));
        }

        private bool intersectsWithInclusive(Rectangle r)
        {
            return !((Left > r.Right) || (Right < r.Left) ||
                (Top > r.Bottom) || (Bottom < r.Top));
        }

        /// <summary>
        /// Gets a Rectangle structure that contains the union of two Rectangle structures.
        /// </summary>
        /// <param name="a">A rectangle to union.</param>
        /// <param name="b">A rectangle to union.</param>
        /// <returns>A Rectangle structure that bounds the union of the two Rectangle structures.</returns>
        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            return FromLTRB(Math.Min(a.Left, b.Left),
                     Math.Min(a.Top, b.Top),
                     Math.Max(a.Right, b.Right),
                     Math.Max(a.Bottom, b.Bottom));
        }

        /// <summary>
        /// Converts the specified RectangleF structure to a Rectangle structure by rounding the RectangleF values to the next higher integer values.
        /// </summary>
        /// <param name="value">The RectangleF structure to be converted.</param>
        /// <returns>Returns a rectangle.</returns>
        public static Rectangle Ceiling(RectangleF value)
        {
            int x, y, w, h;
            checked
            {
                x = (int)Math.Ceiling(value.X);
                y = (int)Math.Ceiling(value.Y);
                w = (int)Math.Ceiling(value.Width);
                h = (int)Math.Ceiling(value.Height);
            }

            return new Rectangle(x, y, w, h);
        }

        /// <summary>
        /// Converts the specified RectangleF to a Rectangle by rounding the RectangleF values to the nearest integer values.
        /// </summary>
        /// <param name="value">The RectangleF to be converted.</param>
        /// <returns>A rectangle.</returns>
        public static Rectangle Round(RectangleF value)
        {
            int x, y, w, h;
            checked
            {
                x = (int)Math.Round(value.X);
                y = (int)Math.Round(value.Y);
                w = (int)Math.Round(value.Width);
                h = (int)Math.Round(value.Height);
            }

            return new Rectangle(x, y, w, h);
        }

        /// <summary>
        /// Converts the specified RectangleF to a Rectangle by truncating the RectangleF values.
        /// </summary>
        /// <param name="value">The RectangleF to be converted.</param>
        /// <returns>A rectangle.</returns>
        public static Rectangle Truncate(RectangleF value)
        {
            int x, y, w, h;
            checked
            {
                x = (int)value.X;
                y = (int)value.Y;
                w = (int)value.Width;
                h = (int)value.Height;
            }

            return new Rectangle(x, y, w, h);
        }
    
        /// <summary>
        /// Compares two Rectangle objects. The return value is
        ///	based on the equivalence of the Location and Size 
        ///	properties of the two rectangles.
        /// </summary>
        /// <param name="left">A rectangle to compare.</param>
        /// <param name="right">A rectangle to compare.</param>
        /// <returns>True if two rectangles have the same location and size, false otherwise.</returns>
        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return ((left.Location == right.Location) &&
                (left.Size == right.Size));
        }

        /// <summary>
        /// Compares two Rectangle objects. The return value is
        ///	based on the equivalence of the Location and Size 
        ///	properties of the two rectangles.
        /// </summary>
        /// <param name="left">A rectangle to compare.</param>
        /// <param name="right">A rectangle to compare.</param>
        /// <returns>True if two rectangles do not have the same location and size, false otherwise.</returns>
        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return ((left.Location != right.Location) ||
                (left.Size != right.Size));
        }


        /// <summary>
        /// Creates a Rectangle from Point and Size values.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="size">Size.</param>
        public Rectangle(Point location, Size size)
        {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Creates a Rectangle from a specified x,y location and
        ///	width and height values.
        /// </summary>
        /// <param name="x">The x-coordinate of the upper-left corner.</param>
        /// <param name="y">the y-coordinate of the upper-left corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x; 
            this.y = y;
            this.width = width;
            this.height = height; 
        }

        /// <summary>
        /// Tests whether all numeric properties of this Rectangle have values of zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((x == 0) && (y == 0) && (width == 0) && (height == 0));
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the left edge of this Rectangle structure.
        /// </summary>
        public int Left
        {
            get
            {
                return X;
            }
        }

        /// <summary>
        /// Gets the x-coordinate that is the sum of X and Width property values of this Rectangle structure.
        /// </summary>
        public int Right
        {
            get
            {
                return X + Width;
            }
        }

        /// <summary>
        /// Gets the y-coordinate of the top edge of this Rectangle structure.
        /// </summary>
        public int Top
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Gets the y-coordinate that is the sum of the Y and Height property values of this Rectangle structure.
        /// </summary>
        public int Bottom
        {
            get
            {
                return y + height;
            }
        }

        /// <summary>
        /// Gets or sets the height of this Rectangle structure.
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of this Rectangle structure.
        /// </summary>
        public Point Location
        {
            get
            {
                return new Point(x, y);
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
        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the width of this Rectangle structure.
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of this Rectangle structure.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of this Rectangle structure.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        /// Determines if the specified point is contained within this Rectangle structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the point to test.</param>
        /// <param name="y">The y-coordinate of the point to test.</param>
        /// <returns>This method returns true if the point defined by x and y is contained within this Rectangle structure; otherwise false.</returns>
        public bool Contains(int x, int y)
        {
            return ((x >= Left) && (x < Right) &&
                (y >= Top) && (y < Bottom));
        }

        /// <summary>
        /// Determines if the specified point is contained within this Rectangle structure.
        /// </summary>
        /// <param name="pt">The point to test.</param>
        /// <returns>This method returns true if the point represented by <paramref name="pt"/> is contained within this Rectangle structure; otherwise false.</returns>
        public bool Contains(Point pt)
        {
            return Contains(pt.X, pt.Y);
        }

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within this Rectangle structure.
        /// </summary>
        /// <param name="rect">The Rectangle to test.</param>
        /// <returns>This method returns true if the rectangular region represented by <paramref name="rect"/> is entirely contained within this Rectangle structure; otherwise false.</returns>
        public bool Contains(Rectangle rect)
        {
            return (rect == Intersect(this, rect));
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        /// <param name="x">The horizontal offset.</param>
        /// <param name="y">The vertical offset.</param>
        public void Offset(int x, int y)
        {
            this.x += x;
            this.y += y;
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        /// <param name="pos">Amount to offset the location.</param>
        public void Offset(Point pos)
        {
            x += pos.X;
            y += pos.Y;
        }

        /// <summary>
        /// Tests whether obj is a Rectangle structure with the same location and size of this Rectangle structure.
        /// </summary>
        /// <param name="obj">The System.Object to test.</param>
        /// <returns>This method returns true if obj is a Rectangle structure and its X, Y, Width, and Height properties are equal to the corresponding properties of this Rectangle structure; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Rectangle))
                return false;

            return (this == (Rectangle)obj);
        }

        /// <summary>
        /// Returns the hash code for this Rectangle structure. For information about the use of hash codes, see System.Object.GetHashCode() .
        /// </summary>
        /// <returns>An integer that represents the hash code for this rectangle.</returns>
        public override int GetHashCode()
        {
            return (height + width) ^ x + y;
        }

        /// <summary>
        /// Converts the attributes of this System.Drawing.Rectangle to a human-readable string.
        /// </summary>
        /// <returns>A string in (x,y,w,h) notation</returns>
        public override string ToString()
        {
            return String.Format("{{X={0},Y={1},Width={2},Height={3}}}",
                         x, y, width, height);
        }
    }
}
