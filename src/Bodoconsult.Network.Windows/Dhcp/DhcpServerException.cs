// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Text;
using Bodoconsult.Network.Windows.Dhcp.Native;

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// DHCP server exception
/// </summary>
public class DhcpServerException : Exception
{
    private readonly DhcpErrors _error;
    private readonly string _message;

    internal DhcpServerException(string apiFunction, DhcpErrors error, string additionalMessage)
    {
        ApiFunction = apiFunction;
        _error = error;
        ApiErrorMessage = BuildApiErrorMessage(error);
        _message = BuildMessage(apiFunction, additionalMessage, error, ApiErrorMessage);
    }

    internal DhcpServerException(string apiFunction, DhcpErrors error)
        : this(apiFunction, error, null)
    { }

    /// <summary>
    /// Name of an API function 
    /// </summary>
    public string ApiFunction { get; }

    /// <summary>
    /// API error
    /// </summary>
    public string ApiError => _error.ToString();

    internal DhcpErrors ApiErrorNative => _error;
    
    /// <summary>
    /// API error ID
    /// </summary>
    public uint ApiErrorId => (uint)_error;

    /// <summary>
    /// API error message
    /// </summary>
    public string ApiErrorMessage { get; }

    /// <summary>Gets a message that describes the current exception.</summary>
    /// <returns>The error message that explains the reason for the exception, or an empty string ("").</returns>
    public override string Message => _message;

    private static string BuildApiErrorMessage(DhcpErrors error)
    {
        var errorType = typeof(DhcpErrors).GetMember(error.ToString());
        if (errorType.Length != 0)
        {
            var errorAttribute = errorType[0].GetCustomAttributes(typeof(DhcpErrorDescriptionAttribute), false);

            if (errorAttribute.Length != 0)
            {
                return ((DhcpErrorDescriptionAttribute)errorAttribute[0]).Description;
            }
        }

        return "Unknown Error";
    }

    private static string BuildMessage(string apiFunction, string additionalMessage, DhcpErrors error, string apiErrorMessage)
    {
        var builder = new StringBuilder();

        if (apiFunction != null)
        {
            builder.Append("An error occurred calling '").Append(apiFunction).Append("'. ");
        }

        if (additionalMessage != null)
        {
            builder.Append(additionalMessage).Append(". ");
        }

        builder.Append(apiErrorMessage).Append(" [").Append(error.ToString()).Append(' ').Append((uint)error).Append(']');

        return builder.ToString();
    }
}