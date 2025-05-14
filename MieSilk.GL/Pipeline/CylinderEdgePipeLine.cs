using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Mie.Silk.Base.Tools;
using Mie.Silk.Base;
using Mie.Silk.OpenGL.Interface;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Runtime.CompilerServices;
using System.Drawing;
using Silk.NET.Vulkan;

namespace Mie.Silk.OpenGL.Pipeline
{
    internal class CylinderEdgePipeLine
    {

        public GLView view { get; set; }
        public bool IsColorful { get; set; }
        private IWindow IWindow => view.IWindow;
        private OrthoCamera Camera => view.Camera;
        private GL openGL => view.openGL;
        private VertexArrayObject<Vertex, uint> meshVao => view.meshVao;
        private BufferObject<uint> cylinederEdgeEbo => view.cylinderEdgeEbo;

        private Shader lineShader_Color => view.lineShader_Color;
        private Shader cyLinderEdgeShader;

        private readonly Vector4[] colors;
        private readonly Vector4[] unColors;

        public CylinderEdgePipeLine(GLView view)
        {
            this.view = view;
            colors = ColorHelper.GetColors_3();
            unColors = ColorHelper.GetDefaultColors();
            cyLinderEdgeShader = new Shader(openGL, "Shader\\Line\\CylinderEdge.vert", "Shader\\Line\\CylinderEdge.geom", "Shader\\Line\\CylinderEdge.frag");

        }
        public unsafe void DoRender()
        {

            openGL.DepthFunc(DepthFunction.Lequal);  // 恢复默认深度测试函数
            if (Camera.Width < 50000)
            {
                meshVao.Bind();
                cylinederEdgeEbo.Bind();

                Vector2 size = new Vector2(view.IWindow.FramebufferSize.X, view.IWindow.FramebufferSize.Y);
                cyLinderEdgeShader.Use();
                cyLinderEdgeShader.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                cyLinderEdgeShader.SetUniform("uView", Camera.View);
                if (IsColorful)
                {
                    cyLinderEdgeShader.SetUniform("ObjectColor", colors);
                }
                else
                {
                    cyLinderEdgeShader.SetUniform("ObjectColor", unColors);
                }
                // cyLinderEdgeShader.SetUniform("vScreenSize", size);
                openGL.PolygonOffset(2.0f, 1.0f); // 调整深度偏移值
                openGL.DrawElements(PrimitiveType.Triangles, (uint)cylinederEdgeEbo.Length, DrawElementsType.UnsignedInt, null);

            }


        }
    }
}

