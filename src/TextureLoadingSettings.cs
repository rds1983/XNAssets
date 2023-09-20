using System;
using System.ComponentModel;
using AssetManagementBase;
using Microsoft.Xna.Framework;

namespace XNAssets
{
	public class TextureLoadingSettings : IAssetSettings
	{
		public static readonly TextureLoadingSettings Default = new TextureLoadingSettings();
		public static readonly TextureLoadingSettings DefaultPremultiplyAlpha = new TextureLoadingSettings(premultiplyAlpha: true);

		/// <summary>
		/// Gets or sets the color used when color keying for a texture is enabled. When color keying, 
		/// all pixels of a specified color are replaced with transparent black.
		/// </summary>
		/// <value>Color value of the material to replace with transparent black.</value>
		[DefaultValue(typeof(Color), "255, 0, 255, 255")]
		[DisplayName("Color Key Color")]
		[Description("If the texture is color keyed, pixels of this color are replaced with transparent black.")]
		[Obsolete("Not yet implemented")]
		public Color ColorKeyColor { get; }


		/// <summary>
		/// Gets or sets a value indicating whether color keying of a texture is enabled.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if color keying is enabled; <see langword="false"/> otherwise.
		/// </value>
		[DefaultValue(false)]
		[DisplayName("Color Key Enabled")]
		[Description("If enabled, the texture is color keyed. Pixels matching the value of \"Color Key Color\" are replaced with transparent black.")]
		[Obsolete("Not yet implemented")]
		public bool ColorKeyEnabled { get; }


		/// <summary>
		/// Gets or sets a value indicating whether a full chain of mipmaps is generated from the input 
		/// texture. Existing mipmaps of the texture are not replaced.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if mipmap generation is enabled; <see langword="false"/> otherwise.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
		[DefaultValue(true)]
		[DisplayName("Generate Mipmaps")]
		[Description("If enabled, a full mipmap chain is generated for the texture. Existing mipmaps are not replaced.")]
		[Obsolete("Not yet implemented")]
		public bool GenerateMipmaps { get; }


		/// <summary>
		/// Gets or sets the gamma of the input texture.
		/// </summary>
		/// <value>The gamma of the input texture. The default value is 2.2.</value>
		[DefaultValue(2.2f)]
		[DisplayName("Input Gamma")]
		[Description("Specifies the gamma of the input texture.")]
		[Obsolete("Not yet implemented")]
		public float InputGamma { get; }


		/// <summary>
		/// Gets or sets the gamma of the output texture.
		/// </summary>
		/// <value>The gamma of the output texture. The default value is 2.2f.</value>
		[DefaultValue(2.2f)]
		[DisplayName("Output Gamma")]
		[Description("Specifies the gamma of the output texture.")]
		[Obsolete("Not yet implemented")]
		public float OutputGamma { get; }


		/// <summary>
		/// Gets or sets a value indicating whether the texture is converted to premultiplied alpha format.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if alpha premultiply is enabled; otherwise, <see langword="false"/>.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
		[DefaultValue(true)]
		[DisplayName("Premultiply Alpha")]
		[Description("If enabled, the texture is converted to premultiplied alpha format.")]
		public bool PremultiplyAlpha { get; }


		/// <summary>
		/// Gets or sets a value indicating whether the texture is resized to the next largest power of 
		/// two.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if resizing is enabled; <see langword="false"/> otherwise.
		/// </value>
		/// <remarks>
		/// Typically used to maximize compatibility with a graphics card because many graphics cards 
		/// do not support a material size that is not a power of two. If 
		/// <see cref="ResizeToPowerOfTwo"/> is enabled, textures are resized to the next largest power 
		/// of two.
		/// </remarks>
		[DefaultValue(false)]
		[DisplayName("Resize to Power of Two")]
		[Description("If enabled, the texture is resized to the next largest power of two, maximizing compatibility. Many graphics cards do not support textures sizes that are not a power of two.")]
		[Obsolete("Not yet implemented")]
		public bool ResizeToPowerOfTwo { get; }

		/// <summary>
		/// Gets or sets the reference alpha value, which is used in the alpha test.
		/// </summary>
		/// <value>The reference alpha value, which is used in the alpha test.</value>
		[DefaultValue(0.9f)]
		[DisplayName("Reference Alpha")]
		[Description("Specifies the reference alpha value, which is used in the alpha test.")]
		[Obsolete("Not yet implemented")]
		public float ReferenceAlpha { get; }


		/// <summary>
		/// Gets or sets a value indicating whether the alpha of the lower mipmap levels should be 
		/// scaled to achieve the same alpha test coverage as in the source image.
		/// </summary>
		/// <value>
		/// <see langword="true"/> to scale the alpha values of the lower mipmap levels; otherwise, 
		/// <see langword="false"/>.
		/// </value>
		[DefaultValue(false)]
		[DisplayName("Scale Alpha To Coverage")]
		[Description("Specifies whether the alpha of the lower mipmap levels should be scaled to achieve the same alpha test coverage as in the source image.")]
		[Obsolete("Not yet implemented")]
		public bool ScaleAlphaToCoverage { get; }

		public TextureLoadingSettings(Color? colorKeyColor = null, bool colorKeyEnabled = false,
			bool generateMipmaps = true, float inputGamma = 2.2f, float outputGamma = 2.2f,
			bool premultiplyAlpha = true, bool resizeToPowerOfTwo = false,
			float referenceAlpha = 0.9f, bool scaleAlphaToCoverage = false)
		{
#pragma warning disable CS0618 // Type or member is obsolete
			ColorKeyColor = colorKeyColor ?? Color.Magenta;
			ColorKeyEnabled = colorKeyEnabled;
			GenerateMipmaps = generateMipmaps;
			InputGamma = inputGamma;
			OutputGamma = outputGamma;
			PremultiplyAlpha = premultiplyAlpha;
			ResizeToPowerOfTwo = resizeToPowerOfTwo;
			ReferenceAlpha = referenceAlpha;
			ScaleAlphaToCoverage = scaleAlphaToCoverage;
#pragma warning restore CS0618 // Type or member is obsolete
		}

		public string BuildKey() => PremultiplyAlpha ? "pm" : "npm";

		public TextureLoadingSettings Clone()
		{
#pragma warning disable CS0618 // Type or member is obsolete
			return new TextureLoadingSettings(ColorKeyColor, ColorKeyEnabled,
				GenerateMipmaps, InputGamma, OutputGamma,
				PremultiplyAlpha, ResizeToPowerOfTwo, ReferenceAlpha, ScaleAlphaToCoverage);
#pragma warning restore CS0618 // Type or member is obsolete
		}
	}
}