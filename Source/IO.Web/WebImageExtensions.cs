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
using System.Net;

namespace DotImaging
{
    /// <summary>
    /// Provides extensions for getting images from the Web.
    /// </summary>
    public static class WebImageExtensions
    {
        /// <summary>
        /// Gets the bytes from the Web using the specified uri.
        /// </summary>
        /// <param name="uri">File Web location.</param>
        /// <param name="onProgress">Function executed when progress changes. Return true to cancel the operation, false to continue.</param>
        /// <returns>Encoded image or undefined output in case if the operation is canceled.</returns>
        public static byte[] GetBytes(this Uri uri, Func<float, bool> onProgress = null)
        {
            byte[] output = null;
            var request = (HttpWebRequest)WebRequest.Create(uri.AbsoluteUri);

            using (WebResponse response = request.GetResponse())
            using (Stream source = response.GetResponseStream())
            using(MemoryStream target = new MemoryStream())
            {
                int bytes, copiedBytes = 0;
                var buffer = new byte[1024];
                while ((bytes = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    target.Write(buffer, 0, bytes);
                    copiedBytes += bytes;

                    if (onProgress != null)
                    {
                        bool shouldCancel = onProgress((float)copiedBytes / response.ContentLength);
                        if (shouldCancel)
                            break;
                    }
                }

                target.Flush();
                output = target.ToArray();
            }

            return output;
        }
    }
}
