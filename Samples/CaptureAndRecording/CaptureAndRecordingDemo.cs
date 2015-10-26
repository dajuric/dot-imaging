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
using System.IO;
using System.Windows.Forms;

namespace CaptureAndRecording
{
    public partial class CaptureAndRecordingDemo : Form
    {
        VideoCaptureBase reader; //use specific class to set specific properties (e.g. for camera, for video, for image dir)
        ImageStreamWriter writer;

        public CaptureAndRecordingDemo()
        {
            InitializeComponent();
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            reader = new CameraCapture(0); //capture from camera
            //reader = new FileCapture(Path.Combine(getResourceDir(), "Welcome.mp4"));
            reader.Open();

            writer = new VideoWriter(@"output.avi", reader.FrameSize, /*reader.FrameRate does not work Cameras*/ 30); //TODO: bug: FPS does not work for cameras
            writer.Open();

            Application.Idle += capture_NewFrame;
        }

        Bgr<byte>[,] frame = null;
        void capture_NewFrame(object sender, EventArgs e)
        { 
            reader.ReadTo(ref frame);

            if (frame == null)
            {
                Application.Idle -= capture_NewFrame;
                return;
            }

            this.pictureBox.Image = frame.ToBitmap();

            bool isFrameWritten = writer.Write(frame.Lock()); //TODO: revise
            if (!isFrameWritten)
                MessageBox.Show("Frame is not written!", "Frame writing error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            GC.Collect();
        }

        private void CaptureAndRecordingDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (reader != null)
                reader.Dispose();

            if (writer != null)
                writer.Dispose();
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
