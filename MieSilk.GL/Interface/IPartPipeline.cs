using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mie.Silk.OpenGL.Interface
{
    internal interface IPartPipeline
    {
        public GLView view { get; set; }
        public unsafe void DoRender();
    }
}
