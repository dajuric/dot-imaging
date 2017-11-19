Provides Linq extensions for 2D .NET arrays.

1) channel modification:

	Bgr<byte>[,] image = ImageIO.LoadColor("sample.jpg").Clone();

	//get the modified blue channel 
	var modifiedImage = image.AsEnumerable()
	                         .Select(x => x.B / 2)
							 .ToArray2D(image.Size());


Discover more extensions as you type :)