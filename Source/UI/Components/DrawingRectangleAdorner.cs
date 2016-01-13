using Eto.Drawing;
using Eto.Forms;
using System;

namespace DotImaging
{
    class DrawingRectangleAdorner : IDisposable
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
    }
}
