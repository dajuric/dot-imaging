using DotImaging;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

[assembly: DebuggerVisualizer(
typeof(ImageDebuggerVisualizer),
typeof(ImageVisualizerObjectSource),
Target = typeof(Bgr<byte>[,]))]

namespace DotImaging
{
    public class ImageVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            var image = target as Bgr<byte>[,];
            if (image == null) return;

            image.ToBitmap().Save(outgoingData, ImageFormat.Png);
            outgoingData.Flush();
        }
    }

    public sealed class ImageDebuggerVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var bmp = Image.FromStream(objectProvider.GetData());
            var image = (bmp as Bitmap).ToBgr();

            image.ShowDialog();
        }
    }

}
