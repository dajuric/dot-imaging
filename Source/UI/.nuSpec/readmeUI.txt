Portable standalone UI elements invokable as extensions (image display, progress bar, open-save file dialogs, folder-selection dialog, color-picker, mask-drawing dialog, rectangle-drawing dialog, menu dialog).

1) image show
   Bgr<byte>[,] image = new Bgr<byte>[480, 640];
   image.SetValue<Bgr<byte>>(Bgr<byte>.Red);
   image.Show();

2) progress bar
   (0.5d).Progress(message: "Working on something..."); //default
   (-1d).Progress(message: "Waiting for something..."); //indeterminate
   (1.0d).Progress();                                   //will close

3) open/save file dialogs, folder selection dialog
   string pickedFile = UI.OpenFile();
   string saveToFile = UI.SaveFile();

   string selectedFolder = UI.SelectFolder();

4) color picker
   Bgr<byte> color = UI.PickColor(); //WARNING: calling thread must be STAThread

5) mask drawing dialog
   Bgr<byte>[,] image = new Bgr<byte>[480, 640];
   image.SetValue<Bgr<byte>>(Bgr<byte>.Red);
   Gray<byte>[,] mask = image.GetMask(); //get user-defined mask dialog 

6) rectangle drawing 
   //blocking (dialog)
   Bgr<byte>[,] image = new Bgr<byte>[480, 640];
   image.SetValue<Bgr<byte>>(Bgr<byte>.Red);
   RectangleF rect = image.GetRectangle(); //get user-defined rectangle dialog

   //non-blocking (e.g. for camera-feed)
   Bgr<byte>[,] image = new Bgr<byte>[480, 640];
   image.SetValue<Bgr<byte>>(Bgr<byte>.Red);
   image.GetRectangle(onDrawn: (rect) => Console.WriteLine(rect)); //press any key to temporary block

7) menu dialog
   var selectedNumber = -1;
   UI.ShowMenu(itemNames: new string[] { "2", "3" },
               actions: new Action[] { () => selectedNumber = 2, () => selectedNumber = 3 });

   
   Console.WriteLine("Selected number: {0}", selectedNumber);

---------------------------------------------------------------------------
In order to support other OS platforms (except Windows) install one (or more) of the following packages:

      - Eto.Platform.Windows (included by default)
      - Eto.Platform.Wpf
      - Eto.Platform.Direct2D
      - Eto.Platform.Gtk
      - Eto.Platform.Gtk3
      - Eto.Platform.Mac
      - Eto.Platform.XamMac  * requires Xamarin Studio on OS X.
      - Eto.Platform.XamMac2  * requires Xamarin Studio on OS X.