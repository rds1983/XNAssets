using AssetManagementBase;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace XNAssets.Tests
{
	[TestFixture]
	public class DDSTests
	{
		[TestCase("checker.dds", SurfaceFormat.Dxt1, 128, 128, 8)]
		[TestCase("dirt.dds", SurfaceFormat.Color, 512, 512, 10)]
		[TestCase("grass.dds", SurfaceFormat.Color, 1024, 1024, 11)]
		[TestCase("rock.dds", SurfaceFormat.Color, 1024, 1024, 11)]
		public void LoadTexture2DFromDDS(string filename, SurfaceFormat format, int width, int height, int levels)
		{
			var assetManager = Utility.CreateAssetManager();

			var result = assetManager.LoadTexture2D(TestsEnvironment.GraphicsDevice, filename);

			Assert.AreEqual(format, result.Format);
			Assert.AreEqual(width, result.Width);
			Assert.AreEqual(height, result.Height);
			Assert.AreEqual(levels, result.LevelCount);
		}

		[TestCase("MilkyWay.dds", SurfaceFormat.Dxt1, 128, 8)]
		public void LoadTextureCubeFromDDS(string filename, SurfaceFormat format, int size, int levels)
		{
			var assetManager = Utility.CreateAssetManager();

			var result = assetManager.LoadTextureCube(TestsEnvironment.GraphicsDevice, filename);

			Assert.AreEqual(format, result.Format);
			Assert.AreEqual(size, result.Size);
			Assert.AreEqual(levels, result.LevelCount);
		}
	}
}
