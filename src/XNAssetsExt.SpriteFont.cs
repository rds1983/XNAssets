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
		private static readonly AssetLoader<SpriteFont> _spriteFontLoader = context =>
		{
			var data = context.ReadDataAsString();
			var graphicsDevice = (GraphicsDevice)context.Settings;

			return BMFontLoader.Load(data, name => context.Manager.LoadByteArray(name, false), graphicsDevice);
		};

		public static SpriteFont LoadSpriteFont(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName) => assetManager.UseLoader(_spriteFontLoader, assetName, graphicsDevice);
	}
}
