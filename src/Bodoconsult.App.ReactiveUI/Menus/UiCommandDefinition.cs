// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Menus;

/// <summary>
/// Basic implementation of <see cref="IUiCommandDefinition"/>
/// </summary>
public class UiCommandDefinition : IUiCommandDefinition
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="executeMethod">Action to execute with the command</param>
    /// <param name="canExecuteMethod">Condition func to return true if the ExcuteMethod may run or false if not</param>
    public UiCommandDefinition(Action executeMethod, Func<bool> canExecuteMethod)
    {
        ExecuteMethod = executeMethod;
        CanExecuteMethod = canExecuteMethod;
    }

    /// <summary>
    /// Action to execute with the command
    /// </summary>
    public Action ExecuteMethod { get;  }

    /// <summary>
    /// Condition func to return true if the ExcuteMethod may run or false if not
    /// </summary>
    public Func<bool> CanExecuteMethod { get;  }
}