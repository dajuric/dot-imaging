#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2015
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
                Application.Idle -= capture_NewFrame;
                return;
                /*reader.Seek(0, SeekOrigin.Begin);
                return;*/
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
