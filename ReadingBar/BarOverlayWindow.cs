using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

using ReadingBar.util;

namespace ReadingBar
{
    public partial class BarOverlayWindow : Window
    {
        private IntPtr hWnd;

        private Rectangle OverlayRect;

        public BarOverlayWindow()
        {
            InitalizeWindow();
        }

        // initalization methods


        /// <summary>
        /// This function is called by WPF
        /// By time it is called, we should be ready to work with the HWND of the WPF window
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            
            base.OnSourceInitialized(e);
            hWnd = new WindowInteropHelper(this).Handle;
            // here we can start passing around win handles
            SetWindowLayered();

            // Here is where we would wire the mouse listener
        }

        /// <summary>
        /// This function initalizes basic window properties, the drawing canvas for the window and the overlay rectange
        /// </summary>
        private void InitalizeWindow()
        {
            SolidColorBrush bgColorBrush;
            Canvas overlayCanvas;

            // set basic window attributes
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Maximized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ShowInTaskbar = false;
            Title = "ReadingBar Overlay";

            // set window background and color
            bgColorBrush = new SolidColorBrush(Colors.Gray){ Opacity = 0.1 };
            Background = bgColorBrush;

            // create canvas for window
            overlayCanvas = new Canvas() { Background = Brushes.Transparent } ;

            // Apply canvas to window
            Content = overlayCanvas;

            // create rectangle
            OverlayRect = new Rectangle()
            {
                Fill = Brushes.Yellow,
                Opacity = 0.4,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 30,
                Width = SystemParameters.PrimaryScreenWidth
            };

            // Apply rect to canvas
            overlayCanvas.Children.Add(OverlayRect);

            // Window should be setup!
        }

        /// <summary>
        /// This function calls Win32 functions that set the extended attributes of the Window.
        /// Since Win32 functions required the window handle, this can only be called after OnSourceInitialized, which is the lifecycle point that we can
        /// acces the hWnd of the WPF window
        /// More info here: https://docs.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles
        /// </summary>
        private void SetWindowLayered()
        {
            // todo throw exception here when hWnd is null
            int winFlags = NativeFunctions.GetWindowLong(hWnd, NativeFunctions.EXTENDED_WINDOW_INDEX);
            NativeFunctions.SetWindowLong(hWnd, NativeFunctions.EXTENDED_WINDOW_INDEX, winFlags | NativeFunctions.WIN_LAYERED | NativeFunctions.WIN_TRANSPARENT);
        }

    }
}
