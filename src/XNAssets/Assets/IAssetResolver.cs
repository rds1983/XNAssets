using System.IO;

namespace XNAssets.Assets
{
	public interface IAssetResolver
	{
		Stream Open(string assetName);
	}
}
