// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Avalonia.ReactiveUI.Models;

//public class MenuCommand : ICommand
//{
//    private readonly Action _execute;

//    private readonly Func<bool> _canExecute;

//    public MenuCommand(Action execute, Func<bool> canExecute)
//    {
//        _execute = execute;
//        _canExecute = canExecute;
//    }

//    public void Execute(object parameter)
//    {
//        _execute();
//    }

//    public bool CanExecute(object parameter)
//    {
//        return _canExecute();
//    }

//    private void RaiseCanExecuteChanged()
//    {
//        CommandManager.InvalidateRequerySuggested();
//    }

//    public event EventHandler CanExecuteChanged
//    {
//        add
//        {
//            CommandManager.RequerySuggested += value;
//        }
//        remove
//        {
//            CommandManager.RequerySuggested -= value;
//        }
//    }
//}