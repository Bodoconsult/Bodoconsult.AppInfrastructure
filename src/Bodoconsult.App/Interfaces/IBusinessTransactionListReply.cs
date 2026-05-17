// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Interfaces;

/// <summary>
/// Basic content of a business transaction reply returning a list of data
/// </summary>
public interface IBusinessTransactionListReply: IBusinessTransactionReply
{
    /// <summary>
    /// Current number of pages (if applicable)
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Current number of rows provided by this call (if applicable)
    /// </summary>
    public int RowCount { get; set; }

}