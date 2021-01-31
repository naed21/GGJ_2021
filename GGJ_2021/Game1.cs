using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using SharpFont;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using System.Text;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace GGJ_2021
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		Point _ScreenSize = new Point(640, 480);

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			_graphics.PreferredBackBufferWidth = _ScreenSize.X;
			_graphics.PreferredBackBufferHeight = _ScreenSize.Y;
			_graphics.ApplyChanges();
		}

		/*
		 * Idea is that you find a robot in a trash pile and it says its lost and wants you to find its home [map01]
		 * -> But the robot just leads you to more robots in trash piles and you fix them up [map02]
		 * -> Then you go to this place and everyone fights you [map03]
		 * -> After you win your robot friend says thanks and they go to where they belong -> "takes over the internet", or whatever that means
		 * 
		 * TODO:
		 * - Create UI using UI pack
		 * - Figure out how to do the ttf font
		 * - Create maps
		 * - Walk around maps
		 * - When transitioning maps, or entering a battle -> pick random Pattern and use it as a transition
		 *	-> -> Can do something like, combine the screen into one texture. But only apply if it matches with a white or black pixel
		 *	-> -> then have it transition by showing each texture overlaped with each other, but then replace it -> then just show the normal draw
		 * - Use the "smiles?" to add some emotion above random people's heads for fun? Or it'll be assigned based on what they say when you talk to them
		 * - The Kenny asset pack has audio too, but it's in ogg format and idk how to use that
		 * - Battle screen
		 *	-> Figure out battle system. Think of something kinda weird/different?
		 *	-> Rock/Paper/Scissors, get 4 of each. Then pick 1 of each type to throw away for that battle? Forever?
		 *	-> When you finish someone off the last card you use upgrades
		 *	-> Choose 2 of 4 per round. If upgraded more than other, tie = win, loss = tie
		 */

		FontService _FontService;
		//Texture2D _textTexture;
		SoundManager _SoundManager;

		MapManager _StartLevel;
		MapManager _CityLevel;

		MenuManager _StartMenu;
		MenuManager _PreBattleMenu;

		GameState _GameState = GameState.MainMenu;

		Texture2D _UrbanSpriteSheet = null;
		Character _PlayerCharacter = null;

		List<Character> _CharacterList = new List<Character>();

		BattleManager _BattleManager;

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			using (var fileStream = new FileStream(Path.Combine(Content.RootDirectory, "Kenney Game Assets 3 version 20", "2D assets", "RPG Urban Pack", "tilemap.png"), FileMode.Open))
			{
				_UrbanSpriteSheet = Texture2D.FromStream(GraphicsDevice, fileStream);
			}

			_SoundManager = new SoundManager(0.8f);
			_SoundManager.AddOgg(SoundType.Background,Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "Audio", "Sad Town.ogg"), "SadTown");
			_SoundManager.AddOgg(SoundType.Background, Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "Audio", "Alpha Dance.ogg"), "AlphaDance");
			_SoundManager.AddOgg(SoundType.Background, Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "Audio", "Time Driving.ogg"), "TimeDriving");

			_SoundManager.Play(SoundType.Background);

			TmxMap startMapData;
			TmxMap cityMapData;

			var serializer = new XmlSerializer(typeof(TmxMap));
			using (var reader = new StreamReader(Path.Combine(Content.RootDirectory, "TitleScreen.tmx")))
			{
				startMapData = (TmxMap)serializer.Deserialize(reader);
			}

			using (var reader = new StreamReader(Path.Combine(Content.RootDirectory, "City.tmx")))
			{
				cityMapData = (TmxMap)serializer.Deserialize(reader);
			}

			_StartLevel = new MapManager(startMapData, _UrbanSpriteSheet, 27, 1);
			_CityLevel = new MapManager(cityMapData, _UrbanSpriteSheet, 27, 1);

			_StartLevel.SetTargetCenterOffset(new Point(25, 25), _ScreenSize, new Vector2(1f, 1f));

			string uiPath = Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "2D assets", "UI Space Pack", "uipackSpace_sheet.xml");
			TextureAtlas uiAtlas;
			using (var reader = new StreamReader(uiPath))
			{
				var textureSerializer = new XmlSerializer(typeof(TextureAtlas));
				uiAtlas = (TextureAtlas)textureSerializer.Deserialize(reader);
			}

			Texture2D uiSpriteSheet = null;
			using (var fileStream = new FileStream(Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "2D assets", "UI Space Pack", "uipackSpace_sheet.png"), FileMode.Open))
			{
				uiSpriteSheet = Texture2D.FromStream(GraphicsDevice, fileStream);
			}

			string fontPath = Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "2D assets", "UI Space Pack", "Fonts", "kenvector_future.ttf");

			_FontService = new FontService();
			_FontService.Size = 12f;
			_FontService.SetFont(fontPath);
			System.Drawing.Bitmap bitmap = _FontService.RenderString("Main Menu", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
			var titleTexture = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap);

			_StartMenu = new MenuManager(uiAtlas, uiSpriteSheet);
			_StartMenu.AddPanel("metalPanel_blueCorner.png", new Rectangle(50, 50, 250, 200));
			_StartMenu.AddText(titleTexture, new Vector2(60, 70));

			bitmap = _FontService.RenderString("Start Game", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
			var startGameTexture = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap);
			_StartMenu.AddText(startGameTexture, new Vector2(120, 130));
			_StartMenu.AddButton("dotWhite.png", "dotGreen.png", new Rectangle(70, 120, 30, 30), StartGameAction);

			bitmap = _FontService.RenderString("Settings", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
			var settingsTexture = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap);
			_StartMenu.AddText(settingsTexture, new Vector2(120, 170));
			_StartMenu.AddButton("dotWhite.png", "dotYellow.png", new Rectangle(70, 160, 30, 30), SettingsAction);

			bitmap = _FontService.RenderString("Exit Game", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
			var leaveameTexture = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap);
			_StartMenu.AddText(leaveameTexture, new Vector2(120, 210));
			_StartMenu.AddButton("dotWhite.png", "dotRed.png", new Rectangle(70, 200, 30, 30), LeaveGame);

			_PreBattleMenu = new MenuManager(uiAtlas, uiSpriteSheet);

			Texture2D rockSprite = null;
			using (var fileStream = new FileStream(Path.Combine(Content.RootDirectory, "RockCard.png"), FileMode.Open))
			{
				rockSprite = Texture2D.FromStream(GraphicsDevice, fileStream);
			}

			Texture2D paperSprite = null;
			using (var fileStream = new FileStream(Path.Combine(Content.RootDirectory, "PaperCard.png"), FileMode.Open))
			{
				paperSprite = Texture2D.FromStream(GraphicsDevice, fileStream);
			}

			Texture2D scissorSprite = null;
			using (var fileStream = new FileStream(Path.Combine(Content.RootDirectory, "ScissorCard.png"), FileMode.Open))
			{
				scissorSprite = Texture2D.FromStream(GraphicsDevice, fileStream);
			}

			_BattleManager = new BattleManager(GotoCityAction);
			_BattleManager.Setup(GraphicsDevice, _FontService, rockSprite, paperSprite, scissorSprite);
		}

		bool didWeGoToCity = false;
		public void GotoCityAction()
		{
			_GameState = GameState.CityLevel;
			_SoundManager.Play(SoundType.Background);
			if (!didWeGoToCity)
			{
				var gridPos = new Point(25, 26);
				var pos = _StartLevel.GetPosOnGrid(gridPos);
				//_PlayerCharacter = CharacterFactory.CreateCharacter(0, _UrbanSpriteSheet, pos, true);
				_PlayerCharacter.GridPosition = gridPos;
				_PlayerCharacter.Position = pos;

				_CityLevel.SetTargetCenterOffset(gridPos, _ScreenSize, new Vector2(10f, 10f));

				//Add to map to have it's pos updated when the map moves
				_CityLevel.AddCharacter(_PlayerCharacter);

				_CharacterList.Clear();
				_CharacterList.Add(_PlayerCharacter);
				//create some AI characters

				AddEnemyToCity(new Point(35, 37), 2);
				AddEnemyToCity(new Point(4, 35), 3);
				AddEnemyToCity(new Point(6, 26), 4);
				AddEnemyToCity(new Point(17, 7), 2);
				AddEnemyToCity(new Point(44, 3), 3);
				AddEnemyToCity(new Point(44, 33), 5);
			}

			didWeGoToCity = true;
		}

		public void AddEnemyToCity(Point pos, int charId)
		{
			var enemyGridPos = pos;
			var enemyPos = _CityLevel.GetPosOnGrid(enemyGridPos);
			var enemy = CharacterFactory.CreateCharacter(charId, _UrbanSpriteSheet, enemyPos, false);
			enemy.GridPosition = enemyGridPos;

			_CityLevel.AddCharacter(enemy);
			_CharacterList.Add(enemy);
		}

		public void StartGameAction()
		{
			//Skip setup for now, just go straight to the game
			//_GameState = GameState.Setup;
			_GameState = GameState.StartLevel;
			var gridPos = new Point(25, 26);
			var pos = _StartLevel.GetPosOnGrid(gridPos);
			_PlayerCharacter = CharacterFactory.CreateCharacter(0, _UrbanSpriteSheet, pos, true);
			_PlayerCharacter.GridPosition = gridPos;

			_StartLevel.SetTargetCenterOffset(gridPos, _ScreenSize, new Vector2(10f, 10f));

			//Add to map to have it's pos updated when the map moves
			_StartLevel.AddCharacter(_PlayerCharacter);
			//idk, draw from Game1.cs?
			_CharacterList.Add(_PlayerCharacter);
			//create some AI characters

			var enemyGridPos = new Point(3, 29);
			var enemyPos = _StartLevel.GetPosOnGrid(enemyGridPos);
			var enemy = CharacterFactory.CreateCharacter(1, _UrbanSpriteSheet, enemyPos, false);
			enemy.GridPosition = enemyGridPos;

			_StartLevel.AddCharacter(enemy);
			_CharacterList.Add(enemy);
		}

		public void SettingsAction()
		{
			//_GameState = GameState.Settings;
		}

		public void LeaveGame()
		{
			Exit();
		}

		public void StartBattleAction()
		{
			_ShowPreBattleMessage = false;
			_GameState = GameState.Battle;
			_BattleManager.Start(_PlayerCharacter, battleTarget);
		}

		MouseState? _prevMouseState = null;
		KeyboardState? _prevKeyState = null;

		Point _MapPushBottomLeft = new Point(20,35);
		Point _MapPushTopRight = new Point(30, 15);

		bool _ShowPreBattleMessage = false;
		Character battleTarget = null;

		//The speed that the map adjusts each step (update)
		TimeSpan _StepSpeed = new System.TimeSpan(0, 0, 0, 0, milliseconds: 100);
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			var mouseState = Mouse.GetState();
			var keyState = Keyboard.GetState();

			if (!_prevMouseState.HasValue)
				_prevMouseState = mouseState;

			if (!_prevKeyState.HasValue)
				_prevKeyState = keyState;

			_SoundManager.Update(gameTime);

			if (_GameState == GameState.MainMenu
				|| _GameState == GameState.Settings
				|| _GameState == GameState.Setup
				|| _GameState == GameState.StartLevel)
			{
				_StartLevel.Update(gameTime, _StepSpeed);
			}

			if(_GameState == GameState.MainMenu)
			{
				if(mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.Value.LeftButton == ButtonState.Released)
				{
					_StartMenu.ClickAtPos(mouseState.Position);
				}

				_StartMenu.HoverAtPos(mouseState.Position);
			}
			
			if(_GameState == GameState.StartLevel)
			{
				_PlayerCharacter.WorldUpdate(gameTime);
				//handle user input to move char around
				//Wait for the map to finish moving
				if(!_StartLevel.IsAdjustingOffset && !_ShowPreBattleMessage)
				{
					//while the map is not adjusting the offset, stop animating
					_PlayerCharacter.ResetAnimation();
					if(keyState.IsKeyDown(Keys.A) && !_prevKeyState.Value.IsKeyDown(Keys.A))
					{
						var target = new Point(_PlayerCharacter.GridPosition.X - 1, _PlayerCharacter.GridPosition.Y);
						if (!_StartLevel.CheckForCollision(target))
						{
							_PlayerCharacter.GridPosition = target;
							//keep y where it is, trying to hide map border
							bool lockY = true;
							if (_MapPushTopRight.Y < target.Y && _MapPushBottomLeft.Y >= target.Y)
								lockY = false;
							if (_MapPushTopRight.X >= target.X && _MapPushBottomLeft.X < target.X)
								_StartLevel.SetTargetCenterOffset(_PlayerCharacter.GridPosition, _ScreenSize, new Vector2(4, 4), lockY:lockY);
							_PlayerCharacter.StartAnimation(FaceDirection.Left);
						}
					}
					else if(keyState.IsKeyDown(Keys.D) && !_prevKeyState.Value.IsKeyDown(Keys.D))
					{
						var target = new Point(_PlayerCharacter.GridPosition.X + 1, _PlayerCharacter.GridPosition.Y);
						if (!_StartLevel.CheckForCollision(target))
						{
							_PlayerCharacter.GridPosition = target;
							bool lockY = true;
							if (_MapPushTopRight.Y < target.Y && _MapPushBottomLeft.Y >= target.Y)
								lockY = false;
							if (_MapPushTopRight.X >= target.X && _MapPushBottomLeft.X < target.X)
								_StartLevel.SetTargetCenterOffset(_PlayerCharacter.GridPosition, _ScreenSize, new Vector2(4, 4), lockY:lockY);
							_PlayerCharacter.StartAnimation(FaceDirection.Right);
						}
					}
					else if (keyState.IsKeyDown(Keys.W) && !_prevKeyState.Value.IsKeyDown(Keys.W))
					{
						var target = new Point(_PlayerCharacter.GridPosition.X, _PlayerCharacter.GridPosition.Y - 1);
						if (!_StartLevel.CheckForCollision(target))
						{
							_PlayerCharacter.GridPosition = target;
							bool lockX = true;
							if (_MapPushTopRight.X >= target.X && _MapPushBottomLeft.X < target.X)
								lockX = false;
							if (_MapPushTopRight.Y < target.Y && _MapPushBottomLeft.Y >= target.Y)
								_StartLevel.SetTargetCenterOffset(_PlayerCharacter.GridPosition, _ScreenSize, new Vector2(4, 4), lockX: lockX);
							_PlayerCharacter.StartAnimation(FaceDirection.Up);
						}
					}
					else if (keyState.IsKeyDown(Keys.S) && !_prevKeyState.Value.IsKeyDown(Keys.S))
					{
						var target = new Point(_PlayerCharacter.GridPosition.X, _PlayerCharacter.GridPosition.Y + 1);
						if (!_StartLevel.CheckForCollision(target))
						{
							_PlayerCharacter.GridPosition = target;
							bool lockX = true;
							if (_MapPushTopRight.X >= target.X && _MapPushBottomLeft.X < target.X)
								lockX = false;
							if (_MapPushTopRight.Y < target.Y && _MapPushBottomLeft.Y >= target.Y)
								_StartLevel.SetTargetCenterOffset(_PlayerCharacter.GridPosition, _ScreenSize, new Vector2(4, 4), lockX: lockX);
							_PlayerCharacter.StartAnimation(FaceDirection.Down);
						}
					}

					if (keyState.IsKeyDown(Keys.Space) && !_prevKeyState.Value.IsKeyDown(Keys.Space))
					{
						foreach(var character in _CharacterList)
						{
							if (character == _PlayerCharacter)
								continue;

							var diff = character.GridPosition - _PlayerCharacter.GridPosition;
							if (diff.X <= 1 && diff.Y <= 1 && diff.X >= -1 && diff.Y >= -1)
							{
								battleTarget = character;
								break;
							}
						}

						if(battleTarget != null)
						{
							//do battle
							_ShowPreBattleMessage = true;

							var bitmap = _FontService.RenderString("Random Person", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
							var person = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap);
							_PreBattleMenu.AddText(person, new Vector2(70, 330));
							_PreBattleMenu.AddPanel("metalPanel_blueCorner.png", new Rectangle(50, 300, 400, 200));

							var bitmap2 = _FontService.RenderString("Hey kid, you seem lost.", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
							var text = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap2);
							_PreBattleMenu.AddText(text, new Vector2(80, 370));

							var bitmap3 = _FontService.RenderString("I'll put you in your place.", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
							var text2 = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap3);
							_PreBattleMenu.AddText(text2, new Vector2(80, 400));

							_PreBattleMenu.AddButton("dotWhite.png", "dotGreen.png", new Rectangle(400, 320, 30, 30), StartBattleAction);
						}
					}
				}

				if(_ShowPreBattleMessage)
				{
					if (mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.Value.LeftButton == ButtonState.Released)
					{
						_PreBattleMenu.ClickAtPos(mouseState.Position);
					}

					_PreBattleMenu.HoverAtPos(mouseState.Position);
				}
			}
			else if(_GameState == GameState.Battle)
			{
				_BattleManager.Update(gameTime, mouseState);
			}
			else if(_GameState == GameState.CityLevel)
			{
				_PlayerCharacter.WorldUpdate(gameTime);
				//handle user input to move char around
				//Wait for the map to finish moving
				_CityLevel.Update(gameTime, _StepSpeed);
				if (!_CityLevel.IsAdjustingOffset && !_ShowPreBattleMessage)
				{
					//while the map is not adjusting the offset, stop animating
					_PlayerCharacter.ResetAnimation();
					if (keyState.IsKeyDown(Keys.A) && !_prevKeyState.Value.IsKeyDown(Keys.A))
					{
						var target = new Point(_PlayerCharacter.GridPosition.X - 1, _PlayerCharacter.GridPosition.Y);
						if (!_CityLevel.CheckForCollision(target) && target.X < 50 && target.X >= 0)
						{
							_PlayerCharacter.GridPosition = target;
							//keep y where it is, trying to hide map border
							bool lockY = true;
							if (_MapPushTopRight.Y < target.Y && _MapPushBottomLeft.Y >= target.Y)
								lockY = false;
							if (_MapPushTopRight.X >= target.X && _MapPushBottomLeft.X < target.X)
								_CityLevel.SetTargetCenterOffset(_PlayerCharacter.GridPosition, _ScreenSize, new Vector2(4, 4), lockY: lockY);
							_PlayerCharacter.StartAnimation(FaceDirection.Left);
						}
					}
					else if (keyState.IsKeyDown(Keys.D) && !_prevKeyState.Value.IsKeyDown(Keys.D))
					{
						var target = new Point(_PlayerCharacter.GridPosition.X + 1, _PlayerCharacter.GridPosition.Y);
						if (!_CityLevel.CheckForCollision(target) && target.X < 50 && target.X >= 0)
						{
							_PlayerCharacter.GridPosition = target;
							bool lockY = true;
							if (_MapPushTopRight.Y < target.Y && _MapPushBottomLeft.Y >= target.Y)
								lockY = false;
							if (_MapPushTopRight.X >= target.X && _MapPushBottomLeft.X < target.X)
								_CityLevel.SetTargetCenterOffset(_PlayerCharacter.GridPosition, _ScreenSize, new Vector2(4, 4), lockY: lockY);
							_PlayerCharacter.StartAnimation(FaceDirection.Right);
						}
					}
					else if (keyState.IsKeyDown(Keys.W) && !_prevKeyState.Value.IsKeyDown(Keys.W))
					{
						var target = new Point(_PlayerCharacter.GridPosition.X, _PlayerCharacter.GridPosition.Y - 1);
						if (!_CityLevel.CheckForCollision(target) && target.Y < 50 && target.Y >= 0)
						{
							_PlayerCharacter.GridPosition = target;
							bool lockX = true;
							if (_MapPushTopRight.X >= target.X && _MapPushBottomLeft.X < target.X)
								lockX = false;
							if (_MapPushTopRight.Y < target.Y && _MapPushBottomLeft.Y >= target.Y)
								_CityLevel.SetTargetCenterOffset(_PlayerCharacter.GridPosition, _ScreenSize, new Vector2(4, 4), lockX: lockX);
							_PlayerCharacter.StartAnimation(FaceDirection.Up);
						}
					}
					else if (keyState.IsKeyDown(Keys.S) && !_prevKeyState.Value.IsKeyDown(Keys.S))
					{
						var target = new Point(_PlayerCharacter.GridPosition.X, _PlayerCharacter.GridPosition.Y + 1);
						if (!_CityLevel.CheckForCollision(target) && target.Y < 50 && target.Y >= 0)
						{
							_PlayerCharacter.GridPosition = target;
							bool lockX = true;
							if (_MapPushTopRight.X >= target.X && _MapPushBottomLeft.X < target.X)
								lockX = false;
							if (_MapPushTopRight.Y < target.Y && _MapPushBottomLeft.Y >= target.Y)
								_CityLevel.SetTargetCenterOffset(_PlayerCharacter.GridPosition, _ScreenSize, new Vector2(4, 4), lockX: lockX);
							_PlayerCharacter.StartAnimation(FaceDirection.Down);
						}
					}

					if (keyState.IsKeyDown(Keys.Space) && !_prevKeyState.Value.IsKeyDown(Keys.Space))
					{
						foreach (var character in _CharacterList)
						{
							if (character == _PlayerCharacter)
								continue;

							var diff = character.GridPosition - _PlayerCharacter.GridPosition;
							if (diff.X <= 1 && diff.Y <= 1 && diff.X >= -1 && diff.Y >= -1)
							{
								battleTarget = character;
								break;
							}
						}

						if (battleTarget != null)
						{
							//do battle
							_ShowPreBattleMessage = true;
							
							//Just use the prev message

							//var bitmap = _FontService.RenderString("Random Person", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
							//var person = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap);
							//_PreBattleMenu.AddText(person, new Vector2(70, 330));
							//_PreBattleMenu.AddPanel("metalPanel_blueCorner.png", new Rectangle(50, 300, 400, 200));

							//var bitmap2 = _FontService.RenderString("You'll never defeat me!", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
							//var text = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap2);
							//_PreBattleMenu.AddText(text, new Vector2(80, 370));

							//var bitmap3 = _FontService.RenderString("I'll put you in your place.", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
							//var text2 = Texture2DHelper.GetTexture2DFromBitmap(GraphicsDevice, bitmap3);
							//_PreBattleMenu.AddText(text2, new Vector2(80, 400));

							//_PreBattleMenu.AddButton("dotWhite.png", "dotGreen.png", new Rectangle(400, 320, 30, 30), StartBattleAction);
						}
					}
				}

				if (_ShowPreBattleMessage)
				{
					if (mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.Value.LeftButton == ButtonState.Released)
					{
						_PreBattleMenu.ClickAtPos(mouseState.Position);
					}

					_PreBattleMenu.HoverAtPos(mouseState.Position);
				}
			}

			_prevMouseState = mouseState;
			_prevKeyState = keyState;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			_spriteBatch.Begin();
			//_spriteBatch.Draw(_textTexture, new Rectangle(10, 10, _textTexture.Width, _textTexture.Height), Color.White);

			if(_GameState == GameState.MainMenu 
				|| _GameState == GameState.Settings 
				|| _GameState == GameState.Setup
				|| _GameState == GameState.StartLevel)
			{
				_StartLevel.Draw(gameTime, _spriteBatch);
			}

			if (_GameState == GameState.MainMenu)
			{
				_StartMenu.Draw(gameTime, _spriteBatch);
			}
			else if(_GameState == GameState.Setup)
			{

			}
			else if(_GameState == GameState.Settings)
			{

			}
			else if(_GameState == GameState.StartLevel)
			{
				foreach(var character in _CharacterList)
				{
					character.WorldDraw(gameTime, _spriteBatch);
				}

				if (_ShowPreBattleMessage)
					_PreBattleMenu.Draw(gameTime, _spriteBatch);
			}
			else if(_GameState == GameState.Battle)
			{
				_BattleManager.Draw(gameTime, _spriteBatch);
			}
			else if(_GameState == GameState.CityLevel)
			{
				_CityLevel.Draw(gameTime, _spriteBatch);
				
				foreach (var character in _CharacterList)
				{
					character.WorldDraw(gameTime, _spriteBatch);
				}

				if (_ShowPreBattleMessage)
					_PreBattleMenu.Draw(gameTime, _spriteBatch);
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
