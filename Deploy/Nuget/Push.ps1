#--references: 
#  list files: https://medium.com/@victorleungtw/replace-text-in-xml-files-with-powershell-504d3e37a058

timeout /T 5

Write-Host
Write-Host Pushing packages:
$files = @(Get-ChildItem *.nupkg -Recurse)
foreach($f in $files)
{
   Write-Host $f
   $cmd = './nuget push "$f" -source https://www.nuget.org/api/v2/package'
   iex $cmd
}

Write-Host
#pause