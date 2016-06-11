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
using System.IO;
using System.Linq;

namespace DotImaging
{
    /// <summary>
    /// Provides functions and properties to access video file in a streamable way.
    /// </summary>
    public class FileCapture: VideoCaptureBase
    {
        private static readonly string[] supportedRemoteFiles = new string[] { ".mp4", ".webm" };
        private static readonly string[] supportedLocalFiles = new string[] { ".mp4", ".avi", ".divx", ".webm", ".wmv" };

        string fileName = null;

        /// <summary>
        /// Creates capture from video file.
        /// </summary>
        /// <param name="sourceName">
        /// Local file, http uri, ftp uri or a named pipe.
        /// <para>In case of named pipe use: String.Format(@"\\.\pipe\{0}", pipeName) where pipeName is the name of the pipe.</para>
        /// </param>
        public FileCapture(string sourceName)
        {
            if (sourceName.Contains(@"\\.\pipe\"))
            {
                this.CanSeek = false;
            }
            else if (Uri.IsWellFormedUriString(sourceName, UriKind.Absolute))
            {
                var fileExt = Path.GetExtension(sourceName);
                if (supportedRemoteFiles.Any(x => x.Equals(fileExt.ToLower())) == false)
                    throw new UriFormatException(String.Format("Uri must point to a supported video file ({0}).", String.Join(", ", supportedRemoteFiles)));

                this.CanSeek = true;
            }
            else
            {
                if (!File.Exists(sourceName))
                    throw new FileNotFoundException(String.Format("The file {0} can not be found.", sourceName));

                var fileExt = Path.GetExtension(sourceName);
                if (supportedRemoteFiles.Any(x => x.Equals(fileExt.ToLower())) == false)
                    throw new UriFormatException(String.Format("File must be a supported video file ({0}).", String.Join(", ", supportedLocalFiles)));

                this.CanSeek = true;
            }
       
            this.fileName = sourceName;
            this.Open(); //to enable property change
        }

        /// <summary>
        /// Opens the video file stream.
        /// </summary>
        public override void Open()
        {
            if (capturePtr != IntPtr.Zero)
                return;

            capturePtr = CvInvoke.cvCreateFileCapture(fileName);
            if (capturePtr == IntPtr.Zero)
                throw new Exception("Cannot open FileStream!");
        }

        /// <summary>
        /// Gets the current position in the stream as frame offset.
        /// </summary>
        public override long Position
        {
            get { return (long)CvInvoke.cvGetCaptureProperty(capturePtr, CaptureProperty.PosFrames); }
            protected set { }  
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// <para>Warning: the underlying OpenCV function seeks to nearest key-frame, therefore the seek operation may not be frame-accurate.</para>
        /// </summary>
        /// <param name="offset">A frame index offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, System.IO.SeekOrigin origin = SeekOrigin.Current)
        { 
            var frameIndex = base.Seek(offset, origin);
            CvInvoke.cvSetCaptureProperty(capturePtr, CaptureProperty.PosFrames, frameIndex);

            return Position;
        }
    }
}
