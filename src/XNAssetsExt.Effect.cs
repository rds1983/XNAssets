using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AssetManagementBase
{
	partial class XNAssetsExt
	{
		private static AssetLoader<Effect> _effectLoader = (manager, assetName, settings) =>
		{
			var data = manager.ReadAssetAsByteArray(assetName);
			var graphicsDevice = (GraphicsDevice)settings;
			return new Effect(graphicsDevice, data);
		};

		public static Effect LoadEffect(this AssetManager assetManager, GraphicsDevice graphicsDevice, string name, Dictionary<string, string> defines = null)
		{
			var key = new StringBuilder();

			var nameWithoutExt = Path.GetFileNameWithoutExtension(name);
			var ext = Path.GetExtension(name);

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

			return assetManager.UseLoader(_effectLoader, key.ToString(), graphicsDevice);
		}
	}
}
