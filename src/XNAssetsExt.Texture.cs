using System;
using XNAssets.Utility;
using XNAssets;

#if !STRIDE
using Microsoft.Xna.Framework.Graphics;
using DdsKtxXna;
#else
using Stride.Graphics;
using Texture2D = Stride.Graphics.Texture;
#endif

namespace AssetManagementBase
{
	public static partial class XNAssetsExt
	{
		private class TextureLoadingSettings : IAssetSettings
		{
			public static readonly TextureLoadingSettings Default = new TextureLoadingSettings(false);
			public static readonly TextureLoadingSettings PremultipliedAlpha = new TextureLoadingSettings(true);

			public bool PremultiplyAlpha { get; }

			public TextureLoadingSettings(bool premultiplyAlpha)
			{
				PremultiplyAlpha = premultiplyAlpha;
			}

			public string BuildKey() => PremultiplyAlpha.ToString();
		}

		private static AssetLoader<Texture> _textureLoader = (manager, assetName, settings, tag) =>
		{
#if !STRIDE
			if (assetName.ToLower().EndsWith(".dds"))
			{
				// TODO: Apply loading settings
				using (var stream = manager.Open(assetName))
				{
					return DdsKtxLoader.FromStream((GraphicsDevice)tag, stream);
				}
			}
#endif

			var premultiplyAlpha = false;
			var textureLoadingSettings = (TextureLoadingSettings)settings;
			if (textureLoadingSettings != null)
			{
				premultiplyAlpha = textureLoadingSettings.PremultiplyAlpha;
			}

			using (var stream = manager.Open(assetName))
			{
				return Texture2DExtensions.FromStream((GraphicsDevice)tag, stream, premultiplyAlpha);
			}
		};

#if !STRIDE
		private static AssetLoader<TextureCube> _textureCubeLoader = (manager, assetName, settings, tag) =>
		{
			using (var stream = manager.Open(assetName))
			{
				return (TextureCube)DdsKtxLoader.FromStream((GraphicsDevice)tag, stream);
			}
		};

		public static TextureCube LoadTextureCube(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName)
		{
			return assetManager.UseLoader(_textureCubeLoader, assetName, tag: graphicsDevice);
		}

		public static Texture LoadTexture(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName)
		{
			return assetManager.UseLoader(_textureLoader, assetName, TextureLoadingSettings.Default, graphicsDevice);
		}
#endif

		public static Texture2D LoadTexture2D(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName, bool premultiplyAlpha = false)
		{
			return (Texture2D)assetManager.UseLoader(_textureLoader, assetName,
				premultiplyAlpha ? TextureLoadingSettings.PremultipliedAlpha : TextureLoadingSettings.Default,
				graphicsDevice);
		}
	}
}