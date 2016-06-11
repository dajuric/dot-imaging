 Provides support for image or video download/streaming (direct video link or Youtube links).

1) image loading:
  
   new Uri("http://vignette3.wikia.nocookie.net/disney/images/5/5d/Lena_headey_.jpg")
        .GetBytes()
        .DecodeAsColorImage()
	.Show(); //requires UI package

2) video streaming:

    var sourceName = String.Empty;

    var pipeName = new Uri("https://www.youtube.com/watch?v=Vpg9yizPP_g").NamedPipeFromYoutubeUri(); //Youtube
    sourceName = String.Format(@"\\.\pipe\{0}", pipeName);
    
    //sourceName = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4"; //direct http streaming 

    //------------------------------------------------------------------
    ImageStreamReader reader = new FileCapture(sourceName, pipeName));
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


3) video download:

   string fileExtension;
   pipeName = new Uri("https://www.youtube.com/watch?v=Vpg9yizPP_g").NamedPipeFromYoutubeUri(out fileExtension); //Youtube
   pipeName.SaveNamedPipeStream("out" + fileExtension);


Discover more types as you type :)
