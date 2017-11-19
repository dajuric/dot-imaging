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

using Eto.Forms;
using DotImaging.Primitives2D;
using System;
using System.Threading;

namespace DotImaging
{
    /// <summary>
    /// Provides simple standalone UI elements throughout static methods or extensions.
    /// </summary>
    public static class UI
    {
        static UI()
        {
            FormCollection.Initialize();
        }

        /// <summary>
        /// Closes all UI controls if displayed.
        /// </summary>
        public static void CloseAll()
        {
            FormCollection.CloseAll();
        }

        #region Simple widgets

        /// <summary>
        /// Shows the progress bar in the specified window.
        /// <para>Values are in [0..1] range. Progress bar will automatically close if vale is &gt; 1.</para>
        /// <para>For indeterminate bar pass values &lt; 0.</para>
        /// </summary>
        /// <param name="value">Value in [0..1] range.</param>
        /// <param name="windowTitle">Window title (ID).</param>
        /// <param name="message">Optional message to display along with the progress bar.</param>
        public static void Progress(this double value, string windowTitle = "Progress", string message = "")
        {
            FormCollection.CreateOrUpdate(() => new ProgressForm(windowTitle),
                           form =>
                           {
                               form.Set(value, message);
                           },
                           windowTitle);
        }

        /// <summary>
        /// Displays an color picker dialog.
        /// <para>REQUIREMENT: calling thread must be STAThread.</para>
        /// </summary>
        /// <param name="defaultColor">Default color.</param>
        /// <returns>Picked or default color in case the selection is cancelled.</returns>
        public static Bgr<byte> PickColor(Bgr<byte> defaultColor = default(Bgr<byte>))
        {            
            Bgr<byte> color = defaultColor;
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = new Eto.Drawing.Color { Bb = defaultColor.B, Gb = defaultColor.G, Rb = defaultColor.R };
                var result = dialog.ShowDialog(null);
                if (result == DialogResult.Ok)
                    color = new Bgr<byte> { B = (byte)dialog.Color.Bb, G = (byte)dialog.Color.Gb, R = (byte)dialog.Color.Rb };
            }

            return color;
        }

        /// <summary>
        /// Displays a menu dialog specified by user defined menu items. 
        /// <para>If no items are specified, or in case of a error, the dialog will not be shown.</para>
        /// </summary>
        /// <param name="windowTitle">Window title (ID).</param>
        /// <param name="itemNames">Names of the menu items.</param>
        /// <param name="actions">Item actions.</param>
        /// <returns>Selected menu index or -1 in case the form is closed.</returns>
        public static int ShowMenu(string windowTitle = "Menu", string[] itemNames = null, Action[] actions = null)
        {
            var idx = FormCollection.CreateAndShowDialog(() =>
            {
                var f = new MenuForm(windowTitle, itemNames, actions);
                f.Title = windowTitle;
                return f;
            },
                        f => f.SelectedIndex);

            return idx;
        }

        #endregion

        #region File/folder dialogs

        /// <summary>
        /// Display an open file dialog and returns the selected file name, null otherwise.
        /// </summary>
        /// <param name="windowTitle">Window title.</param>
        /// <param name="extensions">File extension mask.</param>
        /// <returns>Selected file name, null otherwise</returns>
        public static string OpenFile(string windowTitle = "Open file", params string[] extensions)
        {
            string fileName = null;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = windowTitle;
                dialog.CheckFileExists = true;
                dialog.Filters.Add(new FileDialogFilter("(Default)", extensions));

                var result = dialog.ShowDialog(null);
                if (result == DialogResult.Ok)
                    fileName = dialog.FileName;
            }

            return fileName;
        }

        /// <summary>
        /// Display an save file dialog and returns the selected (specified) file name, null otherwise.
        /// </summary>
        /// <param name="windowTitle">Window title.</param>
        /// <param name="extensions">File extension mask.</param>
        /// <returns>Selected (specified) file name, null otherwise</returns>
        public static string SaveFile(string windowTitle = "Save file", params string[] extensions)
        {
            string fileName = null;

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = windowTitle;
                dialog.Filters.Add(new FileDialogFilter("(Default)", extensions));
    
                var result = dialog.ShowDialog(null);
                if (result == DialogResult.Ok)
                    fileName = dialog.FileName;
            }

            return fileName;
        }

        /// <summary>
        /// Display an open file dialog and returns the selected image file name, null otherwise.
        /// </summary>
        /// <param name="windowTitle">Window title.</param>
        /// <returns>Selected file name, null otherwise</returns>
        public static string OpenImage(string windowTitle = "Open image")
        {
            return OpenFile(windowTitle, ".png", ".jpg", ".jpeg", ".bmp");
        }

        /// <summary>
        /// Display an save file dialog and returns the selected (specified) image file name, null otherwise.
        /// </summary>
        /// <param name="windowTitle">Window title.</param>
        /// <returns>Selected (specified) file name, null otherwise</returns>
        public static string SaveImage(string windowTitle = "Save image")
        {
            return SaveFile(windowTitle, ".png", ".jpg", ".jpeg", ".bmp");
        }

        /// <summary>
        /// Display an folder selection dialog and returns the selected folder name, null otherwise.
        /// </summary>
        /// <param name="windowTitle">Window title.</param>
        /// <returns>Selected folder name, null otherwise</returns>
        public static string SelectFolder(string windowTitle = "Select folder")
        {
            string folderPath = null;

            using (SelectFolderDialog dialog = new SelectFolderDialog())
            {
                dialog.Title = windowTitle;

                var result = dialog.ShowDialog(null);
                if (result == DialogResult.Ok)
                    folderPath = dialog.Directory;
            }

            return folderPath;
        }

        #endregion

        #region Image related widgets

        /// <summary>
        /// Displays the specified image in a window and pauses the execution flow.
        /// <para>Press and hold shift + mouse (track-pad) to translate and zoom an image.</para>
        /// </summary>
        /// <param name="image">Image to show.</param>
        /// <param name="windowTitle">Window title (ID).</param>
        /// <param name="scaleForm">True to adjust form to the image size, false otherwise.</param>
        public static void ShowDialog(this Bgr<byte>[,] image, string windowTitle = "Image", bool scaleForm = false)
        {
            FormCollection.CreateAndShowDialog(() =>
            {
                var f = new ImageForm(windowTitle);
                f.ScaleForm = scaleForm;
                f.SetImage(image);

                return f;
            },
            f => true);
        }

        /// <summary>
        /// Displays the specified image in a window (non blocking mode).
        /// <para>Press and hold shift + mouse (track-pad) to translate and zoom an image.</para>
        /// </summary>
        /// <param name="image">Image to show.</param>
        /// <param name="windowTitle">Window title (ID).</param>
        /// <param name="scaleForm">True to adjust form to the image size, false otherwise.</param>
        public static void Show(this Bgr<byte>[,] image, string windowTitle = "Image", bool scaleForm = false)
        {
            FormCollection.CreateOrUpdate(() => new ImageForm(windowTitle),
                           form =>
                           {
                               form.ScaleForm = scaleForm;
                               form.SetImage(image);
                           },
                           windowTitle);
        }

        /// <summary>
        /// Displays the specified image in a window and waits the user to create a mask by drawing.
        /// <para>Press and hold shift + mouse (track-pad) to translate and zoom an image.</para>
        /// </summary>
        /// <param name="image">Image to display.</param>
        /// <param name="windowTitle">Window title (ID).</param>
        /// <param name="scaleForm">True to adjust form to the image size, false otherwise.</param>
        /// <returns>Drawn mask.</returns>
        public static Gray<byte>[,] GetMask(this Bgr<byte>[,] image, string windowTitle = "Draw image mask (close when finished)", bool scaleForm = false)
        {
            var mask = FormCollection.CreateAndShowDialog(() =>
                        {
                            var f = new DrawingPenForm(windowTitle);
                            f.ScaleForm = scaleForm;
                            f.SetImage(image);

                            return f;
                        },
                        f => f.Mask);

            return mask;        
        }

        /// <summary>
        /// Displays the specified image in a window and waits the user to create a rectangle by drawing.
        /// <para>Press and hold shift + mouse (track-pad) to translate and zoom an image.</para>
        /// </summary>
        /// <param name="image">Image to display.</param>
        /// <param name="windowTitle">Window title (ID).</param>
        /// <param name="scaleForm">True to adjust form to the image size, false otherwise.</param>
        /// <returns>Drawn rectangle.</returns>
        public static RectangleF GetRectangle(this Bgr<byte>[,] image, string windowTitle = "Draw rectangle (close when finished)", bool scaleForm = false)
        {
            var rect = FormCollection.CreateAndShowDialog(() =>
                        {
                            var f = new DrawingRectangleForm(windowTitle);
                            f.ScaleForm = scaleForm;
                            f.SetImage(image);

                            return f;
                        },
                        f => f.Rectangle);

            return rect;
        }

        /// <summary>
        /// Displays the image and enables the user selects an area.
        /// <para>Press and hold any key to temporary block the calling thread (useful when used with video sequence stream).</para>
        /// </summary>
        /// <param name="image">Image to display.</param>
        /// <param name="windowTitle">Window title (ID).</param>
        /// <param name="onDrawn">Action executed when a rectangle is drawn (when mouse is released).</param>
        /// <param name="scaleForm">True to adjust form to the image size, false otherwise.</param>
        /// <param name="startBlocked">
        /// Used only when a form is first initialized. True to start as a dialog, false to show the form in non-blocking way.
        /// <para>When form is the blocking state (dialog) user is required to press and release a key to enable non-blocking mode.</para>
        /// </param>
        public static void GetRectangle(this Bgr<byte>[,] image, string windowTitle = "Draw rectangle", Action<RectangleF> onDrawn = null, bool scaleForm = false, bool startBlocked = false)
        {
            ManualResetEvent resetEvent = null;

            FormCollection.CreateOrUpdate(
                          () => new DrawingRectangleForm(windowTitle, !startBlocked),                     
                          form =>
                          {
                              form.ScaleForm = scaleForm;
                              form.SetImage(image);
                              resetEvent = form.ResetEvent;
                              form.OnDrawn = onDrawn;
                          },
                          windowTitle);

            resetEvent.WaitOne();
        }

        #endregion
    }
}
