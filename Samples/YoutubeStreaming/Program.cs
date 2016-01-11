using DotImaging;
using System;

namespace YoutubeStreaming
{
    class Program
    {
        public static void Main()
        {
            //var pipeName = new Uri("http://trailers.divx.com/divx_prod/divx_plus_hd_showcase/BigBuckBunny_DivX_HD720p_ASP.divx").NamedPipeFromVideoUri(); //web-video
            var pipeName = new Uri("https://www.youtube.com/watch?v=Vpg9yizPP_g").NamedPipeFromYoutubeUri(); //Youtube

            ImageStreamReader reader = new FileCapture(String.Format(@"\\.\pipe\{0}", pipeName));
            reader.Open();

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
            pipeName = new Uri("https://www.youtube.com/watch?v=Vpg9yizPP_g").NamedPipeFromYoutubeUri(out fileExtension); //Youtube

            pipeName.SaveNamedPipeStream("out" + fileExtension);
            Console.WriteLine("Video saved.");
        }
    }
}
