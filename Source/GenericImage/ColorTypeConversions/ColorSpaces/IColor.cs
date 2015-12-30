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

namespace DotImaging
{
    /// <summary>
    /// Default interface for color types.
    /// </summary>
    public interface IColor { }

    /// <summary>
    /// Default generic interface for color types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IColor<T> : IColor
        where T : struct
    { }

    /// <summary>
    /// Interface for 2 channel color type. (Used for compile-time restrictions)
    /// </summary>
    public interface IColor2 : IColor { }

    /// <summary>
    /// Generic interface for 2 channel color type. (Used for compile-time restrictions)
    /// </summary>
    public interface IColor2<T> : IColor2, IColor<T>
         where T : struct
    { }

    /// <summary>
    /// Interface for 3 channel color type. (Used for compile-time restrictions)
    /// </summary>
    public interface IColor3 : IColor { }

    /// <summary>
    /// Generic interface for 3 channel color type. (Used for compile-time restrictions)
    /// </summary>
    /// <typeparam name="T">Channel type.</typeparam>
    public interface IColor3<T> : IColor3, IColor<T>
         where T : struct
    { }

    /// <summary>
    /// Interface for 4 channel color type. (Used for compile-time restrictions)
    /// </summary>
    public interface IColor4 : IColor { }

    /// <summary>
    /// Generic interface for 4 channel color type. (Used for compile-time restrictions)
    /// </summary>
    /// <typeparam name="T">Channel type.</typeparam>
    public interface IColor4<T> : IColor4, IColor<T>
         where T : struct
    { }
}
