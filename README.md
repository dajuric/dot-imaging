<p align="center">
    <a href="https://www.nuget.org/profiles/dajuric"> <img src="Deployment/Logo/logo-big.png" alt="DotImaging logo" width="120" align="center"> </a>
</p>

<p align="center">
    <a href="https://www.nuget.org/profiles/dajuric"> <img src="https://img.shields.io/badge/NuGet-v4.6.0-blue.svg?style=flat-square" alt="NuGet packages version"/>  </a>
</p>

**DotImaging** - .NET array as imaging object  
The framework sets focus on .NET native array as primary imaging object, offers extensibility support via extensions, and provides unified platform-abstract imaging IO API. 

## So why DotImaging ?

+ leverages existing .NET structures
+ portable* 
+ lightweight
+ **so simple**, you don't need a help file

*IO and Drawing assemlies depend on OpenCV

## Libraries / NuGet packages

+ <a href="https://www.nuget.org/packages/DotImaging.GenericImage">DotImaging.GenericImage</a>    
  .NET image array extensions. Color and depth conversions. Slim unmanaged structure for fast pixel manipulation.

  > **Tutorial:** <a href="http://www.codeproject.com/Articles/829349/Introducing-Portable-Generic-Image-Library-for-Csh" target="_blank">Portable Generic Image</a>

 ``` csharp
//convert to grayscale and flip
Bgr<byte>[,] image = ImageIO.LoadColor("sample.jpg").Clone(); //IO package
Gray<byte>[,] grayIm = image.ToGray()
                                .Flip(FlipDirection.Horizontal);
 ```

+ <a href="https://www.nuget.org/packages/DotImaging.IO">DotImaging.IO</a>  
  A unified API for IO image access (camera, file, image directory). Portable image loading/saving.

  > **Tutorial:** <a href="http://www.codeproject.com/Articles/828012/Introducing-Portable-Video-IO-Library-for-Csharp" target="_blank">Portable Imaging IO</a>

 ``` csharp
var reader = new FileCapture(fileName);
reader.Open();

reader.SaveFrames(outputDir, "{0}.jpg", (percentage) =>
{
       Console.Write("\r Extracting video: {0} %", percentage * 100);
});

reader.Close();
 ``` 
 
+ <a href="https://www.nuget.org/packages/DotImaging.IO.Web">DotImaging.IO.Web</a>  
  Image or video download/streaming (direct video link or Youtube links).

 ``` csharp
//------get an image from the Web
new Uri("http://vignette3.wikia.nocookie.net/disney/images/5/5d/Lena_headey_.jpg")
     .GetBytes()
     .DecodeAsColorImage()
	 .Show(); //(UI package)
 
//------stream a video from Youtube
var pipeName = new Uri("https://www.youtube.com/watch?v=Vpg9yizPP_g").NamedPipeFromYoutubeUri(); //Youtube
var reader = new FileCapture(String.Format(@"\\.\pipe\{0}", pipeName)) //IO package
 
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

+ <a href="https://www.nuget.org/packages/DotImaging.Drawing">DotImaging.Drawing</a>  
  .NET image drawing array extensions.

 ``` csharp
//create a managed image
var image = new Bgr<byte>[480, 640];

//draw something
image.Draw(new Rectangle(50, 50, 200, 100), Bgr<byte>.Red, -1);
image.Draw(new Circle(50, 50, 25), Bgr<byte>.Blue, 5);
 ``` 

+ <a href="https://www.nuget.org/packages/DotImaging.BitmapInterop">DotImaging.BitmapInterop</a>  
  Interoperability extensions between .NET array and Bitmap (WinForms).

 ``` csharp
var image = new Gray<byte>[240, 320];
var bmp = image.ToBitmap(); //to Bitmap

var imageFromBmp = bmp.ToArray() as Bgr<byte>[,]; //from Bitmap
 ``` 
 
+ <a href="https://www.nuget.org/packages/DotImaging.BitmapSourceInterop">DotImaging.BitmapSourceInterop</a>  
  Interoperability extensions between .NET array and BitmapSource (WPF).

 ``` csharp
var bmp = new BitmapImage(new Uri("<path>"));
Bgra<byte>[,] colorImg = bmp.ToArray<Bgra<byte>>(); //to bitmap

var imageFromBitmap = colorImg.ToBitmapSource(); //from bitmap
 ```

+ <a href="https://www.nuget.org/packages/DotImaging.UI">DotImaging.UI</a>  
  Portable UI elements (image display, progress bar, open-save file dialogs, folder-selection dialog, color-picker).

 ``` csharp
Bgr<byte>[,] image = new Bgr<byte>[480, 640];
image.Show(); //show image (non-blocking)

(0.4d).Progress(); //progress bar - 40% (non-blocking)

string fileName = UI.OpenFile(); //open-file dialog

Bgr<byte> color = UI.PickColor(); //color picker dialog

Gray<byte>[,] mask = image.GetMask(); //get user-defined mask dialog 
 ```
 
+ <a href="https://www.nuget.org/packages/DotImaging.Linq">DotImaging.Linq</a>  
  2D array Linq extensions

 ``` csharp
//create a managed image
Bgr<byte>[,] image = ...; 

//get the modified blue channel 
var modifiedImage = image.AsEnumerable()
	                         .Select(x => x.B / 2)
							 .ToArray2D(image.Size());
 ``` 
 
+ <a href="https://www.nuget.org/packages/DotImaging.Primitives2D">DotImaging.Primitives2D</a>  
  Portable 2D drawing primitives (Point, Size, Rectangle, ...)
 
## Getting started
+ Just pick what you need. An appropriate readme file will be shown upon selected NuGet package installation. 
+ Each project in "Source" has a short readme
+ Samples

## Want image processing algorithms ?
The framework is the foundation of <a href="https://github.com/dajuric/accord-net-extensions">Accord.NET Extensions</a> which exposes a full power of <a href="http://accord-framework.net/"> Accord.NET </a> through extensions!

## How to Engage, Contribute and Provide Feedback  
Remember: Your opinion is important and will define the future roadmap.
+ questions, comments - message on Github, or write to: darko.juric2 [at] gmail.com
+ **spread the word** 

## Final word
If you like the project please **star it** in order to help to spread the word. That way you will make the framework more significant and in the same time you will motivate me to improve it, so the benefit is mutual.
