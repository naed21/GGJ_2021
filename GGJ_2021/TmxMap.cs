using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GGJ_2021
{
	[XmlRoot("map")]
	public class TmxMap
	{
		[XmlAttribute("version")]
		public string Version { get; set; }
		[XmlAttribute("tiledversion")]
		public string TiledVersion { get; set; }
		[XmlAttribute("orientation")]
		public string Orientation { get; set; }
		[XmlAttribute("renderorder")]
		public string RenderOrder { get; set; }
		[XmlAttribute("width")]
		public string Width { get; set; }
		[XmlAttribute("height")]
		public string Height { get; set; }
		[XmlAttribute("tilewidth")]
		public string TileWidth { get; set; }
		[XmlAttribute("tileheight")]
		public string TileHeight { get; set; }
		[XmlAttribute("infinite")]
		public string Infinite { get; set; }
		[XmlAttribute("nextlayerid")]
		public string NextLayerId { get; set; }
		[XmlAttribute("nextobjectid")]
		public string NextObjectId { get; set; }
		[XmlElement("tileset")]
		public TileSet TileSet { get; set; }
		[XmlElement("layer")]
		public Layer[] Layers { get; set; }
	}

	public class TileSet
	{
		[XmlAttribute("firstgid")]
		public string FirstGid { get; set; }
		[XmlAttribute("source")]
		public string Source { get; set; }
	}

	public class Layer
	{
		[XmlAttribute("id")]
		public string Id { get; set; }
		[XmlAttribute("name")]
		public string Name { get; set; }
		[XmlAttribute("width")]
		public string Width { get; set; }
		[XmlAttribute("height")]
		public string Height { get; set; }
		[XmlAttribute("visible")]
		public string Visible { get; set; }
		[XmlElement("data")]
		public Data Data { get; set; }
	}

	public class Data
	{
		[XmlAttribute("encoding")]
		public string Encoding { get; set; }
		[XmlText]
		public string Value { get; set; }
	}
}
