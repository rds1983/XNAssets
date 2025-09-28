#if !STRIDE

using Microsoft.Xna.Framework.Audio;

namespace AssetManagementBase
{
	partial class XNAssetsExt
	{
		private static AssetLoader<SoundEffect> _soundEffectLoader = (manager, assetName, settings, tag) =>
		{
			using (var stream = manager.Open(assetName))
			{
				return SoundEffect.FromStream(stream);
			}
		};

		public static SoundEffect LoadSoundEffect(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_soundEffectLoader, assetName);
	}
}

#endif