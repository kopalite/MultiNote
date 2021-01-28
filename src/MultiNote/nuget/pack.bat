dotnet publish -r win-x64 -c Release ..\MultiNote.csproj
xcopy /Y "..\bin\Release\net5.0\win-x64\publish" "."
nuget pack .nuspec