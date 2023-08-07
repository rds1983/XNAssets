using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace XNAssets.Tests
{
	[SetUpFixture]
	public class TestsEnvironment
	{
		private static TestGame _game;

		public static GraphicsDevice GraphicsDevice => _game.GraphicsDevice;

		[OneTimeSetUp]
		public void SetUp()
		{
			_game = new TestGame();
		}
	}
}
