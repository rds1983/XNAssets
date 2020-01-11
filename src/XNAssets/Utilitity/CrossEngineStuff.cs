#if !XENKO
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#else
using Xenko.Graphics;
using Texture2D = Xenko.Graphics.Texture;
#endif

namespace XNAssets.Utility
{
	internal static class CrossEngineStuff
	{
		public static Texture2D CreateTexture2D(GraphicsDevice graphicsDevice, int width, int height, byte[] data)
		{
#if !XENKO
			var result = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
			result.SetData(data);
			return result;
#else
			return Texture2D.New2D(graphicsDevice, width, height, PixelFormat.R8G8B8A8_UNorm, data);
#endif
		}
	}
}
