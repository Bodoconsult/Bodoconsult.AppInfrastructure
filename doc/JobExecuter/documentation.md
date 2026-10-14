JobExecuter documentaion
======================

JobExecuter documentation

**********Date created: 14.10.2026 09:47:03


# Table of content {#H0}

>	[About JobExecuter](#H47)

>	[What is JobExecuter](#H50)

>	[Security related issues with JobExecuter](#H58)

>	[Setting up JobExecuter](#H67)

>	[JobExecuter installation](#H68)

>	[JobExecuter: create your own password to secure jobs](#H76)

>	[Overview over the available logging facilities in JobExecuter](#H83)

>	[Configuration of JobExecuter for sending SMTP mails](#H89)

>	[Setting up a JobExecuter job](#H97)

>	[What is JSON and how to edit JSON files for JobExecuter jobs](#H98)

>	[Setting up a test job](#H115)

>	[Command line parameters for JobExecuter](#H124)

>	[CJsonFileName](#H125)

>	[/p (Encrypt password)](#H131)

>	[/c (Encrypt job file)](#H134)

>	[Overview JobTask classes](#H144)

>	[Documentation JobTask classes](#H169)

>	[ActiveDirectoryJobTask](#H170)

>	[BackupBatchJobTask](#H184)

>	[CmdBatchJobTask](#H203)

>	[DhcpJobTask](#H215)

>	[DiskSizeJobTask](#H229)

>	[DnsJobTask](#H238)

>	[DocumentationJobTask](#H253)

>	[FolderBackupJobTask](#H265)

>	[FolderCleanJobTask](#H280)

>	[FolderComplianceJobTask](#H290)

>	[JobDocuJobTask](#H305)

>	[PingCheckJobTask](#H320)

>	[PsBatchJobTask](#H330)

>	[RunAppJobTask](#H340)

>	[SqlBackupAllDatabasesJobTask](#H351)

>	[SqlBackupLogSingleDatabaseJobTask](#H365)

>	[SqlBackupSingleDatabaseJobTask](#H380)

>	[SqlBatchJobTask](#H395)

>	[SqlComplianceAllDatabasesJobTask](#H408)

>	[SqlComplianceSingleDatabaseJobTask](#H423)

>	[SqlDocuAllDatabasesJobTask](#H438)

>	[SqlDocuSingleDatabaseJobTask](#H454)

>	[SqlMaintenanceJobTask](#H471)

>	[TaskSchedulerDocuJobTask](#H488)

>	[Advanced features](#H504)

>	[Using Readme.txt](#H505)

>	[Legal notes](#H519)

>	[Registered and unregistered trademarks](#H521)

>	[Licence information](#H523)


# About JobExecuter {#H47}

JobExecuter is a software by Bodoconsult EDV-Dienstleistungen GmbH.

Bodoconsult EDV-Dienstleistungen GmbH<br />Ostring 45a<br />85591 Vaterstetten<br />http://www.bodoconsult.de<br />info@bodoconsult.de


## What is JobExecuter {#H50}

JobExecuter fulfills helpful tasks especially for small Windows based IT enviroments but it is not limited on them. In such small environments often important features like database backup or file backup are missed and have to be implemented with some effort.

JobExecuter is a software to run different types of administrative jobs like database backup, file copying, running apps and others. It is used used in conjunction with Windows Task Scheduler (WTS). The WTS provides the job scheduling infrastructure. JobExecuter provides the functionality of the jobs in an supervised environment.

JobExecuter supports self-documentation: each job can be documented automatically and all such job documentations can be added to a complete documentation of all JobExecuter based jobs running on diverse server systems.

A JobExecuter job consists of one or more JobTasks. A job and its JobTasks is defined in a UTF8 encoded JSON file. The JSON file must be encrypted to a CJSON file before running the job. To run a job call JobExecuter.exe with the CJSON file with the job definition as parameter. The command line call for JobExecuter looks like:



```
JobExecuter.exe CJsonFileName
```

Or more general:



```
%PathToJobExcuterExe%\JobExecuter.exe %PathToJobJsonFile%\%JsonFileName%.cjson
```


## Security related issues with JobExecuter {#H58}

Some of the JobTasks provided by JobExecuter need credentials to be provided. In that case the passwords do not have to be provide as clear text but as encrpyted string.

JobExecuter provides the command line parameter /p to encrypt passwords. Please see below in the section on command line parameters for JobExecuter.

Run JobExecuter always under an account with minimal required permission.

To encrypt your job file run the following command line:



```
%PathToJobExcuterExe%\JobExecuter.exe /c %PathToJobJsonFile%\%JsonFileName%.cjson
```

You have to provide a password that must be equal to the one stored (encrypted) in the JobExecuter.exe.config file. See below how to encrpyt a password for JobExecuter (command line argument /j).

The result of this command line will be a file %PathToJobJsonFile%\%JsonFileName%.cjson. Run the job with the encrypted password use:



```
%PathToJobExcuterExe%\JobExecuter.exe %PathToJobJsonFile%\%JsonFileName%.cjson
```


# Setting up JobExecuter {#H67}


## JobExecuter installation {#H68}

Create the folder structure for JobExcuter via Windows Explorer starting with the TargetFolder. The TargetFolder may be any local folder on the machine running the Windows Task Scheduler to use.

TargetFolder
: Target path for JobExecuter environment

TargetFolder\JobExecuter
: Application install path

TargetFolder\Jobs
: Folder with subfolders for each job

Then copy the Samples folder in the JobExecuter&#39;s installation path to the TargetFolder\Jobs folder and renamed it to Test.

The Test folder will contain a test job not running from Windows Taslk Scheduler for building and testing new JobTasks.

Use the Test folder as template for creating new jobs.


## JobExecuter: create your own password to secure jobs {#H76}

To secure JobExecuter jobs you have to create your own salt and then to encrypt your password.

Go to JobExecuter&#39;s installation path TargetFolder\JobExecuter.

Then run Salt.bat to create a new salt. It will be copied to clipboard.

Then go edit JobExecuter.config and replace the JobSalt setting value with the just created salt from clipboard. Save JobExecuter.config.

Then run Password.bat to encrypt your password. It will be copied to clipboard.

Then go edit JobExecuter.config again and replace the JobPassword setting value with the just created password from clipboard. Save JobExecuter.config.


## Overview over the available logging facilities in JobExecuter {#H83}

JobExcuter provides the following logging facilities:

Log file
: By default JobExecuter creates always a log file in the job JSON file&#39;s directory with the the file name but the file extension .log.

SMTP summary
: JobExecuter can send a summary of the job&#39;s runtime results via SMTP to a job&#39;s SummaryMailReceiver. SMTP must be configured for JobExecuter. See section below.

SMTP errors
: JobExecuter can send error messages resulting from the job&#39;s runtime via SMTP to a job&#39;s ErrorMailReceiver or to a separate JobTask&#39;s ErrorMailReceiver. SMTP must be configured for JobExecuter. See section below.

Windows Event log
: If needed you can enable JobExecuter to log errors to the Windows Event log. You have to set the Jobs&#39; property WriteToEventLog to true. You can find the log entries under the application log there.


## Configuration of JobExecuter for sending SMTP mails {#H89}

To enable JobExecuter to send SMTP mails you need the SMTP address to use, the username and password to login to the SMTP server and a email address you want to send on behalf of.

!!!! JobExecuter uses only secure SMTP servers !!!!

To configure JobExecuter you must edit the JobExecuter.config located in the install directory of JobExecuter. Here the settings to edit:

SmtpServer:
: SMTP server address like smtp.1und1.de

SmtpAccount:
: account name to login on the SMTP server

SmtpPassword:
: Encrypted password  to login on the SMTP server (use command line parameter /p to encrypt a password. See section above!)

SmtpSendAs:
: email address you send on behalf on, i.e. noreply@test.de


# Setting up a JobExecuter job {#H97}


## What is JSON and how to edit JSON files for JobExecuter jobs {#H98}

JSON is common plain text data format to transfer or store data. It is lightweight and relatively easy understandable compared to the alternative XML. JSON makes it easily possible to store object data outside a software and load it from a file in a software. Here a sample of a JobExecuter&#39;s Job object stored a JSON:



```
&lt;&lt;Error:File not found: D:\Daten\Projekte\OwnProjects\JobExecuter\JobExecuter\Samples\SqlBackupSingleDatabaseJobTask.json&gt;&gt;
```

To edit JSON files you generally need only a good text editor. We recommend Notepad++. Copy and paste is a favourable way for creating new job files for JobExecuter.

JSON itself is formatted plain text with UTF8 encoding in case of JobExecuter. If you edit JSON files you will have to escape certain characters in the text. Please see the list of critical chararcters to escape in JSON:

\b
: Backspace (ascii code 08)

\f
: Form feed (ascii code 0C)

\n
: New line

\r
: Carriage return

\t
: Tab

\&quot;
: Double quote

\\
: Backslash character

Example: Especially if you edit directory or file paths you have to escape the backslash. Instead of the directory path



```
C:\Test\Jobs\backup\
```

you have to write



```
C:\\Test\\Jobs\\backup\\
```

in the JSON file.


## Setting up a test job {#H115}

In the JobExecuter&#39;s installation path there is a folder Samples with  an subfolder SampleJobFolder. This folder contains a sample job with all necessary steps to work with JobExecuter.

You can use this sample folder to create new jobs by copying the folder to a target folder for jobs and adjust the content of the JSON and BAT to your conrete situation.


### Folder content {#}

job.json:
: Job definition file

job.cjson:
: Encrypted job definition file

prepare.bat:
: Encrypt JSON job file to CJSON

test.bat:
: Start job for testing purposes

start.bat:
: Start job from Windows task scheduler


# Command line parameters for JobExecuter {#H124}


## CJsonFileName {#H125}



```
%PathToJobExcuterExe%\JobExecuter.exe %PathToJobJsonFile%\%JsonFileName%.cjson
```

Runs a job defined in the CJSON file %PathToJobJsonFile%\%JsonFileName%.cjson

JSON files to execute with JobExecuter should be UTF8 encoded and must be encrypted via command line parameter /c to a CJSON file. We recommend using Notepad++ as editor for JSON files. Encrypted JSON files must have the extension .cjson.

Each job should have an own folder.

If there is a readme.txt in the same folder as the JSON job file, the JobDocuJobTask documenting the job will add the content of readme.txt to the job documentation.


## /p (Encrypt password) {#H131}



```
%PathToJobExcuterExe%\JobExecuter.exe /p
```

If you run JobExecuter with this parameter you will be ask to insert a clear text password. This password will be encrypted by JobExecuter and copied to the clipboard. From there you can paste it to a JSON job definition file where a password is required in encrypted form.


## /c (Encrypt job file) {#H134}



```
%PathToJobExcuterExe%\JobExecuter.exe /c %PathToJobJsonFile%\%JsonFileName%.json
```

Encrypt a job&#39;s JSON file to a CJSON file. Password required for that transaction! See below.


### /j (setting app password for encrypting job files) {#}



```
%PathToJobExcuterExe%\JobExecuter.exe /j
```

If you run JobExecuter with this parameter you will be ask to insert a clear text password. This password will be encrypted by JobExecuter and stored internally.

This password is valid only on the machine you run this command.


### /t (synthax check for job JSON file) {#}



```
%PathToJobExcuterExe%\JobExecuter.exe /t %PathToJobJsonFile%\%JsonFileName%.json
```

Checks if the JSON synthax of the given file is valid.


# Overview JobTask classes {#H144}

ActiveDirectoryJobTask
:             Creates a report of the current Windows domain&#39;s Active Directory structure.<br />            

BackupBatchJobTask
:             Task to run a backup of the file system.  <br />            

CmdBatchJobTask
:             Task to run a cmd.exe batch from a file or a string.  <br />            

DhcpJobTask
:             Creates a report of the current Windows domain&#39;s DHCP infrastructure<br />            

DiskSizeJobTask
:             Checks the disk size and places an error if free space is below a certain percentage or below a certain absolute value<br />            

DnsJobTask
:             Creates a report for a Microsoft DNS server<br />            

DocumentationJobTask
:             Creates a HTML documentation from diverse JSON files as input.<br />            May be used i.e. for combining database compliance reports from different database servers to one compliance report HTML file.<br />            

FolderBackupJobTask
:             Backup of a folder based on robocopy.exe by Microsoft.<br />            

FolderCleanJobTask
:             Task to clean a folder from old files<br />            

FolderComplianceJobTask
:             Task to create a compliance report for a folder. <br />            

JobDocuJobTask
:             Task to document the current JobExecuter job<br />            

PingCheckJobTask
:             Check successful ping to certain network addresses<br />            

PsBatchJobTask
:             Run a PowerShell script file<br />            

RunAppJobTask
:             Run an app with or without parameters<br />            

SqlBackupAllDatabasesJobTask
:             Task to take a full backup for all SQL databases (with exception of TempDb) on a Microsoft&#174; SqlServer starting with version 2008.<br />            

SqlBackupLogSingleDatabaseJobTask
:             Task to take a log file backup for a single SQL database on Microsoft&#174; SqlServer starting with version 2008.<br />            

SqlBackupSingleDatabaseJobTask
:             Task to take a full backup for a single SQL database on Microsoft&#174; SqlServer starting with version 2008.<br />            

SqlBatchJobTask
:             Task to run a SQL batch from a file or from a string against a Microsoft&#174; SqlServer <br />            

SqlComplianceAllDatabasesJobTask
:             Task to collect compliance relevante information for all SQL databases on a Microsoft&#174; SqlServer <br />            

SqlComplianceSingleDatabaseJobTask
:             Task to collect compliance relevante information for a single SQL database on a Microsoft&#174; SqlServer <br />            

SqlDocuAllDatabasesJobTask
:             Task to take a documentation for all SQL databases on a Microsoft&#174; SqlServer with exception of databases Master, Msdb, Model and TempDb. <br />             

SqlDocuSingleDatabaseJobTask
:              Task to take a documentation for a single SQL database on a Microsoft&#174; SqlServer.<br />             

SqlMaintenanceJobTask
:             Job to perfrom maintenance on a Microsoft&#174; SqlServer instance<br />            

TaskSchedulerDocuJobTask
:             Task to save TaskScheduler tasks as XML and document it<br />            


# Documentation JobTask classes {#H169}


## ActiveDirectoryJobTask {#H170}

<br />            Creates a report of the current Windows domain&#39;s Active Directory structure.<br />            

Work only for single domain networks. Domain forest are not supported. Delivers a report over the structure of the AD with Organizational Units (OU),  users, groups and computers.


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for ActiveDirectoryJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.ActiveDirectoryJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.ActiveDirectoryJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\ActiveDirectory.htm&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Report Active Directory&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;Report the current domain&#39;s Active Directory structure with organisation units (OU), users, groups and computers.&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## BackupBatchJobTask {#H184}

<br />            Task to run a backup of the file system.  <br />            


### Properties {#}

BackupTargets
:             Current list of backup targets<br />            

Command
:             Backup command to run or null if the default should be used<br />            

Days
:             Number of days the backups are backup after date TagDatum before being deleted<br />            

DaysOffset
:             Offset in days<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

Errors
:             Current list of errors<br />            

GetAliveFolder
:             A folder path called to get the storage media alive before starting backup<br />            

Name
:             Name of the job<br />            

ShadowingSourceDrive
:             The source drive used for disk shadowing or null. I.e.: C:<br />            

ShadowingTargetDrive
:             The target drive used for disk shadowing or null. I.e.: H:<br />            

StatusChanged
:             Status changed event<br />            

SummaryFile
:             Name of the summary file to be created<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Weeks
:             Number of weeks the backups are kept before being deleted in weekly backup mode. Default: 5<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for BackupBatchJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.BackupBatchJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.BackupBatchJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ShadowingSourceDrive&quot;: &quot;C:&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ShadowingTargetDrive&quot;: &quot;H:&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Command&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Weeks&quot;: 5,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;GetAliveFolder&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Days&quot;: 14,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;DaysOffset&quot;: 0,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;BackupTargets&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;Bodoconsult.App.Backup.BackupTargetSettings, Bodoconsult.App&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Command&quot;: &quot;robocopy ??source?? ??target??  *.* /FFT /S /E /log:??log?? /COPY:DT /B /NP /MT:48 /R:1 /W:1 /XF thumbs.db /xd ??xd?? /NoDCopy&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Source&quot;: &quot;d:\\tmp\\FolderBackup1&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Target&quot;: &quot;d:\\tmp\\FolderBackup2\\&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Logfile&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\logfile.log&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ExcludeDirs&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;BackupMode&quot;: &quot;Simple&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Thread&quot;: 1,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Count&quot;: 5,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;DaysOffset&quot;: 0,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Errors&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;StatusChanged&quot;: null<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;SummaryFile&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Errors&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;StatusChanged&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Backup file system&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;Take a backup of a filesystem folder using diskshadowing if needed and keeping old backups for a certain amount of time depending on the backup mode&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## CmdBatchJobTask {#H203}

<br />            Task to run a cmd.exe batch from a file or a string.  <br />            

Set either CmdBatchJobTask.Filename or CmdBatchJobTask.BatchFileContent. In doubt CmdBatchJobTask.BatchFileContent wins!<br />            The advantage of running a batch in mode CmdBatchRunType.LineByLine that the batch runs line per line as separated processes. <br />            The lines must be independent from each other.<br />            


### Properties {#}

BatchFileContent
:             Batch commands as string (like in a .bat or .cmd file). Commands in the string run always in mode CmdBatchRunType.LineByLine.  Set either CmdBatchJobTask.Filename or CmdBatchJobTask.BatchFileContent. In doubt CmdBatchJobTask.BatchFileContent wins!<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

Filename
:             Filename of .bat or .cmd file to run. Set either CmdBatchJobTask.Filename or CmdBatchJobTask.BatchFileContent. In doubt CmdBatchJobTask.BatchFileContent wins!<br />            

Name
:             Name of the job<br />            

RunType
:             Type of how to run a cmd batch file<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for CmdBatchJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.CmdBatchJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.CmdBatchJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Filename&quot;: &quot;testbackup.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;RunType&quot;: &quot;WholeFile&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;BatchFileContent&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Run a batch (*.bat or *.cmd) with cmd.exe&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 110<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.CmdBatchJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Filename&quot;: &quot;testbackup.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;RunType&quot;: &quot;LineByLine&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;BatchFileContent&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Run a batch (*.bat or *.cmd) with cmd.exe&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 110<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.CmdBatchJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Filename&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;RunType&quot;: &quot;LineByLine&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;BatchFileContent&quot;: &quot;REm dfgsfd\r\ncopy d:\\tmp\\FolderBackup1\\*.* d:\\tmp\\FolderBackup2\\*.* /y\r\nREM fghdfg\r\ncopy d:\\tmp\\FolderBackup2\\*.* d:\\tmp\\FolderBackup1\\*.* /y\r\nREM fghfsd\r\n&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Run a batch commands from a string&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 110<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## DhcpJobTask {#H215}

<br />            Creates a report of the current Windows domain&#39;s DHCP infrastructure<br />            

User running that job needs to be member of security group DHCP Administrators on the DHCP server


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for DhcpJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.DhcpJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.DhcpJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\DHCP.htm&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Report Dhcp infrastructure&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;Report the current domain&#39;s DHCP infrastructure&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## DiskSizeJobTask {#H229}

<br />            Checks the disk size and places an error if free space is below a certain percentage or below a certain absolute value<br />            


### Properties {#}

Description
:             Description of the job used for documentation<br />            

Drives
:             Drives to check for disk space<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

Name
:             Name of the job<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for DiskSizeJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.DiskSizeJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.DiskSizeJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Drives&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Drive, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;C:&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MinimumPercentageOfFreeSpace&quot;: 0.3,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;IpAddress&quot;: null<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Drive, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;\\\\bododata\\data$&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MinimumPercentageOfFreeSpace&quot;: 0.7,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;IpAddress&quot;: null<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Drive, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;C:&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MinimumPercentageOfFreeSpace&quot;: 0.3,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;IpAddress&quot;: &quot;192.168.12.219&quot;<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Drive, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;C:&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MinimumPercentageOfFreeSpace&quot;: 0.3,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;IpAddress&quot;: &quot;192.168.12.15&quot;<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample for DiskSizeJobTask job task&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## DnsJobTask {#H238}

<br />            Creates a report for a Microsoft DNS server<br />            

User running that job needs to be member of security group DNS administrators on the DNS server


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

Server
:             FQDN of the DNS server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for DnsJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.DnsJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.DnsJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;BCGS02DC.BCG-AD.DE&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\DNS.htm&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Report a DNS server&#39;s structure&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;Report DNS server&#39;s structure&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## DocumentationJobTask {#H253}

<br />            Creates a HTML documentation from diverse JSON files as input.<br />            May be used i.e. for combining database compliance reports from different database servers to one compliance report HTML file.<br />            


### Properties {#}

AppendHelpFile
:             Append the JobExecuter help file to documentation<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

HtmlTargetFiles
:             Filenames for HTML documentation as result of the job<br />            

JsonFiles
:             message to deliver for testing purposes<br />            

Name
:             Name of the job<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for DocumentationJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.DocumentationJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.DocumentationJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonFiles&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;d_:\\tmp\\test.jsonSample.json&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;d:\\tmp\\test.jsonSample.json&quot;<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFiles&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;d:\\tmp\\FolderBackup2\\documentation.htm&quot;<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: &quot;Full documentation&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;AppendHelpFile&quot;: false,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Full documentation&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## FolderBackupJobTask {#H265}

<br />            Backup of a folder based on robocopy.exe by Microsoft.<br />            

Robocopy.exe is installed automatically starting with Windows 7 and Windows 2008 Server.


### Properties {#}

BackupType
:             Defines how backup works. Default is FolderBackupType.PlainCopy.<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

ExcludeDirectories
:             Directories to exclude from backup<br />            Syntax: C:\ToExclude1 &quot;C:\To Exclude2&quot; c:\ToExclude3<br />            

IncludeSubDirectories
:             Include subdirectories in the folder backup. Default: true<br />            

Name
:             Name of the job<br />            

ShowProgress
:             Show progress of backup. May reduce performance. Default: false<br />            

SourcePath
:             Folder to backup<br />            

TargetPath
:             Target folder for the backup<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for FolderBackupJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.FolderBackupJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.FolderBackupJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;SourcePath&quot;: &quot;d:\\tmp\\FolderBackup1&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;TargetPath&quot;: &quot;d:\\tmp\\FolderBackup2&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ExcludeDirectories&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;BackupType&quot;: &quot;CopyOnlyNewerFiles&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ShowProgress&quot;: false,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;IncludeSubDirectories&quot;: true,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Backup a folder to a local or network share target&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## FolderCleanJobTask {#H280}

<br />            Task to clean a folder from old files<br />            


### Properties {#}

Days
:             Clean folder from files older then X days<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

FolderPath
:             Path to clean<br />            

Name
:             Name of the job<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for FolderCleanJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.FolderCleanJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.FolderCleanJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FolderPath&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Days&quot;: 7,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Clean a folder from files older than 7 days&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## FolderComplianceJobTask {#H290}

<br />            Task to create a compliance report for a folder. <br />            

FolderComplianceJobTask requires running under administrative privileges due to accessing file system access control list.


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

FolderPaths
:             Path to create compiance reporting for<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for FolderComplianceJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.FolderComplianceJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.FolderComplianceJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FolderPaths&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.ComplianceFolder, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Path&quot;: &quot;c:\\daten\\projekte&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;IncludeSubFolders&quot;: true,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ShowPermissionsOfPredefinedAccounts&quot;: true<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: &quot;test@test123.de&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\Compliance2.htm&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Create a compliance report for file system folders&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## JobDocuJobTask {#H305}

<br />            Task to document the current JobExecuter job<br />            


### Properties {#}

CurrentJob
:             Current job (will be used if JobDocuJobTask.JobJsonFileName is empty.<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JobJsonFileName
:             Full path of the CJSON file with JobExecuter job to document<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for JobDocuJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.JobDocuJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.JobDocuJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JobJsonFileName&quot;: &quot;&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: &quot;&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Document the current JobExecuter job&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;If JobJsonFileName and DocuJsonFileName left empty, JobExecuter will insert current job file path and as docu file name the current job file path with the suffix _docu.json&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.JobDocuJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JobJsonFileName&quot;: &quot;d:\\tmp\\FolderBackup2\\job.json&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: &quot;d:\\tmp\\FolderBackup2\\job.json&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Document the a JobExecuter job&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;Set JobJsonFileName and DocuJsonFileName to a spezific path if needed&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## PingCheckJobTask {#H320}

<br />            Check successful ping to certain network addresses<br />            


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

IpAdresses
:             IP-addresses separated by semicolon<br />            

Name
:             Name of the job<br />            

PingTimeout
:             Timeout in milliseconds for Ping. Default: 15.000 <br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for PingCheckJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.PingCheckJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.PingCheckJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;IpAdresses&quot;: &quot;192.168.12.15;192.168.12.20;192.168.12.1;8.8.8.8&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PingTimeout&quot;: 15000,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample task for ping check&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## PsBatchJobTask {#H330}

<br />            Run a PowerShell script file<br />            


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

Filename
:             Full path to a powershell script file to run. Path may contain blanks<br />            

Name
:             Name of the job<br />            

PowerShellVersion
:             The version of PowerShell to run the Model.PsBatchJobTask on<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for PsBatchJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.PsBatchJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.PsBatchJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Filename&quot;: &quot;PowerShellScript.ps1.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PowerShellVersion&quot;: &quot;V01&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Running a PowerShell batch file with PowerShell V01&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.PsBatchJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Filename&quot;: &quot;PowerShellScript.ps1.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PowerShellVersion&quot;: &quot;V03&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Running a PowerShell batch file with PowerShell V03&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## RunAppJobTask {#H340}

<br />            Run an app with or without parameters<br />            


### Properties {#}

Arguments
:             Arguments to start the app with<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

Filename
:             Full path to app to run. Path may contain blanks<br />            

InteractiveMode
:             Run app in interactive mode. default: false<br />            

Name
:             Name of the job<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for RunAppJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.RunAppJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.RunAppJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Filename&quot;: &quot;robocopy.exe&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Arguments&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;InteractiveMode&quot;: false,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample for running a app without parmeters&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.RunAppJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Filename&quot;: &quot;robocopy.exe&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Arguments&quot;: &quot;/?&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;InteractiveMode&quot;: false,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample for running a app with parameters&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## SqlBackupAllDatabasesJobTask {#H351}

<br />            Task to take a full backup for all SQL databases (with exception of TempDb) on a Microsoft&#174; SqlServer starting with version 2008.<br />            

Uses the following SQl statement: BACKUP DATABASE [{0}] TO DISK = N&#39;JobExecuter documentation&#39; WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N&#39;{0} full backup&#39;


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

FileName
:             File name schema for the backup file. ;ust contain placeholders. <br />            Otherwise all backups are made to the same file (and overwrite it with each backup)<br />            Must contain placeholders:<br />            ??server?? = Server name or Server and instance name<br />            ??database?? = database name<br />            Optional placeholders are:<br />            ??date?? = date in format yyyymmdd<br />            ??time?? = date and time in format yyyymmddhhnn<br />            

LocalTarget
:             Local target folder to backup database in<br />            

Name
:             Name of the job<br />            

NetworkTarget
:             Target folder for the backup on a network share (if requested).<br />            

RemoveLocalBackup
:             Remove local backup files after copying to network. Default: false<br />            

Server
:             server name or server and instance name of a database server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for SqlBackupAllDatabasesJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlBackupAllDatabasesJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlBackupAllDatabasesJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;LocalTarget&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;NetworkTarget&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FileName&quot;: &quot;??server??_??database??.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;RemoveLocalBackup&quot;: false,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Local backup of all databases on a SqlServer instance&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlBackupAllDatabasesJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;LocalTarget&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;NetworkTarget&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\network&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FileName&quot;: &quot;testbackup.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;RemoveLocalBackup&quot;: false,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Network share backup of all databases on a SqlServer instance&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## SqlBackupLogSingleDatabaseJobTask {#H365}

<br />            Task to take a log file backup for a single SQL database on Microsoft&#174; SqlServer starting with version 2008.<br />            

Uses the following SQl statement: BACKUP LOG [{0}] TO DISK = N&#39;JobExecuter documentation&#39; WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N&#39;{0} full backup&#39;


### Properties {#}

DatabaseName
:             Name of the database to backup<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

FileName
:             File name for the backup file<br />            May contain placeholders:<br />            ??server?? = Server name or Server and instance name<br />            ??database?? = database name<br />            ??date?? = date in format yyyymmdd<br />            ??time?? = date and time in format yyyymmddhhnn<br />            

LocalTarget
:             Local target folder to backup database in<br />            

Name
:             Name of the job<br />            

NetworkTarget
:             Target folder for the backup on a network share (if requested).<br />            

RemoveLocalBackup
:             Remove local backup files after copying to network. Default: false<br />            

Server
:             server name or server and instance name of a database server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for SqlBackupLogSingleDatabaseJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlBackupLogSingleDatabaseJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlBackupLogSingleDatabaseJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;DatabaseName&quot;: &quot;MediaDb&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;LocalTarget&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;NetworkTarget&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\network&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FileName&quot;: &quot;testbackup.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;RemoveLocalBackup&quot;: true,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Backup database to a network path&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## SqlBackupSingleDatabaseJobTask {#H380}

<br />            Task to take a full backup for a single SQL database on Microsoft&#174; SqlServer starting with version 2008.<br />            

Uses the following SQl statement: BACKUP DATABASE [{0}] TO DISK = N&#39;JobExecuter documentation&#39; WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N&#39;{0} full backup&#39;


### Properties {#}

DatabaseName
:             Name of the database to backup<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

FileName
:             File name for the backup file<br />            May contain placeholders:<br />            ??server?? = Server name or Server and instance name<br />            ??database?? = database name<br />            ??date?? = date in format yyyymmdd<br />            ??time?? = date and time in format yyyymmddhhnn<br />            

LocalTarget
:             Local target folder to backup database in<br />            

Name
:             Name of the job<br />            

NetworkTarget
:             Target folder for the backup on a network share (if requested).<br />            

RemoveLocalBackup
:             Remove local backup files after copying to network. Default: false<br />            

Server
:             server name or server and instance name of a database server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for SqlBackupSingleDatabaseJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlBackupSingleDatabaseJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlBackupSingleDatabaseJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;DatabaseName&quot;: &quot;MediaDb&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;LocalTarget&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;NetworkTarget&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FileName&quot;: &quot;testbackup.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;RemoveLocalBackup&quot;: false,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Backup database to local file only&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlBackupSingleDatabaseJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;DatabaseName&quot;: &quot;MediaDb&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;LocalTarget&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;NetworkTarget&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\network&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FileName&quot;: &quot;testbackup.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;RemoveLocalBackup&quot;: true,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Backup database to a network path&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## SqlBatchJobTask {#H395}

<br />            Task to run a SQL batch from a file or from a string against a Microsoft&#174; SqlServer <br />            

SQL statements must be separated by GO. The SQL statements must address database correctly, i.e EXEC testdb.dbo.TestProc.<br />            All SQL statements are running on the same connection. This makes it possible to use temporary tables in SQL statements.


### Properties {#}

BatchContent
:             Direct batch SQL to run<br />            

DatabaseName
:             Name of the database to run the batch file in<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

FileName
:             Path to the SQL batch file to run<br />            

Name
:             Name of the job<br />            

Server
:             server name or server and instance name of a database server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for SqlBatchJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlBatchJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlBatchJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;DatabaseName&quot;: &quot;MediaDb&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FileName&quot;: &quot;testbackup.bak&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;BatchContent&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Run a SQL batch file&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlBatchJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;DatabaseName&quot;: &quot;MediaDb&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;BatchContent&quot;: &quot;exec spTest 1\r\nGO\r\nexec spTest 2\r\nGO\r\nexec spTest 3&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Run SQL batch directly from string&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## SqlComplianceAllDatabasesJobTask {#H408}

<br />            Task to collect compliance relevante information for all SQL databases on a Microsoft&#174; SqlServer <br />            

Databases Master, Msdb, Model and TempDb are excluded by this task. Requires the job running user to be member of the sysadmin or securityadmin server roles.


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

Server
:             server name or server and instance name of a database server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for SqlComplianceAllDatabasesJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlComplianceAllDatabasesJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlComplianceAllDatabasesJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: &quot;info@bodoconsult.de&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\ServerXY_DatabaseComplianceReport.htm&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Create compliance information for all databases on a SqlServer with exception of databases Master, Msdb, Model and TempDb&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## SqlComplianceSingleDatabaseJobTask {#H423}

<br />            Task to collect compliance relevante information for a single SQL database on a Microsoft&#174; SqlServer <br />            


### Properties {#}

DatabaseName
:             Name of the database to backup<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

Server
:             server name or server and instance name of a database server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for SqlComplianceSingleDatabaseJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlComplianceSingleDatabaseJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlComplianceSingleDatabaseJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;DatabaseName&quot;: &quot;DatabaseXY&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: &quot;info@bodoconsult.de&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\ServerXY_DatabaseXY_ComplianceReport.htm&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Create compliance information for a single database&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## SqlDocuAllDatabasesJobTask {#H438}

<br />            Task to take a documentation for all SQL databases on a Microsoft&#174; SqlServer with exception of databases Master, Msdb, Model and TempDb. <br />             

A database may contain stored procs under the schema dbo and names beginning with spDbDocu_ for adding database specific content to the docmentation.

For parts of the documentation the user running the job needs certain permissions. To give least possible permissions, this user should  be part of a user defined server role on the SqlServer with the permissions VIEW ANY DEFINITION and GRANT VIEW SERVER STATE. You should avoid making the user a sysadmin for security reasons. User must have access to each database with least possible permissions!



```
<br />             CREATE SERVER ROLE NameOfNewServerRole<br />            <br />            GO<br />            <br />            GRANT VIEW ANY DEFINITION TO NameOfNewServerRole<br />            <br />            GO<br />            <br />            GRANT VIEW SERVER STATE TO NameOfNewServerRole<br />            <br />            GO<br />            <br />            EXEC sp_addsrvrolemember &#39;domain\SG-SqlKeyUsers&#39;, &#39;NameOfNewServerRole&#39;<br />            <br />             
```


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

Server
:             server name or server and instance name of a database server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for SqlDocuAllDatabasesJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlDocuAllDatabasesJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlDocuAllDatabasesJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\testbackup.bak.htm&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\testbackup.bak.json&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Create a documentation for all databases on a SqlServer with exception of databases Master, Msdb, Model and TempDb&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## SqlDocuSingleDatabaseJobTask {#H454}

<br />             Task to take a documentation for a single SQL database on a Microsoft&#174; SqlServer.<br />             

A database may contain stored procs under the schema dbo and names beginning with spDbDocu_ foradding database specific content to the docmentation.

For parts of the documentation the user running the job needs certain permissions.<br />             To give least possible permissions, this user should be part of a user defined server role on the SqlServer with the permissions VIEW ANY DEFINITION and GRANT VIEW SERVER STATE.<br />             You should avoid making the user a sysadmin for security reasons. User must have access to each database with least possible permissions!



```
<br />             CREATE SERVER ROLE NameOfNewServerRole<br />            <br />            GO<br />            <br />            GRANT VIEW ANY DEFINITION TO NameOfNewServerRole<br />            <br />            GO<br />            <br />            GRANT VIEW SERVER STATE TO NameOfNewServerRole<br />            <br />            GO<br />            <br />            EXEC sp_addsrvrolemember &#39;domain\SG-SqlKeyUsers&#39;, &#39;NameOfNewServerRole&#39;<br />            <br />             
```


### Properties {#}

DatabaseName
:             Name of the database to backup<br />            

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

Server
:             server name or server and instance name of a database server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for SqlDocuSingleDatabaseJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlDocuSingleDatabaseJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlDocuSingleDatabaseJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;DatabaseName&quot;: &quot;MediaDb&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\database_docu.htm&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\database_docu.json&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Create a documentation for a single database&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 60<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## SqlMaintenanceJobTask {#H471}

<br />            Job to perfrom maintenance on a Microsoft&#174; SqlServer instance<br />            

Needs sysadmin permissions to run successfully on the instance of SqlServer


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

FragmentationLevel1
:             Fragmentation level 1 (low -&amp;gt;medium) in percent. 50 means 50 percent. Default: 5<br />            

FragmentationLevel2
:             Fragmentation level 2 (medium -&amp;gt; high) in percent. 50 means 50 percent. Default: 30<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to PDF<br />            

Server
:             server name or server and instance name of a database server<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for SqlMaintenanceJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlMaintenanceJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.SqlMaintenanceJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: &quot;test@test123.de&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\Maintenance.htm&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FragmentationLevel1&quot;: 5,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;FragmentationLevel2&quot;: 30,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Server&quot;: &quot;.\\sqlexpress&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Runs a maintenance job on a SqlServer instance&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 600<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


## TaskSchedulerDocuJobTask {#H488}

<br />            Task to save TaskScheduler tasks as XML and document it<br />            


### Properties {#}

Description
:             Description of the job used for documentation<br />            

ErrorMailReceiver
:             Mail addresses separarted with semicolon to send an error mail to<br />            

Filter
:             Filters task by the filter expression added here. Seperated values with semicolon. Task name has to start with filter expression<br />            

HtmlTargetFileName
:             Export documentation to HTML<br />            

JsonTargetFileName
:             Export documentation as Model.StructuredText object to JSON.<br />            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot; <br />            

MailTo
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner<br />            

Name
:             Name of the job<br />            

PdfTargetFileName
:             Export documentation to Pdf<br />            

TargetServer
:             Target server to access for windows task scheduler. Default: null = localhost<br />            

Timeout
:             Timeout in seconds the job will be terminated after start<br />            

Title
:             Title for documentation<br />            

XmlTargetPath
:             Folder to export the tasks in<br />            



```
{<br />&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.Job, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Sample job for TaskSchedulerDocuJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.TaskSchedulerDocuJobTask&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: &quot;errorreceiver1@test.xxx;errorreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;SummaryMailReceiver&quot;: &quot;summaryreceiver1@test.xxx;summaryreceiver2@test.xxx&quot;,<br />&nbsp;&nbsp;&nbsp;&quot;JobTasks&quot;: [<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.TaskSchedulerDocuJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;XmlTargetPath&quot;: &quot;d:\\tmp\\FolderBackup2&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Filter&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;TargetServer&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;d:\\tmp\\FolderBackup2\\docu.html&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: &quot;d:\\tmp\\FolderBackup2\\docu.json&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Getting all task scheduler tasks from local computer&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;},<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;$type&quot;: &quot;JobExecuter.Dal.Model.TaskSchedulerDocuJobTask, JobExecuter.Dal&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;XmlTargetPath&quot;: &quot;d:\\tmp\\FolderBackup2&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Filter&quot;: &quot;Adobe;Microsoft&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;TargetServer&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Title&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;MailTo&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;HtmlTargetFileName&quot;: &quot;d:\\tmp\\FolderBackup2\\docu.html&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;PdfTargetFileName&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;JsonTargetFileName&quot;: &quot;d:\\tmp\\FolderBackup2\\docu.json&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Name&quot;: &quot;Getting task scheduler data from local computer with use of filter expressions&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Description&quot;: &quot;Filters only tasks starting with Adobe or Microsoft&quot;,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;ErrorMailReceiver&quot;: null,<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 30<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />&nbsp;&nbsp;&nbsp;],<br />&nbsp;&nbsp;&nbsp;&quot;Timeout&quot;: 500,<br />&nbsp;&nbsp;&nbsp;&quot;WriteToEventLog&quot;: true<br />}
```


# Advanced features {#H504}


## Using Readme.txt {#H505}

If in the directory of a JobExecuter job - there where the JSON file of job is located - is Readme.txt placed, this file will be included in the job&#39;s documentation.


### Tags usable in the Readme.txt {#}

In such a Readme.txt you can use the following tags to format the readme.txt if it is processed to the job documentation. The following tags are interpreted:

H1
: Header level 1

H2
: Header level 2

H3
: Header level 3

DL
: Definition list (must be followed by a DD tag)

LI
: List items

All tags will be finished at the next carriage return+line feed (char(13)+char(10)).


### Comments in Readme.txt {#}

Comments made of asterikses (*) will be removed when formatting Readme.txt to plain text or HTML


### Sample {#}

The HTML result of processing such a Readme.txt you can find in the file Readme.htm


# Legal notes {#H519}

Date this documentation was created: 14.10.2026 09:47


## Registered and unregistered trademarks {#H521}

All other registered and unregistered trademarks are the property of their respective owners.


## Licence information {#H523}

&nbsp;


### Licence for Newtonsoft.Json {#}

&nbsp;

Used in JobExecuter to provide JSON support

&nbsp;

URL: https://www.newtonsoft.com/json

&nbsp;

MIT License

MIT Copyright (c) 2003-2010 David Hall

&nbsp;

Permission is hereby granted, free of charge, to any person obtaining a copy of this

software and associated documentation files (the &quot;Software&quot;), to deal in the Software

without restriction, including without limitation the rights to use, copy, modify, merge,

publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons

to whom the Software is furnished to do so, subject to the following conditions:

&nbsp;

The above copyright notice and this permission notice shall be included in all copies or

substantial portions of the Software.

&nbsp;

THE SOFTWARE IS PROVIDED &quot;AS IS&quot;, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,

INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR

PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE

FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,

ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

&nbsp;

&nbsp;


### Licence for dahall/TaskScheduler {#}

&nbsp;

Used in JobExecuter to provide TaskScheduler documentation

&nbsp;

URL: https://github.com/dahall/TaskScheduler

&nbsp;

MIT License

MIT Copyright (c) 2003-2010 David Hall

&nbsp;

Permission is hereby granted, free of charge, to any person obtaining a copy of this

software and associated documentation files (the &quot;Software&quot;), to deal in the Software

without restriction, including without limitation the rights to use, copy, modify, merge,

publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons

to whom the Software is furnished to do so, subject to the following conditions:

&nbsp;

The above copyright notice and this permission notice shall be included in all copies

or substantial portions of the Software.

&nbsp;

THE SOFTWARE IS PROVIDED &quot;AS IS&quot;, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,

INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE

AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,

DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,

OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

&nbsp;

&nbsp;


### Licence for garysharp/DhcpServerApi {#}

&nbsp;

Used in JobExecuter to provide DHCP documentation

&nbsp;

URL: https://github.com/garysharp/DhcpServerApi/

&nbsp;

MIT License

Copyright (c) 2015 Gary Sharp

&nbsp;

Permission is hereby granted, free of charge, to any person obtaining a copy

of this software and associated documentation files (the &quot;Software&quot;), to deal

in the Software without restriction, including without limitation the rights

to use, copy, modify, merge, publish, distribute, sublicense, and/or sell

copies of the Software, and to permit persons to whom the Software is

furnished to do so, subject to the following conditions:

&nbsp;

The above copyright notice and this permission notice shall be included in

all copies or substantial portions of the Software.

&nbsp;

THE SOFTWARE IS PROVIDED &quot;AS IS&quot;, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR

IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,

FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE

AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER

LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,

OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN

THE SOFTWARE.

&nbsp;


### Licence for olahallengren/sql-server-maintenance-solution {#}

&nbsp;

Used in JobExecuter to provide Microsoft (R) SqlServer maintenance

&nbsp;

URL: https://github.com/olahallengren/sql-server-maintenance-solution

&nbsp;

MIT License

Copyright (c) 2019 Ola Hallengren

&nbsp;

Permission is hereby granted, free of charge, to any person obtaining a copy

of this software and associated documentation files (the &quot;Software&quot;), to deal

in the Software without restriction, including without limitation the rights

to use, copy, modify, merge, publish, distribute, sublicense, and/or sell

copies of the Software, and to permit persons to whom the Software is

furnished to do so, subject to the following conditions:

&nbsp;

The above copyright notice and this permission notice shall be included in all

copies or substantial portions of the Software.

&nbsp;

THE SOFTWARE IS PROVIDED &quot;AS IS&quot;, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR

IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,

FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE

AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER

LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,

OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE

SOFTWARE.

&nbsp;

&nbsp;


### Licence for Google prettify {#}

&nbsp;

Used in JobExecuter to create HTML pages

&nbsp;

URL: https://github.com/google/code-prettify

&nbsp;

@license

Copyright (C) 2015 Google Inc.

&nbsp;

Licensed under the Apache License, Version 2.0 (the &quot;License&quot;);

you may not use this file except in compliance with the License.

You may obtain a copy of the License at

&nbsp;

http://www.apache.org/licenses/LICENSE-2.0

&nbsp;

Unless required by applicable law or agreed to in writing, software

distributed under the License is distributed on an &quot;AS IS&quot; BASIS,

WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

See the License for the specific language governing permissions and

limitations under the License.

&nbsp;

&nbsp;

&nbsp;




# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.