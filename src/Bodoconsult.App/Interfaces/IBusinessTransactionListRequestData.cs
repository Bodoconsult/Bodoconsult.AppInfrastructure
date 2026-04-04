// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Interfaces;

/// <summary>
/// Interface for defining minimum requirements request data for business transaction replying data lists
/// </summary>
public interface IBusinessTransactionListRequestData: IBusinessTransactionRequestData
{
        
    /// <summary>
    /// Current page number (if applicable)
    /// </summary>
    int Page { get; set; }
    
    /// <summary>
    /// Current page size (if applicable)
    /// </summary>
    int PageSize  { get; set; }
}