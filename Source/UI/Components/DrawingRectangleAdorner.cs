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
    class DrawingRectangleAdorner : IDrawingAdorner
    {
        const int MIN_RECT_SIZE = 10;

        PictureBox control = null;
        bool isDrawing = false;

        public DrawingRectangleAdorner(PictureBox pictureBox)
        {
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
            pictureBox.Paint += PictureBox_Paint;

            control = pictureBox;

            rect = RectangleF.Empty;

            Pen = new Pen(Colors.Green, 5);
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (control.Image == null) return;

            e.Graphics.SetClip(control.ToPictureBoxCoordinate(new RectangleF(0, 0, control.Image.Width, control.Image.Height)));
            e.Graphics.DrawRectangle(Pen, control.ToPictureBoxCoordinate(rect));
            e.Graphics.ResetClip();
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Buttons != MouseButtons.Primary || control.Image == null) return;

            isDrawing = false;
            if (OnDrawn != null) OnDrawn();
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || control.Image == null) return;

            var ptSecond = control.ToImageCoordinate(e.Location);

            rect = new RectangleF
            {
                X      = Math.Min(ptFirst.X, ptSecond.X),
                Y      = Math.Min(ptFirst.Y, ptSecond.Y),
                Width  = Math.Abs(ptFirst.X - ptSecond.X),
                Height = Math.Abs(ptFirst.Y - ptSecond.Y)
            };

            rect.Width = Math.Max(MIN_RECT_SIZE, rect.Width);
            rect.Height = Math.Max(MIN_RECT_SIZE, rect.Height);

            control.Invalidate();
        }

        PointF ptFirst;
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Buttons != MouseButtons.Primary || control.Image == null) return;

            isDrawing = true;
            ptFirst = control.ToImageCoordinate(e.Location);
            rect.Location = ptFirst; //if user draws MIN_RECT_SIZE, add it to click location
        }

        public Pen Pen { get; set; }

        RectangleF rect;
        public RectangleF Rectangle
        {
            get { return rect; }
        }

        public Bitmap ToImageMask()
        {
            if (control.Image == null) return null;

            Bitmap bmp = new Bitmap(control.Image.Width, control.Image.Height, PixelFormat.Format32bppRgba);
            using (Graphics g = new Graphics(bmp))
            {
                //remember old color
                var oldColor = Pen.Color;

                Pen.Color = Colors.White;
                g.DrawRectangle(Pen, rect);

                //restore color
                Pen.Color = oldColor;
            }

            return bmp;
        }

        public void Dispose()
        {
            control.MouseDown -= PictureBox_MouseDown;
            control.MouseMove -= PictureBox_MouseMove;
            control.MouseUp -= PictureBox_MouseUp;
            control.Paint -= PictureBox_Paint;
        }

        public Action OnDrawn { get; set; }

        public void Clear()
        {
            rect = RectangleF.Empty;
            control.Invalidate();
        }
    }
}
