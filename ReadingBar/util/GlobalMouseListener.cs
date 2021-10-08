using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Win32Point = System.Drawing.Point;

namespace ReadingBar.util
{
    class GlobalMouseListener
    {
        private IntPtr hWnd;
        private EventHandler<Win32Point> handler;
        private Win32Point RawMousePoint = new Win32Point();

        public GlobalMouseListener(IntPtr hWnd, EventHandler<Win32Point> eventHandler)
        {
            this.hWnd = hWnd;
            this.handler = eventHandler;

            StartMouseListeningThread();
        }

        private void StartMouseListeningThread()
        {
            new Thread(() =>
            {
                double dY = 0;
                double dX = 0;

                while (true)
                {
                    NativeFunctions.GetCursorPos(ref RawMousePoint);
                    NativeFunctions.ScreenToClient(hWnd, ref RawMousePoint);

                    if (RawMousePoint.Y != dY && RawMousePoint.X != dX)
                    {
                        // send point to listner
                        handler(this, RawMousePoint);
                    }
                    Thread.Sleep(1);
                }
            })
            {
                IsBackground = true
            }.Start();

        }
    }
}
