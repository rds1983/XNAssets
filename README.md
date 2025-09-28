## XNAssets
[![NuGet](https://img.shields.io/nuget/v/XNAssets.Monogame.svg)](https://www.nuget.org/packages/XNAssets.Monogame/) [![Chat](https://img.shields.io/discord/628186029488340992.svg)](https://discord.gg/ZeHxhCY)

XNAssets is the asset management library for Monogame/FNA. Unlike Content Pipeline, it loads raw assets.

## Adding Reference
DigitalRiseModel consists of following assemblies(click on the name for MonoGame nuget link):
Name|Description
----|-----------
[XNAssets](https://www.nuget.org/packages/XNAssets.Monogame/)|Base assets types(textures, effects, etc)
[XNAssets.FontStashSharp](https://www.nuget.org/packages/XNAssets.FontStashSharp.Monogame/)|[FontStashSharp](https://github.com/FontStashSharp/FontStashSharp) support

See [this]() on how to reference the library in the FNA project.

## Creating AssetManager
Creating AssetManager that loads files:
```c#
AssetManager assetManager = AssetManager.CreateFileAssetManager(@"c:\MyGame\Assets");
```

Creating AssetManager that loads resources:
```c#
AssetManager assetManager = AssetManager.CreateResourceAssetManager(_assembly, "Resources");
```
If _assembly's name is "Assembly.Name" then the above code will create AssetManager that loads resources with prefix "Assembly.Name.Prefix.".

If we don't the assembly's name prepended to the prefix, then pass 'false' as the third param when calling CreateResourceAssetManager. I.e.
```c#
AssetManager assetManager = AssetManager.CreateResourceAssetManager(_assembly, "Full.Path.Resources", false);
```

## Loading Assets
After AssetManager is created, it could be used following way to load SpriteFont:
```c#
    SpriteFont font = assetManager.LoadSpriteFont(graphicsDevice, "fonts/arial64.fnt");
```
Or following way to load Texture2D:
```c#
    Texture2D texture = assetManager.LoadTexture2D(graphicsDevice, "images/LogoOnly_64px.png");
```

Base XNAssets allows to load following asset types:

Type|Method Name|Description
----|-----------|-----------
Texture2D|LoadTexture2D|Texture in BMP, TGA, PNG, JPG, GIF, PSD or DDS format. There's optional parameter that determines whether the alpha should be premultiplied. The parameter is ignored if loading dds.
TextureCube|LoadTextureCube|Cube Texture in DDS format.
SpriteFont|LoadSpriteFont|Font in AngelCode's BMFont .fnt format
SoundEffect|LoadSoundEffect|SoundEffect in WAV format
Effect|LoadEffect|Effect in binary form

## FontStashSharp Support
After referencing XNAssets.FontStashSharp, it would be possible to load FontSystem through following code:
```c#
FontSystem fs = assetManager.LoadFontSystem("arial.ttf");
```

Or StaticSpriteFont through:
```c#
StaticSpriteFont ssf = assetManager.LoadStaticSpriteFont(graphicsDevice, "arial.fnt");
```
## Loading 3D Models
[DigitalRiseModel](https://github.com/DigitalRiseEngine/DigitalRiseModel) is library that allows to load 3d models from GLTF/GLB through XNAssets.

## Additional Documentation
See [AssetManagementBase documentation](https://github.com/rds1983/AssetManagementBase) if you want to learn more(i.e. how to add additional loader methods).
