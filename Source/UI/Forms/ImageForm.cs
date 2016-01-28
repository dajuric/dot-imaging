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

using Eto.Drawing;
using Eto.Forms;

namespace DotImaging
{
    /// <summary>
    /// Image display form.
    /// </summary>
    internal class ImageForm : Form
    {
        Bitmap bmp = null;

        /// <summary>
        /// Sets the specified image.
        /// </summary>
        /// <param name="image">Image to display.</param>
        public void SetImage(Bgr<byte>[,] image)
        {
            if (bmp == null || bmp.Width != image.Width() || bmp.Height != image.Height())
            {
                bmp = new Bitmap(image.Width(), image.Height(), PixelFormat.Format24bppRgb);
            }

            using (BitmapData bmpData = bmp.Lock())
            using (var uIm = image.Lock())
            {
                Copy.UnsafeCopy2D(uIm.ImageData, bmpData.Data, uIm.Stride, bmpData.ScanWidth, uIm.Height);
            }

            PictureBox.Image = bmp;

            if (ScaleForm)
                ClientSize = new Size(image.Width(), image.Height());
        }

        /// <summary>
        /// Creates new image display form.
        /// </summary>
        /// <param name="title">Window title.</param>
        public ImageForm(string title = "")
        {
            Title = title;
            ClientSize = new Size(640, 480);

            PictureBox = new PictureBox { Image = bmp, AutoScale = true };
            Content = PictureBox;

            this.Shown += (s, e) => PictureBox.Image = bmp;
        }

        /// <summary>
        /// Gets or sets whether to rescale the form to the image size.
        /// </summary>
        public bool ScaleForm
        {
            get; set;
        }

        /// <summary>
        /// Gets the underlying picture box control.
        /// </summary>
        public PictureBox PictureBox { get; private set; }
    }
}
