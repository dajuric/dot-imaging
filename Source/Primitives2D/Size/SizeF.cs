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

using System.Globalization;

namespace DotImaging.Primitives2D
{
    /// <summary>
    /// Stores an ordered pair of floating-point numbers, which specify height and width and defines related functions.
    /// </summary>
    public struct SizeF
    {
        private float width, height;

        /// <summary>
        /// An uninitialized Size structure.
        /// </summary>
        public static readonly SizeF Empty;

        /// <summary>
        /// Adds two SizeF structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>SizeF structure.</returns>
        public static SizeF Add(SizeF sz1, SizeF sz2)
        {
           return new SizeF(sz1.Width + sz2.Width,
                            sz1.Height + sz2.Height);
        }

        /// <summary>
        /// Subtracts two SizeF structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>SizeF structure.</returns>
        public static SizeF Subtract(SizeF sz1, SizeF sz2)
        {
            return new SizeF(sz1.Width - sz2.Width,
                             sz1.Height - sz2.Height);
        }

        /// <summary>
        /// Converts the structure to PointF representation.
        /// </summary>
        /// <returns>PointF.</returns>
        public PointF ToPointF()
        {
            return (PointF)this;
        }

        /// <summary>
        /// Converts the structure to Size representation.
        /// </summary>
        /// <returns>Size.</returns>
        public Size ToSize()
        {
            return (Size)this;
        }

        /// <summary>
        /// Adds two SizeF structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>Size structure.</returns>
        public static SizeF operator +(SizeF sz1, SizeF sz2)
        {
            return Add(sz1, sz2);
        }

        /// <summary>
        /// Subtracts two Size structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>Size structure.</returns>
        public static SizeF operator -(SizeF sz1, SizeF sz2)
        {
            return Subtract(sz1, sz2);
        }

        /// <summary>
        /// Compares two SizeF objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two SizeF structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>SizeF structure.</returns>
        public static bool operator ==(SizeF sz1, SizeF sz2)
        {
            return ((sz1.Width == sz2.Width) &&
                (sz1.Height == sz2.Height));
        }

        /// <summary>
        /// Compares two SizeF objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two SizeF structures.
        /// </summary>
        /// <param name="sz1">First structure.</param>
        /// <param name="sz2">Second structure.</param>
        /// <returns>SizeF structure.</returns>
        public static bool operator !=(SizeF sz1, SizeF sz2)
        {
            return ((sz1.Width != sz2.Width) ||
                (sz1.Height != sz2.Height));
        }

        /// <summary>
        /// Returns a PointF based on the dimensions of a given 
        ///	SizeF. Requires explicit cast.
        /// </summary>
        public static explicit operator PointF(SizeF size)
        {
            return new PointF(size.Width, size.Height);
        }

        /// <summary>
        /// Creates a SizeF from a PointF value.
        /// </summary>
        /// <param name="pt">PointF.</param>
        public SizeF(PointF pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        /// Creates a SizeF from an existing SizeF value.
        /// </summary>
        /// <param name="sz">Size.</param>
        public SizeF(SizeF sz)
        {
            width = sz.Width;
            height = sz.Height;
        }

        /// <summary>
        /// Creates a SizeF from specified dimensions.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public SizeF(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Indicates if both width and height are zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((width == 0.0) && (height == 0.0));
            }
        }

        /// <summary>
        /// Gets or sets width value.
        /// </summary>
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Gets or sets height value.
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Checks equivalence of this SizeF and another object.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns>True if the provided object is equal to this structure, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SizeF))
                return false;

            return (this == (SizeF)obj);
        }

        /// <summary>
        /// Calculates a hash value of the object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return (int)width ^ (int)height;
        }

        /// <summary>
        /// Truncates width and height value and returns integer representation of the size.
        /// </summary>
        /// <param name="size">Size to convert into integer Size representation.</param>
        /// <returns>Integer Size representation.</returns>
        public static explicit operator Size(SizeF size)
        {
            return new Size((int)size.Width, (int)size.Height);
        }

        /// <summary>
        /// Formats the structure as a string in coordinate notation.
        /// </summary>
        /// <returns>Structure represented as a string.</returns>
        public override string ToString()
        {
            return string.Format("{{Width={0}, Height={1}}}", width.ToString(CultureInfo.CurrentCulture),
                height.ToString(CultureInfo.CurrentCulture));
        }
    }
}
