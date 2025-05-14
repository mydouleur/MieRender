using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Mie.Silk.Base;
using Mie.Silk.Base.TestData;
using Mie.Silk.Base.Tools;
using Mie.Silk.OpenGL.Interface;
using Mie.Silk.OpenGL.Pipeline;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Video;
using Silk.NET.Windowing;

namespace Mie.Silk.OpenGL
{
    public class GLView : ISilkView
    {
        public IWindow IWindow { get; set; }
        internal GL openGL;
        public bool EnableRender
        {
            get
            {
                return _enbaleRender;
            }
            set
            {
                _enbaleRender = value;
            }
        }
        private bool _enbaleRender = true;
        private bool _needUpdate = false;

        //测试变量
        private double timer = 0;
        #region 缓冲区
        internal BufferObject<Vertex> meshVbo;
        internal BufferObject<Vertex> pointVbo;
        internal BufferObject<ImagePositon> imagePositonVbo;
        internal BufferObject<uint> lineIndicesEbo;
        internal BufferObject<uint> meshIndicesEbo;
        internal VertexArrayObject<Vertex, uint> meshVao;
        internal VertexArrayObject<Vertex, uint> pointVao;
        internal VertexArrayObject<ImagePositon, uint> imageVao;
        internal BufferObject<Vertex> boltVbo;
        internal VertexArrayObject<Vertex, uint> boltVao;
        //圆边线测试缓冲区
        internal BufferObject<uint> cylinderEdgeEbo;
        //旋转中心缓冲区
        internal BufferObject<Vertex> rotateTargetVbo;
        internal VertexArrayObject<Vertex, uint> rotateTargetVao;
        #endregion
        #region 着色器
        internal Shader meshShader_Default;
        internal Shader meshShader_NoLight;
        // internal Shader meshShader_TransCover;
        internal Shader lineShader_Default;
        internal Shader lineShader_Stipple;
        internal Shader lineShader_Color;
        #endregion
        #region 纹理
        internal BitTexture billboardTexture;
        #endregion
        #region 渲染目标
        internal SolidGeometry3D RenderTarget;
        internal SolidGeometry3D BoltTarget;
        #endregion
        #region 渲染管道
        private IPartPipeline[] partPipelines;
        private QuadLinePipeLine quadLinePipeLine;
        private QuadPointPipeline quadPointPipeLine;
        private BillBoardImagePipeline billBoardImagePipeline;
        private PostEffectPipeLine_PartEdgeLight postEffect_PartEdgeLight;
        private BackGroundPipeLine backGroundPipeLine;
        private CylinderEdgePipeLine cylinderEdgePipeLine;
        private WeldSignPipeline weldSignPipeline;
        private BoltPipeline boltPipeline;
        private RotateTargetPipeline rotateTargetPipeline;
        #endregion
        //常态零件渲染模式
        public int PartRenderType { get; set; } = 0;
        //相机
        public OrthoCamera Camera { get; set; }
        public GLView() { }
        public void Init()
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(1920, 1080);
            IWindow = Window.Create(options);
            IWindow.VSync = false;
            IWindow.IsVisible = false;
            IWindow.Title = "如果看见这个，证明你设置子父级窗口失败了";
            IWindow.Load += OnLoad;
            IWindow.Update += OnUpdate;
            IWindow.FramebufferResize += OnFramebufferResize;
            IWindow.Render += OnRender;
            IWindow.Closing += OnClose;
            IWindow.Initialize();
        }

        public async void DoRender()
        {
            IWindow.Run(() =>
            {
                IWindow.DoEvents();
                if (!IWindow.IsClosing && EnableRender)
                {
                    IWindow.DoUpdate();
                }
                if (!IWindow.IsClosing && EnableRender)
                {
                    IWindow.DoRender();
                }
                if (!EnableRender)
                {
                    Thread.Sleep(10);
                }
            });
            IWindow.Dispose();
        }
        private void OnLoad()
        {
            openGL = GL.GetApi(IWindow);
            partPipelines = new IPartPipeline[5]
            {
                new PartPipeline_Wireframe(this),
                new PartPipeline_ShadedWireframe(this),
                new PartPipeline_Grayscale(this),
                new PartPipeline_Rendered(this),
                new PartPipeline_OnlySelected(this),
            };
            //quadLinePipeLine = new QuadLinePipeLine(this);
            //quadPointPipeLine = new QuadPointPipeline(this);
            //billBoardImagePipeline = new BillBoardImagePipeline(this);
            //postEffect_PartEdgeLight = new PostEffectPipeLine_PartEdgeLight(this);
            backGroundPipeLine = new BackGroundPipeLine(this);
            cylinderEdgePipeLine = new CylinderEdgePipeLine(this);
            //weldSignPipeline = new WeldSignPipeline(this);
            rotateTargetPipeline = new RotateTargetPipeline(this);
            boltPipeline = new BoltPipeline(this);
            #region 初步Boxes测试区域
            //testSolid = DefaultMesh.GetBoxes(100, 100, 100);
            //LineIndicesEbo = new BufferObject<uint>(openGL, testSolid.LineIndices, BufferTargetARB.ElementArrayBuffer);
            //MeshIndicesEbo = new BufferObject<uint>(openGL, testSolid.MeshIndices, BufferTargetARB.ElementArrayBuffer);
            //vbo = new BufferObject<float>(openGL, testSolid.VertexCollections, BufferTargetARB.ArrayBuffer);
            //vao = new VertexArrayObject<float, uint>(openGL, vbo, LineIndicesEbo);
            #endregion
            #region 初步HBeam测试区域
            RenderTarget = DefaultMesh.GetHbeams(1, 1, 1);
            //RenderTarget = DefaultMesh.GetHbeams(30, 30, 30);
            //RenderTarget = DefaultMesh.GetHbeams(1, 1, 1);
            //RenderTarget = DefaultMesh.HBeam_HN_400_200_8_13;
            //RenderTarget = TestCylinder.TestCylinder_Hex;
            //RenderTarget = TestCylinder.GetCylineders_Hex(100, 100, 100);
            //RenderTarget = TestCylinder.TestCylinder_32Sides;
            //RenderTarget = TestCylinder.GetCylineders_32Sides(1, 1, 1);
            RenderTarget.SetBuffer(openGL, out meshVbo, out lineIndicesEbo, out meshIndicesEbo, out meshVao, out cylinderEdgeEbo, out pointVbo, out pointVao);
            RenderTarget = null;

            //设置旋转点渲染
            rotateTargetVbo = new BufferObject<Vertex>(openGL, new Vertex[1] { new(0, 0, 0) }, BufferTargetARB.ArrayBuffer);
            rotateTargetVao = new VertexArrayObject<Vertex, uint>(openGL, rotateTargetVbo, lineIndicesEbo);
            rotateTargetVao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 1, 0);
            rotateTargetVao.VertexAttributePointer(1, 1, VertexAttribPointerType.Float, 1, 3 * sizeof(float));
            #endregion
            //测试螺栓形状渲染 
            //BoltTarget = TestBoltData.GetTestBolts(100, 100, 100);
            BoltTarget = TestBoltData.GetTestBolts(1, 1, 1);
            BoltTarget.SetBoltBuffer(openGL, lineIndicesEbo, out boltVbo, out boltVao);
            BoltTarget = null;
            #region 公告牌测试区域
            #endregion
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //openGL.Enable(GLEnum.ClipDistance0);
            meshShader_Default = new Shader(openGL, "Shader\\Mesh\\mesh.vert", "Shader\\Mesh\\mesh.geom", "Shader\\Mesh\\mesh.frag");
            meshShader_NoLight = new Shader(openGL, "Shader\\Mesh\\meshNoLight.vert", "Shader\\Mesh\\meshNoLight.frag");
            //meshShader_TransCover = new Shader(openGL, "Shader\\Mesh\\meshNoLight.vert", "Shader\\Mesh\\meshTransCover.frag");
            lineShader_Default = new Shader(openGL, "Shader\\Line\\line.vert", "Shader\\Line\\line.frag");
            lineShader_Stipple = new Shader(openGL, "Shader\\Line\\lineStipple.vert", "Shader\\Line\\lineStipple.geom", "Shader\\Line\\lineStipple.frag");
            lineShader_Color = new Shader(openGL, "Shader\\Line\\lineColorful.vert", "Shader\\Line\\lineColorful.frag");
            openGL.Enable(EnableCap.PolygonOffsetFill);
            openGL.Enable(EnableCap.PolygonOffsetLine);
            openGL.Enable(EnableCap.DepthTest);
            openGL.Enable(EnableCap.ScissorTest);

        }
        private void OnUpdate(double deltaTime)
        {
            if (_needUpdate)
            {
                _needUpdate = false;
                ChangeRenderTargerTest();
            }
        }
        public void ChangeRenderTargerTest()
        {
            var fw = Glfw.GetApi();
            Random random = new Random();
            var a = 37 + random.Next(15);
            var b = 37 + random.Next(15);
            var c = 37 + random.Next(15);
            //(a, b, c) = (47, 47, 47);//十万的梁测试
            //(a, b, c) = (100, 100, 100);//一百万的梁测试
            (a, b, c) = (1, 1, 1);//一百万的梁测试
            var t1 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            //RenderTarget = DefaultMesh.GetHbeams(a, b, c);
            var t2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            BufferDispose();
            //RenderTarget.SetBuffer(openGL, out meshVbo, out lineIndicesEbo, out meshIndicesEbo, out meshVao, out cylinderEdgeEbo, out pointVbo, out pointVao);
            RenderTarget = null;
            BoltTarget = TestBoltData.GetTestBolts(100, 100, 100);
            BoltTarget.SetBoltBuffer(openGL, lineIndicesEbo, out boltVbo, out boltVao);
            BoltTarget = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var t3 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Debug.WriteLine($"共计{a * b * c}根梁，{t2 - t1}ms用于生产几何，{t3 - t2}ms用于切换渲染");
        }
        public void DoTest()
        {
            EnableRender = true;
            _needUpdate = true;
        }
        private void ClearScreen()
        {
            openGL.ClearColor(1f, 1f, 1f, 1f);
            openGL.Clear((uint)(ClearBufferMask.ColorBufferBit));
            backGroundPipeLine.DoRender();
            openGL.Clear((uint)(ClearBufferMask.DepthBufferBit));
        }
        private unsafe void OnRender(double deltaTime)
        {
            ClearScreen();
            Camera.UpdateMatrix();
            this.partPipelines[this.PartRenderType].DoRender();
            //quadLinePipeLine.DoRender();
            cylinderEdgePipeLine.IsColorful = this.PartRenderType == 0;
            if (this.PartRenderType != 4)
            {
                cylinderEdgePipeLine.DoRender();
            }
            boltPipeline.DoRender();
            //postEffect_PartEdgeLight.DoRender();
            openGL.Clear((uint)(ClearBufferMask.DepthBufferBit));
            //quadPointPipeLine.DoRender();
            //weldSignPipeline.DoRender();
            rotateTargetPipeline.DoRender();
            //billBoardImagePipeline.DoRender();
            timer += deltaTime;
            if (timer > 1)
            {
                Debug.WriteLine($"帧率:{1 / deltaTime} ");
                timer -= 1;
            }


        }
        private void OnFramebufferResize(Vector2D<int> newSize)
        {
            openGL.Viewport(newSize);
        }
        private void OnClose()
        {
            meshVbo.Dispose();
            lineIndicesEbo.Dispose();
            meshVao.Dispose();
            meshShader_Default.Dispose();
        }
        private void BufferDispose()
        {
            meshVbo.Dispose();
            lineIndicesEbo.Dispose();
            meshIndicesEbo.Dispose();
            cylinderEdgeEbo.Dispose();
            meshVao.Dispose();
            // GC.Collect();
            // GC.WaitForPendingFinalizers();
        }
    }


}
