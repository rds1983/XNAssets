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
	/// <summary>
	/// Provides extension methods for loading texture assets.
	/// </summary>
	public static partial class XNAssetsExt
	{
		/// <summary>
		/// Contains settings for texture loading including alpha premultiplication and color key options.
		/// </summary>
		private class TextureLoadingSettings : IAssetSettings
		{
			/// <summary>
			/// The default color key (Magenta).
			/// </summary>
			public static readonly Color DefaultColorKey = Color.Magenta;

			/// <summary>
			/// Preset settings for no premultiplication and no color key.
			/// </summary>
			public static readonly TextureLoadingSettings NoPremultiplyNoColorKey = new TextureLoadingSettings(false, null);
			/// <summary>
			/// Preset settings for premultiplication and no color key.
			/// </summary>
			public static readonly TextureLoadingSettings PremultiplyNoColorKey = new TextureLoadingSettings(true, null);
			/// <summary>
			/// Preset settings for no premultiplication with default color key.
			/// </summary>
			public static readonly TextureLoadingSettings NoPremultiplyDefaultColorKey = new TextureLoadingSettings(false, DefaultColorKey);
			/// <summary>
			/// Preset settings for premultiplication with default color key.
			/// </summary>
			public static readonly TextureLoadingSettings PremultiplyDefaultColorKey = new TextureLoadingSettings(true, DefaultColorKey);

			/// <summary>
			/// Gets whether alpha should be premultiplied.
			/// </summary>
			public bool PremultiplyAlpha { get; }
			/// <summary>
			/// Gets the color key for transparency, or null if no color key is used.
			/// </summary>
			public Color? ColorKey { get; }
			/// <summary>
			/// Gets the cache key for this settings configuration.
			/// </summary>
			public string CacheKey { get; }

			/// <summary>
			/// Initializes a new instance of the TextureLoadingSettings class.
			/// </summary>
			/// <param name="premultiplyAlpha">Whether to premultiply the alpha channel.</param>
			/// <param name="colorKey">The color key for transparency, or null for no color key.</param>
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

			/// <summary>
			/// Builds the cache key for this settings configuration.
			/// </summary>
			/// <returns>The cache key string.</returns>
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
					return DdsLoader.FromStream((GraphicsDevice)tag, stream);
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
				return (TextureCube)DdsLoader.FromStream((GraphicsDevice)tag, stream);
			}
		};

		/// <summary>
		/// Loads a cube texture from a DDS file.
		/// </summary>
		/// <param name="assetManager">The AssetManager instance.</param>
		/// <param name="graphicsDevice">The GraphicsDevice to create the texture with.</param>
		/// <param name="assetName">The name or path of the cube texture asset.</param>
		/// <returns>The loaded TextureCube object.</returns>
		public static TextureCube LoadTextureCube(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName)
		{
			return assetManager.UseLoader(_textureCubeLoader, assetName, tag: graphicsDevice);
		}

		/// <summary>
		/// Loads a texture asset.
		/// </summary>
		/// <param name="assetManager">The AssetManager instance.</param>
		/// <param name="graphicsDevice">The GraphicsDevice to create the texture with.</param>
		/// <param name="assetName">The name or path of the texture asset.</param>
		/// <returns>The loaded Texture object.</returns>
		public static Texture LoadTexture(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName)
		{
			return assetManager.UseLoader(_textureLoader, assetName, TextureLoadingSettings.NoPremultiplyNoColorKey, graphicsDevice);
		}
#endif

		/// <summary>
		/// Loads a 2D texture with optional alpha premultiplication and color key support.
		/// </summary>
		/// <param name="assetManager">The AssetManager instance.</param>
		/// <param name="graphicsDevice">The GraphicsDevice to create the texture with.</param>
		/// <param name="assetName">The name or path of the texture asset.</param>
		/// <param name="premultiplyAlpha">Whether to premultiply the alpha channel. Defaults to false.</param>
		/// <param name="colorKey">The color to treat as transparent, or null for no color key. Defaults to null.</param>
		/// <returns>The loaded Texture2D object.</returns>
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