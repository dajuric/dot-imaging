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

namespace DotImaging
{
    /// <summary>
    /// Provides the base class for the image stream.
    /// </summary>
    /// <typeparam name="TImage">Image type.</typeparam>
    public abstract class ImageStream<TImage>: IDisposable
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ImageStream{TImage}"/>.
        /// </summary>
        protected ImageStream()
        {
            this.CanSeek = false;
            this.IsLiveStream = false;
        }

       /// <summary>
       /// When overridden in a derived class, gets the length in number of frames.
       /// </summary>
        public abstract long Length { get; }

        /// <summary>
        /// When overridden in a derived class, gets the next frame index.
        /// </summary>
        public virtual long Position { get; protected set; }

        /// <summary>
        /// Gets whether the stream is live stream meaning that its length is not constant.
        /// Those streams are usually not seek-able. See also: <see cref="CanSeek"/>.
        /// </summary>
        public virtual bool IsLiveStream { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public virtual bool CanSeek { get; protected set; }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A frame index offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception cref="NotSupportedException">Seek operation is not supported by the current stream.</exception>
        public virtual long Seek(long offset, System.IO.SeekOrigin origin = SeekOrigin.Current)
        {
            if (!this.CanSeek)
                throw new NotSupportedException("Seek operation is not supported by the current stream.");

            long newPosition = 0;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newPosition = offset;
                    break;
                case SeekOrigin.Current:
                    newPosition = this.Position + offset;
                    break;
                case SeekOrigin.End:
                    newPosition = this.Length + offset;
                    break;
            }

            var currentFrame = System.Math.Min(this.Length - 1, System.Math.Max(0, newPosition));
            return currentFrame;
        }

        /// <summary>
        /// Closes the stream and releases all resources.
        /// <para>Use Dispose function rather than Close function.</para>
        /// </summary>
        public virtual void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// When overridden in a derived class, opens the current stream. 
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// When overridden in a derived class, closes the current stream and releases any resources associated with the current stream.
        /// This function is internally called by <see cref="Dispose"/>.
        /// </summary>
        public abstract void Close();
    }
}
