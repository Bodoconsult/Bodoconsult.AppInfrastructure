REM Kopieren
robocopy %~dp0bin\Release\net9.0-windows\ \\192.168.10.122\software$\BodoFileTransfer\ /s /XF appSettings.json

pause