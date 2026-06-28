// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions;

namespace Bodoconsult.App.Delegates;

/// <summary>
/// Delegate for a method creating a business transaction
/// </summary>
/// <returns>The requested business transaction</returns>
public delegate BusinessTransaction CreateBusinessTransactionDelegate();

/// <summary>
/// 
/// Run a business transaction
/// </summary>
/// <param name="requestData">Request data</param>
/// <returns>A buiness action reply</returns>

public delegate IBusinessTransactionReply RunBusinessTransactionDelegate(IBusinessTransactionRequestData requestData);