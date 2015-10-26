 Provides the portable way to determine the execution platform. 
 Provides some additional platform related interoperability functions.

1)
Console.WriteLine(Platform.RunningPlatform); //Windows, Linux, MacOS

2) (tried only on Windows)
Platform.AddDllSearchPath(); //adds the "UnmanagedLibraries/<your platform>/<version>/" to the path (e.g. UnmanagedLibraries/Windows/x64)

3)
string pathToFile = @"bin\SomeFolder/SomeFolder2\\yourFile.txt";
pathtoFile = pathtoFile.NormalizePathDelimiters(); //outputs "bin\\SomeFolder\\SomeFolder2\\yourFile.txt"
