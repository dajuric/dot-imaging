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
using System.Collections.Generic;

namespace DotImaging.Primitives2D
{
    /// <summary>
    /// <para>Defined functions can be used as object extensions.</para>
    /// Provides point extension methods.
    /// </summary>
    public static class Point32iExtensions
    {
        /// <summary>
        /// Selects points which satisfy minimal specified distance.
        /// </summary>
        /// <param name="candidates">Points sorted by importance. Points are tested by sequentially.</param>
        /// <param name="minimalDistance">Minimal enforced distance.</param>
        /// <returns>Filtered points which are spread by minimal <paramref name="minimalDistance"/>.</returns>
        public static List<Point> EnforceMinimalDistance(this IEnumerable<Point> candidates, float minimalDistance)
        {
            var minDistSqr = minimalDistance * minimalDistance;
            List<Point> filteredPoints = new List<Point>();

            foreach (var candidate in candidates)
            {
                bool isEnoughFar = true;
                foreach (var filteredPt in filteredPoints)
                {
                    int dx = candidate.X - filteredPt.X;
                    int dy = candidate.Y - filteredPt.Y;
                    int featureDistanceSqr = dx * dx + dy * dy;

                    if (featureDistanceSqr < minDistSqr)
                    {
                        isEnoughFar = false;
                        break;
                    }
                }

                if (isEnoughFar)
                    filteredPoints.Add(candidate);
            }

            return filteredPoints;
        }

        /// <summary>
        /// Clamps point coordinate according to the specified size (0,0, size.Width, size.Height).
        /// </summary>
        /// <param name="point">The point to clamp.</param>
        /// <param name="size">The valid region.</param>
        /// <returns>Clamped point.</returns>
        public static Point Clamp(this Point point, Size size)
        {
            return new Point
            {
                X = System.Math.Min(System.Math.Max(0, point.X), size.Width),
                Y = System.Math.Min(System.Math.Max(0, point.Y), size.Height)
            };
        }

        /// <summary>
        /// Clamps point coordinate according to the specified size (rect.X, rect.Y, rect.Right, rect.Bottom).
        /// </summary>
        /// <param name="point">The point to clamp.</param>
        /// <param name="rect">The valid region.</param>
        /// <returns>Clamped point.</returns>
        public static Point Clamp(this Point point, Rectangle rect)
        {
            return new Point
            {
                X = System.Math.Min(System.Math.Max(rect.X, point.X), rect.Right),
                Y = System.Math.Min(System.Math.Max(rect.Y, point.Y), rect.Bottom)
            };
        }

        /// <summary>
        /// Negates point coordinates.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>Point with negated coordinates.</returns>
        public static Point Negate(this Point point)
        {
            return new Point
            {
                X = -point.X,
                Y = -point.Y
            };
        }

        /// <summary>
        /// Calculates the Euclidean distance between two points.
        /// </summary>
        /// <param name="pointA">First point.</param>
        /// <param name="pointB">Second point.</param>
        /// <returns>Euclidean distance between the points.</returns>
        public static double DistanceTo(this Point pointA, Point pointB)
        {
            var distnace = System.Math.Sqrt((pointA.X - pointB.X) * (pointA.X - pointB.X) + (pointA.Y - pointB.Y) * (pointA.Y - pointB.Y)); //Euclidean distance
            return distnace;
        }
    }

    /// <summary>
    /// <para>Defined functions can be used as object extensions.</para>
    /// Provides point extension methods.
    /// </summary>
    public static class Point32fExtensions
    {
        /// <summary>
        /// Transforms point to the lower pyramid level.
        /// </summary>
        /// <param name="p">Point.</param>
        /// <param name="levels">Specifies how many levels to take.</param>
        /// <param name="factor">Specifies the pyramid scale factor.</param>
        /// <returns>Scaled point.</returns>
        public static PointF UpScale(this PointF p, int levels = 1, double factor = 2)
        {
            var upscaleFactor = (float)System.Math.Pow(factor, levels);

            return new PointF
            {
                X = p.X * upscaleFactor,
                Y = p.Y * upscaleFactor
            };
        }

        /// <summary>
        /// Transforms point to the higher pyramid level.
        /// </summary>
        /// <param name="p">Point.</param>
        /// <param name="levels">Specifies how many levels to take.</param>
        /// <param name="factor">Specifies the pyramid scale factor.</param>
        /// <returns>Scaled point.</returns>
        public static PointF DownScale(this PointF p, int levels = 1, double factor = 2)
        {
            var downscaleFactor = (float)(1 / System.Math.Pow(factor, levels));

            return new PointF
            {
                X = p.X * downscaleFactor,
                Y = p.Y * downscaleFactor
            };
        }

        /// <summary>
        /// Gets integer point representation by applying floor operation.
        /// </summary>
        /// <param name="p">Point to truncate.</param>
        /// <returns>Truncated point.</returns>
        public static Point Floor(this PointF p)
        { 
            return new Point
            {
                X = (int)p.X,
                Y = (int)p.Y
            };
        }


        /// <summary>
        /// Clamps point coordinate according to the specified size (0,0, size.Width, size.Height).
        /// </summary>
        /// <param name="point">The point to clamp.</param>
        /// <param name="size">The valid region.</param>
        /// <returns>Clamped point.</returns>
        public static PointF Clamp(this PointF point, SizeF size)
        {
            return new PointF
            {
                X = System.Math.Min(System.Math.Max(0, point.X), size.Width),
                Y = System.Math.Min(System.Math.Max(0, point.Y), size.Height)
            };
        }

        /// <summary>
        /// Clamps point coordinate according to the specified size (rect.X, rect.Y, rect.Right, rect.Bottom).
        /// </summary>
        /// <param name="point">The point to clamp.</param>
        /// <param name="rect">The valid region.</param>
        /// <returns>Clamped point.</returns>
        public static PointF Clamp(this PointF point, RectangleF rect)
        {
            return new PointF
            {
                X = System.Math.Min(System.Math.Max(rect.X, point.X), rect.Right),
                Y = System.Math.Min(System.Math.Max(rect.Y, point.Y), rect.Bottom)
            };
        }

        /// <summary>
        /// Negates point coordinates.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>Point with negated coordinates.</returns>
        public static PointF Negate(this PointF point)
        {
            return new PointF
            {
                X = -point.X,
                Y = -point.Y
            };
        }

        /// <summary>
        /// Translates the point by the specified offset.
        /// </summary>
        /// <param name="point">The point to offset.</param>
        /// <param name="offset">Offset to be added.</param>
        /// <returns>Translated point.</returns>
        public static PointF Offset(this PointF point, PointF offset)
        {
            return new PointF
            {
                X = point.X + offset.X,
                Y = point.Y + offset.Y
            };
        }

        /// <summary>
        /// Subtracts the point by the specified offset.
        /// </summary>
        /// <param name="point">The point to subtract.</param>
        /// <param name="offset">Subtract factor.</param>
        /// <returns>Translated point.</returns>
        public static PointF Subtract(this PointF point, PointF offset)
        {
            return new PointF
            {
                X = point.X - offset.X,
                Y = point.Y - offset.Y
            };
        }

        /// <summary>
        /// Calculates the Euclidean distance between two points.
        /// </summary>
        /// <param name="pointA">First point.</param>
        /// <param name="pointB">Second point.</param>
        /// <returns>Euclidean distance between the points.</returns>
        public static double DistanceTo(this PointF pointA, PointF pointB)
        {
            var distnace = System.Math.Sqrt((pointA.X - pointB.X) * (pointA.X - pointB.X) + (pointA.Y - pointB.Y) * (pointA.Y - pointB.Y)); //Euclidean distance
           return distnace;
        }

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="angleDeg">The rotation angle in degrees.</param>
        /// <param name="centerPoint">The center point of rotation.</param>
        /// <returns>Rotated point</returns>
        public static PointF Rotate(this PointF pointToRotate, double angleDeg, PointF centerPoint)
        {
            //taken from: http://stackoverflow.com/questions/13695317/rotate-a-point-around-another-point and modified

            double angleInRadians = angleDeg * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);

            return new PointF
            {
                X =
                    (float)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (float)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="angleDeg">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        public static PointF Rotate(this PointF pointToRotate, double angleDeg)
        {
            return pointToRotate.Rotate(angleDeg, PointF.Empty);
        }

        /// <summary>
        /// Rounds point coordinates.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <returns>Integer point with rounded coordinates.</returns>
        public static Point Round(this PointF point)
        {
            return Point.Round(point);
        }
    }
}
