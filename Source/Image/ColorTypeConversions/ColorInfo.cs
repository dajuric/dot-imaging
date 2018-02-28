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
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DotImaging
{
    /// <summary>
    /// Gets color information from color type and depth type.
    /// </summary>
    public class ColorInfo: IEquatable<ColorInfo>
    {
        /// <summary>
        /// Color type (IColor).
        /// </summary>
        public Type ColorType { get; private set; }
        /// <summary>
        /// Number of channels that color has.
        /// </summary>
        public int ChannelCount { get; private set; }
        /// <summary>
        /// Number of bytes per channel.
        /// </summary>
        public int ChannelSize { get; private set; }
        /// <summary>
        /// Channel type. Only primitive types are supported.
        /// </summary>
        public Type ChannelType { get; private set; }
        /// <summary>
        /// Color size in bytes. Number of channels multiplied by channel size.
        /// </summary>
        public int Size { get { return this.ChannelSize * this.ChannelCount; } }

        /// <summary>
        /// Gets color info (depth is taken from color).
        /// </summary>
        /// <typeparam name="TColor">Member of <see cref="IColor"/></typeparam>
        /// <returns>Color info</returns>
        public static ColorInfo GetInfo<TColor>()
            //where TColor : IColor<T>
        {
            return GetInfo(typeof(TColor));
        }

        /// <summary>
        /// Gets color info (depth is taken from color).
        /// </summary>
        /// <param name="colorType">Color type. (member of IColor)</param>
        /// <returns>Color info</returns>
        public static ColorInfo GetInfo(Type colorType)
        {
            return MethodCache.Global.Invoke(getInfo, colorType);
        }

        private static ColorInfo getInfo(Type colorType)
        {
            ColorInfo ci = new ColorInfo();
            ci.ColorType = colorType;

            Type channelType;  int numberOfChannels;
            getChannelInfo(colorType, out channelType, out numberOfChannels);

            ci.ChannelCount = numberOfChannels;
            ci.ChannelType = channelType;
            ci.ChannelSize = Marshal.SizeOf(channelType);

            return ci;
        }

        private static void getChannelInfo(Type colorType, out Type channelType, out int numberOfChannels)
        {
            numberOfChannels = 0;

            var channelTypes = colorType
                               .GetFields(BindingFlags.Public | ~BindingFlags.Static) //if BindingFlags.Instance and if colorType is byte => zero length array
                               .Select(x => x.FieldType)
                               .ToArray();

            //ensure that all types are the same
            var _channelType = channelTypes[0];
            if (channelTypes.Where(x => x.Equals(_channelType)).Count() != channelTypes.Length)
                throw new Exception("Public fields must have the same type!");

            if (channelTypes.Length == 0)
                throw new Exception("Color structure must have at least one public field!");

            if (!_channelType.IsValueType)
                throw new Exception("Channel type must be a value type!");

            if (!_channelType.IsPrimitive)
                throw new Exception("Channel type must be a primitive type!");

            channelType = _channelType;
            numberOfChannels = channelTypes.Length;
        }

        /// <summary>
        /// Determines whether the object is equal compared to the specified object. 
        /// A default comparison is used. Please see overloads.
        /// </summary>
        /// <param name="other">Other object.</param>
        /// <returns>True if two objects are equal, false otherwise.</returns>
        public bool Equals(ColorInfo other)
        {
            return Equals(other, ComparableParts.Default);
        }

        /// <summary>
        /// Indicates what parts of color info should be compared.
        /// </summary>
        [Flags]
        public enum ComparableParts
        {
            /// <summary>
            /// Checks color depth type
            /// </summary>
            Depth = 0x1,
            /// <summary>
            /// Checks if one color can be casted to other (if colors are binary compatible).
            /// </summary>
            BinaryCompatible = 0x3, 
            /// <summary>
            /// Checks color type and depth type (if it is true all other properties are equal as well)
            /// </summary>
            Default =  0x4
        }

        /// <summary>
        /// Compares two color infos.
        /// </summary>
        /// <param name="other">Other color info.</param>
        /// <param name="cParts">Indicates what to compare. Default is: ComparableParts.Default. </param>
        /// <returns></returns>
        public bool Equals(ColorInfo other, ComparableParts cParts)
        {
            if(cParts == ComparableParts.Default)
            {
                return this.ColorType == other.ColorType && 
                       this.ChannelType == other.ChannelType;
            }

            if (cParts == ComparableParts.BinaryCompatible)
            {
                var castable = (this.ChannelCount == other.ChannelCount) && (this.ChannelType == other.ChannelType);
                return castable;
            }

            if (cParts == ComparableParts.Depth)
            { 
                var depth = this.ChannelType == other.ChannelType;
                return depth;
            }

            throw new Exception("Unknown comparison!");
        }

        /// <summary>
        /// Get string representation.
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return String.Format("<{0}, {1}>", this.ColorType.Name, this.ChannelType.Name);
        }
    }

    /// <summary>
    /// Provides extensions for color to array conversion.
    /// </summary>
    public static class ColorToArrayExtensions
    {
        /// <summary>
        /// Converts color to array of type <typeparamref name="TDepth"/>.
        /// </summary>
        /// <typeparam name="TColor">Color type.</typeparam>
        /// <typeparam name="TDepth">Channel type.</typeparam>
        /// <param name="color">Color</param>
        /// <returns>Array whose length is the same as color's number of channels.</returns>
        public static TDepth[] ColorToArray<TColor, TDepth>(this TColor color)
            where TColor : IColor
            where TDepth : struct
        {
            var fields = typeof(TColor).GetFields(BindingFlags.Public | ~BindingFlags.Static);

            TDepth[] arr = new TDepth[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                var rawVal = fields[i].GetValue(color);
                arr[i] = (TDepth)Convert.ChangeType(rawVal, typeof(TDepth));
            }

            return arr;
        }
    }
}
