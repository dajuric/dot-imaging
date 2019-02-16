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
using System.IO;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Net;
using VideoLibrary;

namespace DotImaging
{
    /// <summary>
    /// Provides named pipe creation extension for Uri, string and Stream.
    /// </summary>
    public static class NamedPipeExtensions
    {
        #region Reading

        /// <summary>
        /// Creates a new named pipe from a file stream.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="namedPipeName">Named pipe.</param>
        /// <param name="onProgress">Function executed when progress changes. Return true to cancel the operation, false to continue.</param>
        /// <returns>Pipe name.</returns>
        public static string NamedPipeFromFileName(this string fileName, string namedPipeName = "filePipe", Func<float, bool> onProgress = null)
        {
            if (File.Exists(fileName))
                throw new ArgumentException("The provided file does not exist.", nameof(fileName));

            Stream source = File.OpenRead(fileName);

            return NamedPipeFromStreamAsync(source, namedPipeName, onProgress, () =>
            {
                source.Dispose();
            });
        }

        /// <summary>
        /// Creates a new named pipe from a link (video-link if used in conjunction with IO package).
        /// </summary>
        /// <param name="uri">Uri to web file.</param>
        /// <param name="namedPipeName">Named pipe.</param>
        /// <param name="onProgress">Function executed when progress changes. Return true to cancel the operation, false to continue.</param>
        /// <returns>Pipe name.</returns>
        public static string NamedPipeFromVideoUri(this Uri uri, string namedPipeName = "webPipe", Func<float, bool> onProgress = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri.AbsoluteUri);
            WebResponse response = request.GetResponse();
            Stream source = response.GetResponseStream();

            return NamedPipeFromStreamAsync(source, namedPipeName, onProgress, () =>
            {
                source.Dispose();
                response.Dispose();
            });
        }

        /// <summary>
        /// Creates a new named pipe from a Youtube video link.
        /// </summary>
        /// <param name="youtubeUri">Uri to Youtube video file.</param>
        /// <param name="fileExtension">Video-file extension.</param>
        /// <param name="namedPipeName">Named pipe.</param>
        /// <param name="onProgress">Function executed when progress changes. Return true to cancel the operation, false to continue.</param>
        /// <returns>Pipe name.</returns>
        public static string NamedPipeFromYoutubeUri(this Uri youtubeUri, out string fileExtension, string namedPipeName = "youtubeVideoPipe", Func<float, bool> onProgress = null)
        {
            if (youtubeUri.Host != "www.youtube.com")
                throw new ArgumentException("The provided URI is not valid Youtube URI.");

            var youtubeVideo = YouTube.Default.GetVideo(youtubeUri.AbsoluteUri); 
            fileExtension = youtubeVideo.FileExtension;

            VideoClient vc = new VideoClient();
            Stream source = vc.Stream(youtubeVideo);
            
            return NamedPipeFromStreamAsync(source, namedPipeName, onProgress, () =>
            {
                source.Dispose();
                vc.Dispose();
            });
        }

        /// <summary>
        /// Creates a new named pipe from a Youtube video link.
        /// </summary>
        /// <param name="youtubeUri">Uri to Youtube video file.</param>
        /// <param name="namedPipeName">Named pipe.</param>
        /// <param name="onProgress">Function executed when progress changes. Return true to cancel the operation, false to continue.</param>
        /// <returns>Pipe name.</returns>
        public static string NamedPipeFromYoutubeUri(this Uri youtubeUri, string namedPipeName = "youtubeVideoPipe", Func<float, bool> onProgress = null)
        {
            string fileExtension;
            return NamedPipeFromYoutubeUri(youtubeUri, out fileExtension, namedPipeName, onProgress);
        }

        /// <summary>
        /// Creates a new named pipe from a Youtube video link.
        /// </summary>
        /// <param name="source">Source stream.</param>
        /// <param name="namedPipeName">Named pipe.</param>
        /// <param name="onProgress">Function executed when progress changes. Return true to cancel the operation, false to continue.</param>
        /// <param name="onFinish">Action executed when a reading operation finishes.</param>
        /// <returns>Pipe name.</returns>
        public static string NamedPipeFromStreamAsync(this Stream source, string namedPipeName, Func<float, bool> onProgress = null, Action onFinish = null)
        {
            if (source == null)
                new ArgumentNullException(nameof(source));

            Task.Factory.StartNew(() =>
            {
                using (NamedPipeServerStream target = new NamedPipeServerStream(namedPipeName))
                {
                    target.WaitForConnection();
                    target.WaitForPipeDrain();

                    int bytes, copiedBytes = 0;
                    var buffer = new byte[1024];
                    while ((bytes = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        target.Write(buffer, 0, bytes);
                        copiedBytes += bytes;

                        if (onProgress != null)
                        {
                            bool shouldCancel = onProgress((float)copiedBytes / source.Length);
                            if (shouldCancel)
                                break;
                        }
                    }

                    target.Flush();
                    if (onFinish != null) onFinish();
                }
            });

            return namedPipeName;
        }

        #endregion

        #region Writing

        /// <summary>
        /// Copies the named pipe stream to the specified stream.
        /// </summary>
        /// <param name="pipeName">Pipe name.</param>
        /// <param name="target">Destination stream.</param>
        public static void CopyNamedPipeStream(this string pipeName, Stream target)
        {
            using (NamedPipeClientStream clientPipe = new NamedPipeClientStream(pipeName))
            {
                clientPipe.Connect();

                int bytes, copiedBytes = 0;
                var buffer = new byte[1024];
                while ((bytes = clientPipe.Read(buffer, 0, buffer.Length)) > 0)
                {
                    target.Write(buffer, 0, bytes);
                    copiedBytes += bytes;

                    /*if (onProgress != null)
                    {
                        bool shouldCancel = onProgress((float)copiedBytes / clientPipe.Length);
                        if (shouldCancel)
                            break;
                    }*/
                }

                target.Flush();
            }
        }

        /// <summary>
        /// Saves the named pipe stream to the specified file.
        /// </summary>
        /// <param name="pipeName">Pipe name.</param>
        /// <param name="targetFile">Destination file.</param>
        public static void SaveNamedPipeStream(this string pipeName, string targetFile)
        {
            using (Stream target = File.Create(targetFile))
            {
                CopyNamedPipeStream(pipeName, target);
            }
        }

        #endregion
    }
}
