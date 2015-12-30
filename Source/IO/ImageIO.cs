#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2016
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
    /// Provides methods for image saving and loading
    /// </summary>
    public static class ImageIO 
    {
        #region Load (file)

        private unsafe static IImage load(string fileName, ImageLoadType imageLoadType)
        {
            var iplImagePtr = CvInvoke.cvLoadImage(fileName, imageLoadType);
            var image = (*iplImagePtr).AsImage((_) =>
            {
                if (iplImagePtr == null) return;
                CvInvoke.cvReleaseImage(ref iplImagePtr);
            });

            return image;
        }

        /// <summary>
        /// Loads an image with the specified path and name as it is.
        /// </summary>
        /// <param name="fileName">Image file name.</param>
        /// <returns>Image.</returns>
        public unsafe static IImage LoadUnchanged(string fileName)
        {
            return load(fileName, ImageLoadType.Unchanged);
        }

        /// <summary>
        /// Loads an image with the specified path and name and performs and RGB conversion.
        /// </summary>
        /// <param name="fileName">Image filename.</param>
        /// <returns>Image.</returns>
        public unsafe static Image<Bgr<byte>> LoadColor(string fileName)
        {
            return load(fileName, ImageLoadType.Color) as Image<Bgr<byte>>;
        }

        /// <summary>
        /// Loads an image with the specified path and name and performs and gray conversion.
        /// </summary>
        /// <param name="fileName">Image filename.</param>
        /// <returns>Image.</returns>
        public unsafe static Image<Gray<byte>> LoadGray(string fileName)
        {
            return load(fileName, ImageLoadType.Grayscale) as Image<Gray<byte>>;
        }

        #endregion

        #region Save (file)

        /// <summary>
        /// Saves the provided image. If the image has non-supported color or depth false value is returned.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Filename.</param>
        /// <returns>True if the image is saved, false otherwise.</returns>
        public unsafe static bool TrySave(IImage image, string fileName)
        {
            IplImage iplImage = default(IplImage);
            try
            {
                iplImage = image.AsCvIplImage();
            }
            catch 
            {
                return false;
            }

            CvInvoke.cvSaveImage(fileName, &iplImage, IntPtr.Zero);
            return true;
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <typeparam name="TColor">Image color.</typeparam>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        private unsafe static void Save<TColor>(this Image<TColor> image, string fileName)
            where TColor : struct, IColor
        {
            var iplImage = image.AsCvIplImage();
            CvInvoke.cvSaveImage(fileName, &iplImage, IntPtr.Zero);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <typeparam name="TColor">Image color.</typeparam>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        private unsafe static void Save<TColor>(this TColor[,] image, string fileName)
            where TColor: struct, IColor
        {
            using (var img = image.Lock())
            {
                var iplImage = img.AsCvIplImage();
                CvInvoke.cvSaveImage(fileName, &iplImage, IntPtr.Zero);
            }
        }

        #region Save-gray

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Gray<byte>[,] image, string fileName)
        {
            image.Save<Gray<byte>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Gray<sbyte>[,] image, string fileName)
        {
            image.Save<Gray<sbyte>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Gray<short>[,] image, string fileName)
        {
            image.Save<Gray<short>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Gray<ushort>[,] image, string fileName)
        {
            image.Save<Gray<ushort>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Gray<int>[,] image, string fileName)
        {
            image.Save<Gray<int>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Gray<float>[,] image, string fileName)
        {
            image.Save<Gray<float>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Gray<double>[,] image, string fileName)
        {
            image.Save<Gray<double>>(fileName);
        }

        #endregion

        #region Save-bgr

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgr<byte>[,] image, string fileName)
        {
            image.Save<Bgr<byte>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgr<sbyte>[,] image, string fileName)
        {
            image.Save<Bgr<sbyte>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgr<short>[,] image, string fileName)
        {
            image.Save<Bgr<short>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgr<ushort>[,] image, string fileName)
        {
            image.Save<Bgr<ushort>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgr<int>[,] image, string fileName)
        {
            image.Save<Bgr<int>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgr<float>[,] image, string fileName)
        {
            image.Save<Bgr<float>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgr<double>[,] image, string fileName)
        {
            image.Save<Bgr<double>>(fileName);
        }

        #endregion

        #region Save-bgra

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgra<byte>[,] image, string fileName)
        {
            image.Save<Bgra<byte>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgra<sbyte>[,] image, string fileName)
        {
            image.Save<Bgra<sbyte>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgra<short>[,] image, string fileName)
        {
            image.Save<Bgra<short>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgra<ushort>[,] image, string fileName)
        {
            image.Save<Bgra<ushort>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgra<int>[,] image, string fileName)
        {
            image.Save<Bgra<int>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgra<float>[,] image, string fileName)
        {
            image.Save<Bgra<float>>(fileName);
        }

        /// <summary>
        /// Saves the specified image.
        /// </summary>
        /// <param name="image">Image to save.</param>
        /// <param name="fileName">Image filename.</param>
        public static void Save(this Bgra<double>[,] image, string fileName)
        {
            image.Save<Bgra<double>>(fileName);
        }

        #endregion

        #endregion

        #region Encode

        /// <summary>
        /// Encodes the specified image into the Jpeg byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="jpegQuality">Jpeg quality [0..100] where 100 is the highest quality.</param>
        /// <returns>Jpeg byte array.</returns>
        public static byte[] EncodeAsJpeg(this Gray<byte>[,] image, int jpegQuality = 95)
        {
            return encodeAsJpeg(image, jpegQuality);
        }

        /// <summary>
        /// Encodes the specified image into the Jpeg byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="jpegQuality">Jpeg quality [0..100] where 100 is the highest quality.</param>
        /// <returns>Jpeg byte array.</returns>
        public static byte[] EncodeAsJpeg(this Bgr<byte>[,] image, int jpegQuality = 95)
        {
            return encodeAsJpeg(image, jpegQuality);
        }

        /// <summary>
        /// Encodes the specified image into the Jpeg byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="jpegQuality">Jpeg quality [0..100] where 100 is the highest quality.</param>
        /// <returns>Jpeg byte array.</returns>
        public static byte[] EncodeAsJpeg(this Gray<ushort>[,] image, int jpegQuality = 95)
        {
            return encodeAsJpeg(image, jpegQuality);
        }

        /// <summary>
        /// Encodes the specified image into the Jpeg byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="jpegQuality">Jpeg quality [0..100] where 100 is the highest quality.</param>
        /// <returns>Jpeg byte array.</returns>
        public static byte[] EncodeAsJpeg(this Bgr<ushort>[,] image, int jpegQuality = 95)
        {
            return encodeAsJpeg(image, jpegQuality);
        }

        /// <summary>
        /// Encodes the specified image into the PNG byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="pngCompression">PNG compression level [0..9] where 9 is the highest compression.</param>
        /// <returns>PNG byte array.</returns>
        public static byte[] EncodeAsPng(this Gray<byte>[,] image, int pngCompression = 3)
        {
            return encodeAsPng(image, pngCompression);
        }

        /// <summary>
        /// Encodes the specified image into the PNG byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="pngCompression">PNG compression level [0..9] where 9 is the highest compression.</param>
        /// <returns>PNG byte array.</returns>
        public static byte[] EncodeAsPng(this Bgr<byte>[,] image, int pngCompression = 3)
        {
            return encodeAsPng(image, pngCompression);
        }

        /// <summary>
        /// Encodes the specified image into the PNG byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="pngCompression">PNG compression level [0..9] where 9 is the highest compression.</param>
        /// <returns>PNG byte array.</returns>
        public static byte[] EncodeAsPng(this Bgra<byte>[,] image, int pngCompression = 3)
        {
            return encodeAsPng(image, pngCompression);
        }

        /// <summary>
        /// Encodes the specified image into the PNG byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="pngCompression">PNG compression level [0..9] where 9 is the highest compression.</param>
        /// <returns>PNG byte array.</returns>
        public static byte[] EncodeAsPng(this Gray<ushort>[,] image, int pngCompression = 3)
        {
            return encodeAsPng(image, pngCompression);
        }

        /// <summary>
        /// Encodes the specified image into the PNG byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="pngCompression">PNG compression level [0..9] where 9 is the highest compression.</param>
        /// <returns>PNG byte array.</returns>
        public static byte[] EncodeAsPng(this Bgr<ushort>[,] image, int pngCompression = 3)
        {
            return encodeAsPng(image, pngCompression);
        }

        /// <summary>
        /// Encodes the specified image into the PNG byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="pngCompression">PNG compression level [0..9] where 9 is the highest compression.</param>
        /// <returns>PNG byte array.</returns>
        public static byte[] EncodeAsPng(this Bgra<ushort>[,] image, int pngCompression = 3)
        {
            return encodeAsPng(image, pngCompression);
        }

        /// <summary>
        /// Encodes the specified image into the specified image type byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="extension">Image type extension (.bmp, .png, .jpg)</param>
        /// <returns>Image type byte array.</returns>
        public static byte[] Encode(this Gray<byte>[,] image, string extension)
        {
            return encode(image, extension, null);
        }

        /// <summary>
        /// Encodes the specified image into the specified image type byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="extension">Image type extension (.bmp, .png, .jpg)</param>
        /// <returns>Image type byte array.</returns>
        public static byte[] Encode(this Bgr<byte>[,] image, string extension)
        {
            return encode(image, extension, null);
        }

        /// <summary>
        /// Encodes the specified image into the specified image type byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="extension">Image type extension (.bmp, .png, .jpg)</param>
        /// <returns>Image type byte array.</returns>
        public static byte[] Encode(this Bgra<byte>[,] image, string extension)
        {
            return encode(image, extension, null);
        }

        /// <summary>
        /// Encodes the specified image into the specified image type byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="extension">Image type extension (.bmp, .png, .jpg)</param>
        /// <returns>Image type byte array.</returns>
        public static byte[] Encode(this Gray<ushort>[,] image, string extension)
        {
            return encode(image, extension, null);
        }

        /// <summary>
        /// Encodes the specified image into the specified image type byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="extension">Image type extension (.bmp, .png, .jpg)</param>
        /// <returns>Image type byte array.</returns>
        public static byte[] Encode(this Bgr<ushort>[,] image, string extension)
        {
            return encode(image, extension, null);
        }

        /// <summary>
        /// Encodes the specified image into the specified image type byte array.
        /// </summary>
        /// <param name="image">Image to encode.</param>
        /// <param name="extension">Image type extension (.bmp, .png, .jpg)</param>
        /// <returns>Image type byte array.</returns>
        public static byte[] Encode(this Bgra<ushort>[,] image, string extension)
        {
            return encode(image, extension, null);
        }

        static byte[] encodeAsJpeg<TColor>(TColor[,] image, int jpegQuality = 95)
            where TColor : struct, IColor
        {
            if (jpegQuality < 0 || jpegQuality > 100)
                throw new ArgumentOutOfRangeException("Jpeg quality must be in range: 0-100.");

            int[] parameters = new int[] { CvInvoke.CV_IMWRITE_JPEG_QUALITY, jpegQuality, 0 };
            return encode(image, ".jpg", parameters);
        }

        static byte[] encodeAsPng<TColor>(TColor[,] image, int pngCompression = 3)
            where TColor : struct, IColor
        {
            if (pngCompression < 0 || pngCompression > 9)
                throw new ArgumentOutOfRangeException("Png compression must be in range: 0-9.");

            int[] parameters = new int[] { CvInvoke.CV_IMWRITE_PNG_COMPRESSION, pngCompression, 0 };
            return encode(image, ".png", parameters);
        }

        static unsafe byte[] encode<TColor>(TColor[,] image, string extension, int[] parameters)
           where TColor : struct, IColor
        {
            CvMat* matEncoded; //a single-row image

            using (var uImg = image.Lock())
            {
                fixed (int* paramsPtr = parameters)
                {
                    var mat = uImg.AsCvMat();
                    matEncoded = CvInvoke.cvEncodeImage(extension, &mat, paramsPtr);
                }
            }

            byte[] imEncoded = new byte[matEncoded->Step * matEncoded->Height];
            fixed (byte* arrPtr = &imEncoded[0])
            {
                Copy.UnsafeCopy(matEncoded->ImageData, (IntPtr)arrPtr, imEncoded.Length);
            }

            CvInvoke.cvReleaseMat(ref matEncoded); //TODOD: check if this deferences the image
            return imEncoded;
        }

        #endregion

        #region Decode

        /// <summary>
        /// Decodes (and converts if necessary) an image as color image using the specified byte array.
        /// </summary>
        /// <param name="encodedImage">Encoded image.</param>
        /// <returns>Decoded image.</returns>
        public unsafe static Bgr<byte>[,] DecodeAsColorImage(this byte[] encodedImage)
        {
            return decodeImage<Bgr<byte>>(encodedImage, ImageLoadType.Color);
        }

        /// <summary>
        /// Decodes (and converts if necessary) an image as gray image using the specified byte array.
        /// </summary>
        /// <param name="encodedImage">Encoded image.</param>
        /// <returns>Decoded image.</returns>
        public unsafe static Gray<byte>[,] DecodeAsGrayImage(this byte[] encodedImage)
        {
            return decodeImage<Gray<byte>>(encodedImage, ImageLoadType.Grayscale);
        }

        unsafe static TColor[,] decodeImage<TColor>(byte[] encodedImage, ImageLoadType loadType)
            where TColor : struct, IColor
        {
            CvMat* matDecoded;
            fixed (byte* encodedImPtr = encodedImage)
            {
                CvMat mat = CvMat.FromUserData((IntPtr)encodedImPtr, encodedImage.Length, 1, encodedImage.Length, CvMat.CvChannelDepth.CV_8U, 1);
                matDecoded = CvInvoke.cvDecodeImageM(&mat, ImageLoadType.Color);
            }

            var imDecoded = (*matDecoded).ToArray<TColor>();
            CvInvoke.cvReleaseMat(ref matDecoded);

            return imDecoded;
        }

        #endregion
    }
}
