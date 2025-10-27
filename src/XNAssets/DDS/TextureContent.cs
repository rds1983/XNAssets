using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace XNAssets.DDS
{
	internal enum TextureDimensions
	{
		Texture2D,
		TextureCube
	}

	internal class TextureContent
	{
		public TextureDimensions Dimensions { get; }
		public SurfaceFormat Format { get; }
		public int Width { get; }
		public int Height { get; }
		public List<List<BitmapContent>> Faces { get; } = new List<List<BitmapContent>>();

		public TextureContent(TextureDimensions dimensions, SurfaceFormat format, int width, int height)
		{
			Dimensions = dimensions;
			Format = format;
			Width = width;
			Height = height;
		}
	}
}
