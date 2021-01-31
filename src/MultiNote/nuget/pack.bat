dotnet publish -c Release ..\MultiNote.csproj
xcopy /Y "..\bin\Release\net5.0\publish" "."
nuget pack .nuspec