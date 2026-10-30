
REM Kopieren

robocopy %~dp0bin\Release\net8.0 \\192.168.10.122\software$\OpenSource\ /XF appSettings.json /s

pause