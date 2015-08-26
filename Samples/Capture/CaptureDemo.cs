#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2015-2015 
// darko.juric2@gmail.com
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
// 
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see <https://www.gnu.org/licenses/lgpl.txt>.
//
#endregion

using DotImaging;
using DotImaging.Linq;
using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using DotImaging.Primitives2D;

namespace Capture
{
    public partial class CaptureDemo : Form
    {
        ImageStreamReader reader;

        public CaptureDemo()
        {
            InitializeComponent();
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            reader = new CameraCapture(0); //capture from camera
            (reader as CameraCapture).FrameSize = new Size(640, 480);

            //reader = new FileCapture(Path.Combine(getResourceDir(), "Welcome.mp4")); //capture from video
            //reader = new ImageDirectoryCapture(Path.Combine(getResourceDir(), "Sequence"), "*.jpg");
            reader.Open();

            Application.Idle += capture_NewFrame;
        }

        Bgr<byte>[,] frame = null;
        void capture_NewFrame(object sender, EventArgs e)
        {
            reader.ReadTo<Bgr<byte>>(ref frame);

            if (frame == null)
            {
                /*Application.Idle -= capture_NewFrame;
                return;*/
                reader.Seek(0, SeekOrigin.Begin);
                return;
            }

            this.pictureBox.Image = frame.ToBitmap();

            //LINQ Grayscale conversion
            /*this.pictureBox.Image = frame.AsEnumerable()
                                         .Select(x => x.ToGray())
                                         .ToArray2D(frame.Size()).ToBitmap();*/
            GC.Collect();
        }

        private void CaptureDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (reader != null)
                reader.Dispose();
        }

        private static string getResourceDir()
        {
            var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent;
            if (directoryInfo != null)
                return Path.Combine(directoryInfo.FullName, "Resources");
            return null;
        }
    }
}
