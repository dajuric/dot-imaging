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

using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;

namespace DotImaging
{
    /// <summary>
    /// Bitmap file save extensions.
    /// </summary>
    public static class BmpIO
    {
        static Dictionary<string, ImageCodecInfo> codecs = null;

        static BmpIO()
        {
            codecs = new Dictionary<string, ImageCodecInfo>();

            codecs.Add(".jpg", getEncoder(ImageFormat.Jpeg));
            codecs.Add(".jpeg", getEncoder(ImageFormat.Jpeg));
            codecs.Add(".png", getEncoder(ImageFormat.Png));
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


        /// <summary>
        /// Saves the specified image.
        /// <para>
        /// Quality parameter is only supported for JPEG, PNG file types. 
        /// For other file types this value is omitted.
        /// </para>
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="targetStream">Target stream.</param>
        /// <param name="imageFormat">Image format.</param>
        /// <param name="quality">Quality parameter [0..100] where 0 means maximum compression.</param>
        public static void Save(this System.Drawing.Image image, Stream targetStream, ImageFormat imageFormat, int quality = 90)
        {
            var encoder = getEncoder(imageFormat);

            if (encoder != null)
            {
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, quality);

                image.Save(targetStream, encoder, myEncoderParameters);
            }
            else
            {
                image.Save(targetStream, imageFormat);
            }
        }


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
            codecs.TryGetValue(new FileInfo(filename).Extension, out ImageCodecInfo codec);
            if (codec == null)
            {
                image.Save(filename);
                return;
            }

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, quality);

            image.Save(filename, codec, myEncoderParameters);
        }
    }
}
