using XNAssets.Utility;

#if !STRIDE
using Microsoft.Xna.Framework.Graphics;
#else
using Stride.Graphics;
#endif

namespace AssetManagementBase
{
	partial class XNAssetsExt
	{
		private static readonly AssetLoader<SpriteFont> _spriteFontLoader = (manager, assetName, settings) =>
		{
			var data = manager.ReadAssetAsString(assetName);
			var graphicsDevice = (GraphicsDevice)settings;

			return BMFontLoader.Load(data, name => manager.ReadAssetAsByteArray(name), graphicsDevice);
		};

		public static SpriteFont LoadSpriteFont(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName) => assetManager.UseLoader(_spriteFontLoader, assetName, graphicsDevice);
	}
}
