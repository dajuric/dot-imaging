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

using DotImaging.Primitives2D;

namespace DotImaging
{
    /// <summary>
    /// Contains extension methods for System.Drawing namespace and Accord.NET extensions structure conversion.
    /// </summary>
    public static class DrawingStructureConversions
    {
        #region Point conversions

        /// <summary>
        /// Converts <see cref="System.Drawing.Point"/> to the <see cref="DotImaging.Primitives2D.Point"/>.
        /// </summary>
        /// <param name="point"><see cref="System.Drawing.Point"/></param>
        /// <returns><see cref="DotImaging.Primitives2D.Point"/></returns>
        public static Point ToPt(this System.Drawing.Point point)
        {
            return new Point(point.X, point.Y);
        }

        /// <summary>
        /// Converts <see cref="System.Drawing.PointF"/> to the <see cref="DotImaging.Primitives2D.PointF"/>.
        /// </summary>
        /// <param name="point"><see cref="System.Drawing.PointF"/></param>
        /// <returns><see cref="DotImaging.Primitives2D.PointF"/></returns>
        public static PointF ToPt(this System.Drawing.PointF point)
        {
            return new PointF(point.X, point.Y);
        }

        /// <summary>
        /// Converts to <see cref="DotImaging.Primitives2D.Point"/> to the <see cref="System.Drawing.Point"/>.
        /// </summary>
        /// <param name="point"><see cref="DotImaging.Primitives2D.Point"/></param>
        /// <returns><see cref="System.Drawing.Point"/></returns>
        public static System.Drawing.Point ToPt(this Point point)
        {
            return new System.Drawing.Point(point.X, point.Y);
        }

        /// <summary>
        /// Converts to <see cref="DotImaging.Primitives2D.PointF"/> to the <see cref="System.Drawing.PointF"/>.
        /// </summary>
        /// <param name="point"><see cref="DotImaging.Primitives2D.PointF"/></param>
        /// <returns><see cref="System.Drawing.PointF"/></returns>
        public static System.Drawing.PointF ToPt(this PointF point)
        {
            return new System.Drawing.PointF(point.X, point.Y);
        }

        #endregion

        #region Rectangle conversions

        /// <summary>
        /// Converts the <see cref="System.Drawing.Rectangle"/> to the <see cref="DotImaging.Primitives2D.Rectangle"/>.
        /// </summary>
        /// <param name="rect"><see cref="System.Drawing.Rectangle"/></param>
        /// <returns><see cref="DotImaging.Primitives2D.Rectangle"/></returns>
        public static Rectangle ToRect(this System.Drawing.Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Converts the <see cref="System.Drawing.RectangleF"/> to the <see cref="DotImaging.Primitives2D.RectangleF"/>.
        /// </summary>
        /// <param name="rect"><see cref="System.Drawing.RectangleF"/></param>
        /// <returns><see cref="DotImaging.Primitives2D.RectangleF"/></returns>
        public static RectangleF ToRect(this System.Drawing.RectangleF rect)
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Converts the <see cref="DotImaging.Primitives2D.Rectangle"/> to the <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="rect"><see cref="DotImaging.Primitives2D.Rectangle"/></param>
        /// <returns><see cref="System.Drawing.Rectangle"/></returns>
        public static System.Drawing.Rectangle ToRect(this Rectangle rect)
        {
            return new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Converts the <see cref="DotImaging.Primitives2D.RectangleF"/> to the <see cref="System.Drawing.RectangleF"/>.
        /// </summary>
        /// <param name="rect"><see cref="DotImaging.Primitives2D.RectangleF"/></param>
        /// <returns><see cref="System.Drawing.RectangleF"/></returns>
        public static System.Drawing.RectangleF ToRect(this RectangleF rect)
        {
            return new System.Drawing.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        #endregion

        #region Size conversions

        /// <summary>
        /// Converts the <see cref="System.Drawing.Size"/> to the <see cref="DotImaging.Primitives2D.Size"/>.
        /// </summary>
        /// <param name="size"><see cref="System.Drawing.Size"/></param>
        /// <returns><see cref="DotImaging.Primitives2D.Size"/></returns>
        public static Size ToSize(this System.Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
        }

        /// <summary>
        /// Converts the <see cref="System.Drawing.SizeF"/> to the <see cref="DotImaging.Primitives2D.SizeF"/>.
        /// </summary>
        /// <param name="size"><see cref="System.Drawing.SizeF"/></param>
        /// <returns><see cref="DotImaging.Primitives2D.SizeF"/></returns>
        public static SizeF ToSize(this System.Drawing.SizeF size)
        {
            return new SizeF(size.Width, size.Height);
        }

        /// <summary>
        /// Converts the <see cref="DotImaging.Primitives2D.Size"/> to the <see cref="System.Drawing.Size"/>.
        /// </summary>
        /// <param name="size"><see cref="DotImaging.Primitives2D.Size"/></param>
        /// <returns><see cref="System.Drawing.Size"/></returns>
        public static System.Drawing.Size ToSize(this Size size)
        {
            return new System.Drawing.Size(size.Width, size.Height);
        }

        /// <summary>
        /// Converts the <see cref="DotImaging.Primitives2D.SizeF"/> to the <see cref="System.Drawing.SizeF"/>.
        /// </summary>
        /// <param name="size"><see cref="DotImaging.Primitives2D.SizeF"/></param>
        /// <returns><see cref="System.Drawing.SizeF"/></returns>
        public static System.Drawing.SizeF ToSize(this SizeF size)
        {
            return new System.Drawing.SizeF(size.Width, size.Height);
        }

        #endregion

    }
}
