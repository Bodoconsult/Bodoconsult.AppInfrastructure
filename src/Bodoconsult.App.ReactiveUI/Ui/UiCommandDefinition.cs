// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using ReactiveUI.Primitives;

namespace Bodoconsult.App.ReactiveUI.Ui;

/// <summary>
/// Basic implementation of <see cref="IUiCommandDefinition"/>
/// </summary>
public class UiCommandDefinition : IUiCommandDefinition
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="executeMethod">Action to execute with the command</param>
    /// <param name="canExecuteMethod">Observable to return true if the ExcuteMethod may run or false if not</param>
    public UiCommandDefinition(Func<Task<RxVoid>> executeMethod, IObservable<bool>? canExecuteMethod)
    {
        ExecuteMethod = executeMethod;
        CanExecuteMethod = canExecuteMethod;
    }

    /// <summary>
    /// Async task to execute with the command
    /// </summary>
    public Func<Task<RxVoid>> ExecuteMethod { get;  }

    /// <summary>
    /// Condition func to return true if the ExcuteMethod may run or false if not
    /// </summary>
    public IObservable<bool>? CanExecuteMethod { get;  }
}