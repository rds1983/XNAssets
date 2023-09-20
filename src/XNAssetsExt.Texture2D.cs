using System;
using XNAssets.Utility;
using XNAssets;
using DdsKtxXna;

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
		private static AssetLoader<Texture> _textureLoader = (manager, assetName, settings, tag) =>
		{
			if (assetName.ToLower().EndsWith(".dds"))
			{
				// TODO: Apply loading settings
				using (var stream = manager.Open(assetName))
				{
					return DdsKtxLoader.FromStream((GraphicsDevice)tag, stream);
				}
			}

			var textureLoadingSettings = (TextureLoadingSettings)settings;

			using (var stream = manager.Open(assetName))
			{
				return Texture2DExtensions.FromStream((GraphicsDevice)tag, stream, textureLoadingSettings.PremultiplyAlpha);
			}
		};

		public static Texture2D LoadTexture2D(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName, bool premultiplyAlpha = false)
		{
			return (Texture2D)assetManager.UseLoader(_textureLoader, assetName,
				premultiplyAlpha ? TextureLoadingSettings.DefaultPremultiplyAlpha : TextureLoadingSettings.Default,
				graphicsDevice);
		}

		public static Texture2D LoadTexture2D(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName, TextureLoadingSettings settings)
		{
			if (settings == null)
			{
				settings = TextureLoadingSettings.Default;
			}

			return (Texture2D)assetManager.UseLoader(_textureLoader, assetName, settings, graphicsDevice);
		}

		public static Texture LoadTexture(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName, TextureLoadingSettings settings)
		{
			return assetManager.UseLoader(_textureLoader, assetName, settings, graphicsDevice);
		}
	}
}