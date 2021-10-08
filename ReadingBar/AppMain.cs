using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace ReadingBar
{
    class AppMain : Application
    {
        private BarOverlayWindow overlayWindow;
        private NotifyIcon systrayIcon;
        private SettingsWindow settingsWindow;

        [STAThread]
        static void Main()
        {
            AppMain appMain = new AppMain();
            appMain.Run();
        }

        public AppMain()
        {
            InitalizeOverlayWindow();
            InitalizeSystrayIcon();
            InitalizeSettingsWindow();
            this.Exit += AppMain_Exit;
        }

        private void InitalizeOverlayWindow()
        {
            overlayWindow = new BarOverlayWindow();
            overlayWindow.Visibility = Visibility.Hidden;
        }

        private void InitalizeSettingsWindow()
        {
            settingsWindow = new SettingsWindow();
            settingsWindow.Closing += SettingsWindow_Closing;
            settingsWindow.ResetButton.Click += SettingsWindow_ResetButton_Click;
        }

        private void InitalizeSystrayIcon()
        {
            var comp = new System.ComponentModel.Container();
            systrayIcon = new NotifyIcon(comp)
            {
                Visible = true,
                Text = "ReadingBar",
                Icon = SystemIcons.Application, // This will be changed to reflect the application icon
            };

            systrayIcon.DoubleClick += Toggle_Overlay;

            InitalizeSystrayMenu();
        }

        private void InitalizeSystrayMenu()
        {
            ToolStripItem[] menuItems =
            {
                new ToolStripMenuItem("Toggle", null, Toggle_Overlay),
                new ToolStripMenuItem("Settings", null, Systray_Toggle_AppSettings),
                new ToolStripMenuItem("Quit", null, Systray_Quit)
            };

            systrayIcon.ContextMenuStrip = new ContextMenuStrip();
            systrayIcon.ContextMenuStrip.Items.AddRange(menuItems);
        }

        private void Toggle_Overlay(object sender, EventArgs e)
        {

            if (overlayWindow.Visibility.Equals(Visibility.Visible))
            {
                overlayWindow.Visibility = Visibility.Hidden;
                return;
            }

            overlayWindow.Visibility = Visibility.Visible;
        }

        private void Systray_Quit(object sender, EventArgs e)
        {
            systrayIcon.Dispose();
            Shutdown();
        }
        private void Systray_Toggle_AppSettings(object sender, EventArgs e)
        {
            if(settingsWindow == null)
            {
                settingsWindow = new SettingsWindow();
            }

            if(settingsWindow.IsVisible)
            {
                return;
            }

            settingsWindow.Visibility = Visibility.Visible;
        }

        private void SettingsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            settingsWindow.Hide();
            ReadingBar.Properties.Settings.Default.Save();
        }

        public void SettingsWindow_ResetButton_Click(object sender, RoutedEventArgs e) => ReadingBar.Properties.Settings.Default.Reset();

        private void AppMain_Exit(object sender, ExitEventArgs e)
        {
            ReadingBar.Properties.Settings.Default.Save();
            systrayIcon.Dispose();
        }
    }
}
