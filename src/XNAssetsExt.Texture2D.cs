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
		private class Texture2DLoadingSettings : IAssetSettings
		{
			public bool PremultiplyAlpha { get; private set; }

			public Texture2DLoadingSettings(bool premultiplyAlapha)
			{
				PremultiplyAlpha = premultiplyAlapha;
			}

			public string BuildKey()
			{
				return PremultiplyAlpha ? "pm" : "npm";
			}
		}

		private static AssetLoader<Texture2D> _textureLoader = (manager, assetName, settings, tag) =>
		{
			var textureLoadingSettings = (Texture2DLoadingSettings)settings;
			using (var stream = manager.OpenAssetStream(assetName))
			{
				return Texture2DExtensions.FromStream((GraphicsDevice)tag, stream, textureLoadingSettings.PremultiplyAlpha);
			}
		};

		public static Texture2D LoadTexture2D(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName, bool premultiplyAlpha = false)
		{
			return assetManager.UseLoader(_textureLoader, assetName, new Texture2DLoadingSettings(premultiplyAlpha), graphicsDevice);
		}
	}
}