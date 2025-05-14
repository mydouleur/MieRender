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
    internal class PartPipeline_Grayscale : IPartPipeline
    {
        public GLView view { get; set; }
        private IWindow IWindow => view.IWindow;
        private OrthoCamera Camera => view.Camera;
        private GL openGL => view.openGL;
        private BufferObject<uint> lineIndicesEbo => view.lineIndicesEbo;
        private BufferObject<uint> meshIndicesEbo => view.meshIndicesEbo;
        private VertexArrayObject<Vertex, uint> meshVao => view.meshVao;
        private Shader meshShader_Default => view.meshShader_Default;
        private Shader meshShader_NoLight => view.meshShader_NoLight;
        private Shader lineShader_Default => view.lineShader_Default;
        private Shader lineShader_Stipple => view.lineShader_Stipple;
        private SolidGeometry3D RenderTarget => view.RenderTarget;
        private readonly Vector4[] colors;
        public PartPipeline_Grayscale(GLView view)
        {
            this.view = view;
            colors = ColorHelper.GetColors_2();
        }
        public unsafe void DoRender()
        {
            openGL.Enable(EnableCap.Blend);
            openGL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            openGL.DepthFunc(DepthFunction.Lequal);  // 恢复默认深度测试函数
            meshVao.Bind();
           

            #region mesh绘制
            if (Camera.Width < 50000)
            {
                openGL.Enable(EnableCap.CullFace);
                openGL.CullFace(GLEnum.Back);
                meshIndicesEbo.Bind();
                meshShader_Default.Use();
                meshShader_Default.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                meshShader_Default.SetUniform("cLook", Camera.LookDirection);
                meshShader_Default.SetUniform("cUp", Camera.UpDirection);
                meshShader_Default.SetUniform("ObjectColor", colors);

                openGL.PolygonOffset(2.0f, 2.0f); // 调整深度偏移值
                openGL.DrawElements(PrimitiveType.Triangles, (uint)meshIndicesEbo.Length, DrawElementsType.UnsignedInt, null);
                openGL.Disable(EnableCap.CullFace);

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
            #endregion
            #region line绘制
            if (Camera.Width < 10000)
            {
                lineIndicesEbo.Bind();
                lineShader_Default.Use();
                lineShader_Default.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                openGL.PolygonOffset(2.0f, 1.0f); // 调整深度偏移值
                openGL.DrawElements(PrimitiveType.Lines, (uint)lineIndicesEbo.Length, DrawElementsType.UnsignedInt, null);
                if (Camera.Width < 5000)
                {
                    openGL.DepthFunc(DepthFunction.Always);  // 线段总是通过深度测试
                    lineShader_Stipple.Use();
                    lineShader_Stipple.SetUniform("uCameraMatrix", Camera.CameraMatrix);
                    openGL.PolygonOffset(2.0f, 1.0f); // 调整深度偏移值
                    openGL.DrawElements(PrimitiveType.Lines, (uint)lineIndicesEbo.Length, DrawElementsType.UnsignedInt, null);
                }
            }
            #endregion
        }
    }
}
