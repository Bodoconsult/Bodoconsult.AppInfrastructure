// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.WinForms.Interfaces;
using Timer = System.Windows.Forms.Timer;
// ReSharper disable LocalizableElement

namespace Bodoconsult.App.WinForms.AppStarter.Forms;

/// <summary>
/// Main window of the app
/// </summary>
public sealed partial class MainWindow : Form
{

    private IMainWindowViewModel _viewModel;

    private const int CpNocloseButton = 0x200;

    private readonly Timer _timer = new()
        {
            Interval = 1000
        };

    private bool _isClosing;

    /// <summary>
    /// Default ctor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Inject the view model
    /// </summary>
    /// <param name="mainWindowViewModel"></param>
    public void InjectViewModel(IMainWindowViewModel mainWindowViewModel)
    {
        _viewModel = mainWindowViewModel;
        Logo.Image = _viewModel.Logo;

        Text = _viewModel.AppVersion;

        var appStartParameters = _viewModel.AppBuilder.AppGlobals.AppStartParameter;

        AppTitle.ForeColor = _viewModel.HeaderBackColor;
        AppTitle.Text = appStartParameters.AppName;

        AppLine.BackColor = _viewModel.HeaderBackColor;

        MsgServerIsListeningOnPort.Text = appStartParameters.Port == 0 ? string.Empty : $"{UiMessages.MsgServerIsListeningOnPort} {appStartParameters.Port}";
        MsgHowToShutdownServer.Text = UiMessages.MsgHowToShutdownServer;
        MsgServerProcessId.Text = $"{UiMessages.MsgServerProcessId} {Process.GetCurrentProcess().Id}";

        LogWindow.DataBindings.Add(new Binding(nameof(LogWindow.Text), _viewModel, "LogData"));

        _timer.Tick += TimerOnTick;
        _timer.Enabled = true;
        _timer.Start();
    }

    private void TimerOnTick(object sender, EventArgs e)
    {
        _timer.Stop();

        try
        {
            _viewModel.CheckLogs();
        }
        catch //(Exception exception)
        {
            // Do nothing
        }


        //LogWindow.SelectionStart = LogWindow.Text.Length;
        //LogWindow.SelectionLength = 0;

        _timer.Start();
    }


    /// <summary>
    /// Hide the Close button
    /// </summary>
    protected override CreateParams CreateParams
    {
        get
        {
            var myCp = base.CreateParams;
            myCp.ClassStyle = myCp.ClassStyle | CpNocloseButton;
            return myCp;
        }
    }


    private void MainWindow_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Check if STRG+C is pressed: if no break here else shutdown
        if (ModifierKeys != Keys.Control || e.KeyChar != '\u0003')
        {
            return;
        }

        _isClosing = true;

        _viewModel.ShutDown();

        Close();
    }

    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
    {

        if (_isClosing)
        {
            _timer.Enabled = true;
            _timer.Stop();
            return;
        }

        // Turn off closing via UI: only STRG+C is allowed
        e.Cancel = true;
    }

    private void LogWindow_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Check if STRG+C is pressed: if no break here else shutdown
        if (ModifierKeys != Keys.Control || e.KeyChar != '\u0003')
        {
            return;
        }

        _isClosing = true;

        AsyncHelper.FireAndForget(() =>
        {
            _viewModel.ShutDown();
            CloseMe();
        });
    }

    /// <summary>
    /// Thread-safe form closing
    /// </summary>
    private void CloseMe()
    {
        Invoke((MethodInvoker)Close);
    }

    private void AppTitle_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        MessageBox.Show(_viewModel.AppBuilder.AppGlobals.AppStartParameter.SoftwareTeam, "Developer team members", MessageBoxButtons.OK);
    }

    private void MainWindow_Resize(object sender, EventArgs e)
    {
        try
        {
            AppHeader.Left = 0;
            AppHeader.Width = Width;
            AppHeader.Top = 0;

            AppLine.Left = 0;
            AppLine.Width = Width;

            LogWindow.Width = Width - 3 * LogWindow.Left;
            LogWindow.Height = Height - LogWindow.Top - 40;
        }
        catch
        {
            // Do nothing
        }

    }
}