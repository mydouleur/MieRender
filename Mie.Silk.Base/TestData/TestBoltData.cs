using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Direct3D12;
using Silk.NET.OpenGL;

namespace Mie.Silk.Base.TestData
{
    public static class TestBoltData
    {
        public static SolidGeometry3D BoltTestData
        {
            get
            {

                return new SolidGeometry3D()
                {
                    VertexCollections = new Vertex[]
                    {
                        new(0,0,0,12),
                        new(0,0,15,20),
                        new(0,20,0,0),
                        new(0,0,15,22),
                        new(0,0,100,20),
                        new(0,10,15,0),
                    }
                };
            }
        }
        public static SolidGeometry3D GetTestBolts(int xCount, int yCount, int zCount, float dis = 120)
        {
            var vList = new List<Vertex>();
            var result = new SolidGeometry3D();
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    for (int k = 0; k < zCount; k++)
                    {
                        Vector3 move = new(i * dis, j * dis, k * dis);
                        // Box.VertexCollections = Box.VertexCollections.Select(v => v + move).ToArray();
                        vList.AddRange(BoltTestData.VertexCollections.Select(v => v + move));
                    }
                }
            }
            result.VertexCollections = vList.ToArray();
            return result;
        }
    }
}
