// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using XNAssets.Utility;
using System.Collections.Generic;

namespace XNAssets.DDS
{
	/// <summary>
	/// Loader class for DDS format image files.
	/// </summary>
	internal class DdsParser
	{
		[Flags]
		public enum Ddsd : uint
		{
			Caps = 0x1,             // Required in every DDS file
			Height = 0x2,           // Required in every DDS file
			Width = 0x4,            // Required in every DDS file
			Pitch = 0x8,            // Required when pitch is provided for an uncompressed texture
			PixelFormat = 0x1000,   // Required in every DDS file
			MipMapCount = 0x2000,   // Required in a mipmapped texture
			LinearSize = 0x80000,   // Required when pitch is provided for a compressed texture
			Depth = 0x800000,       // Required in a depth texture
		}

		[Flags]
		public enum DdsCaps : uint
		{
			Complex = 0x8,       // Optional; must be used on any file that contains more than one surface (a mipmap, a cubic environment map, or mipmapped volume texture)
			MipMap = 0x400000,   // Optional; should be used for a mipmap
			Texture = 0x1000,    // Required
		}

		[Flags]
		public enum DdsCaps2 : uint
		{
			Cubemap = 0x200,
			CubemapPositiveX = 0x400,
			CubemapNegativeX = 0x800,
			CubemapPositiveY = 0x1000,
			CubemapNegativeY = 0x2000,
			CubemapPositiveZ = 0x4000,
			CubemapNegativeZ = 0x8000,
			Volume = 0x200000,

			CubemapAllFaces = Cubemap | CubemapPositiveX | CubemapNegativeX | CubemapPositiveY | CubemapNegativeY | CubemapPositiveZ | CubemapNegativeZ,
		}

		[Flags]
		public enum Ddpf : uint
		{
			AlphaPixels = 0x1,
			Alpha = 0x2,
			FourCC = 0x4,
			Rgb = 0x40,
			Yuv = 0x200,
			Luminance = 0x20000,
		}

		public static uint MakeFourCC(char c1, char c2, char c3, char c4)
		{
			return ((uint)c1 << 24) | ((uint)c2 << 16) | ((uint)c3 << 8) | (uint)c4;
		}

		public static uint MakeFourCC(string cc)
		{
			return ((uint)cc[0] << 24) | ((uint)cc[1] << 16) | ((uint)cc[2] << 8) | (uint)cc[3];
		}

		public enum FourCC : uint
		{
			A32B32G32R32F = 116,
			Dxt1 = 0x31545844,
			Dxt2 = 0x32545844,
			Dxt3 = 0x33545844,
			Dxt4 = 0x34545844,
			Dxt5 = 0x35545844,
			Dx10 = 0x30315844,
		}

		public struct DdsPixelFormat
		{
			public uint dwSize;
			public Ddpf dwFlags;
			public FourCC dwFourCC;
			public uint dwRgbBitCount;
			public uint dwRBitMask;
			public uint dwGBitMask;
			public uint dwBBitMask;
			public uint dwABitMask;
		}

		public struct DdsHeader
		{
			public uint dwSize;
			public Ddsd dwFlags;
			public uint dwHeight;
			public uint dwWidth;
			public uint dwPitchOrLinearSize;
			public uint dwDepth;
			public uint dwMipMapCount;
			public DdsPixelFormat ddspf;
			public DdsCaps dwCaps;
			public DdsCaps2 dwCaps2;
		}

		public static SurfaceFormat GetSurfaceFormat(ref DdsPixelFormat pixelFormat, out bool rbSwap)
		{
			rbSwap = false;
			if (pixelFormat.dwFlags.HasFlag(Ddpf.FourCC))
			{
				switch (pixelFormat.dwFourCC)
				{
					case FourCC.A32B32G32R32F:
						return SurfaceFormat.Vector4;
					case FourCC.Dxt1:
						return SurfaceFormat.Dxt1;
					case FourCC.Dxt2:
						throw new Exception("Unsupported compression format DXT2");
					case FourCC.Dxt3:
						return SurfaceFormat.Dxt3;
					case FourCC.Dxt4:
						throw new Exception("Unsupported compression format DXT4");
					case FourCC.Dxt5:
						return SurfaceFormat.Dxt5;
				}
			}
			else if (pixelFormat.dwFlags.HasFlag(Ddpf.Rgb))
			{
				// Uncompressed format
				if (pixelFormat.dwFlags.HasFlag(Ddpf.AlphaPixels))
				{
					// Format contains RGB and A
					if (pixelFormat.dwRgbBitCount == 16)
					{
						if (pixelFormat.dwABitMask == 0xF)
						{
							rbSwap = pixelFormat.dwBBitMask == 0xF0;
							return SurfaceFormat.Bgra4444;
						}
						rbSwap = pixelFormat.dwBBitMask == 0x3E;
						return SurfaceFormat.Bgra5551;
					}
					else if (pixelFormat.dwRgbBitCount == 32)
					{
						rbSwap = pixelFormat.dwBBitMask == 0xFF;
						return SurfaceFormat.Color;
					}
					throw new Exception("Unsupported RGBA pixel format");
				}
				else
				{
					// Format contains RGB only
					if (pixelFormat.dwRgbBitCount == 16)
					{
						rbSwap = pixelFormat.dwBBitMask == 0x1F;
						return SurfaceFormat.Bgr565;
					}
					else if (pixelFormat.dwRgbBitCount == 24)
					{
						rbSwap = pixelFormat.dwBBitMask == 0xFF;
						return SurfaceFormat.Color;
					}
					else if (pixelFormat.dwRgbBitCount == 32)
					{
						rbSwap = pixelFormat.dwBBitMask == 0xFF;
						return SurfaceFormat.Color;
					}
					throw new Exception("Unsupported RGB pixel format");
				}
			}
			throw new Exception("Unsupported pixel format");
		}

		static int GetBitmapSize(SurfaceFormat format, int width, int height)
		{
			// It is recommended that the dwPitchOrLinearSize field is ignored and we calculate it ourselves
			// https://msdn.microsoft.com/en-us/library/bb943991.aspx
			int pitch = 0;
			int rows = 0;

			switch (format)
			{
				case SurfaceFormat.Color:
				case SurfaceFormat.Bgra4444:
				case SurfaceFormat.Bgra5551:
				case SurfaceFormat.Bgr565:
				case SurfaceFormat.Vector4:
					pitch = width * format.GetSize();
					rows = height;
					break;

				case SurfaceFormat.Dxt1:
				case SurfaceFormat.Dxt3:
				case SurfaceFormat.Dxt5:
					pitch = ((width + 3) / 4) * format.GetSize();
					rows = (height + 3) / 4;
					break;

				default:
					throw new Exception("Unsupported SurfaceFormat " + format);
			}

			return pitch * rows;
		}

		internal static TextureContent Import(BinaryReader reader)
		{
			// Read signature ("DDS ")
			var valid = reader.ReadByte() == 0x44;
			valid = valid && reader.ReadByte() == 0x44;
			valid = valid && reader.ReadByte() == 0x53;
			valid = valid && reader.ReadByte() == 0x20;
			if (!valid)
				throw new Exception("Invalid file signature");

			var header = new DdsHeader();

			// Read DDS_HEADER
			header.dwSize = reader.ReadUInt32();
			if (header.dwSize != 124)
				throw new Exception("Invalid DDS_HEADER dwSize value");
			header.dwFlags = (Ddsd)reader.ReadUInt32();
			header.dwHeight = reader.ReadUInt32();
			header.dwWidth = reader.ReadUInt32();
			header.dwPitchOrLinearSize = reader.ReadUInt32();
			header.dwDepth = reader.ReadUInt32();
			header.dwMipMapCount = reader.ReadUInt32();
			// The next 11 DWORDs are reserved and unused
			for (int i = 0; i < 11; ++i)
				reader.ReadUInt32();
			// Read DDS_PIXELFORMAT
			header.ddspf.dwSize = reader.ReadUInt32();
			if (header.ddspf.dwSize != 32)
				throw new Exception("Invalid DDS_PIXELFORMAT dwSize value");
			header.ddspf.dwFlags = (Ddpf)reader.ReadUInt32();
			header.ddspf.dwFourCC = (FourCC)reader.ReadUInt32();
			header.ddspf.dwRgbBitCount = reader.ReadUInt32();
			header.ddspf.dwRBitMask = reader.ReadUInt32();
			header.ddspf.dwGBitMask = reader.ReadUInt32();
			header.ddspf.dwBBitMask = reader.ReadUInt32();
			header.ddspf.dwABitMask = reader.ReadUInt32();
			// Continue reading DDS_HEADER
			header.dwCaps = (DdsCaps)reader.ReadUInt32();
			header.dwCaps2 = (DdsCaps2)reader.ReadUInt32();
			// dwCaps3 unused
			reader.ReadUInt32();
			// dwCaps4 unused
			reader.ReadUInt32();
			// dwReserved2 unused
			reader.ReadUInt32();

			// Check for the existence of the DDS_HEADER_DXT10 struct next
			if (header.ddspf.dwFlags == Ddpf.FourCC && header.ddspf.dwFourCC == FourCC.Dx10)
			{
				throw new Exception("Unsupported DDS_HEADER_DXT10 struct found");
			}

			int faceCount = 1;
			int mipMapCount = (int)(header.dwCaps.HasFlag(DdsCaps.MipMap) ? header.dwMipMapCount : 1);

			TextureContent output;
			bool rbSwap;
			var format = GetSurfaceFormat(ref header.ddspf, out rbSwap);
			if (header.dwCaps2.HasFlag(DdsCaps2.Cubemap))
			{
				if (!header.dwCaps2.HasFlag(DdsCaps2.CubemapAllFaces))
					throw new Exception("Incomplete cubemap in DDS file");
				faceCount = 6;
				output = new TextureContent(TextureDimensions.TextureCube, format, (int)header.dwWidth, (int)header.dwHeight);
			}
			else
			{
				output = new TextureContent(TextureDimensions.Texture2D, format, (int)header.dwWidth, (int)header.dwHeight);
			}

			for (int f = 0; f < faceCount; ++f)
			{
				var w = (int)header.dwWidth;
				var h = (int)header.dwHeight;
				var mipMaps = new List<BitmapContent>();
				for (int m = 0; m < mipMapCount; ++m)
				{
					var byteCount = GetBitmapSize(format, w, h);
					// A 24-bit format is slightly different
					if (header.ddspf.dwRgbBitCount == 24)
						byteCount = 3 * w * h;
					var bytes = reader.ReadBytes(byteCount);
					if (rbSwap)
					{
						switch (format)
						{
							case SurfaceFormat.Bgr565:
								ByteSwapBGR565(bytes);
								break;
							case SurfaceFormat.Bgra4444:
								ByteSwapBGRA4444(bytes);
								break;
							case SurfaceFormat.Bgra5551:
								ByteSwapBGRA5551(bytes);
								break;
							case SurfaceFormat.Color:
								if (header.ddspf.dwRgbBitCount == 32)
									ByteSwapRGBX(bytes);
								else if (header.ddspf.dwRgbBitCount == 24)
									ByteSwapRGB(bytes);
								break;
						}
					}
					if ((format == SurfaceFormat.Color) && header.ddspf.dwFlags.HasFlag(Ddpf.Rgb) && !header.ddspf.dwFlags.HasFlag(Ddpf.AlphaPixels))
					{
						// Fill or add alpha with opaque
						if (header.ddspf.dwRgbBitCount == 32)
							ByteFillAlpha(bytes);
						else if (header.ddspf.dwRgbBitCount == 24)
							ByteExpandAlpha(ref bytes);
					}
					var content = new BitmapContent(w, h, format, bytes);

					mipMaps.Add(content);
					w = Math.Max(1, w / 2);
					h = Math.Max(1, h / 2);
				}

				output.Faces.Add(mipMaps);
			}

			return output;
		}

		static void ByteFillAlpha(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i += 4)
			{
				bytes[i + 3] = 255;
			}
		}

		static void ByteExpandAlpha(ref byte[] bytes)
		{
			var rgba = new byte[bytes.Length + (bytes.Length / 3)];
			int j = 0;
			for (int i = 0; i < bytes.Length; i += 3)
			{
				rgba[j] = bytes[i];
				rgba[j + 1] = bytes[i + 1];
				rgba[j + 2] = bytes[i + 2];
				rgba[j + 3] = 255;
				j += 4;
			}
			bytes = rgba;
		}

		static void ByteSwapRGB(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i += 3)
			{
				byte r = bytes[i];
				bytes[i] = bytes[i + 2];
				bytes[i + 2] = r;
			}
		}

		static void ByteSwapRGBX(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i += 4)
			{
				byte r = bytes[i];
				bytes[i] = bytes[i + 2];
				bytes[i + 2] = r;
			}
		}

		static void ByteSwapBGRA4444(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i += 2)
			{
				var r = bytes[i] & 0xF0;
				var b = bytes[i + 1] & 0xF0;
				bytes[i] = (byte)((bytes[i] & 0x0F) | b);
				bytes[i + 1] = (byte)((bytes[i + 1] & 0x0F) | r);
			}
		}

		static void ByteSwapBGRA5551(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i += 2)
			{
				var r = (bytes[i] & 0xF8) >> 3;
				var b = (bytes[i + 1] & 0x3E) >> 1;
				bytes[i] = (byte)((bytes[i] & 0x07) | (b << 3));
				bytes[i + 1] = (byte)((bytes[i + 1] & 0xC1) | (r << 1));
			}
		}

		static void ByteSwapBGR565(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i += 2)
			{
				var r = (bytes[i] & 0xF8) >> 3;
				var b = bytes[i + 1] & 0x1F;
				bytes[i] = (byte)((bytes[i] & 0x07) | (b << 3));
				bytes[i + 1] = (byte)((bytes[i + 1] & 0xE0) | r);
			}
		}
	}
}
