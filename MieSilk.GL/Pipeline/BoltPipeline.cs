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
    public class BoltPipeline
    {
        public GLView view { get; set; }
        private IWindow IWindow => view.IWindow;
        private OrthoCamera Camera => view.Camera;
        private GL openGL => view.openGL;

        private BufferObject<Vertex> boltVbo => view.boltVbo;
        private VertexArrayObject<Vertex, uint> boltVao => view.boltVao;
        private Shader boltShape;
        private Shader boltShapeLine;
        private SolidGeometry3D RenderTarget => view.RenderTarget;
        private readonly Vector4[] colors;
        public BoltPipeline(GLView view)
        {
            this.view = view;
            boltShape = new Shader(openGL, "Shader\\Special\\boltShape.vert", "Shader\\Special\\boltShape.geom", "Shader\\Special\\boltShape.frag");
            boltShapeLine = new Shader(openGL, "Shader\\Special\\boltShapeLine.vert", "Shader\\Special\\boltShapeLine.geom", "Shader\\Special\\boltShapeLine.frag");
        }
        public unsafe void DoRender()
        {
            openGL.Enable(EnableCap.CullFace);
            openGL.CullFace(GLEnum.Back);
            openGL.DepthFunc(DepthFunction.Lequal);  // 恢复默认深度测试函数
            boltVao.Bind();
            boltShape.Use();
            boltShape.SetUniform("uCameraMatrix", Camera.CameraMatrix);
            boltShape.SetUniform("cLook", Camera.LookDirection);
            boltShape.SetUniform("cUp", Camera.UpDirection);
            openGL.PolygonOffset(1.0f,1.0f); // 调整深度偏移值
            openGL.DrawArrays(PrimitiveType.Triangles, 0, (uint)boltVbo.Length);
            openGL.Disable(EnableCap.CullFace);
            boltShapeLine.Use();
            boltShapeLine.SetUniform("uCameraMatrix", Camera.CameraMatrix);
            openGL.PolygonOffset(-2.0f, -1.0f); // 调整深度偏移值
            openGL.DrawArrays(PrimitiveType.Triangles, 0, (uint)boltVbo.Length);


        }
    }
}
