using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;

namespace Mie.Silk.Base
{
    /// <summary>
    /// 正交相机
    /// </summary>
    public class OrthoCamera : ICloneable
    {
        public Vector3 Position;
        public Vector3 LookDirection;
        public Vector3 UpDirection;
        public float Width;
        public float Near;
        public float Far;
        public Matrix4x4 CameraMatrix { get; set; }
        public Matrix4x4 Projection { get; set; }
        public Matrix4x4 View { get; set; }
        public ISilkView SilkView { get; set; }
        public OrthoCamera()
        {
        }
        public OrthoCamera(ISilkView silk)
        {
            Position = new Vector3(0.0f, 0.0f, 3.0f);
            LookDirection = new Vector3(0.0f, 0.0f, -1.0f);
            UpDirection = Vector3.UnitY;
            Width = 500f;
            Near = 2000000;
            Far = 2000000;
            SilkView = silk;
        }
        public void UpdateMatrix()
        {
            var size = SilkView.IWindow.FramebufferSize;
            var view = Matrix4x4.CreateLookAt(this.Position, this.Position + this.LookDirection, this.UpDirection);
            if (size.X == 0)
            {
                size.X = 1;
                size.Y = 1;
            }
            var projection = Matrix4x4.CreateOrthographic(this.Width, this.Width * size.Y / size.X, -200000, 2000000);
            this.CameraMatrix = view * projection;
            Projection = projection;
            View = view;
        }
        public OrthoCamera Copy()
        {
            return new OrthoCamera()
            {
                Position = Position,
                LookDirection = LookDirection,
                UpDirection = UpDirection,
                Width = Width,
                Near = Near,
                Far = Far,
                SilkView = SilkView,
            };
        }
        public object Clone()
        {
            return this.Copy();
        }
    }
}
