using System;
using System.IO;

#if !XENKO
using Microsoft.Xna.Framework.Graphics;
#else
using Xenko.Graphics;
#endif

namespace XNAssets
{
	public class AssetLoaderContext
	{
		private readonly AssetManager _assetManager;
		private readonly string _baseFolder;

		public GraphicsDevice GraphicsDevice
		{
			get
			{
				return _assetManager.GraphicsDevice;
			}
		}

		public AssetManagerSettings Settings
		{
			get
			{
				return _assetManager.Settings;
			}
		}

		internal AssetLoaderContext(AssetManager assetManager, string baseFolder)
		{
			_assetManager = assetManager ?? throw new Exception("assetManager");
			_baseFolder = baseFolder;
		}

		/// <summary>
		/// Opens a stream specified by asset path
		/// Throws an exception on failure
		/// </summary>
		/// <param name="assetName"></param>
		/// <returns></returns>
		public Stream Open(string assetName)
		{
			return _assetManager.Open(AssetManager.CombinePath(_baseFolder, assetName));
		}

		/// <summary>
		/// Reads specified asset to string
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public string ReadAsText(string path)
		{
			string result;
			using (var input = Open(path))
			{
				using (var textReader = new StreamReader(input))
				{
					result = textReader.ReadToEnd();
				}
			}

			return result;
		}

		public T Load<T>(string assetName)
		{
			return _assetManager.Load<T>(AssetManager.CombinePath(_baseFolder, assetName));
		}
	}
}