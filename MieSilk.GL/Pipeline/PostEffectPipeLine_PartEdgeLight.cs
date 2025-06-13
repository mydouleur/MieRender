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
using Mie.Silk.Base.TestData;

namespace Mie.Silk.OpenGL.Pipeline
{
    internal class PostEffectPipeLine_PartEdgeLight
    {

        public GLView view { get; set; }
        private IWindow IWindow => view.IWindow;
        private OrthoCamera Camera => view.Camera;
        private GL openGL => view.openGL;
        private BufferObject<uint> lineIndicesEbo;
        private BufferObject<uint> meshIndicesEbo;
        private BufferObject<Vertex> meshVbo;
        private VertexArrayObject<Vertex, uint> meshVao;
        private Shader meshShader_Default => view.meshShader_Default;
        private Shader meshShader_NoLight => view.meshShader_NoLight;
        private Shader lineShader_Default => view.lineShader_Default;


        private Shader partEdgeLight;
        private SolidGeometry3D RenderTarget;
        private readonly Vector4[] colors;

        private uint fbo;
        private uint fboTexture;
        private uint detTexture;
        private uint fullScreenQuadVao;
        public unsafe PostEffectPipeLine_PartEdgeLight(GLView view)
        {
            this.view = view;
            colors = ColorHelper.GetColors_3();
            partEdgeLight = new Shader(openGL, "Shader\\PostEffect\\partEdgeHighLight.vert", "Shader\\PostEffect\\partEdgeHighLight.frag");


            //RenderTarget = DefaultMesh.GetHbeams(100, 100, 100);
            RenderTarget = DefaultMesh.HBeam_HN_400_200_8_13;
            lineIndicesEbo = new BufferObject<uint>(openGL, RenderTarget.LineIndices, BufferTargetARB.ElementArrayBuffer);
            meshIndicesEbo = new BufferObject<uint>(openGL, RenderTarget.MeshIndices, BufferTargetARB.ElementArrayBuffer);
            meshVbo = new BufferObject<Vertex>(openGL, RenderTarget.VertexCollections, BufferTargetARB.ArrayBuffer);
            meshVao = new VertexArrayObject<Vertex, uint>(openGL, meshVbo, lineIndicesEbo);
            meshVao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 1, 0);
            meshVao.VertexAttributePointer(1, 1, VertexAttribPointerType.Float, 1, 3 * sizeof(float));
            RenderTarget = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();





            //尝试补全帧缓冲的部分
            openGL.GenFramebuffers(1, out fbo);
            openGL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);

            openGL.GenTextures(1, out fboTexture);
            openGL.BindTexture(GLEnum.Texture2D, fboTexture);

            openGL.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Linear);
            openGL.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Linear);
            openGL.TexParameterI(GLEnum.Texture2D, GLEnum.TextureWrapS, (int)GLEnum.ClampToEdge);
            openGL.TexParameterI(GLEnum.Texture2D, GLEnum.TextureWrapT, (int)GLEnum.ClampToEdge);
            openGL.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgb, (uint)IWindow.FramebufferSize.X, (uint)IWindow.FramebufferSize.Y, 0, GLEnum.Rgb, GLEnum.UnsignedByte, null);
            openGL.FramebufferTexture2D(GLEnum.Framebuffer, FramebufferAttachment.ColorAttachment0, GLEnum.Texture2D, fboTexture, 0);


            openGL.GenTextures(1, out detTexture);
            openGL.BindTexture(GLEnum.Texture2D, detTexture);

            openGL.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Linear);
            openGL.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Linear);
            openGL.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.DepthComponent, (uint)IWindow.FramebufferSize.X, (uint)IWindow.FramebufferSize.Y, 0, GLEnum.DepthComponent, GLEnum.Float, null);
            openGL.FramebufferTexture2D(GLEnum.Framebuffer, FramebufferAttachment.DepthAttachment, GLEnum.Texture2D, detTexture, 0);


            // 检查帧缓冲完整性
            if (openGL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != GLEnum.FramebufferComplete)
            {
                throw new Exception("Framebuffer is not complete!");
            }

            // 解绑帧缓冲
            openGL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            fullScreenQuadVao = CreateFullScreenQuad();
        }
        public unsafe void DoRender()
        {
            // 绑定帧缓冲
            openGL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
            openGL.BindTexture(GLEnum.Texture2D, fboTexture);
            openGL.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgb, (uint)IWindow.FramebufferSize.X, (uint)IWindow.FramebufferSize.Y, 0, GLEnum.Rgb, GLEnum.UnsignedByte, null);
            float texelSizeX = 1.0f / (uint)IWindow.FramebufferSize.X;
            float texelSizeY = 1.0f / (uint)IWindow.FramebufferSize.Y;
            openGL.ClearColor(1, 1, 1, 0);
            openGL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            openGL.DepthFunc(DepthFunction.Lequal);  // 恢复默认深度测试函数
            meshVao.Bind();


            //#region mesh绘制
            //if (Camera.Width < 50000)
            //{
            //    meshIndicesEbo.Bind();
            //    meshShader_Default.Use();
            //    meshShader_Default.SetUniform("uCameraMatrix", Camera.CameraMatrix);

            //    meshShader_Default.SetUniform("cLook", Camera.LookDirection);
            //    meshShader_Default.SetUniform("cUp", Camera.UpDirection);
            //    meshShader_Default.SetUniform("ObjectColor", colors);
            //    openGL.PolygonOffset(0.0f, 1.0f); // 调整深度偏移值
            //    openGL.DrawElements(PrimitiveType.Triangles, (uint)meshIndicesEbo.Length, DrawElementsType.UnsignedInt, null);
            //}
            //else
            //{
            meshIndicesEbo.Bind();
            meshShader_NoLight.Use();
            meshShader_NoLight.SetUniform("uCameraMatrix", Camera.CameraMatrix);
            meshShader_NoLight.SetUniform("ObjectColor", colors);
            openGL.PolygonOffset(0.0f, 1.0f); // 调整深度偏移值
            openGL.DrawElements(PrimitiveType.Triangles, (uint)meshIndicesEbo.Length, DrawElementsType.UnsignedInt, null);
            //}
            //#endregion

            //#region line绘制
            //if (Camera.Width < 50000)
            //{
            //    lineIndicesEbo.Bind();
            //    lineShader_Default.Use();
            //    lineShader_Default.SetUniform("uCameraMatrix", Camera.CameraMatrix);
            //    openGL.PolygonOffset(2.0f, 1.0f); // 调整深度偏移值
            //    openGL.DrawElements(PrimitiveType.Lines, (uint)lineIndicesEbo.Length, DrawElementsType.UnsignedInt, null);
            //}
            //#endregion

            // 解绑帧缓冲
            openGL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            // 后处理渲染
            //openGL.ClearColor(1, 1, 1, 0);
            // openGL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            partEdgeLight.Use();

            // 绑定帧缓冲纹理
            openGL.ActiveTexture(GLEnum.Texture0);
            openGL.BindTexture(GLEnum.Texture2D, fboTexture);
            partEdgeLight.SetUniform("uTexture", 0);
            partEdgeLight.SetUniform("uTexelSize", new Vector2(texelSizeX, texelSizeY));
            // 绑定全屏四边形 VAO
            openGL.BindVertexArray(fullScreenQuadVao);
            openGL.Enable(EnableCap.Blend);
            openGL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            // 渲染全屏四边形
            openGL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        }

        private unsafe uint CreateFullScreenQuad()
        {
            // 定义全屏四边形的顶点数据
            float[] quadVertices = {
        // 位置 (x, y)     纹理坐标 (u, v)
        -1.0f,  1.0f,     0.0f, 1.0f, // 左上
        -1.0f, -1.0f,     0.0f, 0.0f, // 左下
         1.0f,  1.0f,     1.0f, 1.0f, // 右上
         1.0f, -1.0f,     1.0f, 0.0f  // 右下
    };

            // 创建 VAO 和 VBO
            uint vao = openGL.GenVertexArray();
            uint vbo = openGL.GenBuffer();

            openGL.BindVertexArray(vao);
            openGL.BindBuffer(GLEnum.ArrayBuffer, vbo);

            // 上传顶点数据
            fixed (float* ptr = quadVertices)
            {
                openGL.BufferData(GLEnum.ArrayBuffer, (nuint)(quadVertices.Length * sizeof(float)), ptr, GLEnum.StaticDraw);
            }

            // 设置顶点属性指针
            openGL.EnableVertexAttribArray(0);
            openGL.VertexAttribPointer(0, 2, GLEnum.Float, false, 4 * sizeof(float), (void*)0);

            openGL.EnableVertexAttribArray(1);
            openGL.VertexAttribPointer(1, 2, GLEnum.Float, false, 4 * sizeof(float), (void*)(2 * sizeof(float)));

            // 解绑
            openGL.BindBuffer(GLEnum.ArrayBuffer, 0);
            openGL.BindVertexArray(0);

            return vao;
        }
    }
}
