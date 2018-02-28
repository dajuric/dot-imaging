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

using DotImaging.Primitives2D;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace DotImaging
{
    /// <summary>
    /// OpenCV video codec name.
    /// </summary>
    public class VideoCodec
    {
        /// <summary>
        /// MPEG1 codec name: "PIM1".
        /// </summary>
        public static readonly VideoCodec MPEG1 = VideoCodec.FromName('P', 'I', 'M', '1');
        /// <summary>
        /// Motion JPEG codec name: "MJPG".
        /// </summary>
        public static readonly VideoCodec MotionJpeg = VideoCodec.FromName('M', 'J', 'P', 'G');
        /// <summary>
        /// Intel YUV codec name: "IYUV".
        /// </summary>
        public static readonly VideoCodec IntelYUV = VideoCodec.FromName('I', 'Y', 'U', 'V');
        /// <summary>
        /// User selection - on Windows dialog will be opened. Value: -1.
        /// </summary>
        public static readonly VideoCodec UserSelection = new VideoCodec(-1);

        private const int CODEC_NAME_LENGTH = 4;
        int codec;

        /// <summary>
        /// Creates new video codec from an id.
        /// </summary>
        /// <param name="codec">Codec id.</param>
        private VideoCodec(int codec)
        {
            this.codec = codec;
        }

        /// <summary>
        /// Creates new video codec id from 4-character code. For example, FromName('P','I','M','1') is MPEG-1 codec, FromName('M','J','P','G') is motion-jpeg codec etc.
        /// </summary>
        /// <param name="c1">First char.</param>
        /// <param name="c2">Second char.</param>
        /// <param name="c3">Third char.</param>
        /// <param name="c4">Fourth char.</param>
        /// <returns>Video codec.</returns>
        public static VideoCodec FromName(char c1, char c2, char c3, char c4)
        {
            int codec = (c1 & 255) + ((c2 & 255) << 8) + ((c3 & 255) << 16) + ((c4 & 255) << 24);
            return new VideoCodec(codec);
        }

        /// <summary>
        /// Creates new video codec id from 4-character string.
        /// </summary>
        /// <param name="codecName">4-character string codec name.</param>
        /// <returns>Video codec.</returns>
        public static VideoCodec FromName(string codecName)
        {
            if (codecName.Length != CODEC_NAME_LENGTH)
                throw new Exception("Codec name is 4-characters long!");

            return VideoCodec.FromName(codecName[0], codecName[1], codecName[2], codecName[3]);
        }

        /// <summary>
        /// Casts video codec to an 32-bit integer.
        /// </summary>
        /// <param name="videoCodec">Video codec.</param>
        /// <returns>32-bit integer representation of the video codec.</returns>
        public static implicit operator int(VideoCodec videoCodec)
        {
            return videoCodec.codec;
        }

        /// <summary>
        /// Creates video codec from the 32-bit integer.
        /// </summary>
        /// <param name="code">32-bit code.</param>
        /// <returns>New codec name.</returns>
        public static explicit operator VideoCodec(int code)
        {
            return new VideoCodec(code);
        }

        /// <summary>
        /// Creates video codec from the 4-character string.
        /// </summary>
        /// <param name="code">4-character string.</param>
        /// <returns>New codec name.</returns>
        /// <exception cref="System.Exception">Invalid string length.</exception>
        public static explicit operator VideoCodec(string code)
        {
            return VideoCodec.FromName(code);
        }

        /// <summary>
        /// Gets the string representation of the codec name.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            unsafe 
            {
                fixed (int* intPtr = &this.codec)
                {
                    sbyte* chPtr = (sbyte*)intPtr;
                    return new string(chPtr, 0, CODEC_NAME_LENGTH);
                }
            }
        }
    }

    /// <summary>
    /// OpenCV capture properties for camera and video.
    /// </summary>
    internal enum CaptureProperty: int
    {
        PosMsec = 0,
        PosFrames = 1,
        FrameWidth = 3,
        FrameHeight = 4,
        FPS = 5,
        FrameCount = 7,

        /************** camera properties ******************/
        Brightness = 10,
        Contrast = 11,
        Saturation = 12,
        Hue = 13,
        Gain = 14,
        Exposure = 15,
        /************** camera properties ******************/

        ConvertRGB = 16
    }

    /// <summary>
    /// OpenCV image load mode.
    /// </summary>
    [Flags]
    internal enum ImageLoadType : int
    {
        /// <summary>
        /// Loads the image as is (including the alpha channel if present) 
        /// </summary>
        Unchanged = -1,

        /// <summary>
        /// Loads the image as an intensity image
        /// </summary>
        Grayscale = 0,

        /// <summary>
        ///  Loads the image in the RGB format
        /// </summary>
        Color = 1,

        /// <summary>
        /// Loads the image of any color
        /// </summary>
        AnyColor = 4,

        /// <summary>
        /// Loads the image of any depth
        /// </summary>
        AnyDepth = 2
    }

    /// <summary>
    /// Internal class for OpenCV core / highgui library invocation.
    /// </summary>
    internal static class CvInvoke
    {
        public const CallingConvention CvCallingConvention = CallingConvention.Cdecl;
        public const string OPENCV_CORE_LIBRARY = "opencv_core2412";
        public const string OPENCV_HIGHGUI_LIBRARY = "opencv_highgui2412";

        #region Core

        [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvCallingConvention)]
        public unsafe static extern void cvReleaseMat(ref CvMat* mat);

        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvCallingConvention)]
        public unsafe static extern void cvReleaseImage(ref IplImage* image);
        #endregion

        #region Video reader

        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern IntPtr cvCreateCameraCapture(int index);

        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern IntPtr cvCreateFileCapture([MarshalAs(UnmanagedType.LPStr)] string filename);


        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern void cvReleaseCapture(ref IntPtr capture);


        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern int cvGrabFrame(IntPtr capture);

        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern IntPtr cvQueryFrame(IntPtr capture);


        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern double cvGetCaptureProperty(IntPtr capture, CaptureProperty propertyId);

        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool cvSetCaptureProperty(IntPtr capture, CaptureProperty propertyId, double value);

        public static Size GetImageSize(IntPtr capturePtr)
        {
            return new Size
            {
                Width = (int)CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.FrameWidth),
                Height = (int)CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.FrameHeight)
            };
        }

        public static bool SetImageSize(IntPtr capturePtr, Size newSize)
        {
            bool success;
            success = CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.FrameWidth, newSize.Width);
            success &= CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.FrameHeight, newSize.Height);

            return success;
        }

        #endregion

        #region Video writer

        /// <summary>
        /// Creates video writer structure.
        /// </summary>
        /// <param name="filename">Name of the output video file.</param>
        /// <param name="fourcc">4-character code of codec used to compress the frames. See <see cref="VideoCodec"/> class.</param>
        /// <param name="fps">Frame rate of the created video stream. </param>
        /// <param name="frameSize">Size of video frames.</param>
        /// <param name="isColor">If true, the encoder will expect and encode color frames, otherwise it will work with grayscale frames </param>
        /// <returns>The video writer</returns>
        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern IntPtr cvCreateVideoWriter([MarshalAs(UnmanagedType.LPStr)] String filename, int fourcc, double fps, Size frameSize, [MarshalAs(UnmanagedType.Bool)] bool isColor);

        /// <summary>
        /// Writes/appends one frame to video file.
        /// </summary>
        /// <param name="writer">video writer structure.</param>
        /// <param name="image">the written frame</param>
        /// <returns>True on success, false otherwise</returns>
        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool cvWriteFrame(IntPtr writer, IntPtr image);

        /// <summary>
        /// Finishes writing to video file and releases the structure.
        /// </summary>
        /// <param name="writer">pointer to video file writer structure</param>
        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public static extern void cvReleaseVideoWriter(ref IntPtr writer);
        #endregion

        #region Image IO

        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public unsafe static extern IplImage* cvLoadImage([MarshalAs(UnmanagedType.LPStr)] String filename, ImageLoadType loadType);

        //[SuppressUnmanagedCodeSecurity]
        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public unsafe static extern bool cvSaveImage([MarshalAs(UnmanagedType.LPStr)] String filename, IplImage* image, IntPtr parameters);

        public const int CV_IMWRITE_JPEG_QUALITY = 1;
        public const int CV_IMWRITE_PNG_COMPRESSION = 16;
        public const int CV_IMWRITE_PXM_BINARY = 32;

        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public unsafe static extern CvMat* cvEncodeImage([MarshalAs(UnmanagedType.LPStr)] string ext, CvMat* image, int* parameters = null);

        [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvCallingConvention)]
        public unsafe static extern CvMat* cvDecodeImageM(void* buffer, ImageLoadType loadType);

        #endregion Image IO
    }
}
