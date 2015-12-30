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

using System;
using System.Runtime.CompilerServices;

namespace DotImaging
{
    /// <summary>
    /// Kernel thread structure which represents the working point.
    /// </summary>
    public struct KernelThread
    {
        /// <summary>
        /// Horizontal offset.
        /// </summary>
        public int X;
        /// <summary>
        /// Vertical offset.
        /// </summary>
        public int Y;
    }

    /// <summary>
    /// Provides a launch method and extension methods for parallel array processing.
    /// </summary>
    public static class ParallelLauncher
    {      
        /// <summary>
        /// launches the specified kernel function in parallel.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">Array.</param>
        /// <param name="kernel">Kernel function.</param>
        /// <param name="gridX">Horizontal grid size.</param>
        /// <param name="gridY">Vertical grid size</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Launch<T>(this T[,] array, Action<KernelThread, T[,]> kernel, int gridX, int gridY)
        {
            Launch(thread => 
            {
                kernel(thread, array);
            }, 
            gridX, gridY);
        }

        /// <summary>
        /// Launches the specified kernel function in parallel.
        /// </summary>
        /// <param name="kernel">Kernel function.</param>
        /// <param name="gridX">Horizontal grid size.</param>
        /// <param name="gridY">Vertical grid size</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Launch(Action<KernelThread> kernel, int gridX, int gridY)
        {
            System.Threading.Tasks.Parallel.For(0, gridY, (j) =>
            {
                KernelThread th = new KernelThread();

                th.Y = j;
                for (int i = 0; i < gridX; i++)
                {
                    th.X = i;
                    kernel(th);
                }
            });
        }
    }
}
