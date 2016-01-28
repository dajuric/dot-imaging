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

using DotImaging.Primitives2D;
using System;
using System.Threading;

namespace DotImaging
{
    /// <summary>
    ///  Image display form supporting mask creation.
    /// </summary>
    internal class DrawingRectangleForm : ImageForm
    {
        DrawingRectangleAdorner adorner = null;
        bool isKeyPressed = false; //KeyDown fires many times, possible bug in Eto.Forms ?

        /// <summary>
        /// Creates new image display form supporting rectangle creation.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="setPassingResetEvent">True to set the reset event to passing state, false otherwise.</param>
        public DrawingRectangleForm(string title = "", bool setPassingResetEvent = true)
            :base(title)
        {      
            adorner = new DrawingRectangleAdorner(PictureBox);
            adorner.OnDrawn = () => 
            {
                if (OnDrawn != null && !Rectangle.IsEmpty) OnDrawn(Rectangle);
                adorner.Clear();
            };

            ResetEvent = new ManualResetEvent(setPassingResetEvent);

            KeyDown += (s, e) =>
            {
                if (isKeyPressed) return;
                isKeyPressed = true;

                ResetEvent.Reset();
            };
            KeyUp += (s, e) =>
            {
                isKeyPressed = false;
                ResetEvent.Set();
            };
        }

        public RectangleF Rectangle
        {
            get
            {
                var r = adorner.Rectangle;
                return new RectangleF(r.X, r.Y, r.Width, r.Height);
            }
        }

        public Action<RectangleF> OnDrawn { get; set; }

        public ManualResetEvent ResetEvent { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                adorner.Dispose();

            base.Dispose(disposing);
        }
    }
}
