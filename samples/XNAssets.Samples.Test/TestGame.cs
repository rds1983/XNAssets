using System.IO;
using System.Reflection;
using System;
using AssetManagementBase;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAssets
{
	public class TestGame : Game
	{
#if !STRIDE
		private readonly GraphicsDeviceManager _graphics;
#endif
		private AssetManager _assetManager;
		private SpriteBatch _spriteBatch;
		private SpriteFont _spriteFont;

		public static string ExecutingAssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().Location;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		public TestGame()
		{
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1200,
				PreferredBackBufferHeight = 800,
			};
			Window.AllowUserResizing = true;

			IsMouseVisible = true;
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			_assetManager = AssetManager.CreateFileAssetManager(Path.Combine(ExecutingAssemblyDirectory, "assets"));
			_spriteFont = _assetManager.LoadSpriteFont(GraphicsDevice, "calibri32.fnt");
			_spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			_spriteBatch.Begin();

			_spriteBatch.DrawString(_spriteFont, "The quick brown fox jumps over the lazy dog", Vector2.Zero, Color.LightCoral);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
