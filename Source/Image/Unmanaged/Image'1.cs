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

using System.Drawing;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotImaging
{
    /// <summary>
    /// Implements generic image type and provides a basic data manipulation. Other functions are built as extensions making the class light-weight and portable.
    /// <para>Other extensions and satellite assemblies enables multiple interoperability with other libraries such as AForge.NET, OpenCV, EmguCV.</para>
    /// </summary>
    /// <typeparam name="TColor">Color type. The structure must be blittable.</typeparam>
    public class Image<TColor> : IImage, IEquatable<IImage>, IDisposable
           where TColor: struct
    {
        #region Constructor methods

        object objectReference = null; //prevents disposing parent object if sharing data (GetSubRect(..), casting...)
        Action<object> parentDestructor = null;

        private Image()
        {
            this.ColorInfo = ColorInfo.GetInfo<TColor>(); //an early init is needed during deserialization
        }

        /// <summary>
        /// Creates an unmanaged image by pinning the provided array.
        /// <para>No data is copied.</para>
        /// </summary>
        /// <param name="array">Array to lock.</param>
        /// <returns>Unmanaged image.</returns>
        public static Image<TColor> Lock(TColor[,] array)
        {
            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            int width = array.GetLength(1);
            int height = array.GetLength(0);

            var image = new Image<TColor>(handle.AddrOfPinnedObject(), width, height, ColorInfo.GetInfo<TColor>().Size * width, 
                                          handle, x => ((GCHandle)x).Free());

            return image;
        }

        /// <summary>
        /// Creates an unmanaged image representation from the provided array and image width (un-flattens the array).
        /// <para>No data is copied.</para>
        /// </summary>
        /// <param name="rawImageArray">Raw-byte image data array to lock.</param>
        /// <param name="width">
        /// Image width.
        /// <para>Data length and the provided width must produce an integer height.</para>
        /// </param>
        /// <returns>Unmanaged image.</returns>
        public static Image<TColor> Lock(byte[] rawImageArray, int width)
        {
            GCHandle handle = GCHandle.Alloc(rawImageArray, GCHandleType.Pinned);

            float height = (float)rawImageArray.Length / (width * ColorInfo.GetInfo<TColor>().Size);
            if (height != (int)height)
                throw new ArgumentException("The calculated height is not integer. There is a mismatch between given width, the data length and the image pixel type.");

            var image = new Image<TColor>(handle.AddrOfPinnedObject(), width, (int)height, ColorInfo.GetInfo<TColor>().Size * width,
                                          handle, x => ((GCHandle)x).Free());

            return image;
        }

        /// <summary>
        /// Constructs an image from unmanaged data. Data is shared.
        /// </summary>
        /// <param name="imageData">Pointer to unmanaged data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="stride">Image stride.</param>
        /// <param name="parentReference">To prevent object from deallocating use this parameter.</param>
        /// <param name="parentDestructor">If a parent needs to be destroyed or release use this function. (e.g. unpin object - GCHandle)</param>
        public Image(IntPtr imageData, int width, int height, int stride, object parentReference = null, Action<object> parentDestructor = null)
            :this()
        {
            initializeProperties(this, imageData, width, height, stride);
            this.objectReference = parentReference;
            this.parentDestructor = parentDestructor;
        }

        private static void initializeProperties(Image<TColor> im, IntPtr imageData, int width, int height, int stride)
        {
            im.ImageData = imageData;
            im.Width = width;
            im.Height = height;
            im.Stride = stride;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets unmanaged image data.
        /// </summary>
        public IntPtr ImageData { get; private set; }
        /// <summary>
        /// Gets image width.
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Gets image height.
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// Gets image stride - number of bytes per image row.
        /// </summary>
        public int Stride { get; private set; }
        /// <summary>
        /// Gets image size.
        /// </summary>
        public Size Size { get { return new Size(this.Width, this.Height); } }
        /// <summary>
        /// Gets image color info.
        /// </summary>
        public ColorInfo ColorInfo { get; protected set; } //set in 

        #endregion

        #region Basic helper methods

        /// <summary>
        /// Gets image data at specified location.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <returns>Data pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetData(int row, int col)
        {
            if (col < 0 || col >= this.Width)
                throw new ArgumentOutOfRangeException("Column index is out of range: " + col);

            return this.GetData(row) + col * this.ColorInfo.Size;
        }

        /// <summary>
        /// Gets image data at specified location.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <returns>Data pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetData(int row)
        {
            if (row < 0 || row >= this.Height)
                throw new ArgumentOutOfRangeException("Row index is out of range: " + row);

            return this.ImageData + row * this.Stride;
        }

        /// <summary>
        /// Gets sub-image from specified area. Data is shared.
        /// </summary>
        /// <param name="rect">Area of an image for sub-image creation.</param>
        /// <returns>Sub-image.</returns>
        public Image<TColor> GetSubRect(Rectangle rect)
        {
            if (rect.Right > this.Width || rect.Bottom > this.Height) //Location will be verified through GetData(...) function
                throw new ArgumentOutOfRangeException();

            object objRef = this.objectReference ?? this; //always show at the root

            IntPtr data = GetData(rect.Y, rect.X);
            return new Image<TColor>(data, rect.Width, rect.Height, this.Stride, objRef);
        }

        /// <summary>
        /// Gets sub-image from specified area. Data is shared.
        /// </summary>
        /// <param name="rect">Area of an image for sub-image creation.</param>
        /// <returns>Sub-image.</returns>
        IImage IImage.GetSubRect(Rectangle rect)
        {
            return this.GetSubRect(rect);
        }

        #endregion

        #region IEquatable, IDisposable

        /// <summary>
        /// Compares this image to another image. Only pointer location and image size are compared.
        /// There is no data compassion.
        /// </summary>
        /// <param name="other">Other image.</param>
        /// <returns>Whether two images are equal or not.</returns>
        public bool Equals(IImage other)
        {
            if (other != null &&
                this.ImageData == other.ImageData &&
                this.Size == other.Size)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compares this image to another object. Internally the function overload is called.
        /// </summary>
        /// <param name="obj">Other.</param>
        /// <returns>Is the image equal to an object or not.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as IImage);
        }

        /// <summary>
        /// Image's hash code. Pointer address is used as hash code.
        /// </summary>
        /// <returns>Image's hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (int)this.ImageData.ToInt64(); //support for 64-bit architecture
            }
        }

        bool isDisposed = false;

        /// <summary>
        /// Disposes generic image. 
        /// In case if data is allocated it is released.
        /// If data is shared parent reference (if exists) and parent handle (if exist) is released.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return; //if this function is called for the first time

            if (this.parentDestructor != null)
                this.parentDestructor(objectReference);

            this.parentDestructor = null;
            this.objectReference = null;

            isDisposed = true;
        }

        /// <summary>
        /// Disposes the image.
        /// </summary>
        ~Image()
        {
            Dispose();
        }

        #endregion
    }

}
