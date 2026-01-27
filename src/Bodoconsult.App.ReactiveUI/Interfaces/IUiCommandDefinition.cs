// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Basic definitions to be used to create UI commands
/// </summary>
public interface IUiCommandDefinition
{
    /// <summary>
    /// Action to execute with the command
    /// </summary>
    Action ExecuteMethod { get;  }

    /// <summary>
    /// Condition func to return true if the ExcuteMethod may run or false if not
    /// </summary>
    Func<bool> CanExecuteMethod { get;  }
}