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
using System.Runtime.InteropServices;
using System.Security;

namespace DotImaging
{
    /// <summary>
    /// OpenCV image load mode.
    /// </summary>
    internal enum WindowSizing : int
    {
       Normal = 0x00000000,
       AutoSize = 0x00000001
    }

    /// <summary>
    /// Internal class for OpenCV core / highgui library invocation.
    /// </summary>
    static class CvInvoke
    {
        public const CallingConvention CvCallingConvention = CallingConvention.Cdecl;
        public const string OPENCV_HIGHGUI_LIBRARY = "opencv_highgui2412";


        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern void cvNamedWindow(string name, WindowSizing sizing);

        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern void cvShowImage(string name, ref CvMat image);

        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern void cvWaitKey(int delay);

        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern void cvDestroyAllWindows();

        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern double cvGetWindowProperty(string name, int prop_id);
    }
}
