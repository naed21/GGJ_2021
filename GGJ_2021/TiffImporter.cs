using System;
using System.Collections.Generic;
using System.Text;


namespace GGJ_2021
{
	/*
	 *  Abandoned due to having to include special content pipeline dlls
	 */

	//[ContentImporter(".tif", ".tiff", DisplayName = "TIFF Importer", DefaultProcessor = "TextureProcessor")]
	//public class TiffImporter : ContentImporter<Texture2DContent>
	//{
	//	public override Texture2DContent Import(string filename, ContentImporterContext context)
	//	{
	//		Bitmap bitmap = Image.FromFile(filename) as Bitmap;
	//		var bitmapContent = new PixelBitmapContent<Microsoft.Xna.Framework.Color>(bitmap.Widthitmap.Height);

	//		for (int i = 0; i < bitmap.Width; i++)
	//		{
	//			for (int j = 0; j < bitmap.Height; j++)
	//			{
	//				System.Drawing.Color from = bitmap.GetPixel(i, j);
	//				Microsoft.Xna.Framework.Color to = new Microsoft.Xna.Framework.Color(from.R, from.G, from.B, from.A);
	//				bitmapContent.SetPixel(i, j, to);
	//			}
	//		}

	//		return new Texture2DContent()
	//		{
	//			Mipmaps = new MipmapChain(bitmapContent)
	//		};
	//	}
	//}
}
