using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;

namespace XNAssets.Samples.DdsViewer
{
	internal static class Utility
	{
		public static Texture2D FaceToTexture(this TextureCube texture, CubeMapFace face)
		{
			Texture2D result = null;

			switch(texture.Format)
			{
				case SurfaceFormat.Color:
					{
						var data = new Color[texture.Size * texture.Size];
						texture.GetData(face, data);

						result = new Texture2D(MyraEnvironment.GraphicsDevice, texture.Size, texture.Size);
						result.SetData(data);
					}
					break;
				case SurfaceFormat.Dxt1:
					{
						var data = new byte[texture.Size * texture.Size / 2];
						texture.GetData(face, data);

						result = new Texture2D(MyraEnvironment.GraphicsDevice, texture.Size, texture.Size, false, format: SurfaceFormat.Dxt1);
						result.SetData(data);
					}
					break;
				default:
					throw new Exception($"Format {texture.Format} isn't supported.");
			}
			

			return result;
		}
	}
}
