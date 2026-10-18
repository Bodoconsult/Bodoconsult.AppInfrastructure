
// <c> Based on http://www.nullify.net/Article/304 by Simon Soanes.
//
// (c) 2008 Simon Soanes, All Rights Reserved.  No warranties express or implied.
// DO NOT redistribute this source code publically without a link to the origin and this copyright
// notice fully intact, also please send any modifications back to me at simon@nullifynetwork.com
// Including in your software is fine although attribution would be nice. </c>
// 

namespace Bodoconsult.Network.Windows.Dns;

/// <summary>
/// The type of record in Microsoft (r) DNS server
/// </summary>
public class DnsRecordType
{
    /// <summary>
    /// Create a new DNS record type
    /// </summary>
    /// <param name="textRepresentation">The type to create</param>
    public DnsRecordType(string textRepresentation)
    {
        _textRepresentation = textRepresentation;
    }
    private readonly string _textRepresentation = "";

    /// <summary>
    /// The text representation of the record type
    /// </summary>
    public string TextRepresentation => _textRepresentation.ToUpper();

    /// <summary>
    /// The mode of the record, usually IN but could oneday be something else like OUT
    /// </summary>
    public string RecordMode { get; set; } = "IN";

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return $"{RecordMode} {_textRepresentation}";
    }

    #region Default values
    /// <summary>
    /// An alias
    /// </summary>
    public static DnsRecordType Cname => new("CNAME");

    /// <summary>
    /// An IPv4 address
    /// </summary>
    public static DnsRecordType A => new("A");

    /// <summary>
    /// A reverse host address inside yoursubnet.in-addr.arpa
    /// </summary>
    public static DnsRecordType Ptr => new("PTR");

    /// <summary>
    /// An MX record (mail exchange)
    /// </summary>
    public static DnsRecordType Mx => new("MX");

    /// <summary>
    /// An IPv6 host address
    /// </summary>
    public static DnsRecordType Aaaa => new("AAAA");

    /// <summary>
    /// A text record
    /// </summary>
    public static DnsRecordType Txt => new("TXT");

    /// <summary>
    /// A nameserver record (domain delegation)
    /// </summary>
    public static DnsRecordType Ns => new("NS");

    /// <summary>
    /// An SOA record (start of authority)
    /// </summary>
    public static DnsRecordType Soa => new("SOA");

    #endregion
}