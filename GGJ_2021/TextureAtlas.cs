using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace GGJ_2021
{
	public class TextureAtlas
	{
		[XmlElement("SubTexture")]
		public SubTexture[] SubTextures { get; set; }

		internal Rectangle? GetRectangle(string name)
		{
			Func<string, string> removeExtension = (a) =>
			{
				return Path.GetFileNameWithoutExtension(a);
			};

			for (int x = 0; x < SubTextures.Length; x++)
			{
				if (removeExtension(SubTextures[x].Name) == removeExtension(name))
					return new Rectangle(
						int.Parse(SubTextures[x].Xpos),
						int.Parse(SubTextures[x].Ypos),
						int.Parse(SubTextures[x].Width),
						int.Parse(SubTextures[x].Height));
			}

			return null;
		}
	}

	public class SubTexture
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
		[XmlAttribute("x")]
		public string Xpos { get; set; }
		[XmlAttribute("y")]
		public string Ypos { get; set; }
		[XmlAttribute("width")]
		public string Width { get; set; }
		[XmlAttribute("height")]
		public string Height { get; set; }
	}
}
