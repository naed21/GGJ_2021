using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGJ_2021
{
	public class Card
	{
		public int Level { get; set; }
		public CardType CardType { get; set; }
		public Vector2 Position = Vector2.Zero;
		public Vector2 TargetPosition { get; set; }

		TimeSpan _TimeSpan = TimeSpan.Zero;
		TimeSpan _StepSpeed = new TimeSpan(0, 0, 0, 0, milliseconds: 100);

		Vector2 _MoveSpeed = new Vector2(20, 20);

		public int SelectedNumber = 0;

		public bool IsCardReady { get { return Position == TargetPosition; } }

		public void Update(GameTime gameTime)
		{
			if(Position != TargetPosition)
			{
				_TimeSpan += gameTime.ElapsedGameTime;

				if(_TimeSpan.TotalMilliseconds >= _StepSpeed.TotalMilliseconds)
				{
					_TimeSpan = TimeSpan.Zero;

					var diff = (Position - TargetPosition);
					if (Math.Abs(diff.X) <= _MoveSpeed.X && Math.Abs(diff.Y) <= _MoveSpeed.Y)
					{
						Position = TargetPosition;
						return;
					}

					float adjustX = _MoveSpeed.X;
					float adjustY = _MoveSpeed.Y;

					if (Math.Abs(diff.X) <= _MoveSpeed.X)
						adjustX = Math.Abs(diff.X);
					if (Math.Abs(diff.Y) <= _MoveSpeed.Y)
						adjustY = Math.Abs(diff.Y);

					if (Position.X < TargetPosition.X)
						Position.X += adjustX;
					else if (Position.X > TargetPosition.X)
						Position.X -= adjustX;

					if (Position.Y < TargetPosition.Y)
						Position.Y += adjustY;
					else if (Position.Y > TargetPosition.Y)
						Position.Y -= adjustY;

					if (Position == TargetPosition)
					{
						//m_IsAdjustingOffset = false;
					}
				}
			}
		}

		public Card Clone()
		{
			Card result = new Card()
			{
				CardType = this.CardType,
				Level = this.Level,
			};

			return result;
		}
	}
}
