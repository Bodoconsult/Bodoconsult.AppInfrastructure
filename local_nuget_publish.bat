set version=1.0.9

dotnet nuget push packages\Bodoconsult.App.Abstractions.%version%.nupkg --source \\BCGS03DS\Data$\Projekte\Packages
dotnet nuget push packages\Bodoconsult.App.Abstractions.%version%.snupkg --source \\BCGS03DS\Data$\Projekte\Packages

dotnet nuget push packages\Bodoconsult.App.%version%.nupkg --source \\BCGS03DS\Data$\Projekte\Packages
dotnet nuget push packages\Bodoconsult.App.%version%.snupkg --source \\BCGS03DS\Data$\Projekte\Packages

dotnet nuget push packages\Bodoconsult.App.ReactiveUi.%version%.nupkg --source \\BCGS03DS\Data$\Projekte\Packages
dotnet nuget push packages\Bodoconsult.App.ReactiveUi.%version%.snupkg --source \\BCGS03DS\Data$\Projekte\Packages

dotnet nuget push packages\Bodoconsult.App.Avalonia.%version%.nupkg --source \\BCGS03DS\Data$\Projekte\Packages
dotnet nuget push packages\Bodoconsult.App.Avalonia.%version%.snupkg --source \\BCGS03DS\Data$\Projekte\Packages

dotnet nuget push packages\Bodoconsult.App.Avalonia.ReactiveUi.%version%.nupkg --source \\BCGS03DS\Data$\Projekte\Packages
dotnet nuget push packages\Bodoconsult.App.Avalonia.ReactiveUi.%version%.snupkg --source \\BCGS03DS\Data$\Projekte\Packages

pause