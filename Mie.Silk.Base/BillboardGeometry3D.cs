using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mie.Silk.Base
{
    public struct ImagePositon
    {
        public float X;
        public float Y;
        public float Z;
        public float Width;
        public float Height;
        public ImagePositon(float x, float y, float z, float width, float height)
        {
            X = x;
            Y = y;
            Z = z;
            Width = width;
            Height = height;
        }
        public override string ToString()
        {
            return $"<{this.X},{this.Y},{this.Z},{this.Width},{this.Height}>";
        }
    }
    public class BillboardGeometry3D
    {
        public ImagePositon[] postions;
        public string TexturePath;
    }
}
