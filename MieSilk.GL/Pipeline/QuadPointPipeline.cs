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
    internal class QuadPointPipeline
    {

        public GLView view { get; set; }
        private IWindow IWindow => view.IWindow;
        private OrthoCamera Camera => view.Camera;
        private GL openGL => view.openGL;

        private VertexArrayObject<Vertex, uint> pointVao => view.pointVao;
        private BufferObject<Vertex> pointVbo => view.pointVbo;
        private Shader quadPoint;
        public QuadPointPipeline(GLView view)
        {
            this.view = view;
            quadPoint = new Shader(openGL, "Shader\\QuadPoint\\quadPoint.vert", "Shader\\QuadPoint\\quadPoint.geom", "Shader\\QuadPoint\\quadPoint.frag");
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
            quadPoint.Use();
            quadPoint.SetUniform("uCameraMatrix", Camera.CameraMatrix);
            quadPoint.SetUniform("Width", (float)size.X);
            quadPoint.SetUniform("HWRatio", (float)size.Y / size.X);
            openGL.PolygonOffset(1.0f, 1.0f); // 调整深度偏移值
            //openGL.PolygonMode(GLEnum.FrontAndBack, GLEnum.Line);
            openGL.DrawArrays(PrimitiveType.Points, 0, (uint)pointVbo.Length);
            //openGL.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);
        }
    }
}
