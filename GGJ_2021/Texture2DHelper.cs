using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGJ_2021
{
	public static class Texture2DHelper
	{
		public static Texture2D GetTexture2DFromBitmap(GraphicsDevice device, System.Drawing.Bitmap bitmap)
		{
			Texture2D tex = new Texture2D(device, bitmap.Width, bitmap.Height, false, SurfaceFormat.Color);

			System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

			int bufferSize = data.Height * data.Stride;

			//create data buffer 
			byte[] bytes = new byte[bufferSize];

			// copy bitmap data into buffer
			System.Runtime.InteropServices.Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

			// copy our buffer to the texture
			tex.SetData(bytes);

			// unlock the bitmap data
			bitmap.UnlockBits(data);

			return tex;
		}
	}
}
