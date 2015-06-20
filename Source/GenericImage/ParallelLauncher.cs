#region Licence and Terms
// DotImaging Framework
// https://github.com/dajuric/dot-imaging
//
// Copyright © Darko Jurić, 2014 
// darko.juric2@gmail.com
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
// 
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see <https://www.gnu.org/licenses/lgpl.txt>.
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
