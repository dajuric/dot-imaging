#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2015-2015 
// darko.juric2@gmail.com
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
// 
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see <https://www.gnu.org/licenses/lgpl.txt>.
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
