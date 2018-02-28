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
using System.Collections.Generic;
using System.IO;

namespace DotImaging
{
    /// <summary>
    /// <para>Defined functions can be used as object extensions.</para>
    /// Provides methods for string which is treated as file and directory path.
    /// </summary>
    static class PathExtensions
    {
        /// <summary>
        /// Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <param name="dirInfo">Directory info.</param>
        /// <param name="searchPatterns">The search strings (e.g. new string[]{ ".jpg", ".bmp" }</param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or all subdirectories. The default
        /// value is <see cref="System.IO.SearchOption.TopDirectoryOnly"/>.
        ///</param>
        /// <returns>An enumerable collection of files that matches <paramref name="searchPatterns"/> and <paramref name="searchOption"/>.</returns>
        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo dirInfo, string[] searchPatterns, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var fileInfos = new List<FileInfo>();
            foreach (var searchPattern in searchPatterns)
            {
                var dirFileInfos = dirInfo.EnumerateFiles(searchPattern, searchOption);
                fileInfos.AddRange(dirFileInfos);
            }

            return fileInfos;
        }

    }
}
