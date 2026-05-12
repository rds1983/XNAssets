using FontStashSharp;
using System.IO;

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
	/// Provides extension methods for loading FontStashSharp font assets.
	/// </summary>
	public static class XNAssetsExtFontStashSharp
	{
		/// <summary>
		/// Contains settings for FontSystem loading.
		/// </summary>
		private class FontSystemLoadingSettings : IAssetSettings
		{
			/// <summary>
			/// Gets or sets an existing texture to use for the font atlas.
			/// </summary>
			public Texture2D ExistingTexture { get; set; }
			/// <summary>
			/// Gets or sets the used space in the existing texture.
			/// </summary>
			public Rectangle ExistingTextureUsedSpace { get; set; }
			/// <summary>
			/// Gets or sets additional font files to load.
			/// </summary>
			public string[] AdditionalFonts { get; set; }

			/// <summary>
			/// Builds the cache key for this settings configuration.
			/// </summary>
			/// <returns>An empty string as cache key.</returns>
			public string BuildKey() => string.Empty;
		}

		private static AssetLoader<FontSystem> _fontSystemLoader = (manager, assetName, settings, tag) =>
		{
			var fontSystemSettings = new FontSystemSettings();

			var fontSystemLoadingSettings = (FontSystemLoadingSettings)settings;
			if (fontSystemLoadingSettings != null)
			{
				fontSystemSettings.ExistingTexture = fontSystemLoadingSettings.ExistingTexture;
				fontSystemSettings.ExistingTextureUsedSpace = fontSystemLoadingSettings.ExistingTextureUsedSpace;
			}
			;

			var fontSystem = new FontSystem(fontSystemSettings);
			var data = manager.ReadAsByteArray(assetName);
			fontSystem.AddFont(data);
			if (fontSystemLoadingSettings != null && fontSystemLoadingSettings.AdditionalFonts != null)
			{
				foreach (var file in fontSystemLoadingSettings.AdditionalFonts)
				{
					data = manager.ReadAsByteArray(file);
					fontSystem.AddFont(data);
				}
			}

			return fontSystem;
		};

		private static AssetLoader<StaticSpriteFont> _staticFontLoader = (manager, assetName, settings, tag) =>
		{
			var fontData = manager.ReadAsString(assetName);
			var graphicsDevice = (GraphicsDevice)tag;

			return StaticSpriteFont.FromBMFont(fontData,
						name =>
						{
							var imageData = manager.ReadAsByteArray(name);
							return new MemoryStream(imageData);
						},
						graphicsDevice);
		};

		/// <summary>
		/// Loads a FontSystem asset for dynamic font rendering.
		/// </summary>
		/// <param name="assetManager">The AssetManager instance.</param>
		/// <param name="assetName">The name or path of the primary font file.</param>
		/// <param name="additionalFonts">Optional array of additional font files to load into the system.</param>
		/// <param name="existingTexture">Optional existing texture to use for the font atlas.</param>
		/// <param name="existingTextureUsedSpace">The region in the existing texture that is already in use, if provided.</param>
		/// <returns>The loaded FontSystem object.</returns>
		public static FontSystem LoadFontSystem(this AssetManager assetManager, string assetName, string[] additionalFonts = null, Texture2D existingTexture = null, Rectangle existingTextureUsedSpace = default(Rectangle))
		{
			FontSystemLoadingSettings settings = null;
			if (additionalFonts != null || existingTexture != null)
			{
				settings = new FontSystemLoadingSettings
				{
					AdditionalFonts = additionalFonts,
					ExistingTexture = existingTexture,
					ExistingTextureUsedSpace = existingTextureUsedSpace
				};
			}

			return assetManager.UseLoader(_fontSystemLoader, assetName, settings);
		}

		/// <summary>
		/// Loads a StaticSpriteFont asset from a BMFont definition for static font rendering.
		/// </summary>
		/// <param name="assetManager">The AssetManager instance.</param>
		/// <param name="graphicsDevice">The GraphicsDevice to create the font with.</param>
		/// <param name="assetName">The name or path of the font definition asset (BMFont XML or text format).</param>
		/// <returns>The loaded StaticSpriteFont object.</returns>
		public static StaticSpriteFont LoadStaticSpriteFont(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName)
		{
			return assetManager.UseLoader(_staticFontLoader, assetName, tag: graphicsDevice);
		}
	}
}
