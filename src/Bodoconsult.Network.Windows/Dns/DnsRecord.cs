// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

// <c> Based on http://www.nullify.net/Article/304 by Simon Soanes.
//
// (c) 2008 Simon Soanes, All Rights Reserved.  No warranties express or implied.
// DO NOT redistribute this source code publically without a link to the origin and this copyright
// notice fully intact, also please send any modifications back to me at simon@nullifynetwork.com
// Including in your software is fine although attribution would be nice. </c>
// 
using Microsoft.Management.Infrastructure;

namespace Bodoconsult.Network.Windows.Dns;

/// <summary>
/// An entry in a zone
/// </summary>
public class DnsRecord
{
    /// <summary>
    /// Create an class wrapping a DNS record
    /// Defaults to 1 hour TTL
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="recordType"></param>
    /// <param name="target"></param>
    public DnsRecord(string domain, DnsRecordType recordType, string target) :
        this(domain, recordType, target, new TimeSpan(1, 0, 0))
    {
    }

    /// <summary>
    /// Create an class wrapping a DNS record
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="recordType"></param>
    /// <param name="target"></param>
    /// <param name="ttl"></param>
    public DnsRecord(string domain, DnsRecordType recordType, string target, TimeSpan ttl)
    {
        DomainHost = domain;
        Ttl = ttl;
        Target = target;
        RecordType = recordType;
    }

    /// <summary>
    /// Create an class wrapping a DNS record
    /// </summary>
    /// <param name="wmiObject"></param>
    public DnsRecord(CimInstance wmiObject)
    {
        DomainHost = wmiObject.CimInstanceProperties["OwnerName"].Value.ToString();
        Target = wmiObject.CimInstanceProperties["RecordData"].Value.ToString();
        var recordParts = wmiObject.CimInstanceProperties["TextRepresentation"].Value.ToString().Split(' ', '\t');
        if (recordParts.Length > 2)
        {
            //the third offset is the location in the textual version of the data where the record type is.
            //counting from zero that is location 2 in the array.
            RecordType = new DnsRecordType(recordParts[2]);
        }
        Ttl = new TimeSpan(0, 0, Convert.ToInt32(wmiObject.CimInstanceProperties["TTL"].Value.ToString()));
    }


    /// <summary>
    /// The value of the target is what is written to DNS as the value of a record
    /// </summary>
    /// <remarks>For MX records include the priority as a number with a space or tab between it and the actual target</remarks>
    public string Target { get; set; }

        

    /// <summary>
    /// The time that the resolvers should cache this record for
    /// </summary>
    public TimeSpan Ttl { get; set; }

    /// <summary>
    /// The record type
    /// </summary>
    public DnsRecordType RecordType { get; }

    /// <summary>
    /// The location in the DNS system for this record
    /// </summary>
    public string DomainHost { get; set; }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return $"{DomainHost} {RecordType} {Target}";
    }
}