using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGJ_2021
{
	public static class CharacterFactory
	{
		public static Character CreateCharacter(int id, Texture2D spritesheet, Vector2 position, bool isPlayer)
		{
			Character result = new Character(spritesheet, new TimeSpan(0,0,0,0,milliseconds:100), isPlayer);
			result.Position = position;
			//Character will have hard codded values
			//-> Spritesheet rects
			//-> Battle deck

			int spriteSize = 16;
			int spacing = 1;

			if(id == 0)
			{
				//custom card deck
				
				result.TotalFrames = 3;
				var left = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*23,0,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,1*spriteSize+spacing*1,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,2*spriteSize+spacing*2,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Left, left);
				var right = new List<Rectangle>
				{
					new Rectangle(24*spriteSize+spacing*24,0,spriteSize,spriteSize),
					new Rectangle(24*spriteSize+spacing*24,1*spriteSize+spacing*1,spriteSize,spriteSize),
					new Rectangle(24*spriteSize+spacing*24,2*spriteSize+spacing*2,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Down, right);
				var up = new List<Rectangle>
				{
					new Rectangle(25*spriteSize+spacing*25,0,spriteSize,spriteSize),
					new Rectangle(25*spriteSize+spacing*25,1*spriteSize+spacing*1,spriteSize,spriteSize),
					new Rectangle(25*spriteSize+spacing*25,2*spriteSize+spacing*2,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Up, up);
				var down = new List<Rectangle>
				{
					new Rectangle(26*spriteSize+spacing*26,0,spriteSize,spriteSize),
					new Rectangle(26*spriteSize+spacing*26,1*spriteSize+spacing*1,spriteSize,spriteSize),
					new Rectangle(26*spriteSize+spacing*26,2*spriteSize+spacing*2,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Right, down);
			}
			else if(id == 1)
			{
				result.TotalFrames = 3;
				var left = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*23,3*spriteSize+spacing*3,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,4*spriteSize+spacing*4,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,5*spriteSize+spacing*5,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Left, left);
				var right = new List<Rectangle>
				{
					new Rectangle(24*spriteSize+spacing*24,3*spriteSize+spacing*3,spriteSize,spriteSize),
					new Rectangle(24*spriteSize+spacing*24,4*spriteSize+spacing*4,spriteSize,spriteSize),
					new Rectangle(24*spriteSize+spacing*24,5*spriteSize+spacing*5,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Down, right);
				var up = new List<Rectangle>
				{
					new Rectangle(25*spriteSize+spacing*25,3*spriteSize+spacing*3,spriteSize,spriteSize),
					new Rectangle(25*spriteSize+spacing*25,4*spriteSize+spacing*4,spriteSize,spriteSize),
					new Rectangle(25*spriteSize+spacing*25,5*spriteSize+spacing*5,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Up, up);
				var down = new List<Rectangle>
				{
					new Rectangle(26*spriteSize+spacing*26,3*spriteSize+spacing*3,spriteSize,spriteSize),
					new Rectangle(26*spriteSize+spacing*26,4*spriteSize+spacing*4,spriteSize,spriteSize),
					new Rectangle(26*spriteSize+spacing*26,5*spriteSize+spacing*5,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Right, down);
			}
			else if (id == 2)
			{
				result.TotalFrames = 3;
				var left = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*23,6*spriteSize+spacing*6,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,7*spriteSize+spacing*7,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,8*spriteSize+spacing*8,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Left, left);
				var right = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*24,6*spriteSize+spacing*6,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*24,7*spriteSize+spacing*7,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*24,8*spriteSize+spacing*8,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Down, right);
				var up = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*25,6*spriteSize+spacing*6,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*25,7*spriteSize+spacing*7,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*25,8*spriteSize+spacing*8,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Up, up);
				var down = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*26,6*spriteSize+spacing*6,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*26,7*spriteSize+spacing*7,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*26,8*spriteSize+spacing*8,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Right, down);
			}
			else if (id == 3)
			{
				result.TotalFrames = 3;
				var left = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*23,9*spriteSize+spacing*9,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,10*spriteSize+spacing*10,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,11*spriteSize+spacing*11,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Left, left);
				var right = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*24,9*spriteSize+spacing*9,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*24,10*spriteSize+spacing*10,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*24,11*spriteSize+spacing*11,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Down, right);
				var up = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*25,9*spriteSize+spacing*9,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*25,10*spriteSize+spacing*10,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*25,11*spriteSize+spacing*11,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Up, up);
				var down = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*26,9*spriteSize+spacing*9,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*26,10*spriteSize+spacing*10,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*26,11*spriteSize+spacing*11,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Right, down);
			}
			else if (id == 4)
			{
				result.TotalFrames = 3;
				var left = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*23,12*spriteSize+spacing*12,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,13*spriteSize+spacing*13,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,14*spriteSize+spacing*14,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Left, left);
				var right = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*24,12*spriteSize+spacing*12,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*24,13*spriteSize+spacing*13,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*24,14*spriteSize+spacing*14,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Down, right);
				var up = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*25,12*spriteSize+spacing*12,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*25,13*spriteSize+spacing*13,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*25,14*spriteSize+spacing*14,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Up, up);
				var down = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*26,12*spriteSize+spacing*12,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*26,13*spriteSize+spacing*13,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*26,14*spriteSize+spacing*14,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Right, down);
			}
			else if (id == 5)
			{
				result.TotalFrames = 3;
				var left = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*23,15*spriteSize+spacing*15,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,16*spriteSize+spacing*16,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*23,17*spriteSize+spacing*17,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Left, left);
				var right = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*24,15*spriteSize+spacing*15,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*24,16*spriteSize+spacing*16,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*24,17*spriteSize+spacing*17,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Down, right);
				var up = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*25,15*spriteSize+spacing*15,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*25,16*spriteSize+spacing*16,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*25,17*spriteSize+spacing*17,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Up, up);
				var down = new List<Rectangle>
				{
					new Rectangle(23*spriteSize+spacing*26,15*spriteSize+spacing*15,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*26,16*spriteSize+spacing*16,spriteSize,spriteSize),
					new Rectangle(23*spriteSize+spacing*26,17*spriteSize+spacing*17,spriteSize,spriteSize),
				};
				result.AddDirectionFrames(FaceDirection.Right, down);
			}

			return result;
		}
	}
}
