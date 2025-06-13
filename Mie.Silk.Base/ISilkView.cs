using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;

namespace Mie.Silk.Base
{
    public interface ISilkView
    {
        IWindow IWindow { get; set; }
        OrthoCamera Camera { get; set; }
        public bool EnableRender { get; set; }
        public int PartRenderType { get; set; }
        void Init();
        void DoRender();

    }
}
