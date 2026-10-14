using System;
using JobExecuter.GpoPlugIn.Helper;

namespace JobExecuter.GpoPlugIn.Model;

public class Gpo
{
    public string DisplayName { get; set; }

    public string Domain { get; set; }

    public string Description { get; set; }
    public string Owner { get; set; }
    public string Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    public string Path { get; set; }
    public string GpoStatus { get; set; }



    public void GenerateReportToFile(string fileName)
    {
        var script = $"Get-GPOReport -Guid \"{Id}\" -ReportType HTML -Path \"{fileName}\"";

        PowerShellHelper.Execute(script);
    }





}