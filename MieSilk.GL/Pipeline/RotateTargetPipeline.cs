using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Mie.Silk.Base;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Mie.Silk.OpenGL.Pipeline
{
    internal class RotateTargetPipeline
    {

        public GLView view { get; set; }
        private IWindow IWindow => view.IWindow;
        private OrthoCamera Camera => view.Camera;
        private GL openGL => view.openGL;
        private VertexArrayObject<Vertex, uint> rotateTargetVao => view.rotateTargetVao;
        private BufferObject<Vertex> rotateTargetVbo => view.rotateTargetVbo;
        private Shader rotateTargetSign;
        public RotateTargetPipeline(GLView view)
        {
            this.view = view;
            rotateTargetSign = new Shader(openGL, "Shader\\Special\\rotateTarget.vert", "Shader\\Special\\rotateTarget.geom", "Shader\\Special\\rotateTarget.frag");
        }
        public unsafe void DoRender()
        {
            openGL.DepthFunc(DepthFunction.Lequal);  // 恢复默认深度测试函数
            rotateTargetVao.Bind();
            var size = IWindow.FramebufferSize;
            if (size.X == 0)
            {
                size.X = 1;
                size.Y = 1;
            }
            (var w1, var w2) = (20000, 50000);
            if (Camera.Width < w1)
            {
                rotateTargetSign.Use();
                rotateTargetSign.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                rotateTargetSign.SetUniform("Width", (float)size.X);
                rotateTargetSign.SetUniform("HWRatio", (float)size.Y / size.X);
                rotateTargetSign.SetUniform("SignSize", 1f);
                openGL.PolygonOffset(1.0f, 1.0f); // 调整深度偏移值
                openGL.DrawArrays(PrimitiveType.Points, 0, (uint)rotateTargetVbo.Length);
            }
            else if (Camera.Width < w2)
            {
                rotateTargetSign.Use();
                rotateTargetSign.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                rotateTargetSign.SetUniform("Width", (float)size.X);
                rotateTargetSign.SetUniform("HWRatio", (float)size.Y / size.X);
                rotateTargetSign.SetUniform("SignSize", 1 - (Camera.Width - w1) / (w2 - w1));
                openGL.PolygonOffset(1.0f, 1.0f); // 调整深度偏移值
                openGL.DrawArrays(PrimitiveType.Points, 0, (uint)rotateTargetVbo.Length);
            }
        }
    }
}
