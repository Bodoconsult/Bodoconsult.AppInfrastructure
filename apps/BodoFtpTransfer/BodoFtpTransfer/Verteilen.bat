robocopy %~dp0bin\Release\net10.0-windows10.0.17763\ \\192.168.10.122\software$\FTP_GmbH\ /s /XF AppSettings.json *.sqlite data3.json

%~dp0bin\Release\net10.0-windows10.0.17763\ \\192.168.10.122\software$\FTP_Beteiligungen\ /s /XF AppSettings.json *.sqlite data3.json

robocopy %~dp0bin\Release\net10.0-windows10.0.17763\ \\192.168.10.122\software$\FTP_BodoPrivate\ /s /XF AppSettings.json *.sqlite data3.json

robocopy %~dp0bin\Release\net10.0-windows10.0.17763\ \\192.168.10.122\software$\FTP_Bildarchiv\ /s /XF AppSettings.json *.sqlite data3.json

robocopy %~dp0bin\Release\net10.0-windows10.0.17763\ \\192.168.10.122\software$\FTP_Finanzen\ /s /XF AppSettings.json *.sqlite data3.json

robocopy %~dp0bin\Release\net10.0-windows10.0.17763\ \\192.168.10.122\software$\FTP_BodoRenewable\ /s /XF AppSettings.json *.sqlite data3.json

pause