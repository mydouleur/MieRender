using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Mie.Silk.Base.TestData
{
    public class TestCylinder
    {
        public static SolidGeometry3D TestCylinder_Hex => new SolidGeometry3D()
        {
            VertexCollections = new Vertex[]
            {
                    // 底面六边形顶点 (Z = 0)
                    new (-50f, -28.8675f, 0f),  // 0
                    new (0f, -57.735f, 0f),     // 1
                    new (50f, -28.8675f, 0f),   // 2
                    new (50f, 28.8675f, 0f),    // 3
                    new (0f, 57.735f, 0f),      // 4
                    new (-50f, 28.8675f, 0f),   // 5

                    // 顶面六边形顶点 (Z = 100)
                    new (-50f, -28.8675f, 500f), // 6
                    new (0f, -57.735f,500f),    // 7
                    new (50f, -28.8675f, 500f),  // 8
                    new (50f, 28.8675f,500f),   // 9
                    new (0f, 57.735f,500f),     // 10
                    new (-50f, 28.8675f,500f),   // 11
                    
                    //顶面和底面中心点
                    new(0,0,0,6),
                    new(0,0,500,6)
            },
            MeshIndices = new uint[]
            {
                    // 底面 (顺时针)
                    0, 2, 1,
                    0, 3, 2,
                    0, 4, 3,
                    0, 5, 4,

                    // 顶面 (逆时针)
                    6, 7, 8,
                    6, 8, 9,
                    6, 9, 10,
                    6, 10, 11,

                    // 侧面 (四边形面片，拆分为两个三角形)
                    0, 1, 6, 1, 7, 6,  // 左侧面
                    1, 2, 7, 2, 8, 7,  // 右侧面
                    2, 3, 8, 3, 9, 8,  // 前侧面
                    3, 4, 9, 4, 10, 9, // 后侧面
                    4, 5, 10, 5, 11, 10, // 内侧面
                    5, 0, 11, 0, 6, 11   // 外侧面
            },
            LineIndices = new uint[]
            {
                    // 底面边
                    0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 0,

                    // 顶面边
                    6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 6,

                    // 侧面边
                    //0, 6, 1, 7, 2, 8, 3, 9, 4, 10, 5, 11
            },
            CyLinderEdge = new uint[]
            {
                 
                 // 侧面边
                   // 0, 6, 1, 7, 2, 8, 3, 9, 4, 10, 5, 11
                   12,13,0
            }
        };
        public static SolidGeometry3D GetCylineders_Hex(int xCount, int yCount, int zCount, float dis = 600)
        {
            var result = new SolidGeometry3D();
            var vList = new List<Vertex>();
            var mList = new List<uint>();
            var lList = new List<uint>();
            var eList = new List<uint>();
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    for (int k = 0; k < zCount; k++)
                    {
                        Vector3 move = new(i * dis, j * dis, k * dis);
                        var tempBox = TestCylinder_Hex;
                        // Box.VertexCollections = Box.VertexCollections.Select(v => v + move).ToArray();
                        mList.AddRange(tempBox.MeshIndices.Select(i => i + (uint)vList.Count));
                        lList.AddRange(tempBox.LineIndices.Select(i => i + (uint)vList.Count));
                        eList.AddRange(tempBox.CyLinderEdge.Select(i => i + (uint)vList.Count));
                        tempBox.VertexCollections = tempBox.VertexCollections.Select(v =>
                        {
                            var r = v + move;
                            r.Color = v.Color;
                            return r;
                        }).ToArray();
                        vList.AddRange(tempBox.VertexCollections);
                    }
                }
            }
            result.VertexCollections = vList.ToArray();
            result.MeshIndices = mList.ToArray();
            result.LineIndices = lList.ToArray();
            result.CyLinderEdge = eList.ToArray();
            return result;
        }
        public static SolidGeometry3D TestCylinder_32Sides => new SolidGeometry3D()
        {
            VertexCollections = Generate32SidedCylinderVertices(50f, 500f),
            MeshIndices = Generate32SidedCylinderMeshIndices(),
            LineIndices = Generate32SidedCylinderLineIndices(),
            CyLinderEdge = new uint[] { 64, 65, 0 }

        };
        // 生成 32 边形的顶点坐标
        private static Vertex[] Generate32SidedCylinderVertices(float radius, float height)
        {
            int sides = 32;
            //Vertex[] vertices = new Vertex[sides * 2]; // 底面和顶面各 32 个顶点
            Vertex[] vertices = new Vertex[sides * 2 + 2]; // 底面和顶面各 32 个顶点
            float angleStep = 2 * MathF.PI / sides;    // 每份的角度

            // 底面顶点
            for (int i = 0; i < sides; i++)
            {
                float angle = i * angleStep;
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);
                vertices[i] = new Vertex(x, y, 0f); // 底面 Z = 0
            }

            // 顶面顶点
            for (int i = 0; i < sides; i++)
            {
                float angle = i * angleStep;
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);
                vertices[sides + i] = new Vertex(x, y, height); // 顶面 Z = height
            }
            vertices[64] = new Vertex(0, 0, 0, 32);
            vertices[65] = new Vertex(0, 0, 500, 32);
            return vertices;
        }

        // 生成 32 边形的三角形面片索引
        private static uint[] Generate32SidedCylinderMeshIndices()
        {
            int sides = 32;
            List<uint> indices = new List<uint>();

            // 底面三角形面片
            for (int i = 0; i < sides; i++)
            {
                uint next = (uint)((i + 1) % sides);
                indices.Add(0);         // 中心点（假设底面中心点为 0）
                indices.Add(next);
                indices.Add((uint)i);
            }

            // 顶面三角形面片
            for (int i = 0; i < sides; i++)
            {
                uint current = (uint)(sides + i);
                uint next = (uint)(sides + (i + 1) % sides);
                indices.Add((uint)sides); // 中心点（假设顶面中心点为 sides）
                indices.Add(current);
                indices.Add(next);
            }

            // 侧面四边形面片（拆分为两个三角形）
            for (int i = 0; i < sides; i++)
            {
                uint currentBottom = (uint)i;
                uint nextBottom = (uint)((i + 1) % sides);
                uint currentTop = (uint)(sides + i);
                uint nextTop = (uint)(sides + (i + 1) % sides);

                // 第一个三角形
                indices.Add(currentBottom);
                indices.Add(nextBottom);
                indices.Add(currentTop);

                // 第二个三角形
                indices.Add(nextBottom);
                indices.Add(nextTop);
                indices.Add(currentTop);
            }

            return indices.ToArray();
        }
        // 生成 32 边形的边索引
        private static uint[] Generate32SidedCylinderLineIndices()
        {
            int sides = 32;
            List<uint> indices = new List<uint>();

            // 底面边
            for (int i = 0; i < sides; i++)
            {
                uint current = (uint)i;
                uint next = (uint)((i + 1) % sides);
                indices.Add(current);
                indices.Add(next);
            }

            // 顶面边
            for (int i = 0; i < sides; i++)
            {
                uint current = (uint)(sides + i);
                uint next = (uint)(sides + (i + 1) % sides);
                indices.Add(current);
                indices.Add(next);
            }

            //// 侧面垂直边
            //for (int i = 0; i < sides; i++)
            //{
            //    uint bottom = (uint)i;
            //    uint top = (uint)(sides + i);
            //    indices.Add(bottom);
            //    indices.Add(top);
            //}

            return indices.ToArray();
        }
        public static SolidGeometry3D GetCylineders_32Sides(int xCount, int yCount, int zCount, float dis = 600)
        {
            var result = new SolidGeometry3D();
            var tempSolid = TestCylinder_32Sides;
            var vList = new List<Vertex>();
            var mList = new List<uint>();
            var lList = new List<uint>();
            var eList = new List<uint>();
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    for (int k = 0; k < zCount; k++)
                    {
                        Vector3 move = new(i * dis, j * dis, k * dis);
                        var tempBox = tempSolid.Copy();
                        // Box.VertexCollections = Box.VertexCollections.Select(v => v + move).ToArray();
                        mList.AddRange(tempBox.MeshIndices.Select(i => i + (uint)vList.Count));
                        lList.AddRange(tempBox.LineIndices.Select(i => i + (uint)vList.Count));
                        eList.AddRange(tempBox.CyLinderEdge.Select(i => i + (uint)vList.Count));
                        tempBox.VertexCollections = tempBox.VertexCollections.Select(v =>
                        {
                            var r = v + move;
                            r.Color = v.Color;
                            return r;
                        }).ToArray();
                        vList.AddRange(tempBox.VertexCollections);
                    }
                }
            }
            result.VertexCollections = vList.ToArray();
            result.MeshIndices = mList.ToArray();
            result.LineIndices = lList.ToArray();
            result.CyLinderEdge = eList.ToArray();
            return result;
        }
    }
}

