using DdsKtxXna;
using Microsoft.Xna.Framework.Graphics;

namespace AssetManagementBase
{
	partial class XNAssetsExt
	{
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
	}
}
