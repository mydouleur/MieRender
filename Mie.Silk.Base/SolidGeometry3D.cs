using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenAL;
using Silk.NET.Vulkan;

namespace Mie.Silk.Base
{
    public struct Vertex
    {
        public float X;
        public float Y;
        public float Z;
        public float Color;
        public Vertex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            Color = 10;
            //Color = 174;
        }
        public Vertex(float x, float y, float z, float color)
        {
            X = x;
            Y = y;
            Z = z;
            Color = color;
        }
        public static Vertex operator +(Vertex vertex, Vector3 v3)
        {
            return new Vertex(vertex.X + v3.X, vertex.Y + v3.Y, vertex.Z + v3.Z, vertex.Color);
        }
        public override string ToString()
        {
            return $"<{this.X},{this.Y},{this.Z},{this.Color}>";
        }
    }
    public class SolidGeometry3D : ICloneable
    {
        public SolidGeometry3D() { }
        public Vertex[] VertexCollections { get; set; } = new Vertex[0];

        public Vertex[] ContourPointCollections { get; set; } = new Vertex[0];
        public uint[] MeshIndices { get; set; } = new uint[0];
        public uint[] LineIndices { get; set; } = new uint[0];

        public uint[] CyLinderEdge { get; set; } = new uint[0];
        public static SolidGeometry3D operator +(SolidGeometry3D left, SolidGeometry3D right)
        {
            List<Vertex> vertexList = new List<Vertex>();
            var leftvertexCount = (uint)left.VertexCollections.Length;
            vertexList.AddRange(left.VertexCollections);
            vertexList.AddRange(right.VertexCollections);
            List<uint> meshIndicesList = new List<uint>();
            meshIndicesList.AddRange(left.MeshIndices);
            meshIndicesList.AddRange(right.MeshIndices.Select(i => i + leftvertexCount));
            List<uint> lineIndicesList = new List<uint>();
            lineIndicesList.AddRange(left.LineIndices);
            lineIndicesList.AddRange(right.LineIndices.Select(i => i + leftvertexCount));
            List<uint> cylinderEdgeList = new List<uint>();
            cylinderEdgeList.AddRange(left.CyLinderEdge);
            cylinderEdgeList.AddRange(right.CyLinderEdge.Select(i => i + leftvertexCount));
            return new SolidGeometry3D()
            {
                VertexCollections = vertexList.ToArray(),
                MeshIndices = meshIndicesList.ToArray(),
                LineIndices = lineIndicesList.ToArray(),
                CyLinderEdge = cylinderEdgeList.ToArray(),
            };
        }
        public SolidGeometry3D Copy()
        {
            return new SolidGeometry3D()
            {
                VertexCollections = VertexCollections.Clone() as Vertex[],
                LineIndices = LineIndices.Clone() as uint[],
                MeshIndices = MeshIndices.Clone() as uint[],
                CyLinderEdge = CyLinderEdge.Clone() as uint[],
                ContourPointCollections = ContourPointCollections.Clone() as Vertex[],
            };
        }
        public object Clone()
        {
            return this.Copy();
        }
    }
}
