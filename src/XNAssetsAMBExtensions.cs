﻿using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using XNAssets.Utility;

namespace AssetManagementBase
{
	public static class XNAssetsAMBExtensions
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

		private static AssetLoader<SoundEffect> _soundEffectLoader = (assetManager, assetName, settings) =>
		{
			using (var stream = assetManager.OpenAsset(assetName))
			{
				return SoundEffect.FromStream(stream);
			}
		};

		private static AssetLoader<Texture2D> _textureLoader = (assetManager, assetName, settings) =>
		{
			var textureLoadingSettings = (Texture2DLoadingSettings)settings;
			using (var stream = assetManager.OpenAsset(assetName))
			{
				return Texture2DExtensions.FromStream(textureLoadingSettings.GraphicsDevice, stream, textureLoadingSettings.PremultiplyAlpha);
			}
		};

		public static SoundEffect LoadSoundEffect(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_soundEffectLoader, assetName);
		public static Texture2D LoadTexture2D(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName, bool premultiplyAlpha = false)
		{
			return assetManager.UseLoader(_textureLoader, assetName, new Texture2DLoadingSettings(graphicsDevice)
			{
				PremultiplyAlpha = premultiplyAlpha
			});
		}
	}
}