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
using System;
using System.Windows.Forms;
using DotImaging.Primitives2D;

namespace MultipleCameraCapture
{
    public partial class CaptureWindow : Form
    {
        CameraCapture reader;

        public CaptureWindow(int cameraIndex = -1)
        {
            InitializeComponent();
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            if (cameraIndex == -1)
                return;

            reader = new CameraCapture(cameraIndex);
            reader.FrameSize = new Size(640, 480);
            reader.Open();

            this.Text = "Camera " + cameraIndex;
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
            }

            this.pictureBox.Image = frame.ToBitmap();
            GC.Collect();
        }

        private void CaptureDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (reader != null)
                reader.Dispose();
        }
    }
}
