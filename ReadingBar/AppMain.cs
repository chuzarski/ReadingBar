using System;
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

        // state
        private bool overlayWasHiddenBeforeSettings = false;

        [STAThread]
        static void Main()
        {
            AppMain appMain = new AppMain();
            appMain.Run();
        }

        public AppMain()
        {
            // Initalize all components
            InitalizeOverlayWindow();
            InitalizeSystrayIcon();
            InitalizeSystrayMenu();
            InitalizeSettingsWindow();


            this.Exit += AppMain_Exit;
        }

        private void InitalizeOverlayWindow()
        {
            overlayWindow = new BarOverlayWindow()
            {
                BGShadingOpacity = (Convert.ToDouble(ReadingBar.Properties.Settings.Default.BGShadingOpacity) / 100.0),
                BarOpacity = (Convert.ToDouble(ReadingBar.Properties.Settings.Default.BarOpacity) / 100.0),
                BarHeight = Convert.ToDouble(ReadingBar.Properties.Settings.Default.BarSize)
            };

            // listen for settings changes
            ReadingBar.Properties.Settings.Default.PropertyChanged += OverlayProperty_Changed;
            overlayWindow.Visibility = Visibility.Hidden;
        }

        private void InitalizeSettingsWindow()
        {
            settingsWindow = new SettingsWindow();
            settingsWindow.Closing += SettingsWindow_Closing;
            settingsWindow.ResetButton.Click += SettingsWindow_ResetButton_Click;
            settingsWindow.IsVisibleChanged += SettingsWindow_IsVisibleChanged;
        }

        private void InitalizeSystrayIcon()
        {
            var comp = new System.ComponentModel.Container();
            systrayIcon = new NotifyIcon(comp)
            {
                Visible = true,
                Text = "ReadingBar",
                Icon = ReadingBar.Properties.Resources.readingbar_icon
            };

            systrayIcon.DoubleClick += Toggle_Overlay;
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
            ReadingBar.Properties.Settings.Default.Save(); // Save our settings just in case of a crash later on
        }

        public void SettingsWindow_ResetButton_Click(object sender, RoutedEventArgs e) => ReadingBar.Properties.Settings.Default.Reset();

        /// <summary>
        /// The whole point of this function is to prepare the overlay menu for the user
        /// The idea is, the overlay window will pop up, but disable mouse tracking for the user. 
        /// This way, the user focuses on changing settings instead of the bar following their mouse.
        /// 
        /// If the user DIDN'T have the overlay open when accessing the settings, the overlay will be closed when the settings pane is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // in this case, we are closing the settings window
            if(settingsWindow.Visibility.Equals(Visibility.Hidden))
            {
                overlayWindow.DisableMouseTracking = false; // re-enable mouse tracking
                if(overlayWasHiddenBeforeSettings)
                {
                    // hide overlay
                    overlayWindow.Visibility = Visibility.Hidden;
                    overlayWasHiddenBeforeSettings = false;
                }
                return;
            }

            // here we remember that the user opened the settings without the overlay window open
            if (!overlayWindow.IsVisible)
            {
                overlayWasHiddenBeforeSettings = true;
                overlayWindow.Visibility = Visibility.Visible;
            }

            overlayWindow.DisableMouseTracking = true;
            overlayWindow.CenterOverlayBar();
        }

        /// <summary>
        /// This handler listens for changes in user settings and applies them to the overlay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverlayProperty_Changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "BarOpacity":
                    overlayWindow.BarOpacity = (Convert.ToDouble(ReadingBar.Properties.Settings.Default.BarOpacity) / 100.0);
                    break;
                case "BarSize":
                    overlayWindow.BarHeight = Convert.ToDouble(ReadingBar.Properties.Settings.Default.BarSize);
                    break;
                case "BGShadingOpacity":
                    overlayWindow.BGShadingOpacity = (Convert.ToDouble(ReadingBar.Properties.Settings.Default.BGShadingOpacity) / 100.0);
                    break;
            }
        }

        private void AppMain_Exit(object sender, ExitEventArgs e)
        {
            ReadingBar.Properties.Settings.Default.Save();
            systrayIcon.Dispose();
        }
    }
}
