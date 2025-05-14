using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Mie.Silk.Base;
using Mie.View.ViewControl;
using Cursor = System.Windows.Input.Cursor;
using Cursors = System.Windows.Input.Cursors;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using SPoint = System.Windows.Point;
namespace Mie.View
{
    public class CameraController
    {
        MieViewPort view;
        ISilkView silkView;
        SPoint lastPoint { get; set; } = new SPoint(-1, -1);
        SPoint panLastPoint { get; set; } = new SPoint(-1, -1);
        SPoint rotateLastPoint { get; set; } = new SPoint(-1, -1);
        Cursor lastCursor;

        public CameraController(MieViewPort view)
        {
            this.view = view;
            this.silkView = view.viewHost.View;
            Push();
        }
        public void Push()
        {
            view.PreviewMouseDown += ViewPanMove_PMouseDown;
            view.PreviewMouseWheel += ViewZoom_PMouseWheel;
            view.PreviewMouseDown += ViewRotate_PMouseDown;
        }
        #region 移动视图
        private void ViewPanMove_PMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed && Keyboard.Modifiers == ModifierKeys.None && e.LeftButton == MouseButtonState.Released)
            {
                panLastPoint = e.GetPosition(view);
                view.PreviewMouseMove += ViewPanMove_PMouseMove;
                view.PreviewMouseUp += ViewPanMove_PMouseUp;
                view.CaptureMouse();
                lastCursor = view.Cursor;
                view.Cursor = Cursors.ScrollAll;
            }
        }
        private void ViewPanMove_PMouseMove(object sender, MouseEventArgs e)
        {
            {
                silkView.EnableRender = true;
                var camera = view.Camera;
                System.Windows.Point mousePos = Mouse.GetPosition(view);
                var size = silkView.IWindow.FramebufferSize;
                var widthRatio = camera.Width / silkView.IWindow.FramebufferSize.X;
                Vector3 normalOfLookCrossUp = Vector3.Cross(camera.LookDirection, camera.UpDirection);
                var offsetX = -(mousePos.X - panLastPoint.X);
                var offsetY = (mousePos.Y - panLastPoint.Y);
                var temp = Vector3.Add(Vector3.Multiply((float)(offsetX * widthRatio), normalOfLookCrossUp), Vector3.Multiply((float)(offsetY * widthRatio), camera.UpDirection));
                camera.Position = Vector3.Add(camera.Position, temp);
                panLastPoint = mousePos;
            }
        }
        private void ViewPanMove_PMouseUp(object sender, MouseEventArgs e)
        {
            panLastPoint = new(-1, -1);
            view.PreviewMouseMove -= ViewPanMove_PMouseMove;
            view.PreviewMouseUp -= ViewPanMove_PMouseUp;
            view.ReleaseMouseCapture();
            view.Cursor = lastCursor;
            silkView.EnableRender = false;

        }
        #endregion
        #region 缩放视图
        private void ViewZoom_PMouseWheel(object sender, MouseWheelEventArgs e)
        {
            silkView.EnableRender = true;
            var camera = view.Camera;
            //if (e == null)IInputElement
            //    return;
            // 获取鼠标当前位置
            var mousePos = e.GetPosition(sender as IInputElement);
            //IInputElement
            var size = silkView.IWindow.FramebufferSize;

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
            if (e.Delta > 0)
            {

                camera.Width /= zoomFactor;
                if (camera.Width <= 30)
                {
                    camera.Width = 30;
                }
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

            view.viewHost.LazyRenderDisable(10);
        }
        #endregion
        #region 旋转视图
        private void ViewRotate_PMouseDown(object sender, MouseButtonEventArgs e)
        {

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Mouse.MiddleButton == MouseButtonState.Pressed && Mouse.LeftButton == MouseButtonState.Released)
            {
                rotateLastPoint = e.GetPosition(sender as IInputElement);
                view.PreviewMouseMove += ViewRotate_PMouseMove;
                view.PreviewMouseUp += ViewRotateMove_PMouseUp;
                view.CaptureMouse();
                lastCursor = view.Cursor;
                view.Cursor = Cursors.ScrollAll;
            }
        }
        private void ViewRotate_PMouseMove(object sender, MouseEventArgs e)
        {
            {
                silkView.EnableRender = true;
                var camera = view.Camera;
                var mousePos = e.GetPosition(sender as IInputElement);
                var size = silkView.IWindow.FramebufferSize;
                #region 实际旋转计算
                var move = camera.Position - view.RotationTarget;
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
                camera.Position = (view.RotationTarget + move);
                //  (view3D.DataContext as YGView).SetDisPlayType();
                #endregion
                //if (LastBoltCamera != null && camera.LookDirection.ToYGCommon().CalculateVectorAngle(LastBoltCamera.LookDirection.ToYGCommon()) > 20)
                //{
                //    LastBoltCamera.Position = camera.Position;
                //    LastBoltCamera.LookDirection = camera.LookDirection;
                //    view3D.BoltVision = ViewPortTools.CreateOBBbyOrthographicCamera(LastBoltCamera);
                //}

            }
            rotateLastPoint = e.GetPosition(sender as IInputElement);
        }
        private void ViewRotateMove_PMouseUp(object sender, MouseEventArgs e)
        {

            silkView.EnableRender = false;
            rotateLastPoint = new(-1, -1);
            view.PreviewMouseMove -= ViewRotate_PMouseMove;
            view.PreviewMouseUp -= ViewRotateMove_PMouseUp;
            view.ReleaseMouseCapture();
            view.Cursor = lastCursor;
        }

        #endregion
    }
}
