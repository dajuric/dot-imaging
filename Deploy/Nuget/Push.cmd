::
@echo off
timeout /T 5

echo Pushing packages:
for /r %%f in (*.nupkg) do (
	echo   %%f
	dotnet nuget push "%%f" --source https://www.nuget.org/api/v2/package
)

echo.
pause