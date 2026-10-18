// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bodoconsult.Network.Windows.ActiveDirectory;
using Bodoconsult.Network.Windows.Helpers;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for creating a report of the domains's active dirctory structure
/// </summary>
internal class ActiveDirectoryExecuter : DocuBaseExecuter
{

    private readonly ActiveDirectoryJobTask _jobTask;


    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {

        var result = true;


        try
        {

            var domainName = AdHelper.GetDomainName();

            SetStatus($"Create report for Active Directory domain {domainName}...");

            AdTree root = null;
            var buildOuStructure = Task.Factory.StartNew(() => { root = new AdTree(AdHelper.GetLdapDomainForCurrentUser(), false); });

            buildOuStructure.Wait();

            var domain = root.Domain;

            AdHelper.GetUserData(domain);

            Result.AddHeader1("General information on the directory service");
            Result.AddHeader2("General properties");

            foreach (var prop in domain.GeneralInfos)
            {
                Result.AddDefinitionListLine(prop.Key, prop.Value);
            }

            Result.AddHeader2("DHCP servers");
            foreach (var dhcp in domain.DhcpServers)
            {
                Result.AddDefinitionListLine(dhcp.Name, dhcp.DistinguishedName);

            }

            Result.AddHeader1("Domain structure");
            EnumerateTree(root, 0);

            Result.AddParagraph("\t");
            Result.AddParagraph("OU: Organizational Unit\tUR: user\tSG: group\tCO:computer\tGD: Global Distribution Group\tDD: Domain Local Distribution Group\tUD: Universal Distribution Group\tGS: Global Security Group\tDS: DomainLocalSecurityGroup\tUS: UniversalSecurityGroup\tBI: Built-In");
            Result.AddParagraph("\t");

            AddUsers(domain);

            AddGroups(domain);


            AddSecurityInfo(domain);

            Save($"Structure\r\nof\r\nActive Directory domain\r\n{domainName}");

            SetStatus($"Report for Active Directory domain {domainName} created!");


        }
        catch (Exception ex)
        {
            var msg = "ActiveDirectoryExecuter:Run";
            result = false;
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        return result;
    }

    private void AddSecurityInfo(AdDomain domain)
    {

        Result.AddHeader1("Network security");

        Result.AddHeader2("Users with password");

        var users = domain.Users.Where(x => x.PasswordNotRequired).OrderBy(x => x.Name).ToList();

        if (!users.Any())
        {
            Result.AddParagraph("- no users found -");
        }
        else
        {
            foreach (var user in users)
            {
                Result.AddParagraph(user.Name);
            }
        }


        Result.AddHeader2("Users, which cannot change their password");

        users = domain.Users.Where(x => x.PasswordCantChange).OrderBy(x => x.Name).ToList();

        if (!users.Any())
        {
            Result.AddParagraph("- no users found -");
        }
        else
        {
            foreach (var user in users)
            {
                Result.AddParagraph(user.Name);
            }
        }


        Result.AddHeader2("Users with password no expiring");

        users = domain.Users.Where(x => x.DontExpirePassword).OrderBy(x => x.Name).ToList();

        if (!users.Any())
        {
            Result.AddParagraph("- no users found -");
        }
        else
        {
            foreach (var user in users)
            {
                Result.AddParagraph(user.Name);
            }
        }

        Result.AddHeader2("Disabled users");

        users = domain.Users.Where(x => x.Disabled).OrderBy(x => x.Name).ToList();

        if (!users.Any())
        {
            Result.AddParagraph("- no users found -");
        }
        else
        {
            foreach (var user in users)
            {
                Result.AddParagraph(user.Name);
            }
        }


    }

    private void AddUsers(AdDomain domain)
    {
        Result.AddHeader1("Users: membership in groups");


        foreach (var user in domain.Users.OrderBy(x => x.UserPrincipalName))
        {
            if (user.MemberOf.Count <= 0) continue;

            Result.AddHeader2($"{user.UserPrincipalName} ({user.Name})");

            foreach (var m in user.MemberOf)
            {
                var g = (AdGroup)m;
                Result.AddParagraph($"\t\t{(AdGroupTypeShort)@g.GroupType}: {g.Name}");
            }
        }
    }

    private void AddGroups(AdDomain domain)
    {
        Result.AddHeader1("Groups: member of the groups");

        foreach (var group in domain.Groups.OrderBy(x => x.Name))
        {
            if (group.Members.Count <= 0) continue;

            Result.AddHeader2(group.Name);
            Result.AddParagraph($"{group.GroupType}");
            Result.AddParagraph($"{group.Path}");
            Result.AddParagraph(" ");

            foreach (var m in group.Members)
            {
                if (m.GetType() == typeof(AdGroup))
                {
                    var g = (AdGroup)m;
                    Result.AddParagraph($"\t\t{(AdGroupTypeShort)@g.GroupType}: {g.Name}");
                }
                else
                {
                    Result.AddParagraph($"\t\tUR: {m.Name}");
                }
            }
        }
    }


    private void EnumerateTree(AdTree node, int level)
    {

        var space = string.Empty.PadRight(level).Replace(" ", "\t\t");
        var space1 = string.Empty.PadRight(level + 1).Replace(" ", "\t\t");
        var space2 = string.Empty.PadRight(level + 2).Replace(" ", "\t\t");


        Result.AddParagraph(space + (level == 0 ? "Domain " : "OU: ") + node.Name);

        if (node.Computers.Any())
        {
            foreach (var computer in node.Computers)
            {
                Result.AddParagraph($"{space1}CO: {computer.Name}");
            }
        }

        if (node.Users.Any())
        {
            foreach (var user in node.Users)
            {
                Result.AddParagraph($"{space1}UR: {user.Name}");
            }
        }


        if (node.Groups.Any())
        {
            foreach (var group in node.Groups)
            {
                Result.AddParagraph($"{space1}{(AdGroupTypeShort)group.GroupType}: {group.Name}");


            }
        }

        if (node.ChildOUs.Any())
        {
            foreach (var subNode in node.ChildOUs.OrderBy(x => x.Name))
            {
                EnumerateTree(subNode, level + 1);
            }
        }
    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public ActiveDirectoryExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (ActiveDirectoryJobTask)jobTask;
    }
}