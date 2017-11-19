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
using System.IO;

namespace DotImaging
{

    /// <summary>
    /// Provides extensions for image extraction from image streams.
    /// </summary>
    public static class ImageWriterExtension
    {
        /// <summary>
        /// Reads the image source and save the extracted images to the specified folder.
        /// </summary>
        /// <param name="imageSource">Image stream reader.</param>
        /// <param name="outputDir">Output directory.</param>
        /// <param name="fileNameFormat">Image file name format.</param>
        /// <param name="onFrameCompletition">Progress function executed after a frame is saved.</param>
        public static void SaveFrames(this ImageStreamReader imageSource, string outputDir, 
                                      string fileNameFormat = "img-{0:000}.png", 
                                      Action<float> onFrameCompletition = null)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            if (imageSource.CanSeek)
                imageSource.Seek(0, SeekOrigin.Begin);

            var idx = 0;
            foreach (var frame in imageSource) //use video stream as IEnumerable<IImage> (must support seek operation)
            {
                if (frame != null) //some videos skip key frames (discard those frames)
                {
                    var path = Path.Combine(outputDir, String.Format(fileNameFormat, idx));
                    ImageIO.TrySave(frame, path); //TODO-noncritical: add compression options
                }

                if(onFrameCompletition != null)
                    onFrameCompletition((float)(idx + 1) / imageSource.Length);

                idx++;
            }
        }
    }
}
