using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

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
            overlayWindow.Show();
            //overlayWindow.Visibility = Visibility.Hidden;
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

            systrayIcon.DoubleClick += SystrayIcon_DoubleClick;
        }

        private void SystrayIcon_DoubleClick(object sender, EventArgs e)
        {

            if (overlayWindow.Visibility.Equals(Visibility.Visible))
            {
                overlayWindow.Visibility = Visibility.Hidden;
                return;
            }

            overlayWindow.Visibility = Visibility.Visible;
        }
    }
}
