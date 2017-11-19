using DotImaging;
using System;
using System.Diagnostics;

namespace YoutubeStreaming
{
    class Program
    {
        public static void Main()
        {
            var sourceName = String.Empty;

            //video over pipe (direct link and Youtube) (do not support seek)
            //var pipeName = new Uri("http://trailers.divx.com/divx_prod/divx_plus_hd_showcase/BigBuckBunny_DivX_HD720p_ASP.divx").NamedPipeFromVideoUri(); //web-video
            var pipeName = new Uri("https://www.youtube.com/watch?v=Vpg9yizPP_g").NamedPipeFromYoutubeUri(); //Youtube
            sourceName = String.Format(@"\\.\pipe\{0}", pipeName);

            //video http link (Supports seek)
            //sourceName = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4";

            //---------------------------------------------
            ImageStreamReader reader = new FileCapture(sourceName);
            reader.Open();

            //seek if you can
            if(reader.CanSeek)
                reader.Seek((int)(reader.Length * 0.25), System.IO.SeekOrigin.Begin);

            //read video frames
            Bgr<byte>[,] frame = null;
            do
            {
                reader.ReadTo(ref frame);
                if (frame == null)
                    break;

                frame.Show(scaleForm: true);
                ((double)reader.Position / reader.Length).Progress();
            }
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape));

            Console.WriteLine("The end.");

            //---------------------------------------------------------------------------
            Console.WriteLine("Downloading video...");

            string fileExtension;
            var downloadPipeName = new Uri("https://www.youtube.com/watch?v=Vpg9yizPP_g").NamedPipeFromYoutubeUri(out fileExtension); //Youtube

            downloadPipeName.SaveNamedPipeStream("out" + fileExtension);
            Console.WriteLine("Video saved.");
            Process.Start("out" + fileExtension); //open local file
        }
    }
}
