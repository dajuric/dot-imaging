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

using Eto;
using Eto.Forms;
using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using P = Eto.Platform;

namespace DotImaging
{
    /// <summary>
    /// Form collection management.
    /// </summary>
    internal static class FormCollection
    {
        #region Base

        /// <summary>
        /// Timeout for application initialization
        /// </summary>
        const int ApplicationTimeout = 5 * 1000; //ms

        /// <summary>
        /// Initializes the UI application.
        /// </summary>
        public static void Initialize()
        {
            if (P.Instance != null)
                return; //it is already initialized

            P platform = null;
            try { platform = P.Detect; }
            catch (Exception ex) { throw ex; }

            P.Initialize(platform);

            if (platform.Supports<Application>())
            {
                var ev = new ManualResetEvent(false);
                Exception exception = null;

                Thread thread = new Thread(() =>
                {
                    try
                    {
                        var app = new Application(platform);
                        app.Initialized += (sender, e) => ev.Set();
                        app.Run();
                    }
                    catch (Exception ex)
                    {
                        //Debug.WriteLine("Error running test application: {0}", ex);
                        exception = ex;
                        ev.Set();
                    }
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                if (!ev.WaitOne(ApplicationTimeout))
                    throw new Exception("Could not initialize application");
                if (exception != null)
                    ExceptionDispatchInfo.Capture(exception).Throw();
            }
        }

        /// <summary>
        /// Creates and show a new dialog form.
        /// </summary>
        /// <typeparam name="TForm">Form type.</typeparam>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="creator">Form creator function.</param>
        /// <param name="getResult">Result getter function.</param>
        /// <returns>Result, available after the form is closed.</returns>
        public static TResult CreateAndShowDialog<TForm, TResult>(Func<TForm> creator, Func<TForm, TResult> getResult)
           where TForm : Form
        {
            var ev = new ManualResetEvent(false);
            var application = Application.Instance;
            Exception exception = null;

            TForm form = null;
            TResult result = default(TResult);

            Action run = () =>
            {
                try
                {
                    if (!P.Instance.Supports<Form>())
                        throw new NotSupportedException("This platform does not support IForm");

                    form = creator();
                    form.Show();

                    form.Closed += (s, e) =>
                    {
                        result = getResult(form);
                        ev.Set(); 
                    };
                }
                catch (Exception ex)
                {
                    exception = ex;
                    ev.Set();
                }
            };

            application.Invoke(run);

            if (exception != null)
                ExceptionDispatchInfo.Capture(exception).Throw();

            ev.WaitOne(); //sync

            return result;
        }

        private static TForm createAndShow<TForm>(Func<TForm> creator)
            where TForm : Form
        {
            var ev = new ManualResetEvent(false);
            var application = Application.Instance;
            Exception exception = null;

            TForm form = null;

            Action run = () =>
            {
                try
                {
                    if (!P.Instance.Supports<Form>())
                        throw new NotSupportedException("This platform does not support IForm");

                    form = creator();
                    form.Show();
                }
                catch (Exception ex)
                {
                    exception = ex;
                    ev.Set();
                }
            };

            application.Invoke(run);

            if (exception != null)
                ExceptionDispatchInfo.Capture(exception).Throw();

            return form;
        }


        /// <summary>
        /// Creates a new form or updates the existing one based on window title used as ID.
        /// </summary>
        /// <typeparam name="TForm">Form type.</typeparam>
        /// <param name="creator">Form creator function.</param>
        /// <param name="update">Form update function.</param>
        /// <param name="windowTitle">Window title (ID).</param>
        public static void CreateOrUpdate<TForm>(Func<TForm> creator, Action<TForm> update, string windowTitle = "")
            where TForm : Form
        {
            Application.Instance.Invoke(() =>
            {
                var form = Application.Instance.Windows.Where(x => x.Title == windowTitle).FirstOrDefault();

                if (form != null)
                {
                    if (form is TForm == false)
                        return;
                }
                else
                    form = createAndShow(creator);

                update(form as TForm);
            });
        }

        /// <summary>
        /// Closes all form windows if displayed.
        /// </summary>
        public static void CloseAll()
        {
            Application.Instance.Invoke(() => Application.Instance.Quit());
        }

        #endregion
    }
}
