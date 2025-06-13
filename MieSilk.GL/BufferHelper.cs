using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mie.Silk.Base;
using Silk.NET.OpenGL;
using Silk.NET.Vulkan;

namespace Mie.Silk.OpenGL
{
    internal static class BufferHelper
    {
        public static void SetBuffer(this SolidGeometry3D RenderTarget, GL openGL, out BufferObject<Vertex> meshVbo, out BufferObject<uint> lineIndicesEbo, out BufferObject<uint> meshIndicesEbo, out VertexArrayObject<Vertex, uint> meshVao, out BufferObject<uint> cylinderEdgeEbo, out BufferObject<Vertex> pointVbo, out VertexArrayObject<Vertex, uint> pointVao)
        {
            lineIndicesEbo = new BufferObject<uint>(openGL, RenderTarget.LineIndices, BufferTargetARB.ElementArrayBuffer);
            meshIndicesEbo = new BufferObject<uint>(openGL, RenderTarget.MeshIndices, BufferTargetARB.ElementArrayBuffer);
            cylinderEdgeEbo = new BufferObject<uint>(openGL, RenderTarget.CyLinderEdge, BufferTargetARB.ElementArrayBuffer);
            meshVbo = new BufferObject<Vertex>(openGL, RenderTarget.VertexCollections, BufferTargetARB.ArrayBuffer);
            pointVbo = new BufferObject<Vertex>(openGL, RenderTarget.ContourPointCollections, BufferTargetARB.ArrayBuffer);
            meshVao = new VertexArrayObject<Vertex, uint>(openGL, meshVbo, lineIndicesEbo);
            meshVao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 1, 0);
            meshVao.VertexAttributePointer(1, 1, VertexAttribPointerType.Float, 1, 3 * sizeof(float));
            pointVao = new VertexArrayObject<Vertex, uint>(openGL, pointVbo, lineIndicesEbo);
            pointVao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 1, 0);
            pointVao.VertexAttributePointer(1, 1, VertexAttribPointerType.Float, 1, 3 * sizeof(float));
             
        }
        public static void SetBoltBuffer(this SolidGeometry3D BoltTarget,GL openGL,BufferObject<uint> any,out BufferObject<Vertex> boltVbo,out VertexArrayObject<Vertex,uint> boltVao)
        {
            boltVbo = new BufferObject<Vertex>(openGL, BoltTarget.VertexCollections, BufferTargetARB.ArrayBuffer);
            boltVao = new VertexArrayObject<Vertex, uint>(openGL, boltVbo, any);
            boltVao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 1, 0);
            boltVao.VertexAttributePointer(1, 1, VertexAttribPointerType.Float, 1, 3 * sizeof(float));
        }
    }
}
