using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mie.Silk.Base.TestData
{
    public static class TestImage
    {
        public static BillboardGeometry3D GetImageGeometry(int xCount, int yCount, int zCount, float width, float height, string path)
        {
            var result = new BillboardGeometry3D();
            var positions = new List<ImagePositon>();
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    for (int k = 0; k < zCount; k++)
                    {
                        positions.Add(new ImagePositon(i * 200, j * 200, k * 200, width, height));
                    }
                }
            }
            result.postions = positions.ToArray();
            result.TexturePath = path;
            return result;
        }
    }
}
