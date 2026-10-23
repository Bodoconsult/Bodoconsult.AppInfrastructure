# Run a process as other user


var psi = new ProcessStartInfo
{
    FileName = "notepad",
    UserName = "admin",
    Domain = "",
    Password = pass,
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
Process.Start(psi);

# Elevated

This is a clear answer to your question: [How do I force my .NET application to run as administrator?](https://stackoverflow.com/questions/2818179/how-do-i-force-my-net-application-to-run-as-administrator)

Summary:

Right Click on project -> Add new item -> Application Manifest File

Then in that file change a line like this:

<requestedExecutionLevel level="requireAdministrator" uiAccess="false" />

# Shutdown

https://learn.microsoft.com/de-de/windows-server/administration/windows-commands/shutdown


# Synology NAS

https://www.c-amie.co.uk/technical/how-to-shutdown-a-synology-dsm-6-nas-using-a-script/