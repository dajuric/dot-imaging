using System;
using System.Windows.Forms;
using DotImaging;
using System.Collections.Generic;

namespace MultipleCameraCapture
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var activeForms = new List<Form>();

            var cameraCount = CameraCapture.CameraCount;
            for (int camIdx = 0; camIdx < cameraCount; camIdx++)
            {
                var activeForm = new CaptureWindow(camIdx);
                activeForms.Add(activeForm);
            }

            if (cameraCount == 0)
            {
                MessageBox.Show("No camera device is present.");
                return;
            }

            Application.Run(new MultiFormApplicationContext(activeForms));
        }

        class MultiFormApplicationContext : ApplicationContext
        {
            private void onFormClosed(object sender, EventArgs e)
            {
                if (Application.OpenForms.Count == 0)
                {
                    ExitThread();
                }
            }

            public MultiFormApplicationContext(IEnumerable<Form> forms)
            {
                foreach (var form in forms)
                {
                    form.Show();
                    form.FormClosed += onFormClosed;
                }
            }
        }
    }
}
