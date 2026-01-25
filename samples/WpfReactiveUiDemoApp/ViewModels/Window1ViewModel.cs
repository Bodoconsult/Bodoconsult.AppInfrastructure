// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using Bodoconsult.App.Wpf.ReactiveUI.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.SourceGenerators;

namespace WpfReactiveUiDemoApp.ViewModels
{
    public partial class Window1ViewModel: ReactiveObject, IReactiveUiWindowViewModel
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="regionManager">Current region manager instance</param>
        public Window1ViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        /// <summary>
        /// Current region manager
        /// </summary>
        public IRegionManager RegionManager { get; }

        /// <summary>
        /// Region 1
        /// </summary>
        [Reactive]
        public partial UiRegion? Region1 { get; set; }

        /// <summary>
        /// Region 2
        /// </summary>
        [Reactive]
        public partial UiRegion? Region2 { get; set; }

        /// <summary>
        /// Region 3
        /// </summary>
        [Reactive]
        public partial UiRegion? Region3 { get; set; }

    }
}
