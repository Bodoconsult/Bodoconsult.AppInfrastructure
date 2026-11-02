set version=1.0.11
dotnet nuget push packages\Bodoconsult.App.Abstractions.%version%.nupkg --source https://api.nuget.org/v3/index.json
dotnet nuget push packages\Bodoconsult.App.Abstractions.%version%.snupkg --source https://api.nuget.org/v3/index.json

pause