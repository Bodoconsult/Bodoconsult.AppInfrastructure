// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Web.Mail.Models;

/// <summary>
/// Pwd keys infrastructure
/// </summary>
public class PwdKeys
{
    /// <summary>
    /// Key 1
    /// </summary>
    public string Key1 { get; set; } = "abc";

    /// <summary>
    /// Key 2
    /// </summary>
    public string Key2 { get; set; } = "def";

    /// <summary>
    /// Key 3
    /// </summary>
    public string Key3 { get; set; } = "ghi";

    /// <summary>
    /// Salt
    /// </summary>
    public byte[] Salt { get; set; }

    /// <summary>
    /// Number of iterations
    /// </summary>
    public static int Iterations { get; set; } = 255;
}