using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Direct3D12;
using Silk.NET.Maths;

namespace Mie.Silk.Base.TestData
{
    public static class DefaultMesh
    {

        public static SolidGeometry3D Box => new SolidGeometry3D()
        {
            VertexCollections = new Vertex[]
            {
               // Front face
                new (-0.5f, -0.5f, -0.5f),
                new (0.5f, -0.5f, -0.5f),
                new (0.5f, 0.5f, -0.5f),
                new (-0.5f, 0.5f, -0.5f),

                // Back face
                new (-0.5f, -0.5f, 0.5f),
                new (0.5f, -0.5f, 0.5f),
                new (0.5f, 0.5f, 0.5f),
                new (-0.5f, 0.5f, 0.5f)
            },
            MeshIndices = new uint[]
            {
                0, 1, 2, 0, 2, 3, // Front face
                4, 5, 6, 4, 6, 7, // Back face
                0, 3, 7, 0, 7, 4, // Left face
                1, 2, 6, 1, 6, 5, // Right face
                2, 3, 7, 2, 7, 6, // Top face
                0, 1, 5, 0, 5, 4  // Bottom face
            },
            LineIndices = new uint[]
            {
                0, 1, 1, 2, 2, 3, 3, 0,
                4, 5, 5, 6, 6, 7, 7, 4,
                0, 4, 1, 5, 2, 6, 3, 7,
            }
        };
        public static SolidGeometry3D GetBoxes(int xCount, int yCount, int zCount, float dis = 2)
        {
            var result = new SolidGeometry3D();
            var vList = new List<Vertex>();
            var mList = new List<uint>();
            var lList = new List<uint>();
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    for (int k = 0; k < zCount; k++)
                    {
                        Vector3 move = new(i * dis, j * dis, k * dis);
                        var tempBox = Box;
                        // Box.VertexCollections = Box.VertexCollections.Select(v => v + move).ToArray();
                        tempBox.VertexCollections = tempBox.VertexCollections.Select(v => v + move).ToArray();
                        vList.AddRange(tempBox.VertexCollections);
                        mList.AddRange(tempBox.MeshIndices.Select(i => i + (uint)vList.Count));
                        lList.AddRange(tempBox.LineIndices.Select(i => i + (uint)vList.Count));
                    }
                }
            }
            result.VertexCollections = vList.ToArray();
            result.MeshIndices = mList.ToArray();
            result.LineIndices = lList.ToArray();
            return result;
        }
        public static SolidGeometry3D HBeam_HN_400_200_8_13 => new SolidGeometry3D()
        {
            VertexCollections = new Vertex[]
            {
                new(100,-200,0),
                new(100,-187,0),
                new(4,-187,0),
                new(4,187,0),
                new(100,187,0),
                new(100,200,0),
                new(-100,200,0),
                new(-100,187,0),
                new(-4,187,0),
                new(-4,-187,0),
                new(-100,-187,0),
                new(-100,-200,0),
                new(100,-200,500),
                new(100,-187,500),
                new(4,-187,500),
                new(4,187,500),
                new(100,187,500),
                new(100,200,500),
                new(-100,200,500),
                new(-100,187,500),
                new(-4,187,500),
                new(-4,-187,500),
                new(-100,-187,500),
                new(-100,-200,500),
            },
            MeshIndices = new uint[]
            {
                //起点面 
                0,10,1,
                0,11,10,
                2,9,8,
                2,8,3,
                4,7,6,
                4,6,5,
                //4,6,5,
                //3,6,4,
                //8,6,3,
                //2,8,3,
                //7,6,8,
                //0,2,1,
                //11,2,0,
                //11,9,2,
                //9,8,2,
                //11,10,9,
                //终点面
                12,13,22,
                12,22,23,
                14,20,21,
                14,15,20,
                16,18,19,
                16,17,18,
                //21,22,23,
                //14,20,21,
                //14,21,23,
                //12,14,23,
                //13,14,12,
                //20,18,19,
                //15,20,14,
                //15,18,20,
                //16,18,15,
                //17,18,16,
                //下面是侧面的三角索引
                13,12,0,1,13,0,
                14,13,1,2,14,1,
                15,14,2,3,15,2,
                16,15,3,4,16,3,
                17,16,4,5,17,4,
                18,17,5,6,18,5,
                19,18,6,7,19,6,
                20,19,7,8,20,7,
                21,20,8,9,21,8,
                22,21,9,10,22,9,
                23,22,10,11,23,10,
                12,23,11,0,12,11,
            },
            LineIndices = new uint[]
            {
                0, 1,
                1, 2,
                2, 3,
                3, 4,
                4, 5,
                5, 6,
                6, 7,
                7, 8,
                8, 9,
                9, 10,
                10, 11,
                0, 11,
                22, 23,
                21, 22,
                20, 21,
                19, 20,
                18, 19,
                17, 18,
                16, 17,
                15, 16,
                14, 15,
                13, 14,
                12, 13,
                12, 23,
                0, 12,
                1, 13,
                2, 14,
                3, 15,
                4, 16,
                5, 17,
                6, 18,
                7, 19,
                8, 20,
                9, 21,
                10, 22,
                11, 23,
            }
        };
        public static SolidGeometry3D GetHbeams(int xCount, int yCount, int zCount, float dis = 600)
        {
            var result = new SolidGeometry3D();
            var vList = new List<Vertex>();
            var cpList = new List<Vertex>();
            var mList = new List<uint>();
            var lList = new List<uint>();
            var str = new Vertex(0, 0, 0, 0);
            var end = new Vertex(0, 0, 500, 0);
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    for (int k = 0; k < zCount; k++)
                    {
                        Vector3 move = new(i * dis, j * dis, k * dis);
                        var tempBox = HBeam_HN_400_200_8_13;
                        // Box.VertexCollections = Box.VertexCollections.Select(v => v + move).ToArray();
                        mList.AddRange(tempBox.MeshIndices.Select(i => i + (uint)vList.Count));
                        lList.AddRange(tempBox.LineIndices.Select(i => i + (uint)vList.Count));
                        tempBox.VertexCollections = tempBox.VertexCollections.Select(v =>
                        {
                            var r = v + move;
                            if (xCount * yCount * zCount != 1)
                            {
                                r.Color = (k + i + j) % 255;
                            }
                            return r;
                        }).ToArray();
                        vList.AddRange(tempBox.VertexCollections);
                        cpList.Add(str + move);
                        cpList.Add(end + move);
                    }
                }
            }
            result.VertexCollections = vList.ToArray();
            result.MeshIndices = mList.ToArray();
            result.LineIndices = lList.ToArray();
            result.ContourPointCollections = cpList.ToArray();
            return result;
        }
    }
}
