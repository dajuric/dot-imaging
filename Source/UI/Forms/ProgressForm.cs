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

using Eto.Drawing;
using Eto.Forms;
using System;

namespace DotImaging
{
    /// <summary>
    /// Progress bar form.
    /// </summary>
    internal class ProgressForm : Form
    {
        Label label = null;
        ProgressBar progressBar = null;

        /// <summary>
        /// Creates new progress bar window.
        /// </summary>
        /// <param name="title">Window title.</param>
        public ProgressForm(string title = "")
        {
            Title = title;
            ClientSize = new Size(320, 50);
            MinimumSize = new Size(100, 50);
            this.Maximizable = false;
            this.Resizable = false;

            label = new Label { TextAlignment = TextAlignment.Center };
            progressBar = new ProgressBar { Value = 0, MinValue = 0, MaxValue = 100 };

            Content = new TableLayout
            {
                Spacing = new Size(5, 5), // space between each cell
                Padding = new Padding(5), // space around the table's sides
                Rows =
                {
                    new TableRow(new TableCell(label, true)),
                    new TableRow(new TableCell(progressBar, true)),
                    new TableRow { ScaleHeight = true }
                }
            };
        }

        /// <summary>
        /// Sets the progress value and mesage.
        /// </summary>
        /// <param name="value">Value [0..1].</param>
        /// <param name="message">Message.</param>
        public void Set(double value, string message = "")
        {
            message = message ?? String.Empty;

            var intVal = (int)Math.Round(value * 100);
            if (intVal >= 0)
            {
                progressBar.Indeterminate = false;
                progressBar.Value = intVal;
                label.Text = message + " " + String.Format("{0} %", intVal);

                if (intVal >= 100)
                    this.Close();
            }
            else
            {
                progressBar.Indeterminate = true;
                label.Text = message;
            }
        }
    }
}
