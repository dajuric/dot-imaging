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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DotImaging
{
    /// <summary>
    /// OpenCV's Mat structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CvMat
    {
        /// <summary>
        /// OpenCV's channel depth enumeration.
        /// </summary>
        public enum CvChannelDepth : int
        {
            /// <summary>
            /// 8-bit unsigned type.
            /// </summary>
            CV_8U = 0,
            /// <summary>
            /// 8-bit signed type.
            /// </summary>
            CV_8S = 1,
            /// <summary>
            /// 16-bit unsigned type.
            /// </summary>
            CV_16U = 2,
            /// <summary>
            /// 16-bit signed type.
            /// </summary>
            CV_16S = 3,
            /// <summary>
            /// 32-bit integer signed type.
            /// </summary>
            CV_32S = 4,
            /// <summary>
            /// 32-bit floating-point signed type.
            /// </summary>
            CV_32F = 5,
            /// <summary>
            /// 64-bit floating-point signed type.
            /// </summary>
            CV_64F = 6,
            /// <summary>
            /// User defined type.
            /// </summary>
            UserType = 7
        }

        const int CV_CN_SHIFT = 3;

        /// <summary>
        /// Encoded mat depth and number of channels.
        /// </summary>
        int matType;
        /// <summary>
        /// Number of bytes per image row.
        /// </summary>
        public int Step;

        //internal
        int* refCount;
        int hdr_refcount;

        /// <summary>
        /// Image data pointer.
        /// </summary>
        public IntPtr ImageData;
        /// <summary>
        /// Image height.
        /// </summary>
        public int Height;
        /// <summary>
        /// Image width.
        /// </summary>
        public int Width;

        /// <summary>
        /// Creates new CvMat from the already existing data.
        /// </summary>
        /// <param name="imageData">Image data pointer.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="stride">Number of bytes per row.</param>
        /// <param name="depthType">Channel depth.</param>
        /// <param name="channelCount">Channel count.</param>
        /// <returns></returns>
        public static CvMat FromUserData(IntPtr imageData, int width, int height, int stride, CvChannelDepth depthType, int channelCount)
        {
            //taken from: https://github.com/Itseez/opencv/blob/ddf82d0b154873510802ef75c53e628cd7b2cb13/modules/core/include/opencv2/core/types_c.h
            const int CV_MAT_MAGIC_VAL = 0x42420000;

            //CV_MAKETYPE from: https://github.com/Itseez/opencv/blob/ddf82d0b154873510802ef75c53e628cd7b2cb13/modules/core/include/opencv2/core/cvdef.h
            const int CV_MAT_CONT_FLAG_SHIFT = 14;
            const int CV_MAT_CONT_FLAG = (1 << CV_MAT_CONT_FLAG_SHIFT);

            //CV_MAKETYPE from: https://github.com/Itseez/opencv/blob/ddf82d0b154873510802ef75c53e628cd7b2cb13/modules/core/include/opencv2/core/cvdef.h
            var matDepth = getDepth((int)depthType);
            var cvType = matDepth + ((channelCount - 1) << CV_CN_SHIFT);
            var mType = CV_MAT_MAGIC_VAL | CV_MAT_CONT_FLAG | cvType;

            return new CvMat
            {
                ImageData = imageData,
                Width = width,
                Height = height,
                Step = stride,
                matType = mType,
                hdr_refcount = 0,
                refCount = null
            };
        }

        /// <summary>
        /// Gets the channel depth.
        /// </summary>
        public CvChannelDepth ChannelDepth
        {
            get { return (CvChannelDepth)getDepth(matType); }
        }

        /// <summary>
        /// Gets the channel count.
        /// </summary>
        public int ChannelCount
        {
            get
            {
                //CV_ELEM_SIZE from: https://github.com/Itseez/opencv/blob/ddf82d0b154873510802ef75c53e628cd7b2cb13/modules/core/include/opencv2/core/cvdef.h
                //0x3a50 = 11 10 10 01 01 00 00 ~ array of log2(sizeof(arr_type_elem))
                const int CV_CN_MAX = 512;
                const int CV_MAT_CN_MASK = ((CV_CN_MAX - 1) << CV_CN_SHIFT);

                var CV_MAT_CN = (((matType & CV_MAT_CN_MASK) >> CV_CN_SHIFT) + 1);
                return (CV_MAT_CN << ((((sizeof(int) / 4 + 1) * 16384 | 0x3a50) >> getDepth(matType) * 2) & 3));
            }
        }

        //CV_MATDEPTH from: https://github.com/Itseez/opencv/blob/ddf82d0b154873510802ef75c53e628cd7b2cb13/modules/core/include/opencv2/core/cvdef.h
        static int getDepth(int flags)
        {
            const int CV_DEPTH_MAX = (1 << CV_CN_SHIFT);
            const int CV_MAT_DEPTH_MASK = (CV_DEPTH_MAX - 1);

            return (flags & CV_MAT_DEPTH_MASK);
        }
    }

    /// <summary>
    /// Provides extension CvMat conversion methods
    /// </summary>
    public static class CvMatTypeConversions
    {
        static Dictionary<Type, CvMat.CvChannelDepth> typeConversion = null;

        static CvMatTypeConversions()
        {
            typeConversion = new Dictionary<Type, CvMat.CvChannelDepth>();

            typeConversion.Add(typeof(byte), CvMat.CvChannelDepth.CV_8U);
            typeConversion.Add(typeof(sbyte), CvMat.CvChannelDepth.CV_8S);

            typeConversion.Add(typeof(ushort), CvMat.CvChannelDepth.CV_16U);
            typeConversion.Add(typeof(short), CvMat.CvChannelDepth.CV_16S);

            typeConversion.Add(typeof(int), CvMat.CvChannelDepth.CV_32S);

            typeConversion.Add(typeof(float), CvMat.CvChannelDepth.CV_32F);
            typeConversion.Add(typeof(double), CvMat.CvChannelDepth.CV_64F);
        }

        /// <summary>
        /// Represents the existing image as CvMat structure where data is shared.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <returns>CvMat representation.</returns>
        public static CvMat AsCvMat(this IImage image)
        {
            var depthType = typeConversion[image.ColorInfo.ChannelType];

            return CvMat.FromUserData(image.ImageData, image.Width, image.Height, image.Stride, depthType, image.ColorInfo.ChannelCount);
        }

        /// <summary>
        /// Copies data to managed array.
        /// </summary>
        /// <typeparam name="TColor">Pixel color type.</typeparam>
        /// <param name="cvMat">OpenCV matrix.</param>
        /// <returns>Managed image representation.</returns>
        public static TColor[,] ToArray<TColor>(this CvMat cvMat)
            where TColor: struct, IColor
        {
            var im = new TColor[cvMat.Height, cvMat.Width];

            using (var uIm = Image<TColor>.Lock(im))
            {
                Copy.UnsafeCopy2D(cvMat.ImageData, uIm.ImageData, cvMat.Step, uIm.Stride, uIm.Height);
            }

            return im;
        }
    }
}
