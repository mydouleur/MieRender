using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mie.Silk.Base;
using Mie.Silk.OpenGL;
using Silk.NET.Vulkan;
using UserControl = System.Windows.Controls.UserControl;

namespace Mie.View.ViewControl
{
    /// <summary>
    /// MieViewPort.xaml 的交互逻辑
    /// </summary>
    public partial class MieViewPort : UserControl
    {
        public OrthoCamera Camera { get; set; }
        public CameraController Controller { get; set; }
        public Vector3 RotationTarget { get; set; } = new Vector3(0, 0, 0);
        public MieViewPort()
        {
            this.Cursor = Cursors.Arrow;
            InitializeComponent();
            this.Camera = viewHost.View.Camera;
            Controller = new(this);
            viewHost.LazyRenderDisable();
            //this.PreviewMouseDown += MieViewPort_PreviewMouseDown;
        }

        private void MieViewPort_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //  throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (viewHost.View as GLView).DoTest();
            viewHost.LazyRenderDisable();
        }

        private void MenuItem_Click_0(object sender, RoutedEventArgs e)
        {
            viewHost.View.PartRenderType = 0;
            viewHost.View.EnableRender = true;
            viewHost.LazyRenderDisable();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            viewHost.View.PartRenderType = 1;
            viewHost.View.EnableRender = true;
            viewHost.LazyRenderDisable();
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            viewHost.View.PartRenderType = 2;
            viewHost.View.EnableRender = true;
            viewHost.LazyRenderDisable();
        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            viewHost.View.PartRenderType = 3;
            viewHost.View.EnableRender = true;
            viewHost.LazyRenderDisable();
        }
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            viewHost.View.PartRenderType = 4;
            viewHost.View.EnableRender = true;
            viewHost.LazyRenderDisable();
        }
    }
}
