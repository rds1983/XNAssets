#if !STRIDE

using Microsoft.Xna.Framework.Audio;

namespace AssetManagementBase
{
	/// <summary>
	/// Provides extension methods for loading audio assets.
	/// </summary>
	partial class XNAssetsExt
	{
		private static AssetLoader<SoundEffect> _soundEffectLoader = (manager, assetName, settings, tag) =>
		{
			using (var stream = manager.Open(assetName))
			{
				return SoundEffect.FromStream(stream);
			}
		};

		/// <summary>
		/// Loads a SoundEffect asset from the asset manager.
		/// </summary>
		/// <param name="assetManager">The AssetManager instance.</param>
		/// <param name="assetName">The name or path of the sound effect asset.</param>
		/// <returns>The loaded SoundEffect object.</returns>
		public static SoundEffect LoadSoundEffect(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_soundEffectLoader, assetName);
	}
}

#endif