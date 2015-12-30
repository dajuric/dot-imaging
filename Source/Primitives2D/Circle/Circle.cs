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
    // Accord Math Library
    // The Accord.NET Framework
    // http://accord-framework.net
    //
    // Copyright © César Souza, 2009-2014
    // cesarsouza at gmail.com
    //
    // Copyright © Darko Jurić, 2014-2016-2015
    // darko.juric2 at gmail.com
    //
    //    This library is free software; you can redistribute it and/or
    //    modify it under the terms of the GNU Lesser General Public
    //    License as published by the Free Software Foundation; either
    //    version 2.1 of the License, or (at your option) any later version.
    //
    //    This library is distributed in the hope that it will be useful,
    //    but WITHOUT ANY WARRANTY; without even the implied warranty of
    //    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    //    Lesser General Public License for more details.
    //
    //    You should have received a copy of the GNU Lesser General Public
    //    License along with this library; if not, write to the Free Software
    //    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
    //

    /// <summary>
    ///   2D circle class.
    /// </summary>
    public struct Circle
    {
        /// <summary>
        /// Horizontal center coordinate.
        /// </summary>
        public int X;
        /// <summary>
        /// Vertical center coordinate.
        /// </summary>
        public int Y;
        /// <summary>
        /// Circle radius.
        /// </summary>
        public int Radius;

        /// <summary>
        /// Gets or sets circle's location.
        /// </summary>
        public Point Center
        {
            get { return new Point(X, Y); }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// Creates a new instance of an <see cref="CircleF"/> structure.
        /// </summary>
        /// <param name="position">Center position.</param>
        /// <param name="radius">Circle radius.</param>
        public Circle(Point position, int radius)
            :this(position.X, position.Y, radius)
        {}

        /// <summary>
        /// Creates a new instance of an <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="x">Horizontal center position.</param>
        /// <param name="y">Vertical center position.</param>
        /// <param name="radius">Circle radius.</param>
        public Circle(int x, int y, int radius)
        {
            this.X = x;
            this.Y = y;
            this.Radius = radius;
        }

        /// <summary>
        ///   Gets the area of the circle (πR²).
        /// </summary>
        /// 
        public double Area
        {
            get { return Radius * Radius * System.Math.PI; }
        }

        /// <summary>
        ///   Gets the circumference of the circle (2πR).
        /// </summary>
        /// 
        public double Circumference
        {
            get { return 2 * Radius * System.Math.PI; }
        }

        /// <summary>
        ///   Gets the diameter of the circle (2R).
        /// </summary>
        /// 
        public int Diameter
        {
            get { return 2 * Radius; }
        }

        /// <summary>
        ///   Computes the distance from circle to point.
        /// </summary>
        /// 
        /// <param name="point">The point to have its distance from the circle computed.</param>
        /// 
        /// <returns>The distance from <paramref name="point"/> to this circle.</returns>
        /// 
        public double DistanceToPoint(Point point)
        {
            var centerDiff = this.Center.DistanceTo(point);
            return System.Math.Abs(centerDiff - Radius);
        }

        /// <summary>
        /// Converts integer into floating-point representation.
        /// </summary>
        /// <param name="circle">Integer circle representation.</param>
        /// <returns>Floating-point circle representation.</returns>
        public static implicit operator CircleF(Circle circle)
        {
            return new CircleF(circle.X, circle.Y, circle.Radius);
        }
    }
}
