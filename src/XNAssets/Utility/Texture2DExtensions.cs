﻿using StbImageSharp;
using System.IO;

#if !STRIDE
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#else
using Stride.Core.Mathematics;
using Stride.Graphics;
using Texture2D = Stride.Graphics.Texture;
#endif

namespace XNAssets.Utility
{
	internal static class Texture2DExtensions
	{
		/// <summary>
		/// Creates a Texture2D from Stream and optionally premultiplies alpha
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="stream"></param>
		/// <param name="premultiplyAlpha"></param>
		/// <returns></returns>
		public static unsafe Texture2D FromStream(GraphicsDevice graphicsDevice, Stream stream, bool premultiplyAlpha, Color? colorKey)
		{
			byte[] bytes;

			// Rewind stream if it is at end
			if (stream.CanSeek && stream.Length == stream.Position)
			{
				stream.Seek(0, SeekOrigin.Begin);
			}

			// Copy it's data to memory
			// As some platforms dont provide full stream functionality and thus streams can't be read as it is
			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				bytes = ms.ToArray();
			}

			// The data returned is always four channel BGRA
			var result = ImageResult.FromMemory(bytes, ColorComponents.RedGreenBlueAlpha);

			if (premultiplyAlpha || colorKey != null)
			{
				bool colorKeyEnabled = false;
				var colorKey2 = Color.White;

				if (colorKey != null)
				{
					colorKeyEnabled = true;
					colorKey2 = colorKey.Value;
				}

				fixed (byte* b = &result.Data[0])
				{
					byte* ptr = b;
					for (var i = 0; i < result.Data.Length; i += 4, ptr += 4)
					{
						if (colorKeyEnabled &&
							ptr[0] == colorKey2.R &&
							ptr[1] == colorKey2.G &&
							ptr[2] == colorKey2.B &&
							ptr[3] == colorKey2.A)
						{
							ptr[0] = ptr[1] = ptr[2] = ptr[3] = 0;
						}

						if (premultiplyAlpha)
						{
							var falpha = ptr[3] / 255.0f;
							ptr[0] = (byte)(ptr[0] * falpha);
							ptr[1] = (byte)(ptr[1] * falpha);
							ptr[2] = (byte)(ptr[2] * falpha);
						}
					}
				}
			}

			return CrossEngineStuff.CreateTexture2D(graphicsDevice, result.Width, result.Height, result.Data);
		}

		public static string ToHexString(this Color c)
		{
			return string.Format("#{0}{1}{2}{3}",
				c.R.ToString("X2"),
				c.G.ToString("X2"),
				c.B.ToString("X2"),
				c.A.ToString("X2"));
		}

#if !STRIDE
		public static byte[] ToByteArray(this Stream stream)
		{
			byte[] bytes;

			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				bytes = ms.ToArray();
			}

			return bytes;
		}
#endif
	}
}
