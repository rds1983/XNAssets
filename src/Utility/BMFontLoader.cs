#if !STRIDE

using System;
using System.Collections.Generic;
using Cyotek.Drawing.BitmapFont;
using System.Linq;
using System.Reflection;
using StbImageSharp;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAssets.Utility
{
	internal static class BMFontLoader
	{
		private static SpriteFont Load(BitmapFont data, Func<string, Texture2D> textureGetter)
		{
			if (data.Pages.Length > 1)
			{
				throw new NotSupportedException("For now only BMFonts with single texture are supported");
			}

			var texture = textureGetter(data.Pages[0].FileName);

			var glyphBounds = new List<Rectangle>();
			var cropping = new List<Rectangle>();
			var chars = new List<char>();
			var kerning = new List<Vector3>();

			var characters = data.Characters.Values.OrderBy(c => c.Char);
			foreach (var character in characters)
			{
				var bounds = new Rectangle(character.X, character.Y, character.Width, character.Height);

				glyphBounds.Add(bounds);
				cropping.Add(new Rectangle(character.XOffset, character.YOffset, bounds.Width, bounds.Height));

				chars.Add(character.Char);

				kerning.Add(new Vector3(0, character.Width, character.XAdvance - character.Width));
			}

			var constructorInfo = typeof(SpriteFont).GetTypeInfo().DeclaredConstructors.First();
			var result = (SpriteFont)constructorInfo.Invoke(new object[]
			{
				texture, glyphBounds, cropping,
				chars, data.LineHeight, 0, kerning, ' '
			});

			return result;
		}

		private static BitmapFont LoadBMFont(string data)
		{
			var bmFont = new BitmapFont();
			if (data.StartsWith("<"))
			{
				// xml
				bmFont.LoadXml(data);
			}
			else
			{
				bmFont.LoadText(data);
			}

			return bmFont;
		}

		public static SpriteFont Load(string data, Func<string, byte[]> imageDataGetter, GraphicsDevice device)
		{
			var bmFont = LoadBMFont(data);

			var textures = new Dictionary<string, Texture2D>();
			for (var i = 0; i < bmFont.Pages.Length; ++i)
			{
				var fileName = bmFont.Pages[i].FileName;
				var imageData = imageDataGetter(fileName);
				var image = ImageResult.FromMemory(imageData, ColorComponents.RedGreenBlueAlpha);
				if (image.SourceComp == ColorComponents.Grey)
				{
					// If input image is single byte per pixel, then StbImageSharp will set alpha to 255 in the resulting 32-bit image
					// Such behavior isn't acceptable for us
					// So reset alpha to color value
					for (var j = 0; j < image.Data.Length / 4; ++j)
					{
						image.Data[j * 4 + 3] = image.Data[j * 4];
					}
				}

				var texture = new Texture2D(device, image.Width, image.Height);
				var bounds = new Rectangle(0, 0, image.Width, image.Height);
				texture.SetData(0, bounds, image.Data, 0, bounds.Width * bounds.Height * 4);

				textures[fileName] = texture;
			}

			return Load(bmFont, fileName => textures[fileName]);
		}
	}
}

#endif