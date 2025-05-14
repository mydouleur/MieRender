using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Mie.Silk.Base.Enums;
using Silk.NET.DXGI;

namespace Mie.Silk.Base.Tools
{
    public static class ColorHelper
    {
        public static Vector4[] GetDefaultColors()
        {
            List<Vector4> colors = new List<Vector4>();
            foreach (ColorLevelEnum colorEnum in Enum.GetValues(typeof(ColorLevelEnum)))
            {
                colors.Add(new(0, 0, 0, 1));
            }
            return colors.ToArray();
        }
        public static Vector4[] GetColors_1()
        {
            List<Vector4> colors = new List<Vector4>();
            foreach (ColorLevelEnum colorEnum in Enum.GetValues(typeof(ColorLevelEnum)))
            {
                uint colorUint = Convert.ToUInt32(Enum.GetName(typeof(ColorLevelEnum), colorEnum), 16);
                var temp = new Vector4();
                temp.X = (float)((colorUint & 0xFF) / 255f);
                temp.Y = (float)((colorUint >> 8) & 0xFF) / 255f;
                temp.Z = (float)((colorUint >> 16) & 0xFF) / 255f;
                temp.W = 0.5f;
                colors.Add(temp);
            }
            return colors.ToArray();
        }
        public static Vector4[] GetColors_2()
        {
            List<Vector4> colors = new List<Vector4>();
            foreach (ColorLevelEnum colorEnum in Enum.GetValues(typeof(ColorLevelEnum)))
            {
                uint colorUint = 0xdfdfdf;
                var temp = new Vector4();
                temp.X = (float)((colorUint & 0xFF) / 255f);
                temp.Y = (float)((colorUint >> 8) & 0xFF) / 255f;
                temp.Z = (float)((colorUint >> 16) & 0xFF) / 255f;
                temp.W = 0.5f;
                colors.Add(temp);
            }
            return colors.ToArray();
        }
        public static Vector4[] GetColors_3()
        {
            List<Vector4> colors = new List<Vector4>();
            foreach (ColorLevelEnum colorEnum in Enum.GetValues(typeof(ColorLevelEnum)))
            {
                uint colorUint = Convert.ToUInt32(Enum.GetName(typeof(ColorLevelEnum), colorEnum), 16);
                var temp = new Vector4();
                temp.X = (float)((colorUint & 0xFF) / 255f);
                temp.Y = (float)((colorUint >> 8) & 0xFF) / 255f;
                temp.Z = (float)((colorUint >> 16) & 0xFF) / 255f;
                temp.W = (float)((colorUint >> 24) & 0xFF) / 255f;
                colors.Add(temp);
            }
            return colors.ToArray();
        }
        public static Vector4[] GetColors_4()
        {
            List<Vector4> colors = new List<Vector4>();
            foreach (ColorLevelEnum colorEnum in Enum.GetValues(typeof(ColorLevelEnum)))
            {
                uint colorUint = Convert.ToUInt32(Enum.GetName(typeof(ColorLevelEnum), colorEnum), 16);
                var temp = new Vector4();
                temp.X = (float)((colorUint & 0xFF) / 255f);
                temp.Y = (float)((colorUint >> 8) & 0xFF) / 255f;
                temp.Z = (float)((colorUint >> 16) & 0xFF) / 255f;
                temp.W = 0.1f;
                colors.Add(temp);
            }
            return colors.ToArray();
        }
    }
}
