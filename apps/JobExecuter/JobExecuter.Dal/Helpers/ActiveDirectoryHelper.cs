// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;

namespace JobExecuter.Dal.Helpers;

#pragma warning disable CA1416

/// <summary>
/// Helper functions for Active Directory access
/// </summary>
public class ActiveDirectoryHelper
{



    /// <summary>
    /// Get user information for account name
    /// </summary>
    /// <param name="accountName">Account name written as domain\username</param>
    /// <param name="includeAccountName">Include the account name in the the result string: Default false.</param>
    /// <returns></returns>
    public static string GetAdUser(string accountName, bool includeAccountName=false)
    {
        var name = accountName.Split([@"\"], StringSplitOptions.None);
        var domain = name[0];
        var userName = name[1];

        using var pc = new PrincipalContext(ContextType.Domain, domain);
        using var user = UserPrincipal.FindByIdentity(pc, userName);
        return user == null ? null : $"{(includeAccountName ? $"{domain}\\{user.UserPrincipalName}" : string.Empty)} (Fullname: {{user.Name}} Last logon: {{user.LastLogon}} Enabled: {{user.Enabled}})";
    }







    /// <summary>
    /// Get AD users for a AD security group
    /// </summary>
    /// <param name="domainName">name of the AD domain</param>
    /// <param name="groupName">Group name of AD security group</param>
    /// <returns>List of all users in the group</returns>
    public static SortedSet<string> GetUsernames(string domainName, string groupName)
    {
        //try
        //{
        using var pc = new PrincipalContext(ContextType.Domain, domainName);
        using var gp = GroupPrincipal.FindByIdentity(pc, groupName);
        if (gp == null) return null;
        var result = new SortedSet<string>();
        foreach (var user in gp.GetMembers())
        {
            var x = user.GetType().Name;
            if (user.GetType() == typeof(UserPrincipal))
            {
                var au = (AuthenticablePrincipal)user;
                result.Add($"{domainName}\\{user.SamAccountName} (Fullname: {user.Name} Last logon: {au.LastLogon} Enabled: {au.Enabled})");
            }
            else
            {
                result.Add($"{domainName}\\{user.SamAccountName}");
            }

        }

        return result;

        //}
        //catch
        //{
        //    return new SortedSet<string>();
        //}
    }


    /// <summary>
    /// Full domain group name written as domain\groupname
    /// </summary>
    /// <param name="groupName">group name</param>
    /// <returns>List of all users in the group</returns>
    public static SortedSet<string> GetUsernames(string groupName)
    {
        try
        {
            var erg = groupName.Split(["\\"], StringSplitOptions.RemoveEmptyEntries);
            return GetUsernames(erg[0], erg[1]);
        }
        catch
        {
            return [];
        }
    }
}