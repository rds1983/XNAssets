#if !STRIDE

using XNAssets.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace AssetManagementBase
{
	partial class XNAssetsExt
	{
		private static readonly AssetLoader<SpriteFont> _spriteFontLoader = (manager, assetName, settings, tag) =>
		{
			var data = manager.ReadAsString(assetName);
			var graphicsDevice = (GraphicsDevice)tag;

			return BMFontLoader.Load(data, name => manager.ReadAsByteArray(name), graphicsDevice);
		};

		public static SpriteFont LoadSpriteFont(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName) => assetManager.UseLoader(_spriteFontLoader, assetName, tag: graphicsDevice);
	}
}

#endif