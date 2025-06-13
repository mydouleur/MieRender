using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mie.Silk.Base.Tools
{
    public class GLBReader
    {
        public static SolidGeometry3D Convert(string glbFilePath)
        {
            var model = ModelRoot.Load(glbFilePath);
            var geometry = new SolidGeometry3D();

            // 合并所有网格的顶点和索引
            var vertices = new System.Collections.Generic.List<Vertex>();
            var indices = new System.Collections.Generic.List<uint>();
            
            foreach (Mesh mesh in model.LogicalMeshes)
            {
                var color = 3f;
                var lastVertexCount = vertices.Count;
                //    var temp = positions.OrderBy(v => v.X).First();
                foreach (var primitive in mesh.Primitives)
                {
                    // 1. 提取顶点坐标（POSITION属性）
                    var positions = primitive.GetVertexAccessor("POSITION").AsVector3Array();
                    foreach (var pos in positions)
                    {
                        vertices.Add(new Vertex { X = pos.X , Y = pos.Y , Z = pos.Z, Color = color });
                    }
                    // 2. 提取三角面片索引
                    var primitiveIndices = primitive.GetIndices();
                    if (primitiveIndices != null)
                    {
                        indices.AddRange(primitiveIndices.Select(i => (uint)(i + lastVertexCount)));
                    }
                    color = (int)((color + 1) % 255);
                    lastVertexCount = vertices.Count;
                }
            }
            var move = vertices.OrderBy(v=> v.X).FirstOrDefault();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = vertices[i] + new System.Numerics.Vector3(-move.X, -move.Y, -move.Z);
            }
            geometry.VertexCollections = vertices.ToArray();
            geometry.MeshIndices = indices.ToArray();
            return geometry;
        }
    }
}
