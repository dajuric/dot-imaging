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
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotImaging.Primitives2D
{
    /// <summary>
    /// Provides extension methods for Rectangle and RectangleF structures.
    /// </summary>
    public static class RectangleExtennsions
    {
        /// <summary>
        /// Gets intersection percent of two rectangles.
        /// </summary>
        /// <param name="rect1">First rectangle.</param>
        /// <param name="rect2">Second rectangle.</param>
        /// <returns>Intersection percent (e.g. 1 - full intersection, 0 - no intersection).</returns>
        public static float IntersectionPercent(this Rectangle rect1, Rectangle rect2)
        {
            return RectangleFExtensions.IntersectionPercent(rect1, rect2);
        }

        /// <summary>
        /// Gets the rectangle area.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns>Area of the rectangle.</returns>
        public static int Area(this Rectangle rect)
        {
            return rect.Width * rect.Height;
        }

        /// <summary>
        /// Gets rectangle center.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns>Center of the rectangle.</returns>
        public static Point Center(this Rectangle rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }

        /// <summary>
        /// Gets rectangle vertexes in clock-wise order staring from left-upper corner.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns>Vertexes.</returns>
        public static Point[] Vertices(this Rectangle rect)
        {
            return new Point[]
            {
                new Point(rect.X, rect.Y), //left-upper
                new Point(rect.Right, rect.Y), //right-upper
                new Point(rect.Right, rect.Bottom), //right-bottom
                new Point(rect.X, rect.Bottom) //left-bottom
            };
        }

        /// <summary>
        /// Gets whether the rectangle has an empty area. It is different than <see cref="Rectangle.Empty"/> property.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns>True if the rectangle has an empty area.</returns>
        public static bool IsEmptyArea(this Rectangle rect)
        {
            return rect.Width == 0 || rect.Height == 0;
        }

        /// <summary>
        /// Gets the bounding rectangle of the rectangle collection.
        /// </summary>
        /// <param name="rectangles">Rectangle collection.</param>
        /// <returns>Bounding rectangle.</returns>
        public static Rectangle BoundingRectangle(this IEnumerable<Rectangle> rectangles)
        {
            int minX = Int16.MaxValue, minY = Int16.MaxValue, maxX = Int16.MinValue, maxY = Int16.MinValue;

            foreach (var r in rectangles)
            {
                if (r.Left < minX) minX = r.Left;
                if (r.Top < minY) minY = r.Top;
                if (r.Right > maxX) maxX = r.Right;
                if (r.Bottom > maxY) maxY = r.Bottom;
            }

            return Rectangle.FromLTRB(minX, minY, maxX, maxY);
        }
    }

    /// <summary>
    /// <para>Defined functions can be used as object extensions.</para>
    /// Provides extension methods for rectangle structure.
    /// </summary>
    public static class RectangleFExtensions
    {
        /// <summary>
        /// Gets intersection percent of two rectangles.
        /// </summary>
        /// <param name="rect1">First rectangle.</param>
        /// <param name="rect2">Second rectangle.</param>
        /// <returns>Intersection percent (1 - full intersection, 0 - no intersection).</returns>
        public static float IntersectionPercent(this RectangleF rect1, RectangleF rect2)
        {
            float rect1Area = rect1.Width * rect1.Height;
            float rect2Area = rect2.Width * rect2.Height;

            RectangleF interesectRect = RectangleF.Intersect(rect1, rect2);
            float intersectRectArea = interesectRect.Width * interesectRect.Height;

            float minRectArea = System.Math.Min(rect1Area, rect2Area);

            return (float)intersectRectArea / minRectArea;
        }

        /// <summary>
        /// Gets the rectangle area.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns>Area of the rectangle.</returns>
        public static float Area(this RectangleF rect)
        {
            return rect.Width * rect.Height;
        }

        /// <summary>
        /// Gets rectangle center.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns>Center of the rectangle.</returns>
        public static PointF Center(this RectangleF rect)
        {
            return new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }

        /// <summary>
        /// Gets rectangle vertexes in clock-wise order staring from left-upper corner.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns>Vertexes.</returns>
        public static PointF[] Vertices(this RectangleF rect)
        {
            return new PointF[]
            {
                new PointF(rect.X, rect.Y), //left-upper
                new PointF(rect.Right, rect.Y), //right-upper
                new PointF(rect.Right, rect.Bottom), //right-bottom
                new PointF(rect.X, rect.Bottom) //left-bottom
            };
        }

        /// <summary>
        /// Gets whether the rectangle has an empty area. It is different than <see cref="Rectangle.Empty"/> property.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns>True if the rectangle has an empty area.</returns>
        public static bool IsEmptyArea(this RectangleF rect)
        {
            return rect.Width == 0 || rect.Height == 0;
        }

        /// <summary>
        /// Gets the bounding rectangle of the rectangle collection.
        /// </summary>
        /// <param name="rectangles">Rectangle collection.</param>
        /// <returns>Bounding rectangle.</returns>
        public static RectangleF BoundingRectangle(this IEnumerable<RectangleF> rectangles)
        {
            float minX = Int16.MaxValue, minY = Int16.MaxValue, maxX = Int16.MinValue, maxY = Int16.MinValue;

            foreach (var r in rectangles)
            {
                if (r.Left < minX) minX = r.Left;
                if (r.Top < minY) minY = r.Top;
                if (r.Right > maxX) maxX = r.Right;
                if (r.Bottom > maxY) maxY = r.Bottom;
            }

            return RectangleF.FromLTRB(minX, minY, maxX, maxY);
        }
    }
}
