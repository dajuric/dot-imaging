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
// Copyright (C) 2004 Novell, Inc. http://www.novell.com
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
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
    /// Stores an ordered pair of integer numbers, which specify height and width and defines related functions.
    /// </summary>
    public struct Size
    {
        private int width, height;

        /// <summary>
        /// An uninitialized Size structure.
        /// </summary>
        public static readonly Size Empty;

        /// <summary>
        /// Adds two Size structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>Size structure.</returns>
        public static Size Add(Size sz1, Size sz2)
        {
            return new Size(sz1.Width + sz2.Width,
                            sz1.Height + sz2.Height);
        }

        /// <summary>
        /// Subtracts two Size structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>Size structure.</returns>
        public static Size Subtract(Size sz1, Size sz2)
        {
            return new Size(sz1.Width - sz2.Width,
                            sz1.Height - sz2.Height);
        }

        /// <summary>
        /// Addition of two Size structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>Size structure.</returns>
        public static Size operator +(Size sz1, Size sz2)
        {
            return Add(sz1, sz2);
        }

        /// <summary>
        /// Subtracts two Size structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>Size structure.</returns>
        public static Size operator -(Size sz1, Size sz2)
        {
            return Subtract(sz1, sz2);
        }

        /// <summary>
        /// Produces a Size structure from a SizeF structure by
        ///	taking the ceiling of the Width and Height properties.
        /// </summary>
        /// <param name="value">Floating-point size structure.</param>
        /// <returns>Integer size structure.</returns>
        public static Size Ceiling(SizeF value)
        {
            int w, h;
            checked
            {
                w = (int)Math.Ceiling(value.Width);
                h = (int)Math.Ceiling(value.Height);
            }

            return new Size(w, h);
        }

        /// <summary>
        /// Produces a Size structure from a SizeF structure by
        ///	rounding the Width and Height properties.
        /// </summary>
        /// <param name="value">Floating-point size structure.</param>
        /// <returns>Integer size structure.</returns>
        public static Size Round(SizeF value)
        {
            int w, h;
            checked
            {
                w = (int)Math.Round(value.Width);
                h = (int)Math.Round(value.Height);
            }

            return new Size(w, h);
        }

        /// <summary>
        /// Produces a Size structure from a SizeF structure by
        ///	truncating the Width and Height properties.
        /// </summary>
        /// <param name="value">Floating-point size structure.</param>
        /// <returns>Integer size structure.</returns>
        public static Size Truncate(SizeF value)
        {
            int w, h;
            checked
            {
                w = (int)value.Width;
                h = (int)value.Height;
            }

            return new Size(w, h);
        }

        /// <summary>
        /// Compares two Size objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two Size structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>Size structure.</returns>
        public static bool operator ==(Size sz1, Size sz2)
        {
            return ((sz1.Width == sz2.Width) &&
                (sz1.Height == sz2.Height));
        }

        /// <summary>
        /// Compares two Size objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two Size structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>Size structure.</returns>
        public static bool operator !=(Size sz1, Size sz2)
        {
            return ((sz1.Width != sz2.Width) ||
                (sz1.Height != sz2.Height));
        }

        /// <summary>
        /// Returns a Point based on the dimensions of a given 
        ///	Size. Requires explicit cast.
        /// </summary>
        /// <param name="size">Size structure.</param>
        public static explicit operator Point(Size size)
        {
            return new Point(size.Width, size.Height);
        }

        /// <summary>
        /// Creates a SizeF based on the dimensions of a given 
        ///	Size. No explicit cast is required.
        /// </summary>
        /// <param name="p"></param>
        public static implicit operator SizeF(Size p)
        {
            return new SizeF(p.Width, p.Height);
        }

        /// <summary>
        /// Creates a Size from a Point value.
        /// </summary>
        /// <param name="pt">Point.</param>
        public Size(Point pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        /// Creates a Size from specified dimensions.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Indicates if both width and height are zero.
        /// </summary>
        public bool IsEmpty
        {
            get { return ((width == 0) && (height == 0)); }
        }

        /// <summary>
        /// Gets or sets width value.
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Gets or sets height value.
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Checks equivalence of this Size and another object.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns>True if the provided object is equal to this structure, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Size))
                return false;

            return (this == (Size)obj);
        }

        /// <summary>
        /// Calculates a hash value of the object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return width ^ height;
        }

        /// <summary>
        /// Formats the structure as a string in coordinate notation.
        /// </summary>
        /// <returns>Structure represented as a string.</returns>
        public override string ToString()
        {
            return String.Format("{Width={0}, Height={1}}", width, height);
        }
    }
}
