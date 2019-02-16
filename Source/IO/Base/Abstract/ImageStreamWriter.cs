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

using System.Threading.Tasks;
using System;

namespace DotImaging
{
    /// <summary>
    /// Image stream writer abstract class. 
    /// See generic class also.
    /// </summary>
    public abstract class ImageStreamWriter : ImageStreamWriter<IImage>
    { }

    /// <summary>
    /// Image stream writer abstract class. 
    /// It is the base class for classes providing image stream reading.
    /// </summary>
    /// <typeparam name="TImage">Image type.</typeparam>
    public abstract class ImageStreamWriter<TImage> : ImageStream<TImage>
    {
        /// <summary>
        /// Initializes a new instance of the image stream writer class.
        /// </summary>
        protected ImageStreamWriter()
        {
            this.WriteTimeout = 500;
        }

        /// <summary>
        /// Gets or sets a value, in milliseconds, that determines how long the writer will attempt to write before timing out.
        /// </summary>
        public int WriteTimeout { get; set; }

        /// <summary>
        /// Creates and starts the task responsible for frame writing.
        /// If this function is called <see cref="WriteTimeout"/> must be handled by a user itself.
        /// <remarks>
        /// By using this function writing to some streams can be accelerated.
        /// </remarks>
        /// </summary>
        /// <returns>An image writing task.</returns>
        public Task<bool> WriteAsync(TImage image)
        {
            var writeTask = new Task<bool>(() =>
            {
                bool success = WriteInternal(image);
                return success;
            });

            writeTask.Start();
            return writeTask;
        }

        /// <summary>
        /// Writes an image from the current stream 
        /// and advances the position within the stream by 1 element.
        /// </summary>
        /// <returns>
        /// True if the operation is successfully completed, 
        /// false if the writer failed to write or the <see cref="WriteTimeout"/> has been reached.
        /// </returns>
        public bool Write(TImage image)
        {
            var writeTask = WriteAsync(image);
            writeTask.Wait(this.WriteTimeout);

            return writeTask.IsCompleted && writeTask.Result;
        }

        /// <summary>
        /// When overridden in a derived class returns an image and a status.
        /// Position is advanced.
        /// </summary>
        /// <param name="image">Image to write.</param>
        /// <returns>True if successful, false otherwise.</returns>
        protected abstract bool WriteInternal(TImage image);
    }

    /// <summary>
    /// Provides extensions for an image stream writer.
    /// </summary>
    public static class ImageStreamWriterExtensions
    {
        /// <summary>
        /// Writes a single image into the specified stream.
        /// </summary>
        /// <typeparam name="TColor">Color type.</typeparam>
        /// <param name="writer">image stream writer.</param>
        /// <param name="image">Image to write.</param>
        /// <returns>True if the writing operation is successful, false otherwise.</returns>
        public static bool Write<TColor>(this ImageStreamWriter<Image<TColor>> writer, TColor[,] image)
            where TColor: struct, IColor
        {
            bool result = false;
            using (var uImg = image.Lock())
            {
                result = writer.Write(uImg);
            }

            return result;
        }
    }
}
