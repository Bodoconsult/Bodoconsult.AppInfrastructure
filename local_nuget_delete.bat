set version=1.0.10

del C:\Users\rleisner\.nuget\packages\bodoconsult.app\%version% /S /Q
del C:\Users\rleisner\.nuget\packages\bodoconsult.app.abstractions\%version% /S /Q
del C:\Users\rleisner\.nuget\packages\bodoconsult.app.reactiveui\%version% /S /Q
del C:\Users\rleisner\.nuget\packages\bodoconsult.app.avalonia\%version% /S /Q
del C:\Users\rleisner\.nuget\packages\bodoconsult.app.avalonia.reactiveui\%version% /S /Q
del C:\Users\rleisner\.nuget\packages\bodoconsult.app.backgroundservice\%version% /S /Q
del C:\Users\rleisner\.nuget\packages\bodoconsult.i18n\%version% /S /Q
del C:\Users\rleisner\.nuget\packages\bodoconsult.web.html\%version% /S /Q
del C:\Users\rleisner\.nuget\packages\bodoconsult.web.mail\%version% /S /Q

dotnet nuget delete Bodoconsult.App.Abstractions %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.App %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.App.ReactiveUi %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.App.Avalonia %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.App.Avalonia.ReactiveUi %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.App.BackgroundService %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.I18N %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.Web.Html %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
dotnet nuget delete Bodoconsult.Web.Mail %version% --source \\BCGS03DS\Data$\Projekte\Packages --non-interactive
pause