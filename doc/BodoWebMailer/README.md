# Database installation for BodoWebMailer

BodoWebMailer is a basic app sending formatted emails stored in a database table to the requested receivers.

Currently BodoWebMailer is using a configurable Office365 mailbox to send mails (via Graph).

BodoWebMailer is not a service. It is a simple console app and therefore easy to start from TaskScheduler.

# Prerequisites

BodoWebMailer is currently using a SqlServer database installable on SqlServer Express 2019 and later as minimum requirement.

Create a fresh database BodoWebMailer on the SqlServer (Express) you want to use.

# Create the required database entities

You can find the following SQL commands in the file SQLSever_Install.sql in the folder DB_Install. Run the SQL in your database BodoWebMailer i.e. from SSMS to create the required entities in the database.