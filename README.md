<p align="center">
    <a href="https://www.nuget.org/profiles/dajuric"> <img src="Deploy/Logo/logo-big.png" alt="DotImaging logo" width="120" align="center"> </a>
</p>

<p align="center">
    <a href="https://www.nuget.org/profiles/dajuric"> <img src="https://img.shields.io/badge/NuGet-v5.1.0-blue.svg?style=flat-square" alt="NuGet packages version"/>  </a>
</p>

**DotImaging** - .NET array as imaging object  
The framework sets focus on .NET native array as primary imaging object, offers extensibility support via extensions, and provides unified platform-abstract imaging IO API. 

## News
+ 5.1.0 version came out (17/02/2019)
+ Image enchacement library for DotImaging: <a href="https://github.com/dajuric/dot-devignetting">DotDevignetting</a>!

## So why DotImaging ?

+ Image as native .NET 2D array 
+ portable* 
+ lightweight
+ extensions, extensions, extensions....

*IO and Drawing assemlies depend on OpenCV

## Libraries / NuGet packages


###### Core

+ <a href="https://www.nuget.org/packages/DotImaging.Image"> 
    <img src="https://img.shields.io/badge/DotImaging-Image-red.svg?style=flat-square" alt="DotImaging.Image"/>  
  </a> 
  .NET image array extensions. Color and depth conversions. Slim unmanaged structure for fast pixel manipulation.

  > **Tutorial:** <a href="http://www.codeproject.com/Articles/829349/Introducing-Portable-Generic-Image-Library-for-Csh" target="_blank">Portable Generic Image</a>

 ``` csharp
//convert to grayscale and flip
Bgr<byte>[,] image = ImageIO.LoadColor("sample.jpg").Clone(); //IO package
Gray<byte>[,] grayIm = image.ToGray()
                                .Flip(FlipDirection.Horizontal);

//get the modified blue channel 
var modifiedImage = image.AsEnumerable()
	                      .Select(x => x.B / 2)
							 .ToArray2D(image.Size());
 ```

###### IO

+ <a href="https://www.nuget.org/packages/DotImaging.IO"> 
    <img src="https://img.shields.io/badge/DotImaging-IO-red.svg?style=flat-square" alt="DotImaging.IO"/>  
  </a>
  A unified API for IO image access (camera, file, image directory). Portable image loading/saving.

  > **Tutorial:** <a href="http://www.codeproject.com/Articles/828012/Introducing-Portable-Video-IO-Library-for-Csharp" target="_blank">Portable Imaging IO</a>

 ``` csharp
var reader = new FileCapture("sample.mp4");
reader.Open();

Bgr<byte>[,] frame = null;
while(true)
{
       reader.ReadTo(ref frame);
       if (frame == null)
          break;

       frame.Show(scaleForm: true); //UI package
}

reader.Close();
 ``` 
 
+ <a href="https://www.nuget.org/packages/DotImaging.IO.Web"> 
    <img src="https://img.shields.io/badge/DotImaging-IO.Web-red.svg?style=flat-square" alt="DotImaging.IO.Web"/>  
  </a>
  Image or video download/streaming (direct video link or Youtube links).

 ``` csharp
//------get an image from the Web
new Uri("http://vignette3.wikia.nocookie.net/disney/images/5/5d/Lena_headey_.jpg")
     .GetBytes().DecodeAsColorImage().Show(); //(Show - UI package)
 
//------stream a video from Youtube
var pipeName = new Uri("https://www.youtube.com/watch?v=Vpg9yizPP_g").NamedPipeFromYoutubeUri(); //Youtube
var reader = new FileCapture(String.Format(@"\\.\pipe\{0}", pipeName)) //IO package
 
//... (regular stream reading - see IO package sample)
 ``` 

 
###### Interoperability

+ <a href="https://www.nuget.org/packages/DotImaging.BitmapInterop"> 
    <img src="https://img.shields.io/badge/DotImaging-BitmapInterop-red.svg?style=flat-square" alt="DotImaging.BitmapInterop"/>  
  </a>
  Interoperability extensions between .NET array and Bitmap (WinForms).

 ``` csharp
var image = new Gray<byte>[240, 320];
var bmp = image.ToBitmap(); //to Bitmap

var imageFromBmp = bmp.ToArray() as Bgr<byte>[,]; //from Bitmap
 ``` 
 
 
###### Extensions

+ <a href="https://www.nuget.org/packages/DotImaging.UI.Image"> 
    <img src="https://img.shields.io/badge/DotImaging-UIImage-red.svg?style=flat-square" alt="DotImaging.UI"/>  
  </a> 
  Portable UI elements (image display, progress bar, file/folder dialogs, color-picker, image annotation input).

 ``` csharp
Bgr<byte>[,] image = new Bgr<byte>[480, 640];
image.Show(); //show image (non-blocking)
image.ShowDialog(); //show image (blocking)

//draw something
image.Draw(new Rectangle(50, 50, 200, 100), Bgr<byte>.Red, -1);
image.Draw(new Circle(50, 50, 25), Bgr<byte>.Blue, 5);
 ```
  
## Getting started
+ Just pick what you need. An appropriate readme file will be shown upon selected NuGet package installation. 
+ Samples

## Final word
If you like the project please **star it** in order to help to spread the word. That way you will make the framework more significant and in the same time you will motivate me to improve it, so the benefit is mutual.
