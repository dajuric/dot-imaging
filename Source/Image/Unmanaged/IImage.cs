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
using DotImaging.Primitives2D;

namespace DotImaging
{
    /// <summary>
    /// Represents interface to the <see cref="Image&lt;TColor&gt;"/> class.
    /// </summary>
    public interface IImage: IDisposable, IEquatable<IImage>
    {
        /// <summary>
        /// Gets unmanaged image data.
        /// </summary>
        IntPtr ImageData { get; }
        /// <summary>
        /// Gets image width.
        /// </summary>
        int Width { get; }
        /// <summary>
        /// Gets image height.
        /// </summary>
        int Height { get; }
        /// <summary>
        /// Gets image stride.
        /// </summary>
        int Stride { get; }
        /// <summary>
        /// Gets image size.
        /// </summary>
        Size Size { get; }
        /// <summary>
        /// Gets image color info.
        /// </summary>
        ColorInfo ColorInfo { get; }
        /// <summary>
        /// Gets image data at specified location.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <returns>Data pointer.</returns>
        IntPtr GetData(int row, int col);
        /// <summary>
        /// Gets image data at specified location.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <returns>Data pointer.</returns>
        IntPtr GetData(int row);
        /// <summary>
        /// Gets sub-image from specified area. Data is shared.
        /// </summary>
        /// <param name="rect">Area of an image for sub-image creation.</param>
        /// <returns>Sub-image.</returns>
        IImage GetSubRect(Rectangle rect);
    }
}
