robocopy %~dp0bin\Release\net8.0\ \\192.168.10.122\software$\FTP_GmbH\ /s /XF AppSettings.json *.sqlite

robocopy %~dp0bin\Release\net8.0\ \\192.168.10.122\software$\FTP_Beteiligungen\ /s /XF AppSettings.json *.sqlite

robocopy %~dp0bin\Release\net8.0\ \\192.168.10.122\software$\FTP_BodoPrivate\ /s /XF AppSettings.json *.sqlite

robocopy %~dp0bin\Release\net8.0\ \\192.168.10.122\software$\FTP_Bildarchiv\ /s /XF AppSettings.json *.sqlite

robocopy %~dp0bin\Release\net8.0\ \\192.168.10.122\software$\FTP_Finanzen\ /s /XF AppSettings.json *.sqlite

robocopy %~dp0bin\Release\net8.0\ \\192.168.10.122\software$\FTP_BodoRenewable\ /s /XF AppSettings.json *.sqlite

pause