Interoperability extensions for System.Windows.Media.Imaging.BitmapSource.

1) Array <-> Bitmap conversion:

	//to generic-image
	var colorBitmap = new BitmapImage(new Uri("<path>"));
	Bgra<byte>[,] colorImg = colorBitmap.ToArray<Bgra<byte>>();
 
	//to BitmapSource
	Gray<byte>[,] grayImg = ImageIO.LoadGray("<path>");
	BitmapSource grayBitmap = grayImg.ToBitmapSource();

Discover more extensions as you type :)