using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using JobExecuter.GpoPlugIn.Helper;

namespace JobExecuter.GpoPlugIn.Model;

public class GpDomain
{



    public GpDomain(string domain)
    {
        Domain = domain;
    }

    public string Domain { get;  }


    public IList<Gpo> Gpos { get; } = new List<Gpo>();


    public IList<GpoBackup> GpoBackups { get;  } = new List<GpoBackup>();


    /// <summary>
    /// 
    /// </summary>
    /// <remarks>PS: Get-GPO -domain bcg-ad.de -all</remarks>
    /// <returns></returns>
    public void GetAllGpos()
    {
        Gpos.Clear();

        var script = $"Get-GPO -domain {Domain} -all";

        var results = PowerShellHelper.Execute(script);

        foreach (var item in results)
        {
            Gpos.Add(MapPsObjectToGpo(item));
        }
    }

    public void Backup(string targetPath)
    {
        GpoBackups.Clear();

        var script = $" Backup-GPO -All -Path '{targetPath}' -Comment 'JobExecuter Backup of GPOs' -Domain {Domain}";

        var results = PowerShellHelper.Execute(script);

        foreach (var item in results)
        {
            var b = new GpoBackup
            {
                Id = item.Properties["Id"].Value.ToString(),
                BackupDirectory = item.Properties["BackupDirectory"].Value.ToString()
            };

            GpoBackups.Add(b);
        }
    }



    public static Gpo MapPsObjectToGpo(PSObject item)
    {
        var gpo = new Gpo
        {
            DisplayName = item.Properties["DisplayName"].Value?.ToString(),
            Domain = item.Properties["DomainName"].Value?.ToString(),
            Id = item.Properties["Id"].Value?.ToString(),
            Description = item.Properties["Description"].Value?.ToString(),
            Owner = item.Properties["Owner"].Value?.ToString(),
            GpoStatus = item.Properties["GpoStatus"].Value?.ToString(),
            CreationTime = Convert.ToDateTime(item.Properties["CreationTime"].Value, CultureInfo.InvariantCulture),
            ModificationTime = Convert.ToDateTime(item.Properties["ModificationTime"].Value, CultureInfo.InvariantCulture)
        };
        return gpo;
    }
}