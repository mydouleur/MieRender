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
    internal class BackGroundPipeLine
    {
        public GLView view { get; set; }
        private IWindow IWindow => view.IWindow;
        private GL openGL => view.openGL;


        private Shader BackGroundShader;
        private Shader BackGroundTextureShader;
        private BitTexture BackGroundTexture;

        private uint fbo;
        private uint fboTexture;
        private uint detTexture;
        private uint fullScreenQuadVao;
        public unsafe BackGroundPipeLine(GLView view)
        {
            this.view = view;
            BackGroundShader = new Shader(openGL, "Shader\\ScreenQuad\\screenQuad.vert", "Shader\\ScreenQuad\\screenQuad.frag");
            BackGroundTextureShader = new Shader(openGL, "Shader\\ScreenQuad\\screenQuadTexture.vert", "Shader\\ScreenQuad\\screenQuadTexture.frag");
           // BackGroundTexture = new BitTexture(openGL, "Images\\background.png", TextureUnit.Texture0);
            fullScreenQuadVao = CreateFullScreenQuad();
        }
        public unsafe void DoRender()
        {
            openGL.BindVertexArray(fullScreenQuadVao);
            if (BackGroundTexture != null)
            {
                BackGroundTexture.Bind();
                BackGroundTextureShader.Use();
                BackGroundTextureShader.SetUniform("uTexture", 0);
            }
            else
            {
                BackGroundShader.Use();
            }
            // 渲染全屏四边形
            openGL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        }

        private unsafe uint CreateFullScreenQuad()
        {
            // 定义全屏四边形的顶点数据
            float[] quadVertices = {
        // 位置 (x, y)     纹理坐标 (u, v)
        -1.0f,  -1.0f,     0.0f, 1.0f, // 左上
        -1.0f, 1.0f,     0.0f, 0.0f, // 左下
         1.0f, - 1.0f,     1.0f, 1.0f, // 右上
         1.0f, 1.0f,     1.0f, 0.0f  // 右下
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
