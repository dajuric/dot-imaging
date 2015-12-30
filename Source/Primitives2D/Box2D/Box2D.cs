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

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace DotImaging.Primitives2D
{
    /// <summary>
    /// Box2D equivalent of OpenCV's Box2D Class.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Box2D
    {
         /// <summary>
        /// Gets empty structure.
        /// </summary>
        public static readonly Box2D Empty = new Box2D();

        /// <summary>
        /// Area center.
        /// </summary>
        public PointF Center;
        /// <summary>
        /// Area size.
        /// </summary>
        public SizeF Size;
        /// <summary>
        /// Angle in degrees.
        /// </summary>
        public float Angle;

        /// <summary>
        /// Creates new structure from area and angle.
        /// </summary>
        /// <param name="rect">Area.</param>
        /// <param name="angle">Angle in degrees.</param>
        public Box2D(RectangleF rect, float angle)
            :this(rect.Center(), rect.Size, angle)
        {}

        /// <summary>
        /// Creates new structure from area and angle.
        /// </summary>
        /// <param name="center">Box2D center.</param>
        /// <param name="size">Box 2D size.</param>
        /// <param name="angle">Angle in degrees.</param>
        public Box2D(PointF center, SizeF size, float angle)
        {
            this.Center = new PointF(center.X, center.Y);
            this.Size = size;
            this.Angle = angle;
        }

        /// <summary>
        /// Returns true if the structure is  empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.Equals(Empty); }
        }

        /// <summary>
        /// Gets the minimum enclosing rectangle for this box.
        /// </summary>
        public RectangleF GetMinArea()
        { 
            var vertices = this.GetVertices();

            float minX = vertices.Min(x => x.X);
            float maxX = vertices.Max(x => x.X);

            float minY = vertices.Min(x => x.Y);
            float maxY = vertices.Max(x => x.Y);

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Gets vertices.
        /// </summary>
        /// <returns>Vertices.</returns>
        public PointF[] GetVertices()
        {
            PointF center = this.Center;
            var angleDeg = this.Angle;
          
            PointF[] nonRotatedVertices = getNonRotatedVertices();
            PointF[] rotatedVertices = nonRotatedVertices.Select(x=> new PointF(x.X - center.X, x.Y - center.Y)) //translate to (0,0)
                                                         .Select(x => x.Rotate(angleDeg)) //rotate
                                                         .Select(x => new PointF(x.X + center.X, x.Y + center.Y)) //translate back
                                                         .ToArray();

            return rotatedVertices;
        }

        private PointF[] getNonRotatedVertices()
        {
            float offsetX = this.Size.Width / 2;
            float offsetY = this.Size.Height / 2;

            return new PointF[] 
            {
                new PointF(this.Center.X - offsetX, this.Center.Y - offsetY), //left-upper
                new PointF(this.Center.X + offsetX, this.Center.Y - offsetY), //right-upper
                new PointF(this.Center.X + offsetX, this.Center.Y + offsetY), //right-bottom
                new PointF(this.Center.X - offsetX, this.Center.Y + offsetY) //left-bottom
            };
        }

        /// <summary>
        /// Converts Rectangle to the Box2D representation (angle is zero).
        /// </summary>
        /// <param name="rect">Rectangle to convert.</param>
        /// <returns>Box2D representation.</returns>
        public static implicit operator Box2D(Rectangle rect)
        {
            return new Box2D(rect, 0);
        }

        /// <summary>
        /// Converts RectangleF to the Box2D representation (angle is zero).
        /// </summary>
        /// <param name="rect">Rectangle to convert.</param>
        /// <returns>Box2D representation.</returns>
        public static implicit operator Box2D(RectangleF rect)
        {
            return new Box2D(rect, 0);
        }

        /// <summary>
        /// Determines whether two objects are equal.
        /// </summary>
        /// <param name="obj">Object to test.</param>
        /// <returns>True if two objects are equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Box2D == false) return false;

            var b = (Box2D)obj;
            return b.Center.Equals(this.Center) && b.Angle.Equals(this.Angle);
        }

        /// <summary>
        /// Gets hash-code for the structure.
        /// </summary>
        /// <returns>Hash-code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
