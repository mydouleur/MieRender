using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Mie.Silk.Base.Tools;
using Mie.Silk.Base;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Mie.Silk.OpenGL.Pipeline
{
    public class WeldSignPipeline
    {

        public GLView view { get; set; }
        private IWindow IWindow => view.IWindow;
        private OrthoCamera Camera => view.Camera;
        private GL openGL => view.openGL;

        private VertexArrayObject<Vertex, uint> pointVao => view.pointVao;
        private BufferObject<Vertex> pointVbo => view.pointVbo;
        private Shader weldSign;
        private SolidGeometry3D RenderTarget => view.RenderTarget;
        private readonly Vector4[] colors;
        public WeldSignPipeline(GLView view)
        {
            this.view = view;
            weldSign = new Shader(openGL, "Shader\\Special\\weldSign.vert", "Shader\\Special\\weldSign.geom", "Shader\\Special\\weldSign.frag");
        }
        public unsafe void DoRender()
        {
            openGL.DepthFunc(DepthFunction.Lequal);  // 恢复默认深度测试函数
            pointVao.Bind();
            var size = IWindow.FramebufferSize;
            if (size.X == 0)
            {
                size.X = 1;
                size.Y = 1;
            }
            (var w1, var w2) = (20000, 50000);
            if (Camera.Width < w1)
            {
                weldSign.Use();
                weldSign.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                weldSign.SetUniform("Width", (float)size.X);
                weldSign.SetUniform("HWRatio", (float)size.Y / size.X);
                weldSign.SetUniform("SignSize", 1f);
                openGL.PolygonOffset(1.0f, 1.0f); // 调整深度偏移值
                openGL.DrawArrays(PrimitiveType.Points, 0, (uint)pointVbo.Length);
            }
            else if (Camera.Width < w2)
            {
                weldSign.Use();
                weldSign.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                weldSign.SetUniform("Width", (float)size.X);
                weldSign.SetUniform("HWRatio", (float)size.Y / size.X);
                weldSign.SetUniform("SignSize",1- (Camera.Width - w1) / (w2 - w1));
                openGL.PolygonOffset(1.0f, 1.0f); // 调整深度偏移值
                openGL.DrawArrays(PrimitiveType.Points, 0, (uint)pointVbo.Length);
            }

        }
    }
}
