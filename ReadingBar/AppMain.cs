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
        }

        private void InitalizeOverlayWindow()
        {
            overlayWindow = new BarOverlayWindow();
            overlayWindow.Visibility = Visibility.Hidden;
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
    }
}
