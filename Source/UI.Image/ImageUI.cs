#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2019
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

namespace DotImaging
{
    /// <summary>
    /// Provides simple standalone UI elements throughout static methods or extensions.
    /// </summary>
    public static class ImageUI
    {
        /// <summary>
        /// Closes all UI controls if displayed.
        /// </summary>
        public static void CloseAll()
        {
            CvInvoke.cvDestroyAllWindows();
        }

        /// <summary>
        /// Displays the specified image in a window and pauses the execution flow.
        /// </summary>
        /// <param name="image">Image to show.</param>
        /// <param name="windowTitle">Window title (ID).</param>
        /// <param name="autoSize">True to adjust form to the image size, false otherwise.</param>
        public static void ShowDialog(this Bgr<byte>[,] image, string windowTitle = "Image", bool autoSize = false)
        {
            Show(image, windowTitle, autoSize);

            while (CvInvoke.cvGetWindowProperty(windowTitle, 0) >= 0)
                CvInvoke.cvWaitKey(5);
        }

        /// <summary>
        /// Displays the specified image in a window (non blocking mode).
        /// </summary>
        /// <param name="image">Image to show.</param>
        /// <param name="windowTitle">Window title (ID).</param>
        /// <param name="autoSize">True to adjust form to the image size, false otherwise.</param>
        public static void Show(this Bgr<byte>[,] image, string windowTitle = "Image", bool autoSize = false)
        {
            CvInvoke.cvNamedWindow(windowTitle, autoSize ? WindowSizing.AutoSize : WindowSizing.Normal);

            using (var uIm = image.Lock())
            {
                var mat = uIm.AsCvMat();
                CvInvoke.cvShowImage(windowTitle, ref mat);
            }

            CvInvoke.cvWaitKey(5);
        }

    }
}
