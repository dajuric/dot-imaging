<p align="center">
    <a href="https://www.nuget.org/profiles/dajuric"> <img src="Deployment/Logo/logo-big.png" alt="DotImaging logo" width="120" align="center"> </a>
</p>

<p align="center">
    <img src="https://img.shields.io/badge/Build-passing-brightgreen.svg?style=flat-square" alt="Build passing"/>
    <a href="https://www.nuget.org/profiles/dajuric"> <img src="https://img.shields.io/badge/NuGet-v2.5.0-blue.svg?style=flat-square" alt="NuGet packages version"/>  </a>
</p>

**DotImaging** - .NET array as imaging object  
The framework sets focus on .NET native array as primary imaging object, offers extensibility support via extensions, and provides unified platform-abstract imaging IO API. 

## So why DotImaging ?

+ leverages existing .NET structures
+ portable - designed for ASP.NET vNext
+ lightweight (no 3rd party dependencies), but powerful
+ so simple, you don't need a help file

## Libraries / NuGet packages

+ <a href="https://www.nuget.org/packages/DotImaging.GenericImage">DotImaging.GenericImage</a>    
  .NET image array extensions. Color and depth conversions. Slim unmanaged structure for fast pixel manipulation.

  > **Tutorial:** <a href="http://www.codeproject.com/Articles/829349/Introducing-Portable-Generic-Image-Library-for-Csh" target="_blank">Portable Generic Image</a> <em>(update pending)</em>

 ``` csharp
//convert to grayscale and flip
Bgr<byte> image = ImageIO.LoadColor("sample.jpg").Clone();
Gray<byte> grayIm = image.ToGray()
                             .Flip(FlipDirection.Horizontal);
 ```

+ <a href="https://www.nuget.org/packages/DotImaging.IO">DotImaging.IO</a>  
  A unified API for IO image access (camera, file, image directory). Portable image loading/saving.

  > **Tutorial:** <a href="http://www.codeproject.com/Articles/828012/Introducing-Portable-Video-IO-Library-for-Csharp" target="_blank">Portable Imaging IO</a> <em>(update pending)</em>

 ``` csharp
//create camera (file or image-directory) capture
var reader = new CameraCapture();

reader.Open();

//read single frame
var frame = reader.ReadAs<Bgr<byte>>();

reader.Close();
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
  Interoperability extensions between .NET array and Bitmap.

 ``` csharp
var image = new Gray<byte>[240, 320];
var bmp = image.ToBitmap(); //to Bitmap

var imageFromBmp = bmp.ToArray() as Bgr<byte>[,]; //from Bitmap
 ``` 

+ <a href="https://www.nuget.org/packages/DotImaging.Primitives2D">DotImaging.Primitives2D</a>  
  Portable 2D drawing primitives (Point, Size, Rectangle, ...)
 
## Getting started
+ Just pick what you need. An appropriate readme file will be shown upon selected NuGet package installation. 
+ Documentation: <a href="https://github.com/dajuric/dot-imaging/raw/master/Deployment/Documentation/Help/DotImaging%20Documentation.chm"> Offline </a> - <i>unblock after download!</i> </br/>
+ Samples

## Want image processing algorithms ?
The framework is the foundation of <a href="https://github.com/dajuric/accord-net-extensions">Accord.NET Extensions</a> which exposes a full power of <a href="http://accord-framework.net/"> Accord.NET </a> through extensions!

## How to Engage, Contribute and Provide Feedback  
Remember: Your opinion is important and will define the future roadmap.
+ questions, comments - message on Github, or write to: darko.juric2 [at] gmail.com
+ **spread the word** 

## Final word
If you like the project please **star it** in order to help to spread the word. That way you will make the framework more significant and in the same time you will motivate me to improve it, so the benefit is mutual.
