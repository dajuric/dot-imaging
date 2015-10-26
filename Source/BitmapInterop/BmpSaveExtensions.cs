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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DotImaging
{
    /// <summary>
    /// Bitmap file save extensions.
    /// </summary>
    public static class BmpSaveExtensions
    {
        /// <summary>
        /// Saves the specified image.
        /// <para>
        /// Quality parameter is only supported for JPEG, PNG file types. 
        /// For other file types this value is omitted.
        /// </para>
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="filename">File name.</param>
        /// <param name="quality">Quality parameter [0..100] where 0 means maximum compression.</param>
        public static void Save(this System.Drawing.Image image, string filename, int quality = 90)
        {
            var encoder = getEncoder(new FileInfo(filename).Extension);

            if (encoder != null)
            {
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, quality);

                image.Save(filename, encoder, myEncoderParameters);
            }
            else
            {
                image.Save(filename);
            }   
        }

        private static ImageCodecInfo getEncoder(string extension)
        {
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return getEncoderCashed(ImageFormat.Jpeg);
                case ".png":
                    return getEncoderCashed(ImageFormat.Png);
                default:
                    return null;
            }
        }

        private static ImageCodecInfo getEncoderCashed(ImageFormat imageFormat)
        {
            return MethodCache.Global.Invoke<ImageFormat, ImageCodecInfo>((format) => getEncoder(format), imageFormat);
        }

        private static ImageCodecInfo getEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
