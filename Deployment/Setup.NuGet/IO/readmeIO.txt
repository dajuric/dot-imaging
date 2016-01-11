Unified stream-like platform-abstract API for IO video access: web-camera support, various video-format reading / writing, image-directory reader.
Image loading/saving (file or in-memory).

1) image loading / saving:

   Bgr<byte>[,] image = ImageIO.LoadColor("sample.jpg"); //load Bgr color image
   Gray<float>[,] hdrImage = (ImageIO.LoadUnchanged("hdrImage.png") as Image<Gray<float>>).Clone(); //load HDR grayscale (or any other) image
   image.Save("image.png");


2) image in-memory encoding / decoding

   Bgr<byte>[,] bgrIm = ImageIO.LoadColor("sample.jpg");
   byte[] encodedBgr = bgrIm.EncodeAsJpeg(); //or png, bmp
   Bgr<byte>[,] decodedBgr = encodedBgr.DecodeAsColorImage();


3) media (camera, video, image-directory) capture:

   ImageStreamReader reader = new CameraCapture(); //use specific class for device-specific properties (e.g. exposure, image name, ...)
   reader.Open();

   //read single frame
   var frame = reader.Read().ToBgr();
   reader.Close();
   

4) video writer:

   ImageStreamWriter videoWriter = new VideoWriter("out.avi", new Size(1280, 1024));

   var image = new Bgr<byte>[1024, 1280];
   videoWriter.Write(image.Lock()); //write a single frame

   videoWriter.Close();

5) frame extraction:

	var reader = new FileCapture(fileName);
    reader.Open();

    reader.SaveFrames(outputDir, "{0}.jpg", (percentage) =>
    {
        ((double)percentage).Progress(message: "Extracting video...");
    });

	reader.Close();


Discover more types as you type :)


