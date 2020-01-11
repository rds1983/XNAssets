# XNAssets
[![NuGet](https://img.shields.io/nuget/v/XNAssets.svg)](https://www.nuget.org/packages/XNAssets/) [![Build status](https://ci.appveyor.com/api/projects/status/j1q2injprkq3j18p?svg=true)](https://ci.appveyor.com/project/RomanShapiro/xnassets)

XNAssets is MonoGame/FNA asset manager library that - unlike MonoGame Content Pipeline - loads raw assets.

# Adding Reference
There are two ways of referencing XNAssets in the project:
1. Through nuget(works only for MonoGame): `install-package XNAssets`
2. As submodule(works for both MonoGame and FNA):
    
    a. `git submodule add https://github.com/rds1983/XNAssetss.git`
    
    b. `git submodule update --init --recursive`
    
    c. Copy SolutionDefines.targets from XNAssets/build/MonoGame(or XNAssets/build/FNA) to your solution folder.

      * If FNA is used, SolutionDefines.targets needs to be edited and FNAProj variable should be updated to the location of FNA.csproj next to the XNAssets.csproj location.
    
    d. Add XNAssets/src/XNAssets/XNAssets.csproj to the solution.

# Creating AssetManager
In order to create AssetManager two parameters must be passed to its constructor: GraphicsDevice and [IAssetResolver](https://github.com/rds1983/XNAssets/blob/master/src/XNAssets/Assets/IAssetResolver.cs). XNAAssets provides 3 implementation of latter:
  * [FileAssetResolver](https://github.com/rds1983/XNAssets/blob/master/src/XNAssets/Assets/FileAssetResolver.cs) that opens Stream using File.OpenRead. Sample AssetManager creation code:
```c#
FileAssetResolver assetResolver = new FileAssetResolver(Path.Combine(PathUtils.ExecutingAssemblyDirectory, "Assets"));
AssetManager assetManager = new AssetManager(GraphicsDevice, assetResolver);
```

  * [ResourceAssetResolver](https://github.com/rds1983/XNAssets/blob/master/src/XNAssets/Assets/ResourceAssetResolver.cs) that opens Stream using Assembly.GetManifestResourceStream. Sample AssetManager creation code:
```c#
ResourceAssetResolver assetResolver = new ResourceAssetResolver(typeof(MyGame).Assembly, "Resources.");
AssetManager assetManager = new AssetManager(GraphicsDevice, assetResolver);
```

  * [TitleContainerAssetResolver](https://github.com/rds1983/XNAssets/blob/master/src/XNAssets/Assets/TitleContainerAssetResolver.cs) that opens Stream using TitleContainer.OpenStream. Sample AssetManager creation code:
```c#
TitleContainerAssetResolver assetResolver = new TitleContainerAssetResolver("Assets");
AssetManager assetManager = new AssetManager(GraphicsDevice, assetResolver);
```

# Loading Assets
After AssetManager is created, it could be following way to load SpriteFont:
```c#
SpriteFont font = assetManager.Load<SpriteFont>("fonts/arial64.fnt");
```
Or following way to load Texture2D:
```c#
Texture2D texture = assetManager.Load<Texture2D>("images/LogoOnly_64px.png");
```

XNAssets allows to load following asset type out of the box:

Type|AssetLoader Type|Description
----|----------------|-----------
Texture2D|[Texture2DLoader](https://github.com/rds1983/XNAssets/blob/master/src/XNAssets/Assets/Texture2DLoader.cs)|Texture in BMP, TGA, PNG, JPG, GIF or PSD format. Alpha is being premultiplied after the loading
SpriteFont|[SpriteFontLoader](https://github.com/rds1983/XNAssets/blob/master/src/XNAssets/Assets/SpriteFontLoader.cs)|Font in AngelCode's BMFont .fnt format
string|[StringLoader](https://github.com/rds1983/XNAssets/blob/master/src/XNAssets/Assets/StringLoader.cs)|Loads any resource as string
SoundEffect|[SoundEffectLoader](https://github.com/rds1983/XNAssets/blob/master/src/XNAssets/Assets/SoundEffectLoader.cs)|SoundEffect in WAV format

