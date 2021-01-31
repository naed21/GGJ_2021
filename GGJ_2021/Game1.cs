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

		GameState _GameState = GameState.MainMenu;

		Texture2D _UrbanSpriteSheet = null;
		Character _PlayerCharacter = null;

		List<Character> _CharacterList = new List<Character>();

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

			SoundManager _soundManager = new SoundManager();
			_soundManager.AddOgg(SoundType.Background,Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "Audio", "Sad Town.ogg"), "SadTown");
			_soundManager.AddOgg(SoundType.Background, Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "Audio", "Alpha Dance.ogg"), "AlphaDance");
			_soundManager.AddOgg(SoundType.Background, Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "Audio", "Time Driving.ogg"), "TimeDriving");

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
			var titleTexture = GetTexture2DFromBitmap(GraphicsDevice, bitmap);

			_StartMenu = new MenuManager(uiAtlas, uiSpriteSheet);
			_StartMenu.AddPanel("metalPanel_blueCorner.png", new Rectangle(50, 50, 250, 200));
			_StartMenu.AddText(titleTexture, new Vector2(60, 70));

			bitmap = _FontService.RenderString("Start Game", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
			var startGameTexture = GetTexture2DFromBitmap(GraphicsDevice, bitmap);
			_StartMenu.AddText(startGameTexture, new Vector2(120, 130));
			_StartMenu.AddButton("dotWhite.png", "dotGreen.png", new Rectangle(70, 120, 30, 30), StartGameAction);

			bitmap = _FontService.RenderString("Settings", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
			var settingsTexture = GetTexture2DFromBitmap(GraphicsDevice, bitmap);
			_StartMenu.AddText(settingsTexture, new Vector2(120, 170));
			_StartMenu.AddButton("dotWhite.png", "dotYellow.png", new Rectangle(70, 160, 30, 30), SettingsAction);

			bitmap = _FontService.RenderString("Exit Game", System.Drawing.Color.Black, System.Drawing.Color.Transparent);
			var leaveameTexture = GetTexture2DFromBitmap(GraphicsDevice, bitmap);
			_StartMenu.AddText(leaveameTexture, new Vector2(120, 210));
			_StartMenu.AddButton("dotWhite.png", "dotRed.png", new Rectangle(70, 200, 30, 30), LeaveGame);

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
			_GameState = GameState.Settings;
		}

		public void LeaveGame()
		{
			Exit();
		}

		MouseState? _prevMouseState = null;
		KeyboardState? _prevKeyState = null;

		Point _MapPushBottomLeft = new Point(20,35);
		Point _MapPushTopRight = new Point(30, 15);
		
		//Put dude at: 3,29

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
				if(!_StartLevel.IsAdjustingOffset)
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

					}
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
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}

		public static Texture2D GetTexture2DFromBitmap(GraphicsDevice device, System.Drawing.Bitmap bitmap)
		{
			Texture2D tex = new Texture2D(device, bitmap.Width, bitmap.Height, false, SurfaceFormat.Color);

			System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

			int bufferSize = data.Height * data.Stride;

			//create data buffer 
			byte[] bytes = new byte[bufferSize];

			// copy bitmap data into buffer
			System.Runtime.InteropServices.Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

			// copy our buffer to the texture
			tex.SetData(bytes);

			// unlock the bitmap data
			bitmap.UnlockBits(data);

			return tex;
		}
	}
}
