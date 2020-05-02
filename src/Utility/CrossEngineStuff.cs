#if !STRIDE
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#else
using Stride.Graphics;
using Texture2D = Stride.Graphics.Texture;
#endif

namespace XNAssets.Utility
{
	internal static class CrossEngineStuff
	{
		public static Texture2D CreateTexture2D(GraphicsDevice graphicsDevice, int width, int height, byte[] data)
		{
#if !STRIDE
			var result = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
			result.SetData(data);
			return result;
#else
			return Texture2D.New2D(graphicsDevice, width, height, PixelFormat.R8G8B8A8_UNorm_SRgb, data);
#endif
		}
	}
}
