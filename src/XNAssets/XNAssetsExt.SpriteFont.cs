#if !STRIDE

using XNAssets.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace AssetManagementBase
{
	/// <summary>
	/// Provides extension methods for loading font assets.
	/// </summary>
	partial class XNAssetsExt
	{
		private static readonly AssetLoader<SpriteFont> _spriteFontLoader = (manager, assetName, settings, tag) =>
		{
			var data = manager.ReadAsString(assetName);
			var graphicsDevice = (GraphicsDevice)tag;

			return BMFontLoader.Load(data, name => manager.ReadAsByteArray(name), graphicsDevice);
		};

		/// <summary>
		/// Loads a SpriteFont asset from a BMFont definition.
		/// </summary>
		/// <param name="assetManager">The AssetManager instance.</param>
		/// <param name="graphicsDevice">The GraphicsDevice to create the font with.</param>
		/// <param name="assetName">The name or path of the font definition asset (BMFont XML or text format).</param>
		/// <returns>The loaded SpriteFont object.</returns>
		public static SpriteFont LoadSpriteFont(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName) => assetManager.UseLoader(_spriteFontLoader, assetName, tag: graphicsDevice);
	}
}

#endif