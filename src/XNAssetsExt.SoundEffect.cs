using Microsoft.Xna.Framework.Audio;

namespace AssetManagementBase
{
	partial class XNAssetsExt
	{
		private static AssetLoader<SoundEffect> _soundEffectLoader = (context) =>
		{
			using (var stream = context.DataStreamOpener())
			{
				return SoundEffect.FromStream(stream);
			}
		};

		public static SoundEffect LoadSoundEffect(this AssetManager assetManager, string assetName) => assetManager.UseLoader(_soundEffectLoader, assetName);
	}
}
