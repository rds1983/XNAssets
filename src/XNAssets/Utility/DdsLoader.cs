#if !STRIDE

using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using XNAssets.DDS;

namespace XNAssets.Utility
{
	internal static class DdsLoader
	{
		public static Texture FromStream(GraphicsDevice device, Stream stream)
		{
			TextureContent info;
			using(var reader = new BinaryReader(stream))
			{
				info = DdsParser.Import(reader);
			}

			// Load data
			Texture result;

			switch (info.Dimensions)
			{
				case TextureDimensions.Texture2D:
					{
						// Only one face
						var face = info.Faces[0];
						Texture2D texture2D = new Texture2D(device, info.Width, info.Height, face.Count > 0, info.Format);
						for (var i = 0; i < face.Count; ++i)
						{
							texture2D.SetData(i, null, face[i].Data, 0, face[i].Data.Length);
						}

						result = texture2D;
					}
					break;
				case TextureDimensions.TextureCube:
					{
						var face = info.Faces[0];
						var textureCube = new TextureCube(device, info.Width, face.Count > 1, info.Format);
						for (var i = 0; i < face.Count; ++i)
						{
							var imageData = info.Faces[0][i].Data;
							textureCube.SetData(CubeMapFace.PositiveX, i, null, imageData, 0, imageData.Length);

							imageData = info.Faces[1][i].Data;
							textureCube.SetData(CubeMapFace.NegativeX, i, null, imageData, 0, imageData.Length);

							imageData = info.Faces[2][i].Data;
							textureCube.SetData(CubeMapFace.PositiveY, i, null, imageData, 0, imageData.Length);

							imageData = info.Faces[3][i].Data;
							textureCube.SetData(CubeMapFace.NegativeY, i, null, imageData, 0, imageData.Length);

							imageData = info.Faces[4][i].Data;
							textureCube.SetData(CubeMapFace.PositiveZ, i, null, imageData, 0, imageData.Length);

							imageData = info.Faces[5][i].Data;
							textureCube.SetData(CubeMapFace.NegativeZ, i, null, imageData, 0, imageData.Length);
						}

						result = textureCube;
					}
					break;

				default:
					throw new Exception($"Unknown dimensions {info.Dimensions}");
			}

			return result;
		}
	}
}

#endif