using SpriteFontPlus;

#if !STRIDE
using Microsoft.Xna.Framework.Graphics;
#else
using Stride.Graphics;
using Texture2D = Stride.Graphics.Texture;
#endif

namespace XNAssets
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