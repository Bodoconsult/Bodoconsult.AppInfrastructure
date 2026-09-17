# Database installation for BodoFileTransfer

BodoFileTransfer is an app collecting files from inbox folders to archive folders and sending them on request to an email receiver.

Bodoconsult uses BodoFileTransfer as tool for sending accounting relevante documents to the accounting department at the tax advisor once a month.

BodoFileTransfer is not a service. It is a simple console app and therefore easy to start from TaskScheduler.

# Prerequisites

BodoFileTransfer is currently using a SqlServer database installable on SqlServer Express 2019 and later as minimum requirement.

Create a fresh database BodoFileTransfer on the SqlServer (Express) you want to use.

# Create the required database entities

You can find the following SQL commands in the file SQLServer_Install.sql in the folder DB_Install. Run the SQL in your database BodoFileTransfer i.e. from SSMS to create the required entities in the database.