using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using SharpFont;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using System.Text;

namespace GGJ_2021
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
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
		Texture2D _textTexture;
		SoundManager _soundManager;

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			Texture2D urbanSpriteSheet = null;
			// TODO: use this.Content to load your game content here
			using (var fileStream = new FileStream(Path.Combine(Content.RootDirectory, "Kenney Game Assets 3 version 20", "2D assets", "RPG Urban Pack", "tilemap.png"), FileMode.Open))
			{
				urbanSpriteSheet = Texture2D.FromStream(GraphicsDevice, fileStream);
			}

			string fontPath = Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "2D assets", "UI Space Pack", "Fonts", "kenvector_future.ttf");

			_FontService = new FontService();
			_FontService.Size = 24f;
			_FontService.SetFont(fontPath);
			var bitmap = _FontService.RenderString("This is a test 123", System.Drawing.Color.White, System.Drawing.Color.Transparent);
			_textTexture = GetTexture2DFromBitmap(GraphicsDevice, bitmap);

			SoundManager _soundManager = new SoundManager();
			_soundManager.AddOgg(SoundType.Background,Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "Audio", "Sad Town.ogg"), "SadTown");
			_soundManager.AddOgg(SoundType.Background, Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "Audio", "Alpha Dance.ogg"), "AlphaDance");
			_soundManager.AddOgg(SoundType.Background, Path.Combine(Content.RootDirectory, "Kenney Game Assets (version 41)", "Audio", "Time Driving.ogg"), "TimeDriving");
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();



			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			_spriteBatch.Begin();
			_spriteBatch.Draw(_textTexture, new Rectangle(10, 10, _textTexture.Width, _textTexture.Height), Color.White);
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
