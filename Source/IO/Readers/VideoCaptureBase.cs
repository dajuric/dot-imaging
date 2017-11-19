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

using System;
using DotImaging.Primitives2D;

namespace DotImaging
{
    /// <summary>
    /// Represents the base class for video capture that shares common functions and properties with camera and file capture. 
    /// </summary>
    public abstract class VideoCaptureBase : ImageStreamReader
    {
        /// <summary>
        /// Internal OpenCV pointer for the capture object.
        /// </summary>
        protected IntPtr capturePtr;

        /// <summary>
        /// Releases all resources allocated by capture.
        /// </summary>
        public override void Close()
        {
            if (capturePtr != IntPtr.Zero)
                CvInvoke.cvReleaseCapture(ref capturePtr);
        }

        object syncObj = new object();
        /// <summary>
        /// Reads the next image in the stream and advances the position by one.
        /// </summary>
        /// <param name="image">Read image.</param>
        /// <returns>True if the reading operation was successful, false otherwise.</returns>
        protected override bool ReadInternal(out IImage image)
        {
            bool status = false;
            image = default(IImage);

            lock (syncObj)
            {
                IntPtr cvFramePtr;
                cvFramePtr = CvInvoke.cvQueryFrame(capturePtr);

                if (cvFramePtr != IntPtr.Zero)
                {
                    image = IplImage.FromPointer(cvFramePtr).AsImage();
                    this.Position++;
                    status = true;
                }
            }

            return status;
        }

        /// <summary>
        /// Gets the length in number of frames.
        /// </summary>
        public override long Length
        {
            get { return (long)CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.FrameCount); }
        }

        /// <summary>
        /// Gets or sets whether to force conversion of an input image to Bgr color type.
        /// </summary>
        public bool ConvertRgb
        {
            get { return (int)CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.ConvertRGB) != 0; }
            set { CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.ConvertRGB, value ? 0 : 1); }
        }

        /// <summary>
        /// Gets the frame size.
        /// </summary>
        public Size FrameSize
        {
            get { return CvInvoke.GetImageSize(capturePtr); }
        }

        /// <summary>
        /// Gets the frame rate.
        /// </summary>
        public float FrameRate
        {
            get { return (float)CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.FPS); }
        }
    }
}
