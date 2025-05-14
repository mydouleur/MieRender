using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mie.View.Tools
{
    public static class CreateWindowHelper
    {
       public static  Window Create()
        {
            var newWindow = new System.Windows.Window();
            newWindow.AllowsTransparency = true;
            newWindow.Background = System.Windows.Media.Brushes.Transparent;
            newWindow.WindowStyle = System.Windows.WindowStyle.None;
            newWindow.Width = 400;
            newWindow.Height = 300;
            //var grid = new System.Windows.Controls.Grid();
            //var text = new System.Windows.Controls.TextBlock();
            //text.BeginInit();
            //text.Text = "阿龙在这里";
            //text.Foreground = System.Windows.Media.Brushes.HotPink;
            //text.FontSize = 108;
            //text.EndInit();
            //grid.Children.Add(text);
            //newWindow.Content = grid;
            newWindow.Show();
            return newWindow;
        }
    }
}
