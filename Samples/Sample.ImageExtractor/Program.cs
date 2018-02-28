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

using DotImaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";runtime/win10-x64/"); //only needed if projects are directly referenced

            //emulate input args
            string fileMask = Path.Combine(getResourceDir(), "Welcome.mp4");

            if (args.Length == 1)
                fileMask = args[0];

            var fileNames = enumerateFiles(fileMask);
            foreach (var fileName in fileNames)
            {
                extractVideo(fileName);
            }
        }

        private static void extractVideo(string fileName)
        {
            //get output dir (same as file name and in the same folder as video)
            var fileInfo = new FileInfo(fileName);
            var fileNameNoExt = fileInfo.Name.Replace(fileInfo.Extension, String.Empty);
            string outputDir = Path.Combine(fileInfo.DirectoryName, fileNameNoExt);

            //open video
            var reader = new FileCapture(fileName);
            reader.Open();

            reader.SaveFrames(outputDir, "{0}.jpg", (percentage) =>
            {
                ((double)percentage).Progress(message: "Extracting " + fileNameNoExt);
            });

            UI.CloseAll();
        }

        private static IEnumerable<string> enumerateFiles(string fileMask)
        {
            var pathDelimiter = Path.DirectorySeparatorChar;

            fileMask = normalizePathDelimiters(fileMask, pathDelimiter.ToString());
            string fileMaskWithoutPath = fileMask.Split(pathDelimiter).Last();
            string path = fileMask.Replace(fileMaskWithoutPath, String.Empty);

            var fileNames = Directory.EnumerateFiles(path, fileMaskWithoutPath);
            return fileNames;
        }

        private static string getResourceDir()
        {
            return Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, "Resources");
        }

        private static string normalizePathDelimiters(string path, string normalizedDelimiter) //TODO: replace with extension when available
        {
            return path.Replace("//", normalizedDelimiter)
                       .Replace(@"\", normalizedDelimiter)
                       .Replace(@"\\", normalizedDelimiter)
                       .Replace(@"/", normalizedDelimiter);
        }
    }
}
