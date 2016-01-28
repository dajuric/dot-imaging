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

using Eto.Forms;

namespace DotImaging
{
    /// <summary>
    ///  Image display form supporting mask creation.
    /// </summary>
    internal class DrawingPenForm : ImageForm
    {
        DrawingPenAdorner adorner = null;

        /// <summary>
        /// Creates new image display form supporting mask creation.
        /// </summary>
        /// <param name="title">Window title.</param>
        public DrawingPenForm(string title = "")
            :base(title)
        {
            PictureBox.ContextMenu = new ContextMenu
            {
                Items =
                {
                    new ButtonMenuItem { Text="Increase pen size", Command = new Command((s, e) => changePenSize(true)), Shortcut = Keys.Control | Keys.Up  },
                    new ButtonMenuItem { Text="Decrease pen size", Command = new Command((s, e) => changePenSize(false)), Shortcut = Keys.Control | Keys.Down },
                    new ButtonMenuItem { Text="Undo", Command = new Command((s, e) => adorner.Undo()), Shortcut = Keys.Control | Keys.Z }
                }
            };

            PictureBox.ContextMenu.Opening += (s, e) => 
            {
                PictureBox.ContextMenu.Items[2].Enabled = adorner.CanUndo;
            };

            PictureBox.KeyDown += (s, e) => 
            {
                foreach (var item in PictureBox.ContextMenu.Items)
                {
                    if (item.Shortcut == (e.Modifiers | e.Key))
                    {
                        if (item.Command.CanExecute(null))
                            item.Command.Execute(null);

                        break;
                    }
                }
            };

            adorner = new DrawingPenAdorner(PictureBox);
        }

        private void changePenSize(bool increase)
        {
            var direction = increase ? 1 : -1;
            var newSize = adorner.Pen.Thickness + 5 * direction;
            newSize = System.Math.Min(25, System.Math.Max(1, newSize));
            adorner.Pen.Thickness = newSize;
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
