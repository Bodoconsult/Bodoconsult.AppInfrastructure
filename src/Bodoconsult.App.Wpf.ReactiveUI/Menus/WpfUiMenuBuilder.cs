// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Menus;

namespace Bodoconsult.App.Wpf.ReactiveUI.Menus
{
    /// <summary>
    /// <see cref="IUiMenuBuilder"/> implementation for WPF menus using default <see cref="Menu"/> as base control
    /// </summary>
    public class WpfUiMenuBuilder: UiMenuBuilderBase
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="translationService">Current translation service</param>
        public WpfUiMenuBuilder(II18N translationService) : base(translationService)
        {
        }
    }
}
