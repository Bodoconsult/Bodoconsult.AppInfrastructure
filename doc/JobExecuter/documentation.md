JobExecuter documentation
======================

JobExecuter documentation

**********Date created: 14.10.2026 21:10:32


# Table of content

>	[**About JobExecuter**](#about-jobexecuter)

>>	[What is JobExecuter](#what-is-jobexecuter)

>>	[Security related issues with JobExecuter](#security-related-issues-with-jobexecuter)

>	[**Setting up JobExecuter**](#setting-up-jobexecuter)

>>	[JobExecuter installation](#jobexecuter-installation)

>>	[JobExecuter: create your own password to secure jobs](#jobexecuter-create-your-own-password-to-secure-jobs)

>>	[Overview over the available logging facilities in JobExecuter](#overview-over-the-available-logging-facilities-in-jobexecuter)

>>	[Configuration of JobExecuter for sending SMTP mails](#configuration-of-jobexecuter-for-sending-smtp-mails)

>	[**Setting up a JobExecuter job**](#setting-up-a-jobexecuter-job)

>>	[What is JSON and how to edit JSON files for JobExecuter jobs](#what-is-json-and-how-to-edit-json-files-for-jobexecuter-jobs)

>>	[Setting up a test job](#setting-up-a-test-job)

>	[**Command line parameters for JobExecuter**](#command-line-parameters-for-jobexecuter)

>>	[CJsonFileName](#cjsonfilename)

>>	[/p (Encrypt password)](#p-encrypt-password)

>>	[/c (Encrypt job file)](#c-encrypt-job-file)

>	[**Overview JobTask classes**](#overview-jobtask-classes)

>	[**Documentation JobTask classes**](#documentation-jobtask-classes)

>>	[ActiveDirectoryJobTask](#activedirectoryjobtask)

>>	[BackupBatchJobTask](#backupbatchjobtask)

>>	[CmdBatchJobTask](#cmdbatchjobtask)

>>	[DhcpJobTask](#dhcpjobtask)

>>	[DiskSizeJobTask](#disksizejobtask)

>>	[DnsJobTask](#dnsjobtask)

>>	[DocumentationJobTask](#documentationjobtask)

>>	[FolderBackupJobTask](#folderbackupjobtask)

>>	[FolderCleanJobTask](#foldercleanjobtask)

>>	[FolderComplianceJobTask](#foldercompliancejobtask)

>>	[JobDocuJobTask](#jobdocujobtask)

>>	[PingCheckJobTask](#pingcheckjobtask)

>>	[PsBatchJobTask](#psbatchjobtask)

>>	[RunAppJobTask](#runappjobtask)

>>	[SqlBackupAllDatabasesJobTask](#sqlbackupalldatabasesjobtask)

>>	[SqlBackupLogSingleDatabaseJobTask](#sqlbackuplogsingledatabasejobtask)

>>	[SqlBackupSingleDatabaseJobTask](#sqlbackupsingledatabasejobtask)

>>	[SqlBatchJobTask](#sqlbatchjobtask)

>>	[SqlComplianceAllDatabasesJobTask](#sqlcompliancealldatabasesjobtask)

>>	[SqlComplianceSingleDatabaseJobTask](#sqlcompliancesingledatabasejobtask)

>>	[SqlDocuAllDatabasesJobTask](#sqldocualldatabasesjobtask)

>>	[SqlDocuSingleDatabaseJobTask](#sqldocusingledatabasejobtask)

>>	[SqlMaintenanceJobTask](#sqlmaintenancejobtask)

>>	[TaskSchedulerDocuJobTask](#taskschedulerdocujobtask)

>	[**Advanced features**](#advanced-features)

>>	[Using Readme.txt](#using-readmetxt)

>	[**Legal notes**](#legal-notes)

>>	[Registered and unregistered trademarks](#registered-and-unregistered-trademarks)

>>	[Licence information](#licence-information)


# About JobExecuter

JobExecuter is a software by Bodoconsult EDV-Dienstleistungen GmbH.

Bodoconsult EDV-Dienstleistungen GmbH<br />Ostring 45a<br />85591 Vaterstetten<br />http://www.bodoconsult.de<br />info@bodoconsult.de


## What is JobExecuter

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


## Security related issues with JobExecuter

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


# Setting up JobExecuter


## JobExecuter installation

Create the folder structure for JobExcuter via Windows Explorer starting with the TargetFolder. The TargetFolder may be any local folder on the machine running the Windows Task Scheduler to use.

**TargetFolder**
: Target path for JobExecuter environment

**TargetFolder\JobExecuter**
: Application install path

**TargetFolder\Jobs**
: Folder with subfolders for each job

Then copy the Samples folder in the JobExecuter&#39;s installation path to the TargetFolder\Jobs folder and renamed it to Test.

The Test folder will contain a test job not running from Windows Taslk Scheduler for building and testing new JobTasks.

Use the Test folder as template for creating new jobs.


## JobExecuter: create your own password to secure jobs

To secure JobExecuter jobs you have to create your own salt and then to encrypt your password.

Go to JobExecuter&#39;s installation path TargetFolder\JobExecuter.

Then run Salt.bat to create a new salt. It will be copied to clipboard.

Then go edit JobExecuter.config and replace the JobSalt setting value with the just created salt from clipboard. Save JobExecuter.config.

Then run Password.bat to encrypt your password. It will be copied to clipboard.

Then go edit JobExecuter.config again and replace the JobPassword setting value with the just created password from clipboard. Save JobExecuter.config.


## Overview over the available logging facilities in JobExecuter

JobExcuter provides the following logging facilities:

**Log file**
: By default JobExecuter creates always a log file in the job JSON file&#39;s directory with the the file name but the file extension .log.

**SMTP summary**
: JobExecuter can send a summary of the job&#39;s runtime results via SMTP to a job&#39;s SummaryMailReceiver. SMTP must be configured for JobExecuter. See section below.

**SMTP errors**
: JobExecuter can send error messages resulting from the job&#39;s runtime via SMTP to a job&#39;s ErrorMailReceiver or to a separate JobTask&#39;s ErrorMailReceiver. SMTP must be configured for JobExecuter. See section below.

**Windows Event log**
: If needed you can enable JobExecuter to log errors to the Windows Event log. You have to set the Jobs&#39; property WriteToEventLog to true. You can find the log entries under the application log there.


## Configuration of JobExecuter for sending SMTP mails

To enable JobExecuter to send SMTP mails you need the SMTP address to use, the username and password to login to the SMTP server and a email address you want to send on behalf of.

!!!! JobExecuter uses only secure SMTP servers !!!!

To configure JobExecuter you must edit the JobExecuter.config located in the install directory of JobExecuter. Here the settings to edit:

**SmtpServer:**
: SMTP server address like smtp.1und1.de

**SmtpAccount:**
: account name to login on the SMTP server

**SmtpPassword:**
: Encrypted password  to login on the SMTP server (use command line parameter /p to encrypt a password. See section above!)

**SmtpSendAs:**
: email address you send on behalf on, i.e. noreply@test.de


# Setting up a JobExecuter job


## What is JSON and how to edit JSON files for JobExecuter jobs

JSON is common plain text data format to transfer or store data. It is lightweight and relatively easy understandable compared to the alternative XML. JSON makes it easily possible to store object data outside a software and load it from a file in a software. Here a sample of a JobExecuter&#39;s Job object stored a JSON:



```
<<Error:File not found: D:\Daten\Projekte\OwnProjects\JobExecuter\JobExecuter\Samples\SqlBackupSingleDatabaseJobTask.json>>
```

To edit JSON files you generally need only a good text editor. We recommend Notepad++. Copy and paste is a favourable way for creating new job files for JobExecuter.

JSON itself is formatted plain text with UTF8 encoding in case of JobExecuter. If you edit JSON files you will have to escape certain characters in the text. Please see the list of critical chararcters to escape in JSON:

**\b**
: Backspace (ascii code 08)

**\f**
: Form feed (ascii code 0C)

**\n**
: New line

**\r**
: Carriage return

**\t**
: Tab

**\"**
: Double quote

**\\**
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


## Setting up a test job

In the JobExecuter&#39;s installation path there is a folder Samples with  an subfolder SampleJobFolder. This folder contains a sample job with all necessary steps to work with JobExecuter.

You can use this sample folder to create new jobs by copying the folder to a target folder for jobs and adjust the content of the JSON and BAT to your conrete situation.


### Folder content

**job.json:**
: Job definition file

**job.cjson:**
: Encrypted job definition file

**prepare.bat:**
: Encrypt JSON job file to CJSON

**test.bat:**
: Start job for testing purposes

**start.bat:**
: Start job from Windows task scheduler


# Command line parameters for JobExecuter


## CJsonFileName



```
%PathToJobExcuterExe%\JobExecuter.exe %PathToJobJsonFile%\%JsonFileName%.cjson
```

Runs a job defined in the CJSON file %PathToJobJsonFile%\%JsonFileName%.cjson

JSON files to execute with JobExecuter should be UTF8 encoded and must be encrypted via command line parameter /c to a CJSON file. We recommend using Notepad++ as editor for JSON files. Encrypted JSON files must have the extension .cjson.

Each job should have an own folder.

If there is a readme.txt in the same folder as the JSON job file, the JobDocuJobTask documenting the job will add the content of readme.txt to the job documentation.


## /p (Encrypt password)



```
%PathToJobExcuterExe%\JobExecuter.exe /p
```

If you run JobExecuter with this parameter you will be ask to insert a clear text password. This password will be encrypted by JobExecuter and copied to the clipboard. From there you can paste it to a JSON job definition file where a password is required in encrypted form.


## /c (Encrypt job file)



```
%PathToJobExcuterExe%\JobExecuter.exe /c %PathToJobJsonFile%\%JsonFileName%.json
```

Encrypt a job&#39;s JSON file to a CJSON file. Password required for that transaction! See below.


### /j (setting app password for encrypting job files)



```
%PathToJobExcuterExe%\JobExecuter.exe /j
```

If you run JobExecuter with this parameter you will be ask to insert a clear text password. This password will be encrypted by JobExecuter and stored internally.

This password is valid only on the machine you run this command.


### /t (synthax check for job JSON file)



```
%PathToJobExcuterExe%\JobExecuter.exe /t %PathToJobJsonFile%\%JsonFileName%.json
```

Checks if the JSON synthax of the given file is valid.


# Overview JobTask classes

**ActiveDirectoryJobTask**
:             Creates a report of the current Windows domain&#39;s Active Directory structure.  
            

**BackupBatchJobTask**
:             Task to run a backup of the file system.    
            

**CmdBatchJobTask**
:             Task to run a cmd.exe batch from a file or a string.    
            

**DhcpJobTask**
:             Creates a report of the current Windows domain&#39;s DHCP infrastructure  
            

**DiskSizeJobTask**
:             Checks the disk size and places an error if free space is below a certain percentage or below a certain absolute value  
            

**DnsJobTask**
:             Creates a report for a Microsoft DNS server  
            

**DocumentationJobTask**
:             Creates a HTML documentation from diverse JSON files as input.  
            May be used i.e. for combining database compliance reports from different database servers to one compliance report HTML file.  
            

**FolderBackupJobTask**
:             Backup of a folder based on robocopy.exe by Microsoft.  
            

**FolderCleanJobTask**
:             Task to clean a folder from old files  
            

**FolderComplianceJobTask**
:             Task to create a compliance report for a folder.   
            

**JobDocuJobTask**
:             Task to document the current JobExecuter job  
            

**PingCheckJobTask**
:             Check successful ping to certain network addresses  
            

**PsBatchJobTask**
:             Run a PowerShell script file  
            

**RunAppJobTask**
:             Run an app with or without parameters  
            

**SqlBackupAllDatabasesJobTask**
:             Task to take a full backup for all SQL databases (with exception of TempDb) on a Microsoft&#174; SqlServer starting with version 2008.  
            

**SqlBackupLogSingleDatabaseJobTask**
:             Task to take a log file backup for a single SQL database on Microsoft&#174; SqlServer starting with version 2008.  
            

**SqlBackupSingleDatabaseJobTask**
:             Task to take a full backup for a single SQL database on Microsoft&#174; SqlServer starting with version 2008.  
            

**SqlBatchJobTask**
:             Task to run a SQL batch from a file or from a string against a Microsoft&#174; SqlServer   
            

**SqlComplianceAllDatabasesJobTask**
:             Task to collect compliance relevante information for all SQL databases on a Microsoft&#174; SqlServer   
            

**SqlComplianceSingleDatabaseJobTask**
:             Task to collect compliance relevante information for a single SQL database on a Microsoft&#174; SqlServer   
            

**SqlDocuAllDatabasesJobTask**
:             Task to take a documentation for all SQL databases on a Microsoft&#174; SqlServer with exception of databases Master, Msdb, Model and TempDb.   
             

**SqlDocuSingleDatabaseJobTask**
:              Task to take a documentation for a single SQL database on a Microsoft&#174; SqlServer.  
             

**SqlMaintenanceJobTask**
:             Job to perfrom maintenance on a Microsoft&#174; SqlServer instance  
            

**TaskSchedulerDocuJobTask**
:             Task to save TaskScheduler tasks as XML and document it  
            


# Documentation JobTask classes


## ActiveDirectoryJobTask

<br />            Creates a report of the current Windows domain&#39;s Active Directory structure.<br />            

Work only for single domain networks. Domain forest are not supported. Delivers a report over the structure of the AD with Organizational Units (OU),  users, groups and computers.


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for ActiveDirectoryJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.ActiveDirectoryJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.ActiveDirectoryJobTask, JobExecuter.Dal",
			"Title": null,
			"MailTo": null,
			"HtmlTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\ActiveDirectory.htm",
			"PdfTargetFileName": null,
			"JsonTargetFileName": null,
			"Name": "Report Active Directory",
			"Description": "Report the current domain's Active Directory structure with organisation units (OU), users, groups and computers.",
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## BackupBatchJobTask

<br />            Task to run a backup of the file system.  <br />            


### Properties

**BackupTargets**
:             Current list of backup targets  
            

**Command**
:             Backup command to run or null if the default should be used  
            

**Days**
:             Number of days the backups are backup after date TagDatum before being deleted  
            

**DaysOffset**
:             Offset in days  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**Errors**
:             Current list of errors  
            

**GetAliveFolder**
:             A folder path called to get the storage media alive before starting backup  
            

**Name**
:             Name of the job  
            

**ShadowingSourceDrive**
:             The source drive used for disk shadowing or null. I.e.: C:  
            

**ShadowingTargetDrive**
:             The target drive used for disk shadowing or null. I.e.: H:  
            

**StatusChanged**
:             Status changed event  
            

**SummaryFile**
:             Name of the summary file to be created  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Weeks**
:             Number of weeks the backups are kept before being deleted in weekly backup mode. Default: 5  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for BackupBatchJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.BackupBatchJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.BackupBatchJobTask, JobExecuter.Dal",
			"ShadowingSourceDrive": "C:",
			"ShadowingTargetDrive": "H:",
			"Command": null,
			"Weeks": 5,
			"GetAliveFolder": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\",
			"Days": 14,
			"DaysOffset": 0,
			"BackupTargets": [
				{
					"$type": "Bodoconsult.App.Backup.BackupTargetSettings, Bodoconsult.App",
					"Command": "robocopy ??source?? ??target??  *.* /FFT /S /E /log:??log?? /COPY:DT /B /NP /MT:48 /R:1 /W:1 /XF thumbs.db /xd ??xd?? /NoDCopy",
					"Source": "d:\\tmp\\FolderBackup1",
					"Target": "d:\\tmp\\FolderBackup2\\",
					"Logfile": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\logfile.log",
					"ExcludeDirs": null,
					"BackupMode": "Simple",
					"Thread": 1,
					"Count": 5,
					"DaysOffset": 0,
					"Errors": null,
					"StatusChanged": null
				}
			],
			"SummaryFile": null,
			"Errors": null,
			"StatusChanged": null,
			"Name": "Backup file system",
			"Description": "Take a backup of a filesystem folder using diskshadowing if needed and keeping old backups for a certain amount of time depending on the backup mode",
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## CmdBatchJobTask

<br />            Task to run a cmd.exe batch from a file or a string.  <br />            

Set either CmdBatchJobTask.Filename or CmdBatchJobTask.BatchFileContent. In doubt CmdBatchJobTask.BatchFileContent wins!<br />            The advantage of running a batch in mode CmdBatchRunType.LineByLine that the batch runs line per line as separated processes. <br />            The lines must be independent from each other.<br />            


### Properties

**BatchFileContent**
:             Batch commands as string (like in a .bat or .cmd file). Commands in the string run always in mode CmdBatchRunType.LineByLine.  Set either CmdBatchJobTask.Filename or CmdBatchJobTask.BatchFileContent. In doubt CmdBatchJobTask.BatchFileContent wins!  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**Filename**
:             Filename of .bat or .cmd file to run. Set either CmdBatchJobTask.Filename or CmdBatchJobTask.BatchFileContent. In doubt CmdBatchJobTask.BatchFileContent wins!  
            

**Name**
:             Name of the job  
            

**RunType**
:             Type of how to run a cmd batch file  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for CmdBatchJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.CmdBatchJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.CmdBatchJobTask, JobExecuter.Dal",
			"Filename": "testbackup.bak",
			"RunType": "WholeFile",
			"BatchFileContent": null,
			"Name": "Run a batch (*.bat or *.cmd) with cmd.exe",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 110
		},
		{
			"$type": "JobExecuter.Dal.Model.CmdBatchJobTask, JobExecuter.Dal",
			"Filename": "testbackup.bak",
			"RunType": "LineByLine",
			"BatchFileContent": null,
			"Name": "Run a batch (*.bat or *.cmd) with cmd.exe",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 110
		},
		{
			"$type": "JobExecuter.Dal.Model.CmdBatchJobTask, JobExecuter.Dal",
			"Filename": null,
			"RunType": "LineByLine",
			"BatchFileContent": "REm dfgsfd\r\ncopy d:\\tmp\\FolderBackup1\\*.* d:\\tmp\\FolderBackup2\\*.* /y\r\nREM fghdfg\r\ncopy d:\\tmp\\FolderBackup2\\*.* d:\\tmp\\FolderBackup1\\*.* /y\r\nREM fghfsd\r\n",
			"Name": "Run a batch commands from a string",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 110
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## DhcpJobTask

<br />            Creates a report of the current Windows domain&#39;s DHCP infrastructure<br />            

User running that job needs to be member of security group DHCP Administrators on the DHCP server


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for DhcpJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.DhcpJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.DhcpJobTask, JobExecuter.Dal",
			"Title": null,
			"MailTo": null,
			"HtmlTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\DHCP.htm",
			"PdfTargetFileName": null,
			"JsonTargetFileName": null,
			"Name": "Report Dhcp infrastructure",
			"Description": "Report the current domain's DHCP infrastructure",
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## DiskSizeJobTask

<br />            Checks the disk size and places an error if free space is below a certain percentage or below a certain absolute value<br />            


### Properties

**Description**
:             Description of the job used for documentation  
            

**Drives**
:             Drives to check for disk space  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**Name**
:             Name of the job  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for DiskSizeJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.DiskSizeJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.DiskSizeJobTask, JobExecuter.Dal",
			"Drives": [
				{
					"$type": "JobExecuter.Dal.Model.Drive, JobExecuter.Dal",
					"Name": "C:",
					"MinimumPercentageOfFreeSpace": 0.3,
					"IpAddress": null
				},
				{
					"$type": "JobExecuter.Dal.Model.Drive, JobExecuter.Dal",
					"Name": "\\\\bododata\\data$",
					"MinimumPercentageOfFreeSpace": 0.7,
					"IpAddress": null
				},
				{
					"$type": "JobExecuter.Dal.Model.Drive, JobExecuter.Dal",
					"Name": "C:",
					"MinimumPercentageOfFreeSpace": 0.3,
					"IpAddress": "192.168.12.219"
				},
				{
					"$type": "JobExecuter.Dal.Model.Drive, JobExecuter.Dal",
					"Name": "C:",
					"MinimumPercentageOfFreeSpace": 0.3,
					"IpAddress": "192.168.12.15"
				}
			],
			"Name": "Sample for DiskSizeJobTask job task",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## DnsJobTask

<br />            Creates a report for a Microsoft DNS server<br />            

User running that job needs to be member of security group DNS administrators on the DNS server


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**Server**
:             FQDN of the DNS server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for DnsJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.DnsJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.DnsJobTask, JobExecuter.Dal",
			"Server": "BCGS02DC.BCG-AD.DE",
			"Title": null,
			"MailTo": null,
			"HtmlTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\DNS.htm",
			"PdfTargetFileName": null,
			"JsonTargetFileName": null,
			"Name": "Report a DNS server's structure",
			"Description": "Report DNS server's structure",
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## DocumentationJobTask

<br />            Creates a HTML documentation from diverse JSON files as input.<br />            May be used i.e. for combining database compliance reports from different database servers to one compliance report HTML file.<br />            


### Properties

**AppendHelpFile**
:             Append the JobExecuter help file to documentation  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**HtmlTargetFiles**
:             Filenames for HTML documentation as result of the job  
            

**JsonFiles**
:             message to deliver for testing purposes  
            

**Name**
:             Name of the job  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for DocumentationJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.DocumentationJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.DocumentationJobTask, JobExecuter.Dal",
			"JsonFiles": [
				"d_:\\tmp\\test.jsonSample.json",
				"d:\\tmp\\test.jsonSample.json"
			],
			"HtmlTargetFiles": [
				"d:\\tmp\\FolderBackup2\\documentation.htm"
			],
			"Title": "Full documentation",
			"AppendHelpFile": false,
			"Name": "Full documentation",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## FolderBackupJobTask

<br />            Backup of a folder based on robocopy.exe by Microsoft.<br />            

Robocopy.exe is installed automatically starting with Windows 7 and Windows 2008 Server.


### Properties

**BackupType**
:             Defines how backup works. Default is FolderBackupType.PlainCopy.  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**ExcludeDirectories**
:             Directories to exclude from backup  
            Syntax: C:\ToExclude1 &quot;C:\To Exclude2&quot; c:\ToExclude3  
            

**IncludeSubDirectories**
:             Include subdirectories in the folder backup. Default: true  
            

**Name**
:             Name of the job  
            

**ShowProgress**
:             Show progress of backup. May reduce performance. Default: false  
            

**SourcePath**
:             Folder to backup  
            

**TargetPath**
:             Target folder for the backup  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for FolderBackupJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.FolderBackupJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.FolderBackupJobTask, JobExecuter.Dal",
			"SourcePath": "d:\\tmp\\FolderBackup1",
			"TargetPath": "d:\\tmp\\FolderBackup2",
			"ExcludeDirectories": null,
			"BackupType": "CopyOnlyNewerFiles",
			"ShowProgress": false,
			"IncludeSubDirectories": true,
			"Name": "Backup a folder to a local or network share target",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## FolderCleanJobTask

<br />            Task to clean a folder from old files<br />            


### Properties

**Days**
:             Clean folder from files older then X days  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**FolderPath**
:             Path to clean  
            

**Name**
:             Name of the job  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for FolderCleanJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.FolderCleanJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.FolderCleanJobTask, JobExecuter.Dal",
			"FolderPath": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\",
			"Days": 7,
			"Name": "Clean a folder from files older than 7 days",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## FolderComplianceJobTask

<br />            Task to create a compliance report for a folder. <br />            

FolderComplianceJobTask requires running under administrative privileges due to accessing file system access control list.


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**FolderPaths**
:             Path to create compiance reporting for  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for FolderComplianceJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.FolderComplianceJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.FolderComplianceJobTask, JobExecuter.Dal",
			"FolderPaths": [
				{
					"$type": "JobExecuter.Dal.Model.ComplianceFolder, JobExecuter.Dal",
					"Path": "c:\\daten\\projekte",
					"IncludeSubFolders": true,
					"ShowPermissionsOfPredefinedAccounts": true
				}
			],
			"Title": null,
			"MailTo": "test@test123.de",
			"HtmlTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\Compliance2.htm",
			"PdfTargetFileName": null,
			"JsonTargetFileName": null,
			"Name": "Create a compliance report for file system folders",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## JobDocuJobTask

<br />            Task to document the current JobExecuter job<br />            


### Properties

**CurrentJob**
:             Current job. Will be used if JobDocuJobTask.JobJsonFileName is empty.  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JobJsonFileName**
:             Full path of the CJSON file with JobExecuter job to document  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for JobDocuJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.JobDocuJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.JobDocuJobTask, JobExecuter.Dal",
			"JobJsonFileName": "",
			"Title": null,
			"MailTo": null,
			"HtmlTargetFileName": null,
			"PdfTargetFileName": null,
			"JsonTargetFileName": "",
			"Name": "Document the current JobExecuter job",
			"Description": "If JobJsonFileName and DocuJsonFileName left empty, JobExecuter will insert current job file path and as docu file name the current job file path with the suffix _docu.json",
			"ErrorMailReceiver": null,
			"Timeout": 30
		},
		{
			"$type": "JobExecuter.Dal.Model.JobDocuJobTask, JobExecuter.Dal",
			"JobJsonFileName": "d:\\tmp\\FolderBackup2\\job.json",
			"Title": null,
			"MailTo": null,
			"HtmlTargetFileName": null,
			"PdfTargetFileName": null,
			"JsonTargetFileName": "d:\\tmp\\FolderBackup2\\job.json",
			"Name": "Document the a JobExecuter job",
			"Description": "Set JobJsonFileName and DocuJsonFileName to a spezific path if needed",
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## PingCheckJobTask

<br />            Check successful ping to certain network addresses<br />            


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**IpAdresses**
:             IP-addresses separated by semicolon  
            

**Name**
:             Name of the job  
            

**PingTimeout**
:             Timeout in milliseconds for Ping. Default: 15.000   
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for PingCheckJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.PingCheckJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.PingCheckJobTask, JobExecuter.Dal",
			"IpAdresses": "192.168.12.15;192.168.12.20;192.168.12.1;8.8.8.8",
			"PingTimeout": 15000,
			"Name": "Sample task for ping check",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## PsBatchJobTask

<br />            Run a PowerShell script file<br />            


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**Filename**
:             Full path to a powershell script file to run. Path may contain blanks  
            

**Name**
:             Name of the job  
            

**PowerShellVersion**
:             The version of PowerShell to run the Model.PsBatchJobTask on  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for PsBatchJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.PsBatchJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.PsBatchJobTask, JobExecuter.Dal",
			"Filename": "PowerShellScript.ps1.bak",
			"PowerShellVersion": "V01",
			"Name": "Running a PowerShell batch file with PowerShell V01",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 30
		},
		{
			"$type": "JobExecuter.Dal.Model.PsBatchJobTask, JobExecuter.Dal",
			"Filename": "PowerShellScript.ps1.bak",
			"PowerShellVersion": "V03",
			"Name": "Running a PowerShell batch file with PowerShell V03",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## RunAppJobTask

<br />            Run an app with or without parameters<br />            


### Properties

**Arguments**
:             Arguments to start the app with  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**Filename**
:             Full path to app to run. Path may contain blanks  
            

**InteractiveMode**
:             Run app in interactive mode. default: false  
            

**Name**
:             Name of the job  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for RunAppJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.RunAppJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.RunAppJobTask, JobExecuter.Dal",
			"Filename": "robocopy.exe",
			"Arguments": null,
			"InteractiveMode": false,
			"Name": "Sample for running a app without parmeters",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 30
		},
		{
			"$type": "JobExecuter.Dal.Model.RunAppJobTask, JobExecuter.Dal",
			"Filename": "robocopy.exe",
			"Arguments": "/?",
			"InteractiveMode": false,
			"Name": "Sample for running a app with parameters",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## SqlBackupAllDatabasesJobTask

<br />            Task to take a full backup for all SQL databases (with exception of TempDb) on a Microsoft&#174; SqlServer starting with version 2008.<br />            

Uses the following SQl statement: BACKUP DATABASE [{0}] TO DISK = N&#39;JobExecuter documentation&#39; WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N&#39;{0} full backup&#39;


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**FileName**
:             File name schema for the backup file. ;ust contain placeholders.   
            Otherwise all backups are made to the same file (and overwrite it with each backup)  
            Must contain placeholders:  
            ??server?? = Server name or Server and instance name  
            ??database?? = database name  
            Optional placeholders are:  
            ??date?? = date in format yyyymmdd  
            ??time?? = date and time in format yyyymmddhhnn  
            

**LocalTarget**
:             Local target folder to backup database in  
            

**Name**
:             Name of the job  
            

**NetworkTarget**
:             Target folder for the backup on a network share (if requested).  
            

**RemoveLocalBackup**
:             Remove local backup files after copying to network. Default: false  
            

**Server**
:             server name or server and instance name of a database server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for SqlBackupAllDatabasesJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlBackupAllDatabasesJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.SqlBackupAllDatabasesJobTask, JobExecuter.Dal",
			"LocalTarget": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter",
			"NetworkTarget": null,
			"FileName": "??server??_??database??.bak",
			"RemoveLocalBackup": false,
			"Server": ".\\sqlexpress",
			"Name": "Local backup of all databases on a SqlServer instance",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		},
		{
			"$type": "JobExecuter.Dal.Model.SqlBackupAllDatabasesJobTask, JobExecuter.Dal",
			"LocalTarget": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter",
			"NetworkTarget": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\network",
			"FileName": "testbackup.bak",
			"RemoveLocalBackup": false,
			"Server": ".\\sqlexpress",
			"Name": "Network share backup of all databases on a SqlServer instance",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## SqlBackupLogSingleDatabaseJobTask

<br />            Task to take a log file backup for a single SQL database on Microsoft&#174; SqlServer starting with version 2008.<br />            

Uses the following SQl statement: BACKUP LOG [{0}] TO DISK = N&#39;JobExecuter documentation&#39; WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N&#39;{0} full backup&#39;


### Properties

**DatabaseName**
:             Name of the database to backup  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**FileName**
:             File name for the backup file  
            May contain placeholders:  
            ??server?? = Server name or Server and instance name  
            ??database?? = database name  
            ??date?? = date in format yyyymmdd  
            ??time?? = date and time in format yyyymmddhhnn  
            

**LocalTarget**
:             Local target folder to backup database in  
            

**Name**
:             Name of the job  
            

**NetworkTarget**
:             Target folder for the backup on a network share (if requested).  
            

**RemoveLocalBackup**
:             Remove local backup files after copying to network. Default: false  
            

**Server**
:             server name or server and instance name of a database server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for SqlBackupLogSingleDatabaseJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlBackupLogSingleDatabaseJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.SqlBackupLogSingleDatabaseJobTask, JobExecuter.Dal",
			"DatabaseName": "MediaDb",
			"LocalTarget": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter",
			"NetworkTarget": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\network",
			"FileName": "testbackup.bak",
			"RemoveLocalBackup": true,
			"Server": ".\\sqlexpress",
			"Name": "Backup database to a network path",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## SqlBackupSingleDatabaseJobTask

<br />            Task to take a full backup for a single SQL database on Microsoft&#174; SqlServer starting with version 2008.<br />            

Uses the following SQl statement: BACKUP DATABASE [{0}] TO DISK = N&#39;JobExecuter documentation&#39; WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N&#39;{0} full backup&#39;


### Properties

**DatabaseName**
:             Name of the database to backup  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**FileName**
:             File name for the backup file  
            May contain placeholders:  
            ??server?? = Server name or Server and instance name  
            ??database?? = database name  
            ??date?? = date in format yyyymmdd  
            ??time?? = date and time in format yyyymmddhhnn  
            

**LocalTarget**
:             Local target folder to backup database in  
            

**Name**
:             Name of the job  
            

**NetworkTarget**
:             Target folder for the backup on a network share (if requested).  
            

**RemoveLocalBackup**
:             Remove local backup files after copying to network. Default: false  
            

**Server**
:             server name or server and instance name of a database server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for SqlBackupSingleDatabaseJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlBackupSingleDatabaseJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.SqlBackupSingleDatabaseJobTask, JobExecuter.Dal",
			"DatabaseName": "MediaDb",
			"LocalTarget": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter",
			"NetworkTarget": null,
			"FileName": "testbackup.bak",
			"RemoveLocalBackup": false,
			"Server": ".\\sqlexpress",
			"Name": "Backup database to local file only",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		},
		{
			"$type": "JobExecuter.Dal.Model.SqlBackupSingleDatabaseJobTask, JobExecuter.Dal",
			"DatabaseName": "MediaDb",
			"LocalTarget": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter",
			"NetworkTarget": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\network",
			"FileName": "testbackup.bak",
			"RemoveLocalBackup": true,
			"Server": ".\\sqlexpress",
			"Name": "Backup database to a network path",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## SqlBatchJobTask

<br />            Task to run a SQL batch from a file or from a string against a Microsoft&#174; SqlServer <br />            

SQL statements must be separated by GO. The SQL statements must address database correctly, i.e EXEC testdb.dbo.TestProc.<br />            All SQL statements are running on the same connection. This makes it possible to use temporary tables in SQL statements.


### Properties

**BatchContent**
:             Direct batch SQL to run  
            

**DatabaseName**
:             Name of the database to run the batch file in  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**FileName**
:             Path to the SQL batch file to run  
            

**Name**
:             Name of the job  
            

**Server**
:             server name or server and instance name of a database server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for SqlBatchJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlBatchJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.SqlBatchJobTask, JobExecuter.Dal",
			"DatabaseName": "MediaDb",
			"FileName": "testbackup.bak",
			"BatchContent": null,
			"Server": ".\\sqlexpress",
			"Name": "Run a SQL batch file",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		},
		{
			"$type": "JobExecuter.Dal.Model.SqlBatchJobTask, JobExecuter.Dal",
			"DatabaseName": "MediaDb",
			"FileName": null,
			"BatchContent": "exec spTest 1\r\nGO\r\nexec spTest 2\r\nGO\r\nexec spTest 3",
			"Server": ".\\sqlexpress",
			"Name": "Run SQL batch directly from string",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## SqlComplianceAllDatabasesJobTask

<br />            Task to collect compliance relevante information for all SQL databases on a Microsoft&#174; SqlServer <br />            

Databases Master, Msdb, Model and TempDb are excluded by this task. Requires the job running user to be member of the sysadmin or securityadmin server roles.


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**Server**
:             server name or server and instance name of a database server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for SqlComplianceAllDatabasesJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlComplianceAllDatabasesJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.SqlComplianceAllDatabasesJobTask, JobExecuter.Dal",
			"Server": ".\\sqlexpress",
			"Title": null,
			"MailTo": "info@bodoconsult.de",
			"HtmlTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\ServerXY_DatabaseComplianceReport.htm",
			"PdfTargetFileName": null,
			"JsonTargetFileName": null,
			"Name": "Create compliance information for all databases on a SqlServer with exception of databases Master, Msdb, Model and TempDb",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## SqlComplianceSingleDatabaseJobTask

<br />            Task to collect compliance relevante information for a single SQL database on a Microsoft&#174; SqlServer <br />            


### Properties

**DatabaseName**
:             Name of the database to backup  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**Server**
:             server name or server and instance name of a database server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for SqlComplianceSingleDatabaseJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlComplianceSingleDatabaseJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.SqlComplianceSingleDatabaseJobTask, JobExecuter.Dal",
			"Server": ".\\sqlexpress",
			"DatabaseName": "DatabaseXY",
			"Title": null,
			"MailTo": "info@bodoconsult.de",
			"HtmlTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\ServerXY_DatabaseXY_ComplianceReport.htm",
			"PdfTargetFileName": null,
			"JsonTargetFileName": null,
			"Name": "Create compliance information for a single database",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## SqlDocuAllDatabasesJobTask

<br />            Task to take a documentation for all SQL databases on a Microsoft&#174; SqlServer with exception of databases Master, Msdb, Model and TempDb. <br />             

A database may contain stored procs under the schema dbo and names beginning with spDbDocu_ for adding database specific content to the docmentation.

For parts of the documentation the user running the job needs certain permissions. To give least possible permissions, this user should  be part of a user defined server role on the SqlServer with the permissions VIEW ANY DEFINITION and GRANT VIEW SERVER STATE. You should avoid making the user a sysadmin for security reasons. User must have access to each database with least possible permissions!



```

             CREATE SERVER ROLE NameOfNewServerRole
            
            GO
            
            GRANT VIEW ANY DEFINITION TO NameOfNewServerRole
            
            GO
            
            GRANT VIEW SERVER STATE TO NameOfNewServerRole
            
            GO
            
            EXEC sp_addsrvrolemember 'domain\SG-SqlKeyUsers', 'NameOfNewServerRole'
            
             
```


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**Server**
:             server name or server and instance name of a database server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for SqlDocuAllDatabasesJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlDocuAllDatabasesJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.SqlDocuAllDatabasesJobTask, JobExecuter.Dal",
			"Server": ".\\sqlexpress",
			"Title": null,
			"MailTo": null,
			"HtmlTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\testbackup.bak.htm",
			"PdfTargetFileName": null,
			"JsonTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\testbackup.bak.json",
			"Name": "Create a documentation for all databases on a SqlServer with exception of databases Master, Msdb, Model and TempDb",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## SqlDocuSingleDatabaseJobTask

<br />             Task to take a documentation for a single SQL database on a Microsoft&#174; SqlServer.<br />             

A database may contain stored procs under the schema dbo and names beginning with spDbDocu_ foradding database specific content to the docmentation.

For parts of the documentation the user running the job needs certain permissions.<br />             To give least possible permissions, this user should be part of a user defined server role on the SqlServer with the permissions VIEW ANY DEFINITION and GRANT VIEW SERVER STATE.<br />             You should avoid making the user a sysadmin for security reasons. User must have access to each database with least possible permissions!



```

             CREATE SERVER ROLE NameOfNewServerRole
            
            GO
            
            GRANT VIEW ANY DEFINITION TO NameOfNewServerRole
            
            GO
            
            GRANT VIEW SERVER STATE TO NameOfNewServerRole
            
            GO
            
            EXEC sp_addsrvrolemember 'domain\SG-SqlKeyUsers', 'NameOfNewServerRole'
            
             
```


### Properties

**DatabaseName**
:             Name of the database to backup  
            

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**Server**
:             server name or server and instance name of a database server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for SqlDocuSingleDatabaseJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlDocuSingleDatabaseJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.SqlDocuSingleDatabaseJobTask, JobExecuter.Dal",
			"Server": ".\\sqlexpress",
			"DatabaseName": "MediaDb",
			"Title": null,
			"MailTo": null,
			"HtmlTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\database_docu.htm",
			"PdfTargetFileName": null,
			"JsonTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\database_docu.json",
			"Name": "Create a documentation for a single database",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 60
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## SqlMaintenanceJobTask

<br />            Job to perfrom maintenance on a Microsoft&#174; SqlServer instance<br />            

Needs sysadmin permissions to run successfully on the instance of SqlServer


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**FragmentationLevel1**
:             Fragmentation level 1 (low -&amp;gt;medium) in percent. 50 means 50 percent. Default: 5  
            

**FragmentationLevel2**
:             Fragmentation level 2 (medium -&amp;gt; high) in percent. 50 means 50 percent. Default: 30  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to PDF  
            

**Server**
:             server name or server and instance name of a database server  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for SqlMaintenanceJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.SqlMaintenanceJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.SqlMaintenanceJobTask, JobExecuter.Dal",
			"MailTo": "test@test123.de",
			"Title": null,
			"HtmlTargetFileName": "C:\\Users\\rleisner\\AppData\\Local\\Temp\\JobExecuter\\Maintenance.htm",
			"PdfTargetFileName": null,
			"JsonTargetFileName": null,
			"FragmentationLevel1": 5,
			"FragmentationLevel2": 30,
			"Server": ".\\sqlexpress",
			"Name": "Runs a maintenance job on a SqlServer instance",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 600
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


## TaskSchedulerDocuJobTask

<br />            Task to save TaskScheduler tasks as XML and document it<br />            


### Properties

**Description**
:             Description of the job used for documentation  
            

**ErrorMailReceiver**
:             Mail addresses separarted with semicolon to send an error mail to  
            

**Filter**
:             Filters task by the filter expression added here. Seperated values with semicolon. Task name has to start with filter expression  
            

**HtmlTargetFileName**
:             Export documentation to HTML  
            

**JsonTargetFileName**
:             Export documentation as Model.StructuredText object to JSON.  
            If empty, the file will be placed in app subfolder docu with the name &quot;job_docu.json&quot;   
            

**MailTo**
:             Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner  
            

**Name**
:             Name of the job  
            

**PdfTargetFileName**
:             Export documentation to Pdf  
            

**TargetServer**
:             Target server to access for windows task scheduler. Default: null = localhost  
            

**Timeout**
:             Timeout in seconds the job will be terminated after start  
            

**Title**
:             Title for documentation  
            

**XmlTargetPath**
:             Folder to export the tasks in  
            



```
{
	"$type": "JobExecuter.Dal.Model.Job, JobExecuter.Dal",
	"Name": "Sample job for TaskSchedulerDocuJobTask",
	"Description": "This job contains one or more sample JobTasks for class JobExecuter.Dal.Model.TaskSchedulerDocuJobTask",
	"ErrorMailReceiver": "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
	"SummaryMailReceiver": "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
	"JobTasks": [
		{
			"$type": "JobExecuter.Dal.Model.TaskSchedulerDocuJobTask, JobExecuter.Dal",
			"XmlTargetPath": "d:\\tmp\\FolderBackup2",
			"Filter": null,
			"TargetServer": null,
			"Title": null,
			"MailTo": null,
			"HtmlTargetFileName": "d:\\tmp\\FolderBackup2\\docu.html",
			"PdfTargetFileName": null,
			"JsonTargetFileName": "d:\\tmp\\FolderBackup2\\docu.json",
			"Name": "Getting all task scheduler tasks from local computer",
			"Description": null,
			"ErrorMailReceiver": null,
			"Timeout": 30
		},
		{
			"$type": "JobExecuter.Dal.Model.TaskSchedulerDocuJobTask, JobExecuter.Dal",
			"XmlTargetPath": "d:\\tmp\\FolderBackup2",
			"Filter": "Adobe;Microsoft",
			"TargetServer": null,
			"Title": null,
			"MailTo": null,
			"HtmlTargetFileName": "d:\\tmp\\FolderBackup2\\docu.html",
			"PdfTargetFileName": null,
			"JsonTargetFileName": "d:\\tmp\\FolderBackup2\\docu.json",
			"Name": "Getting task scheduler data from local computer with use of filter expressions",
			"Description": "Filters only tasks starting with Adobe or Microsoft",
			"ErrorMailReceiver": null,
			"Timeout": 30
		}
	],
	"Timeout": 500,
	"WriteToEventLog": true
}
```


# Advanced features


## Using Readme.txt

If in the directory of a JobExecuter job - there where the JSON file of job is located - is Readme.txt placed, this file will be included in the job&#39;s documentation.


### Tags usable in the Readme.txt

In such a Readme.txt you can use the following tags to format the readme.txt if it is processed to the job documentation. The following tags are interpreted:

**H1**
: Header level 1

**H2**
: Header level 2

**H3**
: Header level 3

**DL**
: Definition list (must be followed by a DD tag)

**LI**
: List items

All tags will be finished at the next carriage return+line feed (char(13)+char(10)).


### Comments in Readme.txt

Comments made of asterikses (*) will be removed when formatting Readme.txt to plain text or HTML


### Sample

The HTML result of processing such a Readme.txt you can find in the file Readme.htm


# Legal notes

Date this documentation was created: 14.10.2026 21:10


## Registered and unregistered trademarks

All other registered and unregistered trademarks are the property of their respective owners.


## Licence information

&nbsp;


### Licence for Newtonsoft.Json

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


### Licence for dahall/TaskScheduler

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


### Licence for garysharp/DhcpServerApi

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


### Licence for olahallengren/sql-server-maintenance-solution

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


### Licence for Google prettify

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