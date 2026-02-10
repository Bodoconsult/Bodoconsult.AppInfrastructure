// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Test.BufferPool;

/// <summary>
/// Test class for resetable classes implementing <see cref="IResetable"/>
/// </summary>
internal class TestDataResetable: IResetable
{
    /// <summary>
    /// Date with default null
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Reset the class to default values
    /// </summary>
    public void Reset()
    {
        Date = null;
    }
}