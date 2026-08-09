// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

/*
 * Copyright 2021 Kazuhiro Fujieda <fujieda@roundwide.com>
   
   Permission to use, copy, modify, and/or distribute this software for any
   purpose with or without fee is hereby granted.
   
   THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH
   REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY
   AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT,
   INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
   LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR
   OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR
   PERFORMANCE OF THIS SOFTWARE.
 */

using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Bodoconsult.App.Abstractions.Interfaces;
using Application = System.Windows.Application;

namespace Bodoconsult.App.Wpf.AppStarter;

/// <summary>
/// Handles the creation of a tasbar icon
/// </summary>
public class NotifyIconWrapper : FrameworkElement, IDisposable
{
    private static readonly RoutedEvent OpenSelectedEvent = EventManager.RegisterRoutedEvent("OpenSelected",
        RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(NotifyIconWrapper));

    private static readonly RoutedEvent ExitSelectedEvent = EventManager.RegisterRoutedEvent("ExitSelected",
        RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(NotifyIconWrapper));

    private readonly NotifyIcon _notifyIcon;

    /// <summary>
    /// Default ctor
    /// </summary>
    public NotifyIconWrapper()
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        OpenMenuText = "Open";
        ExitMenuText = "Exit";

        _notifyIcon = new NotifyIcon
        {
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
            Visible = true,
            ContextMenuStrip = CreateContextMenu()
        };
        _notifyIcon.DoubleClick += OpenItemOnClick;
        Application.Current.Exit += (obj, args) => { _notifyIcon.Dispose(); };
    }

    /// <summary>
    /// Dependecy property Text
    /// </summary>
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(NotifyIconWrapper), new PropertyMetadata(
            (d, e) =>
            {
                var notifyIcon = ((NotifyIconWrapper)d)._notifyIcon;
                if (notifyIcon is null)
                {
                    return;
                }
                notifyIcon.Text = (string)e.NewValue;
            }));

    /// <summary>
    /// Dependecy property OpenMenuText
    /// </summary>
    public static readonly DependencyProperty OpenMenuTextProperty =
        DependencyProperty.Register("OpenMenuText", typeof(string), typeof(NotifyIconWrapper), new PropertyMetadata(
            (d, e) =>
            {
                var wr = (NotifyIconWrapper)d;
                if (wr is null)
                {
                    return;
                }
                wr.OpenMenuText = (string)e.NewValue;
            }));

    /// <summary>
    /// Dependecy property ExitMenuText
    /// </summary>
    public static readonly DependencyProperty ExitMenuTextProperty =
        DependencyProperty.Register("ExitMenuText", typeof(string), typeof(NotifyIconWrapper), new PropertyMetadata(
            (d, e) =>
            {
                var wr = (NotifyIconWrapper)d;
                if (wr is null)
                {
                    return;
                }
                wr.ExitMenuText = (string)e.NewValue;
            }));

    /// <summary>
    /// Dependecy property NotifyRequest
    /// </summary>
    private static readonly DependencyProperty NotifyRequestProperty =
        DependencyProperty.Register("NotifyRequest", typeof(NotifyRequestRecord), typeof(NotifyIconWrapper),
            new PropertyMetadata(
                (d, e) =>
                {
                    var r = (NotifyRequestRecord)e.NewValue;
                    ((NotifyIconWrapper)d)._notifyIcon?.ShowBalloonTip(r.Duration, r.Title, r.Text, ToolTipIcon.Info);
                }));

    /// <summary>
    /// Menu text for open menu in system tray bar
    /// </summary>
    public string OpenMenuText
    {
        get => (string)GetValue(OpenMenuTextProperty);
        set => SetValue(OpenMenuTextProperty, value);
    }

    /// <summary>
    /// Menu text for exit menu in system tray bar
    /// </summary>
    public string ExitMenuText
    {
        get => (string)GetValue(ExitMenuTextProperty);
        set => SetValue(ExitMenuTextProperty, value);
    }

    /// <summary>
    /// Text
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Notifaction request record
    /// </summary>
    public NotifyRequestRecord NotifyRequest
    {
        get => (NotifyRequestRecord)GetValue(NotifyRequestProperty);
        set => SetValue(NotifyRequestProperty, value);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _notifyIcon?.Dispose();
    }

    /// <summary>
    /// Event handler for OpenSelected
    /// </summary>
    public event RoutedEventHandler OpenSelected
    {
        add => AddHandler(OpenSelectedEvent, value);
        remove => RemoveHandler(OpenSelectedEvent, value);
    }

    /// <summary>
    /// Event handler for ExitSelected
    /// </summary>
    public event RoutedEventHandler ExitSelected
    {
        add => AddHandler(ExitSelectedEvent, value);
        remove => RemoveHandler(ExitSelectedEvent, value);
    }

    private ContextMenuStrip CreateContextMenu()
    {
        var openItem = new ToolStripMenuItem();
        var myBinding = new Binding("Text", this, "OpenMenuText");
        openItem.DataBindings.Add(myBinding);
        openItem.Click += OpenItemOnClick;

        var exitItem = new ToolStripMenuItem();
        myBinding = new Binding("Text", this, "ExitMenuText");
        exitItem.DataBindings.Add(myBinding);
        exitItem.Click += ExitItemOnClick;
        var contextMenu = new ContextMenuStrip {Items = {openItem, exitItem}};
        return contextMenu;
    }

    private void OpenItemOnClick(object sender, EventArgs eventArgs)
    {
        var args = new RoutedEventArgs(OpenSelectedEvent);
        RaiseEvent(args);
    }

    private void ExitItemOnClick(object sender, EventArgs eventArgs)
    {
        var args = new RoutedEventArgs(ExitSelectedEvent);
        RaiseEvent(args);
    }
}