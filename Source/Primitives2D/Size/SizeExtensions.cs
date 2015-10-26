#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014-2015
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

namespace DotImaging.Primitives2D
{
    /// <summary>
    /// <para>Defined functions can be used as object extensions.</para>
    /// Provides size extension methods.
    /// </summary>
    public static class SizeExtensions
    {
        /// <summary>
        /// Gets the size area.
        /// </summary>
        /// <param name="size">Size.</param>
        /// <returns>Area.</returns>
        public static int Area(this Size size)
        {
            return size.Width * size.Height;
        }
    }

    /// <summary>
    /// <para>Defined functions can be used as object extensions.</para>
    /// Provides size extension methods.
    /// </summary>
    public static class SizeFExtensions
    {
        /// <summary>
        /// Gets the size area.
        /// </summary>
        /// <param name="size">Size.</param>
        /// <returns>Area.</returns>
        public static float Area(this SizeF size)
        {
            return size.Width * size.Height;
        }
    }
}
