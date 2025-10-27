using Microsoft.Xna.Framework.Graphics;

namespace XNAssets.DDS
{
	internal class BitmapContent
	{
		public int Width { get; }
		public int Height { get; }
		public SurfaceFormat Format { get; }
		public byte[] Data { get; }

		public BitmapContent(int width, int height, SurfaceFormat format, byte[] data)
		{
			Width = width;
			Height = height;
			Format = format;
			Data = data;
		}
	}
}
