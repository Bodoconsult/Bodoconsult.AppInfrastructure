// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bodoconsult.App.Wpf.ReactiveUI.Models
{

    /// <summary>
    /// Represents a menu item for a menubar
    /// </summary>
    public partial class BaseMenuItem : ReactiveObject
    {
        
        /// <summary>
        /// Ctor providing data
        /// </summary>
        /// <param name="header">Header text</param>
        /// <param name="command"></param>
        public BaseMenuItem(string header, ReactiveCommand<Unit, Unit> command)
        {
            _header = header;
            _command = command;
        }

        /// <summary>
        /// Default ctor
        /// </summary>
        public BaseMenuItem()
        { }

        /// <summary>
        /// Header text
        /// </summary>
        [Reactive] private string _header;

        /// <summary>
        /// Contains submenu items of the current menu item
        /// </summary>
        [Reactive]
        private List<BaseMenuItem> _items = new();

        /// <summary>
        /// Command to be executed by the menu item
        /// </summary>
        [Reactive] private ReactiveCommand<Unit, Unit> _command;

        /// <summary>
        /// Command name
        /// </summary>
        [Reactive] private string _commandName;

        /// <summary>
        /// Icon for the command button
        /// </summary>
        [Reactive] private object _icon;

        /// <summary>
        /// Is checkable?
        /// </summary>
        [Reactive] private bool _isCheckable;

        /// <summary>
        /// Is checked?
        /// </summary>
        [Reactive] private bool _isChecked;

        /// <summary>
        /// Is visible?
        /// </summary>

        [Reactive] private bool _visible;

        /// <summary>
        /// Is separator
        /// </summary>
        [Reactive] private bool _isSeparator;

        /// <summary>
        /// Input gesture text
        /// </summary>
        [Reactive] private string _inputGestureText;

        /// <summary>
        /// Tooltip
        /// </summary>
        [Reactive] private string _toolTip;

        /// <summary>
        /// ID in the menu hierarchy
        /// </summary>
        [Reactive] private int _menuHierarchyId;

        /// <summary>
        /// ID of the parent in the menu hierarchy
        /// </summary>
        [Reactive] private int _parentMenuHierarchyId;

        /// <summary>
        /// Icon path
        /// </summary>
        [Reactive] private string _iconPath;

        /// <summary>
        /// Is for admins only
        /// </summary>
        [Reactive] private bool _isAdminOnly;

        /// <summary>
        /// Context object
        /// </summary>
        [Reactive] private object _context;

        /// <summary>
        /// Integer sequence
        /// </summary>
        [Reactive] private int _intSequence;

        /// <summary>
        /// Integer key index
        /// </summary>
        [Reactive] private int _intKeyIndex;
    }
}
