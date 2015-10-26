#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2015
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
        /// Inflates the rectangle by specified width and height (can be negative) and automatically clamps rectangle coordinates.
        /// </summary>
        /// <param name="rect">Rectangle to inflate.</param>
        /// <param name="width">Horizontal amount.</param>
        /// <param name="height">Vertical amount.</param>
        /// <param name="constrainedArea">If specified rectangle region will be clamped.</param>
        /// <returns>Inflated rectangle.</returns>
        public static Rectangle Inflate(this Rectangle rect, int width, int height, Size constrainedArea = default(Size))
        {
            Rectangle newRect = new Rectangle
            {
                X = rect.X - width,
                Y = rect.Y - height,
                Width = rect.Width + 2 * width,
                Height = rect.Height + 2 * height
            };

            if (constrainedArea.IsEmpty == false)
                newRect.Intersect(new Rectangle(new Point(), constrainedArea));

            return newRect;
        }

        /// <summary>
        /// Inflates the rectangle by specified width and height (can be negative) and automatically clamps rectangle coordinates.
        /// </summary>
        /// <param name="rect">Rectangle to inflate.</param>
        /// <param name="widthScale">Horizontal scale.</param>
        /// <param name="heightScale">Vertical scale.</param>
        /// <param name="constrainedArea">If specified rectangle region will be clamped.</param>
        /// <returns>Inflated rectangle.</returns>
        public static Rectangle Inflate(this Rectangle rect, double widthScale, double heightScale, Size constrainedArea = default(Size))
        {
            Rectangle newRect = new Rectangle
            {
                X = (int)(rect.X - rect.Width * widthScale / 2),
                Y = (int)(rect.Y - rect.Height * heightScale / 2),
                Width = (int)(rect.Width + rect.Width * widthScale),
                Height = (int)(rect.Height + rect.Height * heightScale)
            };

            if (constrainedArea.IsEmpty == false)
                newRect.Intersect(new Rectangle(new Point(), constrainedArea));

            return newRect;
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
        /// Intersects the rectangle with other rectangle and returns intersected rectangle.
        /// </summary>
        /// <param name="rect">The input rectangle.</param>
        /// <param name="other">The rectangle to intersect with.</param>
        /// <param name="preserveScale">
        /// If true the size components will be cropped by equal amount.
        /// If false the size ratio will not be checked.
        /// </param>
        /// <returns>Intersected rectangle.</returns>
        public static Rectangle Intersect(this Rectangle rect, Rectangle other, bool preserveScale = false)
        {
            var croppedRect = rect;
            croppedRect.Intersect(other);

            if (!preserveScale)
                return croppedRect;

            var originalWidthHeightRatio = (float)rect.Width / rect.Height;
            var newRect = croppedRect;

            var dW = croppedRect.Width - croppedRect.Height * originalWidthHeightRatio;
            if (dW > 0)
            {
                newRect.Width -= (int)System.Math.Round(dW);
            }
            else
            {
                var dH = croppedRect.Height - croppedRect.Width * (1 / originalWidthHeightRatio);
                newRect.Height -= (int)System.Math.Round(dH);
            }

            return newRect;
        }

        /// <summary>
        /// Calculates intersected rectangle from specified area (transformed into rectangle with location (0,0)) 
        /// which can be useful for image area intersections.
        /// </summary>
        /// <param name="rect">Rectangle to intersect.</param>
        /// <param name="area">Maximum bounding box represented as size.</param>
        /// <param name="preserveScale">
        /// If true the size components will be cropped by equal amount.
        /// If false the size ratio will not be checked.
        /// </param>
        /// <returns>Intersected rectangle.</returns>
        public static Rectangle Intersect(this Rectangle rect, Size area, bool preserveScale = false)
        {
            Rectangle newRect = rect.Intersect(new Rectangle(0, 0, area.Width, area.Height), preserveScale);
            return newRect;
        }

        /// <summary>
        /// Changes rectangle's location in order to fit the specified area.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="area">Valid bounding box.</param>
        /// <param name="translatedRectangle">Translated rectangle.</param>
        /// <returns>True if the translation exist, false otherwise.</returns>
        public static bool MoveToFit(this Rectangle rect, Size area, out Rectangle translatedRectangle)
        {
            var leftOffset = (rect.X < 0) ? -rect.X : 0;
            var topOffset = (rect.Y < 0) ? -rect.Y : 0;

            rect.X += leftOffset;
            rect.Y += topOffset;

            var rightOffset = (rect.Right > area.Width) ? (area.Width - rect.Right) : 0;
            var bottomOffset = (rect.Bottom > area.Height) ? (area.Height - rect.Bottom) : 0;

            rect.X += rightOffset;
            rect.Y += bottomOffset;

            translatedRectangle = rect;
            if (rect.X < 0 || rect.Y < 0 || rect.Right > area.Width || rect.Bottom > area.Height)
                return false;

            return true;
        }
    }

    /// <summary>
    /// <para>Defined functions can be used as object extensions.</para>
    /// Provides extension methods for rectangle structure.
    /// </summary>
    public static class RectangleFExtensions
    {
        /// <summary>
        /// Changes rectangle's location in order to fit the specified area.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="area">Valid bounding box.</param>
        /// <param name="translatedRectangle">Translated rectangle.</param>
        /// <returns>True if the translation exist, false otherwise.</returns>
        public static bool MoveToFit(this RectangleF rect, SizeF area, out RectangleF translatedRectangle)
        {
            var leftOffset = (rect.X < 0) ? -rect.X : 0;
            var topOffset = (rect.Y < 0) ? -rect.Y : 0;

            rect.X += leftOffset;
            rect.Y += topOffset;

            var rightOffset = (rect.Right > area.Width) ? (area.Width - rect.Right) : 0;
            var bottomOffset = (rect.Bottom > area.Height) ? (area.Height - rect.Bottom) : 0;

            rect.X += rightOffset;
            rect.Y += bottomOffset;

            translatedRectangle = rect;
            if (rect.X < 0 || rect.Y < 0 || rect.Right > area.Width || rect.Bottom > area.Height)
                return false;

            return true;
        }

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
        /// Transforms rectangle to the lower pyramid level.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="level">Specifies how many levels to take.</param>
        /// <param name="factor">Specifies the pyramid scale factor.</param>
        /// <returns>Transformed rectangle.</returns>
        public static RectangleF UpScale(this RectangleF rect, int level = 1, float factor = 2)
        {
            float pyrScale = (float)System.Math.Pow(factor, level);

            return new RectangleF
            {
                X = rect.X * pyrScale,
                Y = rect.Y * pyrScale,
                Width = rect.Width * pyrScale,
                Height = rect.Height * pyrScale
            };
        }

        /// <summary>
        /// Transforms rectangle to the higher pyramid level.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="level">Specifies how many levels to take.</param>
        /// <param name="factor">Specifies the pyramid scale factor.</param>
        /// <returns>Transformed rectangle.</returns>
        public static RectangleF DownScale(this RectangleF rect, int level = 1, float factor = 2)
        {
            float pyrScale = 1 / (float)System.Math.Pow(factor, level);

            return new RectangleF
            {
                X = rect.X * pyrScale,
                Y = rect.Y * pyrScale,
                Width = rect.Width * pyrScale,
                Height = rect.Height * pyrScale
            };
        }

        /// <summary>
        /// Inflates the rectangle by specified width and height (can be negative) and automatically clamps rectangle coordinates.
        /// </summary>
        /// <param name="rect">Rectangle to inflate.</param>
        /// <param name="widthScale">Horizontal scale.</param>
        /// <param name="heightScale">Vertical scale.</param>
        /// <param name="constrainedArea">If specified rectangle region will be clamped.</param>
        /// <returns>Inflated rectangle.</returns>
        public static RectangleF Inflate(this RectangleF rect, double widthScale, double heightScale, SizeF constrainedArea = default(SizeF))
        {
            RectangleF newRect = new RectangleF
            {
                X = (float)(rect.X - rect.Width * widthScale / 2),
                Y = (float)(rect.Y - rect.Height * heightScale / 2),
                Width = (float)(rect.Width + rect.Width * widthScale),
                Height = (float)(rect.Height + rect.Height * heightScale)
            };

            if (constrainedArea.IsEmpty == false)
                newRect.Intersect(new RectangleF(new PointF(), constrainedArea));

            return newRect;
        }

        /// <summary>
        /// Scales rectangles by the specified amount.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="scaleFactor">Multiplication factor for vertexes coordinates.</param>
        /// <returns>Scaled rectangle.</returns>
        public static RectangleF Scale(this RectangleF rect, float scaleFactor)
        {
            return new RectangleF 
            {
                X = rect.X * scaleFactor,
                Y = rect.Y * scaleFactor,
                Width = rect.Width * scaleFactor,
                Height  =rect.Height * scaleFactor
            };
        }

        /// <summary>
        /// Changes rectangle scale by a minimum amount in term of area change to match the scale of the user-specified size scale.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="other">Size from which the scale is taken.</param>
        /// <param name="correctLocation">Moves rectangle to minimize the impact of scaling regarding original location.</param>
        /// <returns>Rectangle that has the same scale as </returns>
        public static RectangleF ScaleTo(this RectangleF rect, SizeF other, bool correctLocation = true)
        {
            return ScaleTo(rect, other.Width / other.Height);
        }

        /// <summary>
        /// Changes rectangle scale by a minimum amount in term of area change to match the scale of the user-specified size scale.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="widthHeightRatio">Width / height ratio that must be satisfied.</param>
        /// <param name="correctLocation">Moves rectangle to minimize the impact of scaling regarding original location.</param>
        /// <returns>Rectangle that has the same scale as </returns>
        public static RectangleF ScaleTo(this RectangleF rect, float widthHeightRatio, bool correctLocation = true)
        {
            var sizeScale = widthHeightRatio;

            var newWidthCandidate = rect.Height * sizeScale;
            var newHeightCandidate = rect.Width * (1 / sizeScale);

            //if we choose newWidth...
            var newWidthAreaChangeFactor = rect.Area() / (newWidthCandidate * rect.Height);
            newWidthAreaChangeFactor = newWidthAreaChangeFactor - 1; //for how much the area will change (+/- X percent)

            //if we choose newHeight...
            var newHeightAreaChangeFactor = rect.Area() / (newHeightCandidate * rect.Width);
            newHeightAreaChangeFactor = newHeightAreaChangeFactor - 1; //for how much the area will change (+/- X percent)

            if (System.Math.Abs(newWidthAreaChangeFactor) < System.Math.Abs(newHeightAreaChangeFactor))
            {
                var xOffset = 0f;

                if (correctLocation)
                    xOffset = -(newWidthCandidate - rect.Width) / 2;

                return new RectangleF(rect.X + xOffset, rect.Y, newWidthCandidate, rect.Height);
            }
            else
            {
                var yOffset = 0f;

                if (correctLocation)
                    yOffset = -(newHeightCandidate - rect.Height) / 2;

                return new RectangleF(rect.X, rect.Y + yOffset, rect.Width, newHeightCandidate);
            }
        }
    }
}
