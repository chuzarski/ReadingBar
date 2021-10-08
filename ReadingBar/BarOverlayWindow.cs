using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using Win32Point = System.Drawing.Point;

using ReadingBar.util;

namespace ReadingBar
{
    public partial class BarOverlayWindow : Window
    {
        private IntPtr hWnd;
        private Matrix pointTransformMatrix;
        private Point wpfPoint = new Point();
        private Point translatedMousePoint = new Point();

        private Rectangle OverlayRect;
        private GlobalMouseListener MouseListener;

        private double ShadingOpacity;

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
            // here we can start passing around win handles
            hWnd = new WindowInteropHelper(this).Handle;
            
            SetWindowLayered();

            // Here is where we would wire the mouse listener
            pointTransformMatrix = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            MouseListener = new GlobalMouseListener(hWnd, MouseMoved_Hander);
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
            bgColorBrush = new SolidColorBrush(Colors.Black){ Opacity = 0.05 };
            Background = bgColorBrush;

            // create canvas for window
            overlayCanvas = new Canvas() { Background = Brushes.Transparent } ;

            // Apply canvas to window
            Content = overlayCanvas;

            // create rectangle
            OverlayRect = new Rectangle()
            {
                Fill = Brushes.Yellow,
                Opacity = 1,
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


        /// Event Handlers

        public void MouseMoved_Hander(object sender, Win32Point pt)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                // Scale point based on local scaling
                // Since the Transform Matrix will not take Win32 points, we have to convert to a WPF point
                // Turns out it is quicker to call the Win32 function and convert from one point format to another
                wpfPoint.X = pt.X;
                wpfPoint.Y = pt.Y;
                translatedMousePoint = pointTransformMatrix.Transform(wpfPoint);

                // Move our bar
                Canvas.SetTop(OverlayRect, CalculateRectangleMidpoint(translatedMousePoint.Y));
            }));
        }


        /// util functions
        private double CalculateRectangleMidpoint(double Y) => Y - (OverlayRect.Height / 2);

    }
}
