set version=1.0.9

dotnet nuget delete Bodoconsult.App.Abstractions %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.App %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.App.ReactiveUi %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.App.Avalonia %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.App.Avalonia.ReactiveUi %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive

pause