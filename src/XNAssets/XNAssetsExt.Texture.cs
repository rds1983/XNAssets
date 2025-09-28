using System;
using XNAssets.Utility;
using XNAssets;
using System.Text;

#if !STRIDE
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#else
using Stride.Core.Mathematics;
using Stride.Graphics;
using Texture2D = Stride.Graphics.Texture;
#endif

namespace AssetManagementBase
{
	public static partial class XNAssetsExt
	{
		private class TextureLoadingSettings : IAssetSettings
		{
			public static readonly Color DefaultColorKey = Color.Magenta;

			public static readonly TextureLoadingSettings NoPremultiplyNoColorKey = new TextureLoadingSettings(false, null);
			public static readonly TextureLoadingSettings PremultiplyNoColorKey = new TextureLoadingSettings(true, null);
			public static readonly TextureLoadingSettings NoPremultiplyDefaultColorKey = new TextureLoadingSettings(false, DefaultColorKey);
			public static readonly TextureLoadingSettings PremultiplyDefaultColorKey = new TextureLoadingSettings(true, DefaultColorKey);

			public bool PremultiplyAlpha { get; }
			public Color? ColorKey { get; }
			public string CacheKey { get; }

			public TextureLoadingSettings(bool premultiplyAlpha, Color? colorKey)
			{
				PremultiplyAlpha = premultiplyAlpha;
				ColorKey = colorKey;

				var sb = new StringBuilder();
				sb.Append(premultiplyAlpha);
				if (colorKey != null)
				{
					sb.Append(",");
					sb.Append(colorKey.Value.ToHexString());
				}

				CacheKey = sb.ToString();
			}

			public string BuildKey() => CacheKey;
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

			var textureLoadingSettings = (TextureLoadingSettings)settings;
			if (textureLoadingSettings == null)
			{
				textureLoadingSettings = TextureLoadingSettings.NoPremultiplyNoColorKey;
			}

			using (var stream = manager.Open(assetName))
			{
				return Texture2DExtensions.FromStream((GraphicsDevice)tag, stream, textureLoadingSettings.PremultiplyAlpha, textureLoadingSettings.ColorKey);
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
			return assetManager.UseLoader(_textureLoader, assetName, TextureLoadingSettings.NoPremultiplyNoColorKey, graphicsDevice);
		}
#endif

		public static Texture2D LoadTexture2D(this AssetManager assetManager, GraphicsDevice graphicsDevice,
			string assetName, bool premultiplyAlpha = false, Color? colorKey = null)
		{
			TextureLoadingSettings settings;
			if (!premultiplyAlpha && colorKey == null)
			{
				settings = TextureLoadingSettings.NoPremultiplyNoColorKey;
			}
			else if (premultiplyAlpha && colorKey == null)
			{
				settings = TextureLoadingSettings.PremultiplyNoColorKey;
			}
			else if (!premultiplyAlpha && colorKey.Value == TextureLoadingSettings.DefaultColorKey)
			{
				settings = TextureLoadingSettings.NoPremultiplyDefaultColorKey;
			}
			else if (premultiplyAlpha && colorKey.Value == TextureLoadingSettings.DefaultColorKey)
			{
				settings = TextureLoadingSettings.PremultiplyDefaultColorKey;
			}
			else
			{
				settings = new TextureLoadingSettings(premultiplyAlpha, colorKey);
			}

			return (Texture2D)assetManager.UseLoader(_textureLoader, assetName, settings, graphicsDevice);
		}
	}
}