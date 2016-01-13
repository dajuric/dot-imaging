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
using Eto.Drawing;
using Eto.Forms;
using System.Globalization;

namespace DotImaging
{
    /// <summary>
    ///  Image display form supporting mask creation.
    /// </summary>
    internal class DrawingPenForm : Form
    {
        PictureBox pictureBox = null;
        DrawingPenAdorner adorner = null;

        public void setImage(Bgr<byte>[,] image)
        {
            if (image == null) return;

            var bmp = new Bitmap(image.Width(), image.Height(), PixelFormat.Format24bppRgb);

            using (var bmpData = bmp.Lock())
            using (var uImg = image.Lock())
            {
                Copy.UnsafeCopy2D(uImg.ImageData, bmpData.Data, uImg.Stride, bmpData.ScanWidth, uImg.Height);
            }

            if (ScaleForm)
                ClientSize = new Size(image.Width(), image.Height());

            pictureBox.Image = bmp;
        }

        /// <summary>
        /// Creates new image display form supporting mask creation.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="image">Image to display.</param>
        public DrawingPenForm(string title = "", Bgr<byte>[,] image = null)
        {
            Title = title;
            ClientSize = new Size(640, 480);
           
            pictureBox = new PictureBox();
            pictureBox.ContextMenu = new ContextMenu
            {
                Items =
                {
                    new ButtonMenuItem { Text="Increase pen size", Command = new Command((s, e) => changePenSize(true)), Shortcut = Keys.Control | Keys.Up  },
                    new ButtonMenuItem { Text="Decrease pen size", Command = new Command((s, e) => changePenSize(false)), Shortcut = Keys.Control | Keys.Down },
                    new ButtonMenuItem { Text="Undo", Command = new Command((s, e) => adorner.Undo()), Shortcut = Keys.Control | Keys.Z }
                }
            };

            pictureBox.ContextMenu.Opening += (s, e) => 
            {
                pictureBox.ContextMenu.Items[2].Enabled = adorner.CanUndo;
            };

            pictureBox.KeyDown += (s, e) => 
            {
                foreach (var item in pictureBox.ContextMenu.Items)
                {
                    if (item.Shortcut == (e.Modifiers | e.Key))
                    {
                        if (item.Command.CanExecute(null))
                            item.Command.Execute(null);

                        break;
                    }
                }
            };

            Content = pictureBox;
            this.Shown += (s, e) => setImage(image);
            adorner = new DrawingPenAdorner(pictureBox);

            pictureBox.CanFocus = true;
            pictureBox.Focus();
        }

        private void changePenSize(bool increase)
        {
            var direction = increase ? 1 : -1;
            var newSize = adorner.Pen.Thickness + 5 * direction;
            newSize = System.Math.Min(25, System.Math.Max(1, newSize));
            adorner.Pen.Thickness = newSize;
        }

        /// <summary>
        /// Gets or sets whether to rescale the form to the image size.
        /// </summary>
        public bool ScaleForm
        {
            get; set;
        }

        /// <summary>
        /// Gets the created mask.
        /// </summary>
        public Gray<byte>[,] Mask
        {
            get
            {
                var bmpMask = adorner.ToImageMask();
                var image = new Bgra<byte>[bmpMask.Height, bmpMask.Width];

                using (var bmpData = bmpMask.Lock())                    
                using (var uImg = image.Lock())
                {
                    Copy.UnsafeCopy2D(bmpData.Data, uImg.ImageData, bmpData.ScanWidth, uImg.Stride, uImg.Height);
                }

                return image.ToGray();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                adorner.Dispose();

            base.Dispose(disposing);
        }
    }
}
