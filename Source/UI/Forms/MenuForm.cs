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

using Eto.Drawing;
using Eto.Forms;
using System;

namespace DotImaging
{
    /// <summary>
    /// Menu form.
    /// </summary>
    internal class MenuForm : Form
    {
        /// <summary>
        /// Creates a new menu form.
        /// </summary>
        /// <param name="title">Window title.</param>
        public MenuForm(string title = "")
            :this(title, null, null)
        { }

        /// <summary>
        /// Creates a new menu form.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="itemNames">Item names.</param>
        /// <param name="actions">Actions.</param>
        public MenuForm(string title, string[] itemNames, Action[] actions)
        {
            Title = title;
            SelectedIndex = -1;

            if (itemNames == null || actions == null)
                return;
            if (itemNames.Length != actions.Length)
                return;

            var stackLayout = new StackLayout {Orientation = Orientation.Vertical, HorizontalContentAlignment = HorizontalAlignment.Stretch };

            for (int i = 0; i < itemNames.Length; i++)
            {
                var idx = i;

                var button = new Button { Text = itemNames[idx], Size = new Size(240, 60) }; 
                button.Click += (s, e) =>
                {
                    actions[idx]();
                    SelectedIndex = idx;
                    this.Close();
                };

                stackLayout.Items.Add(new StackLayoutItem(button, true));
            }

            Content = stackLayout;
            Size = new Size(-1, -1);
        }

        /// <summary>
        /// Gets the selected menu item index or returns -1 in case the form is closed.
        /// </summary>
        public int SelectedIndex
        {
            get; private set;
        }
    }

}
