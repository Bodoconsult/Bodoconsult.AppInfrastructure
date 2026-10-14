Readme for JobExecuter
==================

# Overview

JobExecuter fulfills helpful tasks especially for small Windows based IT enviroments but it is not limited on them. In such small environments often important features like database backup or file backup are missed and have to be implemented with some effort.

JobExecuter is a software to run different types of administrative jobs like database backup, file copying, running apps and others. It is used used in conjunction with Windows Task Scheduler (WTS). The WTS provides the job scheduling infrastructure. JobExecuter provides the functionality of the jobs in an supervised environment.

JobExecuter supports self-documentation: each job can be documented automatically and all job documentations can be added to a complete documentation of all JobExecuter based jobs running on diverse server systems.

See the [JobExecuter documentation](documentation.md) for more details of how to use JobExecuter.

# Why You Should Use JobExecuter


Windows’built-in Task Scheduler offers a very stable way to execute scheduled tasks (jobs). However, from a professional administrator's perspective, the monitoring capabilities are somewhat limited. 
While the system can notify the admin if a job fails completely, issues arising within individual task steps often go unnoticed because the Task Scheduler typically does not react to them.

Why use JobExecuter? While everything JobExecuter does can technically be achieved using standard Windows or SQL Server tools, doing so requires significantly more effort for setup and system monitoring.

JobExecuter can greatly simplify administration, particularly when used with the free Express editions of Microsoft SQL Server. Tasks such as backups, database documentation, and access rights monitoring become much easier to manage.

It also simplifies other routine jobs, such as backing up application and file servers or handling data imports and exports.

Best of all, JobExecuter runs all jobs in a fully monitored environment. Detailed logging and automated email notifications to the responsible administrator help detect issues early on.
JobExecuter also handles the backup of the scheduled job configurations themselves. This can save a great deal of time when re-establishing scheduled tasks during system recovery.

Documenting scheduled jobs is often a tedious task that administrators tend to neglect. JobExecuter assists by enabling automated documentation.

Once set up, the administrator simply needs to add proper comments for any new or modified job steps. JobExecuter then automatically generates documentation that meets professional standards. 
This significantly reduces the time administrators spend on documentation.

JobExecuter can thus contribute to a significant professionalization of IT, particularly in smaller networks—and without major financial outlay.

See the [JobExecuter documentation](documentation.md) for more details of how to use JobExecuter.

# Features missing currently


# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.