using System;
using XNAssets.Utility;

#if !STRIDE
using Microsoft.Xna.Framework.Graphics;
#else
using Stride.Graphics;
using Texture2D = Stride.Graphics.Texture;
#endif

namespace AssetManagementBase
{
	public static partial class XNAssetsExt
	{
		private class Texture2DLoadingSettings
		{
			public GraphicsDevice GraphicsDevice { get; }

			public bool PremultiplyAlpha { get; set; }

			public Texture2DLoadingSettings(GraphicsDevice graphicsDevice)
			{
				GraphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
			}

			public override string ToString()
			{
				return PremultiplyAlpha ? "pm" : "npm";
			}
		}

		private static AssetLoader<Texture2D> _textureLoader = (context) =>
		{
			var textureLoadingSettings = (Texture2DLoadingSettings)context.Settings;
			using (var stream = context.DataStreamOpener())
			{
				return Texture2DExtensions.FromStream(textureLoadingSettings.GraphicsDevice, stream, textureLoadingSettings.PremultiplyAlpha);
			}
		};

		public static Texture2D LoadTexture2D(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName, bool premultiplyAlpha = false)
		{
			return assetManager.UseLoader(_textureLoader, assetName, new Texture2DLoadingSettings(graphicsDevice)
			{
				PremultiplyAlpha = premultiplyAlpha
			});
		}
	}
}