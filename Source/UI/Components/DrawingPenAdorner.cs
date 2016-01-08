using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotImaging
{
    class DrawingPenAdorner : IDisposable
    {
        public class Path
        {
            public Path(Pen pen, float zoomFactor = 1f)
            {
                this.Pen = pen;
                this.ZoomFactor = zoomFactor;
                this.Points = new List<PointF>();
            }

            public Pen Pen { get; private set; }

            public float ZoomFactor { get; private set; }

            public List<PointF> Points { get; private set; }

            public Pen GetScaledPen(float newZoomFactor)
            {
                var thickness = Pen.Thickness / ZoomFactor;
                var newPen = Pen.Clone();
                newPen.Thickness =  thickness * newZoomFactor;
                return newPen;
            }
        }

        PictureBox control = null;
        bool isDrawing = false;

        public DrawingPenAdorner(PictureBox pictureBox)
        {
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
            pictureBox.Paint += PictureBox_Paint;

            control = pictureBox;

            Paths = new List<Path>();

            Pen = new Pen(Colors.Green, 10);
            Pen.LineCap = PenLineCap.Round;
            Pen.LineJoin = PenLineJoin.Round;
        }

        private static void drawPaths(Graphics g, List<Path> paths, Func<PointF, PointF> pointTransform, float zoomFactor = 1.0f)
        {
            g.AntiAlias = false;

            for (int i = 0; i < paths.Count; i++)
            {
                var path = paths[i];
                for (int pointIdx = 0; pointIdx < (path.Points.Count - 1); pointIdx++)
                {
                    var p1 = pointTransform(path.Points[pointIdx]);
                    var p2 = pointTransform(path.Points[pointIdx + 1]);

                    var pen = path.GetScaledPen(zoomFactor);
                    g.DrawLine(pen, p1, p2);
                }
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (control.Image == null) return;

            e.Graphics.SetClip(control.ToPictureBoxCoordinate(new RectangleF(0, 0, control.Image.Width, control.Image.Height)));
            drawPaths(e.Graphics, Paths, p => control.ToPictureBoxCoordinate(p), control.ZoomFactor);
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

            var currentPath = Paths.Last();
            currentPath.Points.Add(control.ToImageCoordinate(e.Location));
            control.Invalidate();
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Buttons != MouseButtons.Primary || control.Image == null) return;

            isDrawing = true;
            Paths.Add(new Path(Pen.Clone(), control.ZoomFactor));
        }

        public void Undo()
        {
            if (Paths.Count > 0)
            {
                Paths.RemoveAt(Paths.Count - 1);
                control.Invalidate();
            }
        }

        public bool CanUndo
        {
            get { return Paths.Count > 0; }
        }

        public Pen Pen { get; set; }

        public List<Path> Paths
        {
            get;
            private set;
        }

        public Bitmap ToImageMask()
        {
            if (control.Image == null) return null;

            Bitmap bmp = new Bitmap(control.Image.Width, control.Image.Height, PixelFormat.Format32bppRgba);
            using (Graphics g = new Graphics(bmp))
            {
                //rember old colors
                var oldColors = Paths.Select(x => x.Pen.Color).ToArray();

                Paths.ForEach(x => x.Pen.Color = Colors.White);
                drawPaths(g, Paths, p => p);

                //restore colors
                for (int i = 0; i < oldColors.Length; i++)
                {
                    Paths[i].Pen.Color = oldColors[i];
                }
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
