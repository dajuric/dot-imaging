﻿#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2019
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
using System.Runtime.InteropServices;
using System.Drawing;

namespace DotImaging
{
    /// <summary>
    /// Provides extensions for color format conversion to and from CvSclalar.
    /// </summary>
    internal static class ColorCvScalarConversionExtensions
    {
        public static CvScalar ToCvScalar(this Bgr<byte> color)
        {
            return new CvScalar{ V0 = color.B, V1 = color.G, V2 = color.R, V3 = Byte.MaxValue };
        }

        public static CvScalar ToCvScalar(this Bgra<byte> color)
        {
            return new CvScalar{ V0 = color.B, V1 = color.G, V2 = color.R, V3 = color.A };
        }
    }

    /// <summary>
    /// Provides extension drawing methods which operate on color RGB/RGBA images.
    /// </summary>
    public static class Drawing
    {
        private unsafe static void draw(Bgr<byte>[,] image, byte opacity, Action<IplImage> drawingAction)
        {
            using (var uImg = image.Lock())
            {
                var cvImg = uImg.AsCvIplImage();
                var cvOverlayImPtr = CvCoreInvoke.cvCloneImage(&cvImg);

                drawingAction(*cvOverlayImPtr);
                CvCoreInvoke.cvAddWeighted(cvOverlayImPtr, (float)opacity / Byte.MaxValue, &cvImg, 1 - (float)opacity / Byte.MaxValue, 0, &cvImg);

                CvCoreInvoke.cvReleaseImage(&cvOverlayImPtr);
            }
        }

        #region Rectangle

        /// <summary>
        /// Draws rectangle.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="rect">Rectangle.</param>
        /// <param name="color">Object's color.</param>
        /// <param name="thickness">Border thickness (-1 to fill the object).</param>
        /// <param name="opacity">Sets alpha channel where 0 is transparent and 255 is full opaque.</param>
        public unsafe static void DrawRectangle(this Bgr<byte>[,] image, Rectangle rect, Bgr<byte> color, int thickness, byte opacity = Byte.MaxValue)
        {
            if (float.IsNaN(rect.X) || float.IsNaN(rect.Y))
                return;


            draw(image, opacity, cvImg => 
            {
                CvCoreInvoke.cvRectangleR(&cvImg, rect, color.ToCvScalar(), thickness, LineTypes.EightConnected, 0);
            });
        }

        #endregion

        #region Text

        /// <summary>
        /// Draws text on the provided image.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="text">User text.</param>
        /// <param name="font">Font.</param>
        /// <param name="botomLeftPoint">Bottom-left point.</param>
        /// <param name="color">Text color.</param>
        /// <param name="opacity">Sets alpha channel where 0 is transparent and 255 is full opaque.</param>
        public unsafe static void DrawText(this Bgr<byte>[,] image, string text, Font font, Point botomLeftPoint, Bgr<byte> color, byte opacity = Byte.MaxValue)
        {
            draw(image, opacity, cvImg => 
            {
                CvCoreInvoke.cvPutText(&cvImg, text, botomLeftPoint, ref font, color.ToCvScalar());
            });
        }

        #endregion

        #region Contour

        /// <summary>
        /// Draws contour.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="contour">Contour points.</param>
        /// <param name="color">Contour color.</param>
        /// <param name="thickness">Contours thickness (it does not support values smaller than 1).</param>
        /// <param name="opacity">Sets alpha channel where 0 is transparent and 255 is full opaque.</param>
        public unsafe static void DrawPolygon(this Bgr<byte>[,] image, Point[] contour, Bgr<byte> color, int thickness, byte opacity = Byte.MaxValue)
        {
            var contourHandle = GCHandle.Alloc(contour, GCHandleType.Pinned);

            draw(image, opacity, cvImg =>
            {
                //TODO - noncritical: implement with cvContour
                CvCoreInvoke.cvPolyLine(&cvImg, new IntPtr[] { contourHandle.AddrOfPinnedObject() }, new int[] { contour.Length }, 1,
                                           true, color.ToCvScalar(), thickness, LineTypes.EightConnected, 0);
            });

            contourHandle.Free();
        }

        #endregion

        #region Annotations

        /// <summary>
        /// Draws rectangle with the specified text on top.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="rect">User specified area to annotate.</param>
        /// <param name="text">Label.</param>
        /// <param name="font">Font to use.</param>
        public static void DrawAnnotation(this Bgr<byte>[,] image, Rectangle rect, string text, Font font)
        {
            const int VERTICAL_OFFSET = 5;

            image.DrawRectangle(rect, Bgr<byte>.Red, 1);

            var textSize = font.GetTextSize(text, 0);
            var bottomLeftPt = new Point(rect.X + rect.Width / 2 - textSize.Width / 2, rect.Top - VERTICAL_OFFSET);
            image.DrawText(text, font, bottomLeftPt, Bgr<byte>.Black);
        }

        #endregion


        #region Ellipse

        /// <summary>
        /// Draws ellipse.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="ceneter">Center.</param>
        /// <param name="angle">Rotation angle.</param>
        /// <param name="size">Axes size.</param>
        /// <param name="color">Object's color.</param>
        /// <param name="thickness">Border thickness (-1 to fill the object).</param>
        /// <param name="opacity">Sets alpha channel where 0 is transparent and 255 is full opaque.</param>
        public unsafe static void DrawEllipse(this Bgr<byte>[,] image, Point ceneter, Size size, float angle, Bgr<byte> color, int thickness, byte opacity = Byte.MaxValue)
        {
            draw(image, opacity, cvImg =>
            {
                CvCoreInvoke.cvEllipse(&cvImg, ceneter, size, angle,
                                       0, 360, color.ToCvScalar(), thickness, LineTypes.EightConnected, 0);
            });
        }

        /// <summary>
        /// Draws circle.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="center">Center.</param>
        /// <param name="radius">Circle radius.</param>
        /// <param name="color">Circle color.</param>
        /// <param name="thickness">Contours thickness.</param>
        /// <param name="opacity">Sets alpha channel where 0 is transparent and 255 is full opaque.</param>
        public unsafe static void DrawCircle(this Bgr<byte>[,] image, Point center, int radius, Bgr<byte> color, int thickness, byte opacity = Byte.MaxValue)
        {
            draw(image, opacity, cvImg =>
            {
                CvCoreInvoke.cvCircle(&cvImg, center, radius, color.ToCvScalar(),
                                      thickness, LineTypes.EightConnected, 0);
            });
        }

        #endregion
    }
}