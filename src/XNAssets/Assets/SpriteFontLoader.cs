using SpriteFontPlus;

#if !XENKO
using Microsoft.Xna.Framework.Graphics;
#else
using Xenko.Graphics;
using Texture2D = Xenko.Graphics.Texture;
#endif

namespace XNAssets.Assets
{
	internal class SpriteFontLoader : IAssetLoader<SpriteFont>
	{
		public SpriteFont Load(AssetLoaderContext context, string assetName)
		{
			var fontData = context.Load<string>(assetName);

			return BMFontLoader.Load(fontData, name => TextureGetter(context, name));
		}

		private TextureWithOffset TextureGetter(AssetLoaderContext context, string name)
		{
			var texture = context.Load<Texture2D>(name);
			return new TextureWithOffset(texture);
		}
	}
}