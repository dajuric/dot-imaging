:: The script was taken from: Accord.NET project and modified
@echo off

echo.
echo DotImaging Framework NuGet package publisher
echo =========================================================
echo. 
echo This Windows batch file uses NuGet to automatically
echo push the framework packages to the gallery.
echo. 

timeout /T 5

:: Directory settings
set output=bin\
set current=%~dp0

echo.
echo Current directory: %current%
echo Output  directory: %output%
echo.

forfiles /p %output% /m *.nupkg /c "cmd /c %current%\NuGet.exe push @file"

pause