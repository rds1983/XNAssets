using AssetManagementBase;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace XNAssets.Tests
{
	[TestFixture]
	public class XNAssetsTests
	{
		private static readonly Assembly _assembly = typeof(XNAssetsTests).Assembly;

		private static AssetManager CreateResourceAssetManager() => AssetManager.CreateResourceAssetManager(_assembly, "Resources");

		private static void TestLoadImage(AssetManager assetManager)
		{
			string imageName = "LogoOnly_64px.png";

			var texturePremultiplied = assetManager.LoadTexture2D(TestsEnvironment.GraphicsDevice, imageName, true);
			var textureNonPremultiplied = assetManager.LoadTexture2D(TestsEnvironment.GraphicsDevice, imageName, false);
			Assert.AreEqual(texturePremultiplied.Width, 64);
			Assert.AreEqual(texturePremultiplied.Height, 64);

			Assert.AreEqual(textureNonPremultiplied.Width, 64);
			Assert.AreEqual(textureNonPremultiplied.Height, 64);

			// Test that alpha premultiplication worked
			var dataPremultiplied = new Color[texturePremultiplied.Width * texturePremultiplied.Height];
			texturePremultiplied.GetData(dataPremultiplied);

			var dataNonPremultiplied = new Color[texturePremultiplied.Width * texturePremultiplied.Height];
			textureNonPremultiplied.GetData(dataNonPremultiplied);

			for (var i = 0; i < dataPremultiplied.Length; i++)
			{
				var c1 = dataPremultiplied[i];
				var c2 = dataNonPremultiplied[i];

				var falpha = c1.A / 255.0f;

				Assert.AreEqual((byte)(c2.R * falpha), c1.R);
				Assert.AreEqual((byte)(c2.G * falpha), c1.G);
				Assert.AreEqual((byte)(c2.B * falpha), c1.B);
			}

			// Cache should have 2 records
			Assert.AreEqual(assetManager.Cache.Count, 2);

			// And it should keep the size on subsequent fetches
			texturePremultiplied = assetManager.LoadTexture2D(TestsEnvironment.GraphicsDevice, imageName, true);
			textureNonPremultiplied = assetManager.LoadTexture2D(TestsEnvironment.GraphicsDevice, imageName, false);
			Assert.AreEqual(assetManager.Cache.Count, 2);
		}

		[Test]
		public void TestLoadImageResource()
		{
			var assetManager = CreateResourceAssetManager();
			TestLoadImage(assetManager);
		}

		[Test]
		public void TestLoadImageFile()
		{
			var assetManager = AssetManager.CreateFileAssetManager(Path.Combine(Utility.ExecutingAssemblyDirectory, "Resources"));
			TestLoadImage(assetManager);
		}

		[Test]
		public void TestLoadEffects()
		{
			var assetManager = CreateResourceAssetManager();

			var effect = assetManager.LoadEffect(TestsEnvironment.GraphicsDevice, "DefaultEffect.efb");
			Assert.AreEqual(effect.Parameters.Count, 5);

			effect = assetManager.LoadEffect(TestsEnvironment.GraphicsDevice, "DefaultEffect.efb", new Dictionary<string, string>()
			{
				["BONES"] = "4",
				["LIGHTNING"] = "1"
			});
			Assert.AreEqual(effect.Parameters.Count, 7);
		}

		[Test]
		public void TestLoadSpriteFont()
		{
			var assetManager = CreateResourceAssetManager();
			var font = assetManager.LoadSpriteFont(TestsEnvironment.GraphicsDevice, "arial64.fnt");
			Assert.AreEqual(font.Characters.Count, 191);
		}

		[Test]
		public void TestColorKey()
		{
			var assetManager = CreateResourceAssetManager();

			string imageName = "HUD_Aiming.png";

			// Call LoadTexture2D twice for every parameter to make sure caching works correctly
			var textureWithoutColorKey = assetManager.LoadTexture2D(TestsEnvironment.GraphicsDevice, imageName, false, null);
			textureWithoutColorKey = assetManager.LoadTexture2D(TestsEnvironment.GraphicsDevice, imageName, false, null);
			var textureWithColorKey = assetManager.LoadTexture2D(TestsEnvironment.GraphicsDevice, imageName, false, Color.Magenta);
			textureWithColorKey = assetManager.LoadTexture2D(TestsEnvironment.GraphicsDevice, imageName, false, Color.Magenta);

			Assert.AreEqual(2, assetManager.Cache.Count);

			var colorData = new Color[textureWithoutColorKey.Width * textureWithoutColorKey.Height];
			textureWithoutColorKey.GetData(colorData);

			// Should have magenta pixels
			Assert.AreEqual(Color.Magenta, colorData[0]);

			// Should have transparent pixels
			textureWithColorKey.GetData(colorData);
			Assert.AreEqual(new Color(0, 0, 0, 0), colorData[0]);
		}
	}
}
