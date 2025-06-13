using Silk.NET.OpenGL;
using System;
using System.Drawing.Imaging;

namespace Mie.Silk.OpenGL
{
    public class BitTexture : IDisposable
    {
        private uint _handle;
        private GL _gl;
        private TextureUnit _unit;
        public unsafe BitTexture(GL gl, string path, TextureUnit unit = TextureUnit.Texture0)
        {
            _gl = gl;
            _handle = _gl.GenTexture();
            _unit = unit;

            //using (var bitmap = new Bitmap(path))
            //{
            //    BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            //    // 复制像素数据到临时缓冲区
            //    byte[] buffer = new byte[data.Stride * data.Height];
            //    System.Runtime.InteropServices.Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

            //    bitmap.UnlockBits(data);

            //    // 使用临时缓冲区上传纹理
            //    _gl.BindTexture(TextureTarget.Texture2D, _handle);
            //    fixed (byte* ptr = buffer)
            //    {
            //        _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)bitmap.Width, (uint)bitmap.Height, 0, GLEnum.Bgra, PixelType.UnsignedByte, ptr);
            //    }
            //}

            SetParameters();
        }

        public unsafe BitTexture(GL gl, Span<byte> data, uint width, uint height)
        {
            _gl = gl;

            _handle = _gl.GenTexture();
            Bind();

            fixed (void* d = &data[0])
            {
                _gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, GLEnum.Rgba, PixelType.UnsignedByte, d);
                SetParameters();
            }
        }

        private void SetParameters()
        {
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);
            _gl.GenerateMipmap(TextureTarget.Texture2D);
        }

        public void Bind()
        {
            _gl.ActiveTexture(_unit);
            _gl.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void Dispose()
        {
            _gl.DeleteTexture(_handle);
        }
    }
}
