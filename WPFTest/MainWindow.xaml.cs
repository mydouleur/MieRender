#define GL
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Mie.Silk.OpenGL;
using Mie.Silk.DirectX;
namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // DispatcherTimer timer2 = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            //this.PreviewMouseMove += MainWindow_PreviewMouseMove;
        }
    }
}