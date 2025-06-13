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
using Silk.NET.Vulkan;

namespace Mie.Silk.OpenGL.Pipeline
{
    internal class QuadLinePipeLine
    {

        public GLView view { get; set; }
        private IWindow IWindow => view.IWindow;
        private OrthoCamera Camera => view.Camera;
        private GL openGL => view.openGL;
        private BufferObject<uint> lineIndicesEbo => view.lineIndicesEbo;
        private BufferObject<uint> meshIndicesEbo => view.meshIndicesEbo;

        private VertexArrayObject<Vertex, uint> meshVao => view.meshVao;
        private Shader lineShader_Color => view.lineShader_Color;
        private Shader meshShader_NoLight => view.meshShader_NoLight;
        private Shader quadLine;
        private SolidGeometry3D RenderTarget => view.RenderTarget;
        private readonly Vector4[] colors;
        public QuadLinePipeLine(GLView view)
        {
            this.view = view;
            colors = ColorHelper.GetColors_3();
            quadLine = new Shader(openGL, "Shader\\QuadLine\\quadLine.vert", "Shader\\QuadLine\\quadLine.geom", "Shader\\QuadLine\\quadLine.frag");
        }
        public unsafe void DoRender()
        {
            openGL.DepthFunc(DepthFunction.Lequal);  // 恢复默认深度测试函数
            meshVao.Bind();
            var size = IWindow.FramebufferSize;
            if (size.X == 0)
            {
                size.X = 1;
            }

            lineIndicesEbo.Bind();
            quadLine.Use();
            quadLine.SetUniform("uCameraMatrix", Camera.CameraMatrix);
            quadLine.SetUniform("Width", (float)size.X);

            openGL.PolygonOffset(-1.0f, -1.0f); // 调整深度偏移值
            openGL.DrawElements(PrimitiveType.Lines, (uint)lineIndicesEbo.Length, DrawElementsType.UnsignedInt, null);
        }
    }
}
