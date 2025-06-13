using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mie.Silk.OpenGL
{
    public class Program
    {
        static void Main(string[] args)
        {
            var view = new GLView();
            view.Init();
            view.DoRender();
        }
    }
}
