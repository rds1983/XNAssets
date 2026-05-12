#if !STRIDE

using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace AssetManagementBase
{
	/// <summary>
	/// Provides extension methods for loading various asset types through an AssetManager.
	/// </summary>
	partial class XNAssetsExt
	{
		private static AssetLoader<Effect> _effectLoader = (manager, assetName, settings, tag) =>
		{
			var data = manager.ReadAsByteArray(assetName);
			var graphicsDevice = (GraphicsDevice)tag;
			return new Effect(graphicsDevice, data)
			{
				Name = assetName
			};
		};

		/// <summary>
		/// Loads an Effect asset from the asset manager with optional shader defines.
		/// </summary>
		/// <param name="assetManager">The AssetManager instance.</param>
		/// <param name="graphicsDevice">The GraphicsDevice to create the Effect with.</param>
		/// <param name="name">The name or path of the effect asset.</param>
		/// <param name="defines">Optional dictionary of preprocessor defines for shader compilation.</param>
		/// <returns>The loaded Effect object.</returns>
		public static Effect LoadEffect(this AssetManager assetManager, GraphicsDevice graphicsDevice, string name, Dictionary<string, string> defines = null)
		{
			var key = new StringBuilder();

			var nameWithoutExt = name;
			var ext = string.Empty;
			var extPos = name.LastIndexOf('.');

			if (extPos != -1)
			{
				nameWithoutExt = name.Substring(0, extPos);
				ext = name.Substring(extPos + 1);
			}

			key.Append(nameWithoutExt);

			if (defines != null && defines.Count > 0)
			{
				var keys = (from def in defines.Keys orderby def select def).ToArray();
				foreach (var k in keys)
				{
					key.Append("_");
					key.Append(k);
					var value = defines[k];
					if (value != "1")
					{
						key.Append("_");
						key.Append(value);
					}
				}
			}

			var keyString = key.ToString();

			if (!string.IsNullOrEmpty(ext))
			{
				if (!ext.StartsWith("."))
				{
					key.Append('.');
				}
				key.Append(ext);
			}

			return assetManager.UseLoader(_effectLoader, key.ToString(), tag: graphicsDevice);
		}
	}
}

#endif