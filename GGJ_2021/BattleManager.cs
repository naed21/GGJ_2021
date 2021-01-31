using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGJ_2021
{
	public class BattleManager
	{
		//Texture2D _BackOfCard;
		Texture2D _RockCard;
		Texture2D _PaperCard;
		Texture2D _ScissorCard;

		FontService _FontService;
		//These are generated as needed
		Texture2D _FirstSelectedLabel;
		Texture2D _SecondSelectedLabel;
		Texture2D _HoverLabel;
		Dictionary<int, Texture2D> ScoreTextures = new Dictionary<int, Texture2D>();

		Texture2D _PlayerDeckCountLabel;
		Texture2D _ScoreLabel;
		Texture2D _DashLabel;
		Texture2D _PlayerLabel;
		Texture2D _EnemyLabel;
		Texture2D _SubmitLabel;
		Texture2D _VictoryLabel;
		Texture2D _DefeatLabel;

		public BattleState BattleState = BattleState.Draw;

		Character _Player;
		Character _Enemy;
		List<Card> _PlayerHand = new List<Card>();
		List<Card> _PlayerDeck = new List<Card>();
		List<Card> _EnemyDeck = new List<Card>();

		int _PlayerScore = 0;
		int _EnemyScore = 0;

		GraphicsDevice _GraphicsDevice;

		Action _GameOverAction;
		public BattleManager(Action gameOverAction)
		{
			_GameOverAction = gameOverAction;
		}

		public void Setup(GraphicsDevice graphicsDevice, FontService fontService, Texture2D rock, Texture2D paper, Texture2D scissor)
		{			
			_GraphicsDevice = graphicsDevice;
			
			_RockCard = rock;
			_PaperCard = paper;
			_ScissorCard = scissor;
			
			_FontService = fontService;

			var bitmap = _FontService.RenderString("[1]", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			var bitmap2 = _FontService.RenderString("[2]", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			var bitmap3 = _FontService.RenderString("[^]", System.Drawing.Color.White, System.Drawing.Color.Transparent);

			_FirstSelectedLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap);
			_SecondSelectedLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap2);
			_HoverLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap3);

			var bitmap4 = _FontService.RenderString("Cards Remaining:", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			_PlayerDeckCountLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap4);
			
			var bitmap5 = _FontService.RenderString("Score:", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			_ScoreLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap5);

			var bitmap6 = _FontService.RenderString("-", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			_DashLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap6);

			var bitmap7 = _FontService.RenderString("Player", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			_PlayerLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap7);

			var bitmap8 = _FontService.RenderString("Enemy", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			_EnemyLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap8);

			var bitmap9 = _FontService.RenderString("[Submit]", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			_SubmitLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap9);

			var bitmap10 = _FontService.RenderString("[Victory]", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			_VictoryLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap10);

			var bitmap11 = _FontService.RenderString("[Defeat]", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			_DefeatLabel = Texture2DHelper.GetTexture2DFromBitmap(graphicsDevice, bitmap11);
		}

		public void Start(Character player, Character enemy)
		{
			_Player = player;
			_Enemy = enemy;
			_PlayerScore = 0;
			_EnemyScore = 0;
			_PlayerDeck = _Player.CopyDeck();
			_EnemyDeck = _Enemy.CopyDeck();
			_Victory = false;
			_Defeat = false;
			BeginDrawCards();
		}

		/*
		 * Draw 5 cards
		 * User picks two cards
		 * Once two are picked, AI picks two
		 * Only show 2 and then show +- foreach player
		 * Check for winner
		 * Repeat
		 */

		public void BeginDrawCards()
		{
			BattleState = BattleState.Draw;
			
			_PlayerHand = GetCards(_PlayerDeck, 5);

			if(_PlayerHand.Count == 0)
			{
				//switch to game over
				BeginResult(true);
			}

			int cardId = 0;
			foreach(var card in _PlayerHand)
			{
				card.Position = new Vector2(600, 300);
				card.TargetPosition = new Vector2(cardId * 100 + 50, 300);
				cardId++;
			}
		}

		bool _Victory = false;
		bool _Defeat = false;
		public void BeginResult(bool endGame = false)
		{
			if (!endGame)
			{
				if (_playerCardOne.CardType == CardType.Paper
					&& _enemyCardOne.CardType == CardType.Scissor)
				{
					_PlayerScore--;
					_EnemyScore++;
				}

				if (_playerCardOne.CardType == CardType.Paper
					&& _enemyCardOne.CardType == CardType.Rock)
				{
					_PlayerScore++;
					_EnemyScore--;
				}

				if (_playerCardOne.CardType == CardType.Rock
					&& _enemyCardOne.CardType == CardType.Scissor)
				{
					_PlayerScore++;
					_EnemyScore--;
				}

				if (_playerCardOne.CardType == CardType.Rock
					&& _enemyCardOne.CardType == CardType.Paper)
				{
					_PlayerScore--;
					_EnemyScore++;
				}

				if (_playerCardOne.CardType == CardType.Scissor
					&& _enemyCardOne.CardType == CardType.Rock)
				{
					_PlayerScore--;
					_EnemyScore++;
				}

				if (_playerCardOne.CardType == CardType.Scissor
					&& _enemyCardOne.CardType == CardType.Paper)
				{
					_PlayerScore++;
					_EnemyScore--;
				}

				///////////////////////////////////////

				if (_playerCardTwo.CardType == CardType.Paper
					&& _enemyCardTwo.CardType == CardType.Scissor)
				{
					_PlayerScore--;
					_EnemyScore++;
				}

				if (_playerCardTwo.CardType == CardType.Paper
					&& _enemyCardTwo.CardType == CardType.Rock)
				{
					_PlayerScore++;
					_EnemyScore--;
				}

				if (_playerCardTwo.CardType == CardType.Rock
					&& _enemyCardTwo.CardType == CardType.Scissor)
				{
					_PlayerScore++;
					_EnemyScore--;
				}

				if (_playerCardTwo.CardType == CardType.Rock
					&& _enemyCardTwo.CardType == CardType.Paper)
				{
					_PlayerScore--;
					_EnemyScore++;
				}

				if (_playerCardTwo.CardType == CardType.Scissor
					&& _enemyCardTwo.CardType == CardType.Rock)
				{
					_PlayerScore--;
					_EnemyScore++;
				}

				if (_playerCardTwo.CardType == CardType.Scissor
					&& _enemyCardTwo.CardType == CardType.Paper)
				{
					_PlayerScore++;
					_EnemyScore--;
				}

				if (_PlayerScore >= 5 || _EnemyScore >= 5)
					endGame = true;
			}

			//not else if, prev block can set this
			if(endGame)
			{
				//_GameOverAction();
				if (_PlayerScore >= _EnemyScore)
					_Victory = true;
				else
					_Defeat = true;

				BattleState = BattleState.Result;
			}
			else
			{
				BeginDrawCards();
			}
		}

		Random _Random = new Random();
		//TAKES the card from the deck, so the deck can "run out"
		private List<Card> GetCards(List<Card> deck, int numCards)
		{
			List<Card> results = new List<Card>();
			for (int x = 0; x < numCards; x++)
			{
				if (deck.Count != 0)
				{
					int index = _Random.Next(0, deck.Count);
					results.Add(deck[index]);
					deck.RemoveAt(index);
				}
			}

			return results;
		}

		Card _playerCardOne;
		Card _playerCardTwo;
		Card _enemyCardOne;
		Card _enemyCardTwo;
		public void BeginCompare()
		{
			BattleState = BattleState.Compare;

			foreach(var card in _PlayerHand)
			{
				//Put the card back into the deck
				if (card.SelectedNumber == 0)
					_PlayerDeck.Add(card);

				if (card.SelectedNumber == 1)
					_playerCardOne = card;
				if (card.SelectedNumber == 2)
					_playerCardTwo = card;
			}

			var enemyChoices = GetCards(_EnemyDeck, 2);
			_enemyCardOne = enemyChoices[0];
			_enemyCardTwo = enemyChoices[1];

			_enemyCardOne.Position = new Vector2(-100, -100);
			_enemyCardTwo.Position = new Vector2(-100, -100);

			_enemyCardOne.TargetPosition = _playerCardOne.Position + new Vector2(0, -150);
			_enemyCardTwo.TargetPosition = _playerCardTwo.Position + new Vector2(0, -150);
		}

		MouseState? prevMouseState = null;
		public void Update(GameTime gameTime, MouseState mouseState)
		{
			if (!prevMouseState.HasValue)
				prevMouseState = mouseState;

			if (BattleState == BattleState.Draw)
			{
				bool waitingForCards = false;
				foreach (var card in _PlayerHand)
				{
					card.Update(gameTime);
					if (!card.IsCardReady)
						waitingForCards = true;
				}

				if (!waitingForCards)
				{
					if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.Value.LeftButton == ButtonState.Released)
					{
						bool oneSelected = false;
						bool twoSelected = false;
						Card selected = null;
						var mouseRect = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

						foreach (var card in _PlayerHand)
						{
							var rect = new Rectangle((int)card.Position.X, (int)card.Position.Y, _RockCard.Width, _RockCard.Height);

							if (rect.Intersects(mouseRect))
							{
								//Select this card
								selected = card;
							}

							if (card.SelectedNumber == 1)
								oneSelected = true;
							else if (card.SelectedNumber == 2)
								twoSelected = true;
						}

						if (selected != null)
						{
							if (oneSelected && twoSelected && selected.SelectedNumber == 0)
							{
								//ignore click
							}
							else if (oneSelected && !twoSelected && selected.SelectedNumber == 0)
								selected.SelectedNumber = 2;
							else if (!oneSelected && twoSelected && selected.SelectedNumber == 0)
								selected.SelectedNumber = 1;
							else if (!oneSelected && !twoSelected && selected.SelectedNumber == 0)
								selected.SelectedNumber = 1;
							else if (selected.SelectedNumber != 0)
								selected.SelectedNumber = 0;
						}

						var submitRect = new Rectangle((int)_SubmitButtonPos.X, (int)_SubmitButtonPos.Y, _SubmitLabel.Width, _SubmitLabel.Height);

						if (oneSelected && twoSelected && submitRect.Intersects(mouseRect))
						{
							BeginCompare();
						}
					}
				}
			}
			else if (BattleState == BattleState.Compare)
			{
				//show AI choices and player choice
				_enemyCardOne.Update(gameTime);
				_enemyCardTwo.Update(gameTime);

				bool waiting = !_enemyCardOne.IsCardReady && !_enemyCardTwo.IsCardReady;

				if (!waiting && mouseState.LeftButton == ButtonState.Pressed && prevMouseState.Value.LeftButton == ButtonState.Released)
				{
					var mouseRect = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
					var submitRect = new Rectangle((int)_SubmitButtonPos.X, (int)_SubmitButtonPos.Y, _SubmitLabel.Width, _SubmitLabel.Height);
					if(submitRect.Intersects(mouseRect))
					{
						BeginResult();
					}
				}
			}
			else if(BattleState == BattleState.Result)
			{
				//click final message

				if(mouseState.LeftButton == ButtonState.Pressed && prevMouseState.Value.LeftButton == ButtonState.Released)
				{
					var mouseRect = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
					if (_Victory)
					{
						var finalRect = new Rectangle((int)_gameOverLabelPos.X, (int)_gameOverLabelPos.Y, _VictoryLabel.Width, _VictoryLabel.Height);
						if (mouseRect.Intersects(finalRect))
							_GameOverAction();
					}
					else if(_Defeat)
					{
						var finalRect = new Rectangle((int)_gameOverLabelPos.X, (int)_gameOverLabelPos.Y, _DefeatLabel.Width, _DefeatLabel.Height);
						if (mouseRect.Intersects(finalRect))
							_GameOverAction();
					}
				}
			}

			//Build text textures on the fly
			if (!ScoreTextures.ContainsKey(_PlayerDeck.Count))
			{
				var bitmap = _FontService.RenderString("+" + _PlayerDeck.Count.ToString(), System.Drawing.Color.White, System.Drawing.Color.Transparent);
				ScoreTextures.Add(_PlayerDeck.Count, Texture2DHelper.GetTexture2DFromBitmap(_GraphicsDevice, bitmap));
			}

			if (!ScoreTextures.ContainsKey(_PlayerScore))
			{
				System.Drawing.Bitmap bitmap;
				if (_PlayerScore >= 0)
					bitmap = _FontService.RenderString("+" + _PlayerScore.ToString(), System.Drawing.Color.White, System.Drawing.Color.Transparent);
				else
					bitmap = _FontService.RenderString(_PlayerScore.ToString(), System.Drawing.Color.White, System.Drawing.Color.Transparent);
				ScoreTextures.Add(_PlayerScore, Texture2DHelper.GetTexture2DFromBitmap(_GraphicsDevice, bitmap));
			}

			if (!ScoreTextures.ContainsKey(_EnemyScore))
			{
				System.Drawing.Bitmap bitmap;
				if (_EnemyScore >= 0)
					bitmap = _FontService.RenderString("+" + _EnemyScore.ToString(), System.Drawing.Color.White, System.Drawing.Color.Transparent);
				else
					bitmap = _FontService.RenderString(_EnemyScore.ToString(), System.Drawing.Color.White, System.Drawing.Color.Transparent);
				ScoreTextures.Add(_EnemyScore, Texture2DHelper.GetTexture2DFromBitmap(_GraphicsDevice, bitmap));
			}

			prevMouseState = mouseState;
		}

		Vector2 _ScoreLabelPos = new Vector2(240, 10);
		//Relative to score label pos
		Vector2 _DashLabelPos = new Vector2(15, 30);
		//Relative to score label pos
		Vector2 _PlayerLabelPos = new Vector2(-100, 10);
		Vector2 _EnemyLabelPos = new Vector2(100, 10);
		//Relative to player/enemy and score label
		Vector2 _ScorePos = new Vector2(10, 30);

		Vector2 _SubmitButtonPos = new Vector2(250, 450);

		Vector2 _gameOverLabelPos = new Vector2(300, 200);
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_ScoreLabel, _ScoreLabelPos, Color.White);
			
			//Need to wait 1 update for these to generate
			if(ScoreTextures.ContainsKey(_PlayerScore))
			{
				spriteBatch.Draw(_PlayerLabel, _ScoreLabelPos + _PlayerLabelPos, Color.White);
				spriteBatch.Draw(ScoreTextures[_PlayerScore], _ScoreLabelPos + _PlayerLabelPos + _ScorePos, Color.White);
			}

			//spriteBatch.Draw(_DashLabel, _ScoreLabelPos + _DashLabelPos, Color.White);

			if(ScoreTextures.ContainsKey(_EnemyScore))
			{
				spriteBatch.Draw(_EnemyLabel, _ScoreLabelPos + _EnemyLabelPos, Color.White);
				spriteBatch.Draw(ScoreTextures[_EnemyScore], _ScoreLabelPos + _EnemyLabelPos + _ScorePos, Color.White);
			}

			if (BattleState == BattleState.Draw)
			{
				foreach(var card in _PlayerHand)
				{
					if (card.CardType == CardType.Rock)
						spriteBatch.Draw(_RockCard, card.Position, Color.White);
					else if (card.CardType == CardType.Paper)
						spriteBatch.Draw(_PaperCard, card.Position, Color.White);
					else if (card.CardType == CardType.Scissor)
						spriteBatch.Draw(_ScissorCard, card.Position, Color.White);

					if(card.SelectedNumber == 1)
					{
						spriteBatch.Draw(_FirstSelectedLabel, card.Position + new Vector2(10, -20), Color.White);
					}

					if (card.SelectedNumber == 2)
					{
						spriteBatch.Draw(_SecondSelectedLabel, card.Position + new Vector2(10, -20), Color.White);
					}
				}

				spriteBatch.Draw(_SubmitLabel, _SubmitButtonPos, Color.White);
			}
			else if(BattleState == BattleState.Compare)
			{
				if (_playerCardOne.CardType == CardType.Rock)
					spriteBatch.Draw(_RockCard, _playerCardOne.Position, Color.White);
				else if (_playerCardOne.CardType == CardType.Paper)
					spriteBatch.Draw(_PaperCard, _playerCardOne.Position, Color.White);
				else if (_playerCardOne.CardType == CardType.Scissor)
					spriteBatch.Draw(_ScissorCard, _playerCardOne.Position, Color.White);

				if (_playerCardTwo.CardType == CardType.Rock)
					spriteBatch.Draw(_RockCard, _playerCardTwo.Position, Color.White);
				else if (_playerCardTwo.CardType == CardType.Paper)
					spriteBatch.Draw(_PaperCard, _playerCardTwo.Position, Color.White);
				else if (_playerCardTwo.CardType == CardType.Scissor)
					spriteBatch.Draw(_ScissorCard, _playerCardTwo.Position, Color.White);

				if (_enemyCardOne.CardType == CardType.Rock)
					spriteBatch.Draw(_RockCard, _enemyCardOne.Position, Color.White);
				else if (_enemyCardOne.CardType == CardType.Paper)
					spriteBatch.Draw(_PaperCard, _enemyCardOne.Position, Color.White);
				else if (_enemyCardOne.CardType == CardType.Scissor)
					spriteBatch.Draw(_ScissorCard, _enemyCardOne.Position, Color.White);

				if (_enemyCardTwo.CardType == CardType.Rock)
					spriteBatch.Draw(_RockCard, _enemyCardTwo.Position, Color.White);
				else if (_enemyCardTwo.CardType == CardType.Paper)
					spriteBatch.Draw(_PaperCard, _enemyCardTwo.Position, Color.White);
				else if (_enemyCardTwo.CardType == CardType.Scissor)
					spriteBatch.Draw(_ScissorCard, _enemyCardTwo.Position, Color.White);

				spriteBatch.Draw(_SubmitLabel, _SubmitButtonPos, Color.White);
			}
			else if(BattleState == BattleState.Result)
			{
				if (_Victory)
					spriteBatch.Draw(_VictoryLabel, _gameOverLabelPos, Color.White);
				else if (_Defeat)
					spriteBatch.Draw(_DefeatLabel, _gameOverLabelPos, Color.White);
			}
		}
	}
}
