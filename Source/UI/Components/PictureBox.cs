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

using Eto.Drawing;
using Eto.Forms;
using System;

namespace DotImaging
{
    class PictureBox : Drawable
    {
        public PictureBox()
        {
            this.ActionModifierKey = Keys.Shift;
            this.AutoScale = true;
        }

        #region Point transformation

        public PointF ToPictureBoxCoordinate(PointF point)
        {
            var scaleX = (float)imageBounds.Width / image.Width;
            var scaleY = (float)imageBounds.Height / image.Height;

            return new PointF
            {
                X = point.X * scaleX + imageBounds.X,
                Y = point.Y * scaleY + imageBounds.Y
            };
        }

        public PointF ToImageCoordinate(PointF point)
        {
            var scaleX = (float)imageBounds.Width / image.Width;
            var scaleY = (float)imageBounds.Height / image.Height;

            return new PointF
            {
                X = (point.X - imageBounds.X) / scaleX,
                Y = (point.Y - imageBounds.Y) / scaleY
            };
        }

        public RectangleF ToPictureBoxCoordinate(RectangleF imageRect)
        {
            var upperLeft = ToPictureBoxCoordinate(imageRect.Location);
            var bottomRight = ToPictureBoxCoordinate(new PointF(imageRect.Right, imageRect.Bottom));

            return RectangleF.FromSides(upperLeft.X, upperLeft.Y, bottomRight.X, bottomRight.Y);
        }

        public RectangleF ToImageCoordinate(RectangleF pictureBoxRect)
        {
            var upperLeft = ToPictureBoxCoordinate(pictureBoxRect.Location);
            var bottomRight = ToPictureBoxCoordinate(new PointF(pictureBoxRect.Right, pictureBoxRect.Bottom));

            return RectangleF.FromSides(upperLeft.X, upperLeft.Y, bottomRight.X, bottomRight.Y);
        }

        public float ZoomFactor
        {
            get
            {
                if (Image == null) return 0;
                return (float)imageBounds.Width / image.Width; //or height
            }
        }

        #endregion

        private Bitmap image;
        public Bitmap Image
        { 
            get { return image; }
            set
            {
                this.image = value; 
                
                if (image != null)
                {
                    if (this.AutoScale || this.imageBounds == default(RectangleF))
                        this.imageBounds = fitAndCenterImage(image.Size, this.ClientSize);
                }

                this.Invalidate();
            }
        }

        public bool AutoScale { get; set; }

        public Keys ActionModifierKey
        {
            get;
            set;
        }

        RectangleF imageBounds = default(RectangleF);
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Image != null)
            {
                e.Graphics.DrawImage(Image, imageBounds);
            }

            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (image != null)
            {
                if (AutoScale ||
                   (imageBounds.Width < image.Width && imageBounds.Height < image.Height))
                {
                    imageBounds = fitAndCenterImage(image.Size, this.ClientSize);
                }
            }

            base.OnSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnShown(EventArgs e)
        {
            if (image != null)
            {
                imageBounds = fitAndCenterImage(image.Size, this.ClientSize);
            }

            base.OnShown(e);
            this.Invalidate();
        }

        #region Image mouse translate & scale

        Cursor cursor = Cursors.Default;
        PointF startDragLocation = PointF.Empty;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool shouldTranslate = (e.Modifiers & ActionModifierKey) != Keys.None;

            if (shouldTranslate)
            {
                if (startDragLocation.IsZero) //on start drag
                {
                    startDragLocation = e.Location;
                    cursor = this.Cursor;
                    Cursor = Cursors.Pointer;
                }

                //on drag
                imageBounds.X += e.Location.X - startDragLocation.X;
                imageBounds.Y += e.Location.Y - startDragLocation.Y;
                startDragLocation = e.Location;

                this.Invalidate();
            }
            else //on end drag
            {
                startDragLocation = PointF.Empty;
                Cursor = cursor;
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            var shouldScroll = (e.Modifiers & ActionModifierKey) != Keys.None;

            if (shouldScroll)
            {
                var zoomCenter = new PointF(e.Location.X - imageBounds.X, e.Location.Y - imageBounds.Y);
                var zoomIn = e.Delta.Height > 0;

                updateImageBounds(ref imageBounds, image.Size, zoomIn, zoomCenter);

                if (imageBounds.Width < image.Width && imageBounds.Height < image.Height && !zoomIn)
                    imageBounds = fitAndCenterImage(image.Size, this.ClientSize);
            }

            base.OnMouseWheel(e);

            if (shouldScroll)
                this.Invalidate();
        }

        #endregion

        private static RectangleF fitAndCenterImage(Size imageSize, Size clientSize)
        {
            var scale = Math.Min((float)clientSize.Width / imageSize.Width, (float)clientSize.Height / imageSize.Height);
            var offsetX = (float)(clientSize.Width - imageSize.Width * scale) / 2;
            var offsetY = (float)(clientSize.Height - imageSize.Height * scale) / 2;

            return new RectangleF(offsetX, offsetY, imageSize.Width * scale, imageSize.Height * scale);
        }

        private static void updateImageBounds(ref RectangleF imageBounds, Size imageSize, bool zoomIn, PointF zoomCenter)
        {
            var previousZoom = (float)imageBounds.Width / imageSize.Width;
            var currentZoom = getZoomFactor(imageBounds, imageSize, zoomIn);

            if (Math.Abs(previousZoom - currentZoom) < 1E-2) return;
            var zoomRatio = currentZoom / previousZoom;

            //update bounds size
            imageBounds.Width = imageBounds.Width * zoomRatio;
            imageBounds.Height = imageBounds.Height * zoomRatio;

            //return the zoom center to the previous position
            imageBounds.X += (1 - zoomRatio) * zoomCenter.X;
            imageBounds.Y += (1 - zoomRatio) * zoomCenter.Y;
        }

        private static float getZoomFactor(RectangleF imageBounds, Size imageSize, bool zoomIn)
        {
            var previousZoom = (float)imageBounds.Width / imageSize.Width;
            return zoomIn ? (previousZoom + 0.2f) : (previousZoom - 0.2f);
        }
    }

    static class PenExtension //TODO: remove when Pen is updated
    {
        public static Pen Clone(this Pen pen)
        {
            var clonedPen = new Pen(pen.Color, pen.Thickness);
            clonedPen.LineCap = pen.LineCap;
            clonedPen.LineJoin = pen.LineJoin;
            clonedPen.MiterLimit = pen.MiterLimit;
            clonedPen.DashStyle = pen.DashStyle; //deep copy needed ?

            return clonedPen;
        }
    }
}
