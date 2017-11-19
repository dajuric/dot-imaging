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
using System.Reflection;

namespace DotImaging
{
    /// <summary>
    /// Contains functions and properties for platform interoperability.
    /// </summary>
    internal static class Platform
    {
        /// <summary>
        /// Operating system type.
        /// </summary>
        public enum OperatingSystem
        {
            /// <summary>
            /// Windows family.
            /// </summary>
            Windows,
            /// <summary>
            /// Linux family
            /// </summary>
            Linux,
            /// <summary>
            /// MacOS family
            /// </summary>
            MacOS
        }

        static Platform()
        {
            RunningPlatform = getRunningPlatform();
        }

        /// <summary>
        /// Gets operating system name.
        /// </summary>
        private static OperatingSystem getRunningPlatform()
        { 
            //Taken from: <a href="http://stackoverflow.com/questions/10138040/how-to-detect-properly-windows-linux-mac-operating-systems"/> and modified.
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    // Well, there are chances MacOSX is reported as Unix instead of MacOSX.
                    // Instead of platform check, we'll do a feature checks (Mac specific root folders)
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                        return OperatingSystem.MacOS;
                    else
                        return OperatingSystem.Linux;

                case PlatformID.MacOSX:
                    return OperatingSystem.MacOS;

                default:
                    return OperatingSystem.Windows;
            }
        }

        /// <summary>
        /// Gets operating system name.
        /// </summary>
        public static OperatingSystem RunningPlatform
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether the process is the 64-bit process.
        /// </summary>
        public static bool Is64BitProcess
        {
            get { return IntPtr.Size == sizeof(long); }
        }

        /// <summary>
        /// Adds the specified directory to unmanaged library search path for functions that load unmanaged library. See <paramref name="dllDirectory"/> attribute is also included.
        /// Internally it changes process environmental variable.
        /// </summary>
        /// <param name="dllDirectory">Directory where to search unmanaged libraries.</param>
        public static void AddDllSearchPath(string dllDirectory)
        {
            dllDirectory = dllDirectory.NormalizePathDelimiters(Path.DirectorySeparatorChar.ToString());

            var path = "";
            switch (RunningPlatform)
            {
                case OperatingSystem.Windows:
                    path = "PATH";
                    break;
                case OperatingSystem.MacOS:
                    path = "LD_LIBRARY_PATH";
                    break;
                case OperatingSystem.Linux:
                    path = "DYLD_FRAMEWORK_PATH";
                    break;
            }

            Environment.SetEnvironmentVariable(path, Environment.GetEnvironmentVariable(path) + Path.PathSeparator + dllDirectory);
        }

        /// <summary>
        /// Gets a default unmanaged library search directory.
        /// The default directory is platform specific:
        /// <para>Windows: /UnmanagedLibraries/Windows/x86/ or /UnmanagedLibraries/Windows/x64/</para>
        /// <para>  MacOS: /UnmanagedLibraries/MacOS/</para>
        /// <para>  Linux: /UnmanagedLibraries/Linux/</para>
        /// </summary>
        /// <param name="rootDirectory">Root directory which marks the starting point (e.g. executing assembly directory).</param>
        /// <returns>Default unmanaged library search directory.</returns>
        public static string GetDefaultDllSearchPath(string rootDirectory)
        {
            var baseDirectory = Path.Combine(rootDirectory, "References");
            var loadDirectory = Path.Combine(baseDirectory, Platform.RunningPlatform.ToString());

            if (RunningPlatform == OperatingSystem.Windows)
                loadDirectory = Path.Combine(loadDirectory, Is64BitProcess ? "x64" : "x86");

            return loadDirectory;
        }

        /// <summary>
        /// Adds the default directory to unmanaged library search path for functions that load unmanaged library. The root directory is the current directory. 
        /// The default directory is platform specific:
        /// <para>Windows: /UnmanagedLibraries/Windows/x86/ or /UnmanagedLibraries/Windows/x64/</para>
        /// <para>  MacOS: /UnmanagedLibraries/MacOS/</para>
        /// <para>  Linux: /UnmanagedLibraries/Linux/</para>
        /// </summary>
        public static void AddDllSearchPath()
        {
            var dllSearchPathPath = GetDefaultDllSearchPath(AppDomain.CurrentDomain.BaseDirectory);
            AddDllSearchPath(dllSearchPathPath);
        }

        /// <summary>
        /// Gets a platform specific module format (e.g. Windows {0}.dll).
        /// </summary>
        /// <returns>Module format string.</returns>
        public static String GetModuleFormatString()
        {
            String formatString = null;

            switch (RunningPlatform)
            {
                case OperatingSystem.Windows:
                    formatString = "{0}.dll";
                    break;
                case OperatingSystem.MacOS:
                    formatString = "lib{0}.dylib";
                    break;
                case OperatingSystem.Linux:
                    formatString = "lib{0}.so";
                    break;
                default:
                    formatString = "{0}";
                    break;
            }

            return formatString;
        }

        /// <summary>
        /// Replaces path delimiters with platform-specific one defined in <see cref="Path.DirectorySeparatorChar"/>.
        /// </summary>
        /// <param name="path">Path to replace delimiters.</param>
        /// <returns>Path with replaced delimiters.</returns>
        public static string NormalizePathDelimiters(this string path)
        {
            return NormalizePathDelimiters(path, Path.DirectorySeparatorChar.ToString());
        }

        /// <summary>
        /// Replaces path delimiters with specified one.
        /// </summary>
        /// <param name="path">Path to replace delimiters.</param>
        /// <param name="normalizedDelimiter">Replacing delimiter.</param>
        /// <returns>Path with replaced delimiters.</returns>
        public static string NormalizePathDelimiters(this string path, string normalizedDelimiter)
        {
            return path.Replace("//", normalizedDelimiter)
                       .Replace(@"\", normalizedDelimiter)
                       .Replace(@"\\", normalizedDelimiter)
                       .Replace(@"/", normalizedDelimiter);
        }

        /// <summary>
        /// Sets the executing assembly directory as the current directory.
        /// </summary>
        public static void SetAppDirectoryAsWorking()
        {
            var dir = new FileInfo(Assembly.GetExecutingAssembly().Location).FullName;
            Directory.SetCurrentDirectory(dir);
        }
    }
}
