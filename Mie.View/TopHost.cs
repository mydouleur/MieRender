using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using Mie.View.Tools;

namespace Mie.View
{
    [ContentProperty("Content")]
    public class TopHost : HwndHost
    {
        [System.ComponentModel.Bindable(true)]
        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(TopHost), new(null, (d, e) =>
            {
                var host = d as TopHost;
                host._hwndWindow.Content = e.NewValue;
            }));
        IntPtr _hwndHost { get; set; }
        Window _hwndWindow { get; set; }
        public TopHost()
        {
            var window = CreateWindowHelper.Create();
            _hwndHost = new WindowInteropHelper(window).Handle;
            _hwndWindow = window;
        }
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            SetWindowLong(_hwndHost, GWL_STYLE, WS_CHILD|WS_EX_NOACTIVATE);
            long exStyle = GetWindowLong(_hwndHost, GWL_EXSTYLE);
            // 设置 Win32 窗口的父窗口为 WPF 窗口
            SetParent(_hwndHost, hwndParent.Handle);
            // 设置 Win32 窗口的大小和位置
            ResizeWindow((int)this.Width, (int)this.Height);
            //SetLayeredWindowAttributes(_hwndHost, 0, 255, LWA_CCOLORKEY);
            _hwndWindow.Owner = Application.Current.MainWindow;

            return new HandleRef(this, _hwndHost);
        }
        private void ResizeWindow(int width, int height)
        {
            SetWindowPos(_hwndHost, IntPtr.Zero, 0, 0, width, height, SWP_NOZORDER);
        }
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            _hwndWindow.Close();
            //   throw new NotImplementedException();
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
