// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Linq;
using Bodoconsult.Network.Windows.Dhcp;
using Bodoconsult.Network.Windows.Dhcp.Client;
using Bodoconsult.Network.Windows.Helpers;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for reporting DHCP servers
/// </summary>
internal class DhcpExecuter : DocuBaseExecuter
{

    private readonly DhcpJobTask _jobTask;

    private string _domain;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {

        var result = true;

        try
        {

            _domain = AdHelper.GetLdapDomainForCurrentUser();

            SetStatus($"Create report for DHCP infrastructure of domain {_domain}...");


            foreach (var dhcpServer in DhcpServer.Servers)
            {
                CreateServerReport(dhcpServer.Name);
            }


            Save("DHCP infrastructure");

            SetStatus($"Report for DHCP infrastructure of domain {_domain} created!");


        }
        catch (Exception ex)
        {
            var msg = $"DhcpExecuter:Run:Maybe not enough permissions to access DHCP settings:{ex.Message}";
            result = false;
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        return result;
    }

    private void CreateServerReport(string dhcpServerName)
    {
        var dhcpServer = DhcpServer.Connect(dhcpServerName);

        Result.AddHeader1($"DHCP server {dhcpServerName}");
        Result.AddParagraph($"Domain: {_domain}");
        Result.AddHeader2("Basic DHCP Configuration");

        // Display some configuration
        Result.AddDefinitionListLine("Protocol Support:", dhcpServer.Configuration.ApiProtocolSupport.ToString());
        Result.AddDefinitionListLine("Database Name:", dhcpServer.Configuration.DatabaseName);
        Result.AddDefinitionListLine("Database Path:", dhcpServer.Configuration.DatabasePath);

        // Show all bound interfaces
        foreach (var binding in dhcpServer.BindingElements)
        {
            Result.AddDefinitionListLine("Binding Interface Id:", binding.InterfaceGuidId.ToString());
            Result.AddDefinitionListLine("Description:", binding.InterfaceDescription);
            Result.AddDefinitionListLine("Adapter Address:", binding.AdapterPrimaryIpAddress.ToString());
            Result.AddDefinitionListLine("Adapter Subnet:", binding.AdapterSubnetAddress.ToString());
        }


        // Display scope information
        Result.AddHeader2("Scope information");
        foreach (var scope in dhcpServer.Scopes)
        {
            Result.AddHeader3($"Scope '{scope.Name}'");
            Result.AddDefinitionListLine("Address:", scope.Address.ToString());
            Result.AddDefinitionListLine("Mask:", scope.Mask.ToString());
            Result.AddDefinitionListLine("Range:", scope.IpRange.ToString());
            Result.AddDefinitionListLine("State:", scope.State.ToString());

            var activeClients = scope.Clients
                .Where(c => c.AddressState == DhcpServerClientAddressStates.Active);

            Result.AddHeader4("Client leases");

            // Display client information
            foreach (var client in activeClients.OrderBy(x => x.IpAddress.ToString()))
            {
                Result.AddDefinitionListLine(client.IpAddress.ToString(),
                    $"[{client.HardwareAddress}] {client.Name}, Expires: {client.LeaseExpires}");
            }

            Result.AddHeader4("Reservations");
            foreach (var reservation in scope.Reservations.OrderBy(x => x.Address.ToString()))
            {
                Result.AddDefinitionListLine(reservation.Address.ToString(),
                    $"[{reservation.HardwareAddress}] {reservation.Client.Name}");
            }


            Result.AddHeader4("Scope options");
            foreach (var optionValue in scope.Options)
            {
                var s = string.Empty;
                foreach (var value in optionValue.Values)
                {
                    s += $"{value.Value} [{value.Type}]\r\n";
                }

                Result.AddDefinitionListLine($"{optionValue.Option.Name} [{optionValue.OptionId}]:", s);
            }
        }

    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public DhcpExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (DhcpJobTask)jobTask;
    }
}