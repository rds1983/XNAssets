using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using XNAssets.Assets;
using XNAssets.Utility;

namespace XNAssets.Samples.AssetManagement
{
	public class AssetManagementGame : Game
	{
		private readonly GraphicsDeviceManager _graphics;
		private AssetManager AssetManager;
		private SpriteFont _font;
		private Texture2D _texture;
		private SpriteBatch _spriteBatch;

		public AssetManagementGame()
		{
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1200,
				PreferredBackBufferHeight = 800
			};
			Window.AllowUserResizing = true;
			IsMouseVisible = true;
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			var assetResolver = new FileAssetResolver(Path.Combine(PathUtils.ExecutingAssemblyDirectory, "Assets"));
			AssetManager = new AssetManager(GraphicsDevice, assetResolver);

			_font = AssetManager.Load<SpriteFont>("fonts/arial64.fnt");
			_texture = AssetManager.Load<Texture2D>("images/LogoOnly_64px.png");

			_spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();

			_spriteBatch.DrawString(_font, "The quick brown fox jumps over the lazy dog",
				new Vector2(0, 0), Color.White);

			_spriteBatch.Draw(_texture, new Vector2(400, 100), Color.White);

			_spriteBatch.End();
		}
	}
}