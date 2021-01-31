using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGJ_2021
{
	public class MapManager
	{
		Texture2D _MapTexture;

		bool m_IsAdjustingOffset = false;
		public bool IsAdjustingOffset { get { return m_IsAdjustingOffset; } }

		Vector2 _TargetOffset = Vector2.Zero;
		Vector2 _CurrentOffset = Vector2.Zero;
		Vector2 _OffsetAdjustmentSpeed = Vector2.Zero;

		TimeSpan _LastAdjustment = TimeSpan.Zero;

		Dictionary<int, List<Tile>> DrawLayers = new Dictionary<int, List<Tile>>();
		List<Tile> CollisionLayer = new List<Tile>();

		int _MapHeight = 1;
		int _MapWidth = 1;
		int _TileHeight = 1;
		int _TileWidth = 1;

		List<Character> _CharacterList = new List<Character>();

		public MapManager(TmxMap mapData, Texture2D mapTexture, int textureColumns, int textureSpacing)
		{
			//process map
			_MapTexture = mapTexture;
			int.TryParse(mapData.Height, out _MapHeight);
			int.TryParse(mapData.Width, out _MapWidth);
			int.TryParse(mapData.TileWidth, out _TileWidth);
			int.TryParse(mapData.TileHeight, out _TileHeight);

			int drawOrder = 0;
			foreach(var layer in mapData.Layers)
			{
				if(layer.Name.ToLower() == "collision")
				{
					var collisionSplit = layer.Data.Value.Replace("\n", "").Split(",");
					//store as collision data
					for (int y = 0; y < _MapHeight; y++)
					{
						for(int x = 0; x < _MapWidth; x++)
						{
							int index = y * _MapWidth + x;
							var strTileId = collisionSplit[index];
							if (strTileId == "0")
								continue;

							CollisionLayer.Add(new Tile
							{
								GridPosition = new Point(x, y),
								DrawPosition = Vector2.Zero,
								SourceRectangle = Rectangle.Empty
							});
						}
					}

					continue;
				}

				//int layerId = 0;
				//int.TryParse(layer.Id, out layerId);

				//Map editor lists layer by draw order, not by id
				DrawLayers.Add(drawOrder, new List<Tile>());
				
				
				var dataSplit = layer.Data.Value.Replace("\n", "").Split(",");
				for(int y = 0; y < _MapHeight; y++)
				{
					for(int x = 0; x < _MapWidth; x++)
					{
						int index = y * _MapWidth + x;
						var strTileId = dataSplit[index];
						int tileId = 0;
						tileId = int.Parse(strTileId);

						//This is an empty space
						if (tileId == 0)
							continue;

						//They start at 1 so they can use 0 as empty
						tileId -= 1;

						int column = tileId % textureColumns;
						int row = (tileId - column) / textureColumns;
						Rectangle sourceRect = new Rectangle(
							column * _TileWidth + column * textureSpacing,
							row * _TileHeight + row * textureSpacing,
							_TileWidth,
							_TileHeight);
						Vector2 pos = new Vector2(x * _TileWidth, y * _TileHeight);

						DrawLayers[drawOrder].Add(new Tile
						{
							DrawPosition = pos,
							SourceRectangle = sourceRect,
							GridPosition = new Point(x, y)
						});
					}
				}

				drawOrder++;
			}
		}

		/// <summary>
		/// Checks if the pos is on the collision layer
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool DoesMapPosHaveCollision(int x, int y)
		{
			return false;
		}

		/// <summary>
		/// Set where you want the map to move to and the speed in which it does it
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="speed"></param>
		public void SetTargetOffset(Vector2 offset, Vector2 speed)
		{
			m_IsAdjustingOffset = true;
			_TargetOffset = offset;
			_OffsetAdjustmentSpeed = speed;
		}

		public void SetTargetCenterOffset(Point gridPos, Point screenSize, Vector2 speed, bool lockX = false, bool lockY = false)
		{
			m_IsAdjustingOffset = true;

			Vector2 screenCenter = new Vector2(screenSize.X / 2, screenSize.Y / 2);
			Vector2 gridPosVect = new Vector2(gridPos.X * _TileWidth, gridPos.Y * _TileHeight);
			_TargetOffset = screenCenter - gridPosVect;

			if (lockX)
				_TargetOffset.X = _CurrentOffset.X;
			if (lockY)
				_TargetOffset.Y = _CurrentOffset.Y;

			_OffsetAdjustmentSpeed = speed;
		}

		public Vector2 GetPosOnGrid(Point gridPos)
		{
			Vector2 gridPosVect = new Vector2(gridPos.X * _TileWidth, gridPos.Y * _TileHeight);
			return gridPosVect + _CurrentOffset;
		}

		public void AddCharacter(Character playerCharacter)
		{
			_CharacterList.Add(playerCharacter);
		}

		/// <summary>
		/// Use the step speed to control how smooth the offset change is
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="stepSpeed"></param>
		public void Update(GameTime gameTime, TimeSpan stepSpeed)
		{
			/*
			 * If the current offset doesn't match the target offset
			 * then use the speed var to update the offset until it matches
			 */
			if (m_IsAdjustingOffset)
			{
				_LastAdjustment += gameTime.ElapsedGameTime;

				if (_LastAdjustment.TotalMilliseconds >= stepSpeed.TotalMilliseconds)
				{
					var diff = (_CurrentOffset - _TargetOffset);
					if (Math.Abs(diff.X) <= _OffsetAdjustmentSpeed.X && Math.Abs(diff.Y) <= _OffsetAdjustmentSpeed.Y)
					{
						_CurrentOffset = _TargetOffset;
						m_IsAdjustingOffset = false;
						_LastAdjustment = TimeSpan.Zero;
						return;
					}

					float adjustX = _OffsetAdjustmentSpeed.X;
					float adjustY = _OffsetAdjustmentSpeed.Y;

					if (Math.Abs(diff.X) <= _OffsetAdjustmentSpeed.X)
						adjustX = Math.Abs(diff.X);
					if (Math.Abs(diff.Y) <= _OffsetAdjustmentSpeed.Y)
						adjustY = Math.Abs(diff.Y);

					if (_CurrentOffset.X < _TargetOffset.X)
						_CurrentOffset.X += adjustX;
					else if (_CurrentOffset.X > _TargetOffset.X)
						_CurrentOffset.X -= adjustX;

					if (_CurrentOffset.Y < _TargetOffset.Y)
						_CurrentOffset.Y += adjustY;
					else if (_CurrentOffset.Y > _TargetOffset.Y)
						_CurrentOffset.Y -= adjustY;

					if (_CurrentOffset == _TargetOffset)
					{
						m_IsAdjustingOffset = false;
					}

					_LastAdjustment = TimeSpan.Zero;
				}
			}

			//Just update this every frame to make sure it's correct
			foreach (var character in _CharacterList)
			{
				var temp = GetPosOnGrid(character.GridPosition);
				character.Position = temp;
			}
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			/*
			 * Draw each layer, but skip if visiable = "0"
			 * its probably a collision layer so ignore it
			 */

			foreach(var layer in DrawLayers.Keys.OrderBy(x=>x))
			{
				foreach(var tile in DrawLayers[layer])
				{
					spriteBatch.Draw(_MapTexture, tile.DrawPosition + _CurrentOffset, tile.SourceRectangle, Color.White);
				}
			}
		}

		struct Tile
		{
			public Rectangle SourceRectangle;
			public Vector2 DrawPosition;
			public Point GridPosition;
		}

		internal bool CheckForCollision(Point target)
		{
			foreach (var c in CollisionLayer)
				if (c.GridPosition == target)
					return true;

			return false;
		}
	}
}
