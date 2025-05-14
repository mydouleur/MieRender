using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Mie.Silk.Base;
using Mie.Silk.OpenGL;
using Mie.View.Tools;
using Silk.NET.Windowing;

namespace Mie.View
{
    public class MieHost : HwndHost
    {
        private IntPtr _hwndHost;
        private IWindow window;
        public ISilkView View;

        public MieHost()
        {
            var view = new GLView();
            view.Camera = new OrthoCamera(view);
            Task.Run(() =>
            {
                view.Init();
                view.DoRender();
            });
            Thread.Sleep(1000);
            View = view;
            this.SizeChanged += MieHost_SizeChanged;

        }

        private void MieHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (View.EnableRender == false)
            {
                View.EnableRender = true;
                LazyRenderDisable(10);
            }
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            window = View.IWindow;
            // 创建 Win32 窗口
            _hwndHost = window.Native.Win32.Value.Hwnd;
            SetWindowLong(_hwndHost, GWL_STYLE, WS_CHILD | WS_EX_NOACTIVATE);
            long exStyle = GetWindowLong(_hwndHost, GWL_EXSTYLE);
            // 设置 Win32 窗口的父窗口为 WPF 窗口
            SetParent(_hwndHost, hwndParent.Handle);
            // 设置 Win32 窗口的大小和位置
            ResizeWindow((int)this.Width, (int)this.Height);
            //SetLayeredWindowAttributes(_hwndHost, 0, 255, LWA_CCOLORKEY);
            var href = new HandleRef(this, _hwndHost);
            return href;
        }

        public async void LazyRenderDisable(int milliseconds = 1000)
        {
            await Task.Delay(milliseconds);
            this.View.EnableRender = false;
        }
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            // 销毁 Win32 窗口
            window.IsClosing = true;
            // window.Reset() ;
            // window.Dispose();
        }

        private void ResizeWindow(int width, int height)
        {
            SetWindowPos(_hwndHost, IntPtr.Zero, 0, 0, width, height, SWP_NOZORDER);
        }

        #region P/Invoke 声明

        private const int GWL_STYLE = -16;
        private const int WS_CHILD = 0x40000000;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;
        private const long GWL_EXSTYLE = -20;
        private const long WS_EX_NOACTIVATE = 0x08000000L;
        private const long WS_EX_LAYERED = 0x00080000;
        private const long WS_EX_COMPOSITED = 0x02000000L;
        private const long LWA_CCOLORKEY = 0x00000001;
        private const long WS_CLIPCHILDREN = 0x02000000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, long nIndex, long dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern long GetWindowLong(IntPtr hWnd, long nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte bAlpha, long dwFlags);
        #endregion
    }
}

