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

namespace Mie.Silk.OpenGL.Pipeline
{
    internal class PartPipeline_Wireframe : IPartPipeline
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

        private SolidGeometry3D RenderTarget => view.RenderTarget;
        private readonly Vector4[] colors;
        public PartPipeline_Wireframe(GLView view)
        {
            this.view = view;
            colors = ColorHelper.GetColors_3();
        }
        public unsafe void DoRender()
        {

            openGL.DepthFunc(DepthFunction.Lequal);  // 恢复默认深度测试函数
            meshVao.Bind();
           
            if (Camera.Width < 50000)
            {

                lineIndicesEbo.Bind();
                lineShader_Color.Use();
                lineShader_Color.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                lineShader_Color.SetUniform("ObjectColor", colors);
                openGL.PolygonOffset(2.0f, 1.0f); // 调整深度偏移值

                openGL.DrawElements(PrimitiveType.Lines, (uint)lineIndicesEbo.Length, DrawElementsType.UnsignedInt, null);
            }
            else
            {
                meshIndicesEbo.Bind();
                meshShader_NoLight.Use();
                meshShader_NoLight.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                meshShader_NoLight.SetUniform("ObjectColor", colors);
                openGL.PolygonOffset(2.0f, 2.0f); // 调整深度偏移值
                openGL.DrawElements(PrimitiveType.Triangles, (uint)meshIndicesEbo.Length, DrawElementsType.UnsignedInt, null);
            }

        }
    }
}

