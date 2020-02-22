using System.IO;

namespace XNAssets
{
	public interface IAssetResolver
	{
		Stream Open(string assetName);
	}
}
