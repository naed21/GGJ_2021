using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGJ_2021
{
	public class MenuManager
	{
		TextureAtlas _Atlas;
		Texture2D _SpriteSheet;

		List<MenuText> _TextTextures = new List<MenuText>();
		List<MenuButton> _Buttons = new List<MenuButton>();
		List<MenuPanel> _Panels = new List<MenuPanel>();

		public MenuManager(TextureAtlas atlas, Texture2D spritesheet)
		{
			_Atlas = atlas;
			_SpriteSheet = spritesheet;
		}

		public void AddButton(string atlasName, string onHoverAtlas, Rectangle destination, Action clickAction)
		{
			var rect = _Atlas.GetRectangle(atlasName);
			//if(!string.IsNullOrEmpty(onHoverAtlas))
			var onHoverRect = _Atlas.GetRectangle(onHoverAtlas);
			if(rect.HasValue && onHoverRect.HasValue)
				_Buttons.Add(new MenuButton
				{
					SourceRectangle = rect.Value,
					DestinationRectangle = destination,
					OnHoverSourceRectangle = onHoverRect.Value,
					ClickAction = clickAction
				});
		}

		public void AddText(Texture2D textTexture, Vector2 pos)
		{
			_TextTextures.Add(new MenuText
			{
				Texture = textTexture,
				Position = pos
			});
		}

		public void AddPanel(string atlasName, Rectangle destination)
		{
			var rect = _Atlas.GetRectangle(atlasName);
			if (rect.HasValue)
				_Panels.Add(new MenuPanel
				{
					SourceRectangle = rect.Value,
					DestinationRectangle = destination
				});
		}

		public void ClickAtPos(Point pos)
		{
			Rectangle rect = new Rectangle(pos.X, pos.Y, 1, 1);
			foreach(var button in _Buttons)
			{
				if (button.DestinationRectangle.Intersects(rect))
				{
					button.ClickAction();
					return;
				}
			}
		}

		public void HoverAtPos(Point pos)
		{
			Rectangle rect = new Rectangle(pos.X, pos.Y, 1, 1);
			foreach (var button in _Buttons)
			{
				if (button.DestinationRectangle.Intersects(rect))
				{
					button.IsHover = true;
				}
				else
					button.IsHover = false;
			}
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach(var panel in _Panels)
			{
				spriteBatch.Draw(_SpriteSheet, panel.DestinationRectangle, panel.SourceRectangle, Color.White);
			}

			foreach(var button in _Buttons)
			{
				if(button.IsHover)
					spriteBatch.Draw(_SpriteSheet, button.DestinationRectangle, button.OnHoverSourceRectangle, Color.White);
				else
					spriteBatch.Draw(_SpriteSheet, button.DestinationRectangle, button.SourceRectangle, Color.White);
			}

			foreach(var text in _TextTextures)
			{
				spriteBatch.Draw(text.Texture, text.Position, Color.White);
			}
		}
	}

	struct MenuText
	{
		public Texture2D Texture;
		public Vector2 Position;
	}

	class MenuButton
	{
		public Rectangle SourceRectangle { get; set; }
		public Rectangle OnHoverSourceRectangle { get; set; }
		public Rectangle DestinationRectangle { get; set; }
		public Action ClickAction { get; set; }
		public bool IsHover { get; set; }
	}

	struct MenuPanel
	{
		public Rectangle SourceRectangle;
		public Rectangle DestinationRectangle;
	}
}
