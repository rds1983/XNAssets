namespace XNAssets
{
	internal class ByteArrayLoader : IAssetLoader<byte[]>
	{
		public byte[] Load(AssetLoaderContext context, string assetName)
		{
			return context.ReadAsByteArray(assetName);
		}
	}
}
