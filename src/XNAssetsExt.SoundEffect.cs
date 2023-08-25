using Microsoft.Xna.Framework.Audio;

namespace AssetManagementBase
{
	partial class XNAssetsExt
	{
		private static AssetLoader<SoundEffect> _soundEffectLoader = (manager, assetName, settings) =>
		{
			using (var stream = manager.OpenAssetStream(assetName))
			{
				return SoundEffect.FromStream(stream);
			}
		};

		public static SoundEffect LoadSoundEffect(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_soundEffectLoader, assetName);
	}
}
