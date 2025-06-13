using Mie.Silk.Base;
using Silk.NET.Input;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Mie.Silk.OpenGL
{
    public class CameraController
    {
        Vector2 lastPoint { get; set; } = new Vector2(-1, -1);
        Vector2 panLastPoint { get; set; } = new Vector2(-1, -1);
        Vector2 rotateLastPoint { get; set; } = new Vector2(-1, -1);
        Vector3 RotationTarget { get; set; } = new Vector3(0, 0, 0);
        IKeyboard keyboard;
        IInputContext input;
        IMouse mouse;
        GLView GLView;
        public void init(IWindow window,GLView view)
        {
            var input = window.CreateInput();
            this.input = input;
            keyboard = input.Keyboards.FirstOrDefault();
            mouse = input.Mice.FirstOrDefault();
            this.GLView = view;
            view.Camera = new OrthoCamera(view);
            mouse.MouseDown += ViewPanMove_PMouseDown;
            mouse.Scroll += ViewZoom_PMouseWheel;
            mouse.MouseDown += ViewRotate_PMouseDown;

        }
        #region pan
        private void ViewPanMove_PMouseDown(IMouse mouse, MouseButton button)
        {
            if (mouse.IsButtonPressed(MouseButton.Middle) && !(keyboard.IsKeyPressed(Key.ControlLeft) || keyboard.IsKeyPressed(Key.ControlRight)) && !mouse.IsButtonPressed(MouseButton.Left))
            {
                panLastPoint = mouse.Position;
                mouse.MouseMove += ViewPanMove_PMouseMove;
                mouse.MouseUp += ViewPanMove_PMouseUp;
                
            }
        }
        private void ViewPanMove_PMouseMove(IMouse mouse, Vector2 position)
        {
            {
                GLView.EnableRender = true;
                var camera = GLView.Camera;
                var mousePos = position;
                var size = GLView.IWindow.FramebufferSize;
                var widthRatio = camera.Width / GLView.IWindow.FramebufferSize.X;
                Vector3 normalOfLookCrossUp = Vector3.Cross(camera.LookDirection, camera.UpDirection);
                var offsetX = -(mousePos.X - panLastPoint.X);
                var offsetY = (mousePos.Y - panLastPoint.Y);
                var temp = Vector3.Add(Vector3.Multiply((float)(offsetX * widthRatio), normalOfLookCrossUp), Vector3.Multiply((float)(offsetY * widthRatio), camera.UpDirection));
                camera.Position = Vector3.Add(camera.Position, temp);
                panLastPoint = mousePos;
            }
        }
        private void ViewPanMove_PMouseUp(IMouse mouse,MouseButton button)
        {
            panLastPoint = new(-1, -1);
            mouse.MouseMove -= ViewPanMove_PMouseMove;
            mouse.MouseUp -= ViewPanMove_PMouseUp;
            GLView.EnableRender = false;

        }
        #endregion
        #region zoom
        private void ViewZoom_PMouseWheel(IMouse sender, ScrollWheel scrollWheel)
        {
            GLView.EnableRender = true;
            var camera = GLView.Camera;
            //if (e == null)IInputElement
            //    return;
            // 获取鼠标当前位置
            var mousePos = sender.Position;
            //IInputElement
            var size = GLView.IWindow.FramebufferSize;

            // 计算缩放因子
            float zoomFactor = 1.2f; // 缩放因子，根据需要调整

            // 计算缩放的中心点
            float centerX = size.X / 2;
            float centerY = size.Y / 2;

            // 根据鼠标位置调整缩放的中心点
            float offsetX = -((float)mousePos.X - centerX) / size.X;
            float offsetY = ((float)mousePos.Y - centerY) / size.X;

            Vector3 look = camera.LookDirection;
            look = Vector3.Normalize(look);
            Vector3 up = camera.UpDirection;
            up = Vector3.Normalize(up);
            Vector3 normalOfLookCrossUp = Vector3.Cross(look, up);
            camera.Position -= offsetX * normalOfLookCrossUp * camera.Width;
            camera.Position -= offsetY * up * camera.Width;
            // 根据滚轮滚动方向进行缩放
            if (scrollWheel.Y > 0)
            {

                camera.Width /= zoomFactor;
                //if (camera.Width <= 30)
                //{
                //    camera.Width = 30;
                //}
            }
            else
            {

                camera.Width *= zoomFactor;

                if (camera.Width >= 1000000)
                {
                    camera.Width = 1000000;
                }
            }
            // 调整缩放的中心点
            camera.Position += offsetX * normalOfLookCrossUp * camera.Width;
            camera.Position += offsetY * up * camera.Width;
            GLView.LazyRenderDisable(10);
        }
        #endregion
        #region rotate
        private void ViewRotate_PMouseDown(IMouse mouse, MouseButton button)
        {

            if ((keyboard.IsKeyPressed(Key.ControlLeft) || keyboard.IsKeyPressed(Key.ControlRight)) && mouse.IsButtonPressed(MouseButton.Middle) && !mouse.IsButtonPressed(MouseButton.Left))
            {
                rotateLastPoint = mouse.Position;
                mouse.MouseMove += ViewRotate_PMouseMove;
                mouse.MouseUp += ViewRotateMove_PMouseUp;
            }
        }
        private void ViewRotate_PMouseMove(IMouse sender, Vector2 position)
        {
            {
                GLView.EnableRender = true;
                var camera = GLView.Camera;
                var mousePos = position;
                var size = GLView.IWindow.FramebufferSize;
                #region 实际旋转计算
                var move = camera.Position - RotationTarget;
                var look = camera.LookDirection;
                var up = camera.UpDirection;
                var right = Vector3.Cross(up, look);
                float angelX = (float)((mousePos.X - rotateLastPoint.X) / size.X * 2 * Math.PI);
                float angelY = (float)(-(mousePos.Y - rotateLastPoint.Y) / size.Y * 2 * Math.PI);
                var rotateX = Matrix4x4.CreateRotationZ(angelX);
                var rotateY = Matrix4x4.CreateFromAxisAngle(right, angelY);
                // 预先计算旋转矩阵的乘积，减少重复计算
                var combinedRotation = rotateY * rotateX;
                // 应用旋转到向量
                move = Vector3.Transform(move, combinedRotation);
                // move = combinedRotation * move;
                camera.LookDirection = Vector3.Normalize(Vector3.Transform(look, combinedRotation));
                camera.UpDirection = Vector3.Normalize(Vector3.Transform(up, combinedRotation));
                camera.Position = (RotationTarget + move);
                //  (view3D.DataContext as YGView).SetDisPlayType();
                #endregion
                //if (LastBoltCamera != null && camera.LookDirection.ToYGCommon().CalculateVectorAngle(LastBoltCamera.LookDirection.ToYGCommon()) > 20)
                //{
                //    LastBoltCamera.Position = camera.Position;
                //    LastBoltCamera.LookDirection = camera.LookDirection;
                //    view3D.BoltVision = ViewPortTools.CreateOBBbyOrthographicCamera(LastBoltCamera);
                //}

            }
            rotateLastPoint = position;
        }
        private void ViewRotateMove_PMouseUp(IMouse mouose, MouseButton button)
        {

            GLView.EnableRender = false;
            rotateLastPoint = new(-1, -1);
            mouse.MouseMove -= ViewRotate_PMouseMove;
            mouse.MouseUp -= ViewRotateMove_PMouseUp;
        }
        #endregion
    }
}
