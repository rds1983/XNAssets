using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AssetManagementBase
{
	partial class XNAssetsExt
	{
		private static AssetLoader<Effect> _effectLoader = (manager, assetName, settings, tag) =>
		{
			var data = manager.ReadAsByteArray(assetName);
			var graphicsDevice = (GraphicsDevice)tag;
			return new Effect(graphicsDevice, data);
		};

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
