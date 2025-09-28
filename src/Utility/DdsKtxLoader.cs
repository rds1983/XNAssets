#if !STRIDE

using DdsKtxSharp;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using static DdsKtxSharp.DdsKtx;

namespace XNAssets.Utility
{
	internal static class DdsKtxLoader
	{
		private static byte[] LoadFace(DdsKtxParser parser, int faceIndex, int levelIndex)
		{
			ddsktx_texture_info info = parser.Info;

			ddsktx_sub_data sub_data;
			var imageData = parser.GetSubData(0, faceIndex, levelIndex, out sub_data);

			switch (info.format)
			{
				case ddsktx_format.DDSKTX_FORMAT_BGRA8:
					// Switch B and R
					for (var i = 0; i < imageData.Length / 4; ++i)
					{
						var temp = imageData[i * 4];
						imageData[i * 4] = imageData[i * 4 + 2];
						imageData[i * 4 + 2] = temp;
						imageData[i * 4 + 3] = 255;
					}

					break;

				case ddsktx_format.DDSKTX_FORMAT_RGB8:
					// Add alpha channel
					var newImageData = new byte[sub_data.width * sub_data.height * 4];
					for (var i = 0; i < newImageData.Length / 4; ++i)
					{
						newImageData[i * 4] = imageData[i * 3 + 2];
						newImageData[i * 4 + 1] = imageData[i * 3 + 1];
						newImageData[i * 4 + 2] = imageData[i * 3];
						newImageData[i * 4 + 3] = 255;
					}

					imageData = newImageData;
					break;
			}

			return imageData;
		}

		public static Texture FromStream(GraphicsDevice device, Stream stream)
		{
			DdsKtxParser parser = DdsKtxParser.FromMemory(stream.ToByteArray());
			ddsktx_texture_info info = parser.Info;

			var format = SurfaceFormat.Color;
			switch (info.format)
			{
				case ddsktx_format.DDSKTX_FORMAT_BC1:
					format = SurfaceFormat.Dxt1;
					break;

				case ddsktx_format.DDSKTX_FORMAT_BC2:
					format = SurfaceFormat.Dxt3;
					break;

				case ddsktx_format.DDSKTX_FORMAT_BC3:
					format = SurfaceFormat.Dxt5;
					break;

				case ddsktx_format.DDSKTX_FORMAT_A8:
					format = SurfaceFormat.Alpha8;
					break;

				case ddsktx_format.DDSKTX_FORMAT_RGBA8:
					break;

				case ddsktx_format.DDSKTX_FORMAT_BGRA8:
					break;

				case ddsktx_format.DDSKTX_FORMAT_RGB8:
					break;

				default:
					throw new Exception($"Format {info.format} is not supported.");
			}

			// Load data
			var isCubeMap = (info.flags & ddsktx_texture_flags.DDSKTX_TEXTURE_FLAG_CUBEMAP) != 0;

			Texture result;

			if (!isCubeMap)
			{
				Texture2D texture2D = new Texture2D(device, info.width, info.height, info.num_mips > 1, format);
				for (var i = 0; i < info.num_mips; ++i)
				{
					var imageData = LoadFace(parser, 0, i);
					texture2D.SetData(i, null, imageData, 0, imageData.Length);
				}

				result = texture2D;
			}
			else
			{
				var textureCube = new TextureCube(device, info.width, info.num_mips > 1, format);
				for (var i = 0; i < info.num_mips; ++i)
				{
					var imageData = LoadFace(parser, 0, i);
					textureCube.SetData(CubeMapFace.PositiveX, i, null, imageData, 0, imageData.Length);

					imageData = LoadFace(parser, 1, i);
					textureCube.SetData(CubeMapFace.NegativeX, i, null, imageData, 0, imageData.Length);

					imageData = LoadFace(parser, 2, i);
					textureCube.SetData(CubeMapFace.PositiveY, i, null, imageData, 0, imageData.Length);

					imageData = LoadFace(parser, 3, i);
					textureCube.SetData(CubeMapFace.NegativeY, i, null, imageData, 0, imageData.Length);

					imageData = LoadFace(parser, 4, i);
					textureCube.SetData(CubeMapFace.PositiveZ, i, null, imageData, 0, imageData.Length);

					imageData = LoadFace(parser, 5, i);
					textureCube.SetData(CubeMapFace.NegativeZ, i, null, imageData, 0, imageData.Length);
				}

				result = textureCube;
			}

			return result;
		}
	}
}

#endif