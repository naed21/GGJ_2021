using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGJ_2021
{
	public class Character
	{
		int _CurrentFrame = 0;
		public int TotalFrames = 0;
		Texture2D _SpriteSheet;
		TimeSpan _StepSpeed;
		bool _IsPlayer = false;

		List<Card> _DeckOfCards = new List<Card>();

		FaceDirection _FaceDirection = FaceDirection.Down;

		public Vector2 Position { get; set; }
		public Point GridPosition { get; set; }

		public bool PlayAnimation { get; set; }

		public Character(Texture2D spriteSheet, TimeSpan stepSpeed, bool isPlayer)
		{
			_SpriteSheet = spriteSheet;
			_StepSpeed = stepSpeed;
			_IsPlayer = isPlayer;
			PlayAnimation = false;
		}

		Dictionary<FaceDirection, List<Rectangle>> _AnimationFrames = new Dictionary<FaceDirection, List<Rectangle>>();

		public void AddDirectionFrames(FaceDirection dir, List<Rectangle> frames)
		{
			if (!_AnimationFrames.ContainsKey(dir))
				_AnimationFrames[dir] = new List<Rectangle>();

			_AnimationFrames[dir].AddRange(frames);
		}

		public void CreateStartingDeck(List<Card> cards)
		{
			_DeckOfCards = cards;
		}

		TimeSpan _Timespan = TimeSpan.Zero;
		public void WorldUpdate(GameTime gameTime)
		{
			//Handle walking around and stuff
			if (PlayAnimation && _Timespan.TotalMilliseconds >= _StepSpeed.TotalMilliseconds)
			{
				_CurrentFrame++;
				if (_CurrentFrame >= TotalFrames)
					_CurrentFrame = 0;

				_Timespan = TimeSpan.Zero;
			}
			else if(PlayAnimation)
				_Timespan += gameTime.ElapsedGameTime;
		}

		public List<Card> CopyDeck()
		{
			List<Card> results = new List<Card>();

			foreach(var card in _DeckOfCards)
			{
				results.Add(card.Clone());
			}

			return results;
		}

		//Moved to battle manager

		//Random _Random = new Random();
		//internal List<Card> GetCards(int numCards)
		//{
		//	List<Card> results = new List<Card>();
		//	for(int x = 0; x < numCards; x++)
		//	{
		//		int index = _Random.Next(0, _DeckOfCards.Count);
		//		results.Add(_DeckOfCards[index].Clone());
		//	}

		//	return results;
		//}

		public void BattleUpdate(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
		{
			if(_IsPlayer)
			{
				//Do battle stuff
			}
			else
			{
				//do AI? Or should battle manager handle this?
			}
		}

		public void WorldDraw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			var rect = _AnimationFrames[_FaceDirection][_CurrentFrame];
			
			spriteBatch.Draw(_SpriteSheet, Position, rect, Color.White);
		}

		public void BattleDraw(GameTime gameTime, SpriteBatch spriteBatch)
		{

		}

		internal void ResetAnimation()
		{
			_CurrentFrame = 0;
			PlayAnimation = false;
			_Timespan = TimeSpan.Zero;
		}

		internal void StartAnimation(FaceDirection direction)
		{
			PlayAnimation = true;
			_FaceDirection = direction;
		}
	}
}
