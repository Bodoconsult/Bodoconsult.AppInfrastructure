// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Bodoconsult.App.Wpf.ReactiveUI.Helper;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell
    {

        //private readonly IEventAggregator _eventAggregator;

        public Shell(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            //CreateMainMenu();

            eventAggregator.GetEvent<StatusChangedEvent>().Publish("Load main window...");

            var localDir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

            try
            {
                var icon = Path.Combine(localDir, "icon.ico");

                if (File.Exists(icon))
                {
                    var iconUri = new Uri(Path.Combine(localDir, "icon.ico"), UriKind.RelativeOrAbsolute);
                    Icon = BitmapFrame.Create(iconUri);
                }
            }
            catch
            {
                // ignored
            }

            ApplicationHelper.SetCurrentWindowTitle("Hauptmenü");
        }






    }
}
