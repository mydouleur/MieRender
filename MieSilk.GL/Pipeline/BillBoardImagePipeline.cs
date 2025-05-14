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
    public class BillBoardImagePipeline
    {
        public GLView view { get; set; }
        private IWindow IWindow => view.IWindow;
        private OrthoCamera Camera => view.Camera;
        private GL openGL => view.openGL;

        private VertexArrayObject<Vertex, uint> pointVao => view.pointVao;
        private BufferObject<Vertex> pointVbo => view.pointVbo;
        private Shader billboardImage;
        private BitTexture billboardTexture;
        public BillBoardImagePipeline(GLView view)
        {
            this.view = view;
            billboardImage = new Shader(openGL, "Shader\\Billboard\\billboard.vert", "Shader\\Billboard\\billboard.geom", "Shader\\Billboard\\billboard.frag");
          //  billboardTexture = new BitTexture(openGL, "Images\\background.png", TextureUnit.Texture0);
            billboardTexture = new BitTexture(openGL, "Images\\LYZ.jpg", TextureUnit.Texture0);

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
            billboardImage.Use();
            billboardImage.SetUniform("uCameraMatrix", Camera.CameraMatrix);
            billboardImage.SetUniform("Width", (float)size.X);
            billboardImage.SetUniform("HWRatio", (float)size.Y / size.X);
            billboardTexture.Bind();
            billboardImage.SetUniform("uTexture", 0);
            openGL.PolygonOffset(1.0f, 1.0f); // 调整深度偏移值
            //openGL.PolygonMode(GLEnum.FrontAndBack, GLEnum.Line);
            openGL.DrawArrays(PrimitiveType.Points, 0, (uint)pointVbo.Length);
            //openGL.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);
        }
    }
}
