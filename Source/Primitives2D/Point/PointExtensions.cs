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
using System.Linq;
using System.Drawing;

namespace DotImaging.Primitives2D
{
    /// <summary>
    /// Provides point extension methods.
    /// </summary>
    public static class Point32iExtensions
    {
        /// <summary>
        /// Transforms point to the lower pyramid level.
        /// </summary>
        /// <param name="p">Point.</param>
        /// <param name="levels">Specifies how many levels to take.</param>
        /// <param name="factor">Specifies the pyramid scale factor.</param>
        /// <returns>Scaled point.</returns>
        public static PointF UpScale(this Point p, int levels = 1, double factor = 2)
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
        public static PointF DownScale(this Point p, int levels = 1, double factor = 2)
        {
            var downscaleFactor = (float)(1 / System.Math.Pow(factor, levels));

            return new PointF
            {
                X = p.X * downscaleFactor,
                Y = p.Y * downscaleFactor
            };
        }

        /// <summary>
        /// Translates the point by the specified offset.
        /// </summary>
        /// <param name="point">The point to offset.</param>
        /// <param name="offset">Offset to be added.</param>
        /// <returns>Translated point.</returns>
        public static Point Add(this Point point, Point offset)
        {
            return new Point
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
        public static Point Subtract(this Point point, Point offset)
        {
            return new Point
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
        public static double DistanceTo(this Point pointA, Point pointB)
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
        public static PointF Rotate(this Point pointToRotate, double angleDeg, Point centerPoint)
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
        /// Rotates one point around the (0,0).
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="angleDeg">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        public static PointF Rotate(this Point pointToRotate, double angleDeg)
        {
            return pointToRotate.Rotate(angleDeg, Point.Empty);
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
    }

    /// <summary>
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
        /// Translates the point by the specified offset.
        /// </summary>
        /// <param name="point">The point to offset.</param>
        /// <param name="offset">Offset to be added.</param>
        /// <returns>Translated point.</returns>
        public static PointF Add(this PointF point, PointF offset)
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
        /// Rotates one point around the (0,0).
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
    }

    /// <summary>
    /// Provides point collection extensions.
    /// </summary>
    public static class Point32fCollectionExtensions
    {
        //taken from:http://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon and modified
        /// <summary>
        /// Checks whether the specified location is in the polygon.
        /// </summary>
        /// <param name="poly">Polygon.</param>
        /// <param name="x">Horizontal coordinate.</param>
        /// <param name="y">VErtical coordinate.</param>
        /// <returns>True if the point resides inside the polygon, false otherwise.</returns>
        public static bool IsInPolygon(this IList<PointF> poly, float x, float y)
        {
            PointF p1, p2;

            bool inside = false;

            if (poly.Count < 3)
            {
                return inside;
            }

            var oldPoint = new PointF(poly[poly.Count - 1].X, poly[poly.Count - 1].Y);

            for (int i = 0; i < poly.Count; i++)
            {
                var newPoint = new PointF(poly[i].X, poly[i].Y);

                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }

                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }


                if ((newPoint.X < x) == (x <= oldPoint.X) && 
                    (y - (long)p1.Y) * (p2.X - p1.X) < (p2.Y - (long)p1.Y) * (x - p1.X))
                {
                    inside = !inside;
                }


                oldPoint = newPoint;
            }

            return inside;
        }

        /// <summary>
        /// Gets the minimum bounding rectangle around the points.
        /// </summary>
        /// <param name="points">Contour points.</param>
        /// <returns>Bounding rectangle.</returns>
        public static RectangleF BoundingRect(this IEnumerable<PointF> points)
        {
            if (points.Any() == false) return RectangleF.Empty;

            float minX = Single.MaxValue, maxX = Single.MinValue,
                  minY = Single.MaxValue, maxY = Single.MinValue;

            foreach (var pt in points)
            {
                if (pt.X < minX)
                    minX = pt.X;
                if (pt.X > maxX)
                    maxX = pt.X;

                if (pt.Y < minY)
                    minY = pt.Y;
                if (pt.Y > maxY)
                    maxY = pt.Y;
            }

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Gets the center of the mass of the contour.
        /// </summary>
        /// <param name="points">Contour points.</param>
        /// <returns>The center of the mass of the contour.</returns>
        public static PointF Center(this IEnumerable<PointF> points)
        {
            PointF average = new PointF();
            int nSamples = 0;

            foreach (var pt in points)
            {
                average.X += pt.X;
                average.Y += pt.Y;
                nSamples++;
            }

            average.X /= nSamples;
            average.Y /= nSamples;

            return average;
        }

        /// <summary>
        /// Determines whether the polygon forms rectangle.
        /// </summary>
        /// <param name="points">Polygon.</param>
        /// <returns>True if the polygon forms rectangle, false otherwise.</returns>
        public static bool IsRectangle(this IEnumerable<PointF> points)
        {
            if (points.Count() != 4)
                return false;

            var rect = points.BoundingRect();

            bool hasTopLeft = false, hasTopRight = false, hasBottomLeft = false, hasBottomRight = false;

            foreach (var pt in points)
            {
                if (rect.Top == pt.Y)
                {
                    if (rect.X == pt.X)
                        hasTopLeft = true;

                    if (rect.Right == pt.X)
                        hasTopRight = true;
                }

                if (rect.Bottom == pt.Y)
                {
                    if (rect.X == pt.X)
                        hasBottomLeft = true;

                    if (rect.Right == pt.X)
                        hasBottomRight = true;
                }
            }

            return hasTopLeft && hasTopRight && hasBottomLeft && hasBottomRight;
        }
    }

    /// <summary>
    /// Provides point collection extensions.
    /// </summary>
    public static class Point32iCollectionExtensions
    {
        //taken from:http://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon and modified
        /// <summary>
        /// Checks whether the specified location is in the polygon.
        /// </summary>
        /// <param name="poly">Polygon.</param>
        /// <param name="x">Horizontal coordinate.</param>
        /// <param name="y">VErtical coordinate.</param>
        /// <returns>True if the point resides inside the polygon, false otherwise.</returns>
        public static bool IsInPolygon(this IList<Point> poly, float x, float y)
        {
            Point p1, p2;

            bool inside = false;

            if (poly.Count < 3)
            {
                return inside;
            }

            var oldPoint = new Point(poly[poly.Count - 1].X, poly[poly.Count - 1].Y);

            for (int i = 0; i < poly.Count; i++)
            {
                var newPoint = new Point(poly[i].X, poly[i].Y);

                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }

                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }


                if ((newPoint.X < x) == (x <= oldPoint.X) &&
                    (y - (long)p1.Y) * (p2.X - p1.X) < (p2.Y - (long)p1.Y) * (x - p1.X))
                {
                    inside = !inside;
                }


                oldPoint = newPoint;
            }

            return inside;
        }

        /// <summary>
        /// Gets the minimum bounding rectangle around the points.
        /// </summary>
        /// <param name="points">Contour points.</param>
        /// <returns>Bounding rectangle.</returns>
        public static Rectangle BoundingRect(this IEnumerable<Point> points)
        {
            if (points.Any() == false) return Rectangle.Empty;

            int minX = Int32.MaxValue, maxX = Int32.MinValue,
                minY = Int32.MaxValue, maxY = Int32.MinValue;

            foreach (var pt in points)
            {
                if (pt.X < minX)
                    minX = pt.X;
                if (pt.X > maxX)
                    maxX = pt.X;

                if (pt.Y < minY)
                    minY = pt.Y;
                if (pt.Y > maxY)
                    maxY = pt.Y;
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Gets the center of the mass of the contour.
        /// </summary>
        /// <param name="points">Contour points.</param>
        /// <returns>The center of the mass of the contour.</returns>
        public static PointF Center(this IEnumerable<Point> points)
        {
            PointF average = new PointF();
            int nSamples = 0;

            foreach (var pt in points)
            {
                average.X += pt.X;
                average.Y += pt.Y;
                nSamples++;
            }

            average.X /= nSamples;
            average.Y /= nSamples;

            return average;
        }

        /// <summary>
        /// Determines whether the polygon forms rectangle.
        /// </summary>
        /// <param name="points">Polygon.</param>
        /// <returns>True if the polygon forms rectangle, false otherwise.</returns>
        public static bool IsRectangle(this IEnumerable<Point> points)
        {
            if (points.Count() != 4)
                return false;

            var rect = points.BoundingRect();

            bool hasTopLeft = false, hasTopRight = false, hasBottomLeft = false, hasBottomRight = false;

            foreach (var pt in points)
            {
                if (rect.Top == pt.Y)
                {
                    if (rect.X == pt.X)
                        hasTopLeft = true;

                    if (rect.Right == pt.X)
                        hasTopRight = true;
                }

                if (rect.Bottom == pt.Y)
                {
                    if (rect.X == pt.X)
                        hasBottomLeft = true;

                    if (rect.Right == pt.X)
                        hasBottomRight = true;
                }
            }

            return hasTopLeft && hasTopRight && hasBottomLeft && hasBottomRight;
        }
    }
}
