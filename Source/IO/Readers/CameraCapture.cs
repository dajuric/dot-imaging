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

using System;
using DotImaging.Primitives2D;

namespace DotImaging
{
    /// <summary>
    /// Represents camera stream-able source and provides functions and properties to access a device in a stream-able way.
    /// </summary>
    public class CameraCapture: VideoCaptureBase
    {
        int cameraIdx = 0;

        /// <summary>
        /// Creates capture from camera.
        /// </summary>
        /// <param name="cameraIdx">Camera index.</param>
        public CameraCapture(int cameraIdx = 0)
        {
            this.cameraIdx = cameraIdx;
            this.CanSeek = false;
            this.IsLiveStream = true;
            this.Open(); //to enable property change
        }

        /// <summary>
        /// Opens the camera stream.
        /// </summary>
        public override void Open()
        {
            if (capturePtr != IntPtr.Zero)
                return;
            
            capturePtr = CvInvoke.cvCreateCameraCapture(cameraIdx);
            if (capturePtr == IntPtr.Zero)
                throw new Exception("Cannot open camera stream! It seems that camera device can not be found.");
        }

        /// <summary>
        /// Gets or sets the brightness of the camera.
        /// <para>If the property is not supported by device 0 will be returned.</para>
        /// </summary>
        public double Brightness
        {
            get { return CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.Brightness); }
            set { CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.Brightness, value); }
        }

        /// <summary>
        /// Gets or sets the contrast of the camera.
        /// <para>If the property is not supported by device 0 will be returned.</para>
        /// </summary>
        public double Contrast
        {
            get { return CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.Contrast); }
            set { CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.Contrast, value); }
        }

        /// <summary>
        /// Gets or sets the exposure of the camera.
        /// <para>If the property is not supported by device 0 will be returned.</para>
        /// </summary>
        public double Exposure
        {
            get { return CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.Exposure); }
            set { CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.Exposure, value); }
        }

        /// <summary>
        /// Gets or sets the gain of the camera.
        /// <para>If the property is not supported by device 0 will be returned.</para>
        /// </summary>
        public double Gain
        {
            get { return CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.Gain); }
            set { CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.Gain, value); }
        }

        /// <summary>
        /// Gets or sets the hue of the camera.
        /// <para>If the property is not supported by device 0 will be returned.</para>
        /// </summary>
        public double Hue
        {
            get { return CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.Hue); }
            set { CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.Hue, value); }
        }

        /// <summary>
        /// Gets or sets the saturation of the camera.
        /// <para>If the property is not supported by device 0 will be returned.</para>
        /// </summary>
        public double Saturation
        {
            get { return CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.Saturation); }
            set { CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.Saturation, value); }
        }

        /// <summary>
        /// Gets or sets the frame size of the camera.
        /// </summary>
        public new Size FrameSize
        {
            get { return CvInvoke.GetImageSize(capturePtr); }
            set { CvInvoke.SetImageSize(capturePtr, value); }
        }

        /// <summary>
        /// Gets or sets the frame rate of the camera.
        /// <para>If the property is not supported by device 0 will be returned.</para>
        /// </summary>
        public new double FrameRate
        {
            get { return CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.FPS); }
            set { CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.FPS, value); }
        }

        /// <summary>
        /// Gets the available device count.
        /// <para>Warning: the function closes existing streams, so use it before any camera capture object is created.</para>
        /// </summary>
        public static int CameraCount
        {
            get
            {
                int cameraIdx = 0;
                while (true)
                {
                    var capturePtr = CvInvoke.cvCreateCameraCapture(cameraIdx);
                    if (capturePtr != IntPtr.Zero)
                    {
                        CvInvoke.cvReleaseCapture(ref capturePtr);
                        cameraIdx++;
                    }
                    else
                        break;
                }

                return cameraIdx;
            }
        }
    }
}
