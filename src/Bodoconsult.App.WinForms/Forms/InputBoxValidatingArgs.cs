// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.WinForms.Forms;

/// <summary>
/// EventArgs used to validate an InputBox
/// </summary>
public class InputBoxValidatingArgs : EventArgs
{
    /// <summary>
    /// Input text
    /// </summary>
    public string Text;

    /// <summary>
    /// validation message
    /// </summary>
    public string Message;

    /// <summary>
    /// Cancel?
    /// </summary>
    public bool Cancel;
}