## XNAssets
[![NuGet](https://img.shields.io/nuget/v/XNAssets.Monogame.svg)](https://www.nuget.org/packages/XNAssets.Monogame/) [![Chat](https://img.shields.io/discord/628186029488340992.svg)](https://discord.gg/ZeHxhCY)

XNAssets is the asset management library for Monogame/FNA. Unlike Content Pipeline, it loads raw assets.

## Adding a Reference
XNAssets consists of the following assemblies (click on the name for MonoGame nuget link):
Name|Description
----|-----------
[XNAssets](https://www.nuget.org/packages/XNAssets.Monogame/)|Base asset types (textures, effects, etc)
[XNAssets.FontStashSharp](https://www.nuget.org/packages/XNAssets.FontStashSharp.Monogame/)|[FontStashSharp](https://github.com/FontStashSharp/FontStashSharp) support

See [this guide](https://github.com/DigitalRiseEngine/DigitalRiseModel/wiki/Adding-Reference-For-FNA-Project) on how to reference the library in the FNA project.

## Outputting Asset Loading Logging to Console
```c#
AMBConfiguration.Logger = Console.WriteLine;
```

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

If you don't want the assembly's name prepended to the prefix, then pass 'false' as the third parameter when calling CreateResourceAssetManager. For example:
```c#
AssetManager assetManager = AssetManager.CreateResourceAssetManager(_assembly, "Full.Path.Resources", false);
```

## Loading Assets
After AssetManager is created, you can load a SpriteFont as follows:
```c#
    SpriteFont font = assetManager.LoadSpriteFont(graphicsDevice, "fonts/arial64.fnt");
```
Or load a Texture2D like this:
```c#
    Texture2D texture = assetManager.LoadTexture2D(graphicsDevice, "images/LogoOnly_64px.png");
```

Base XNAssets allows you to load the following asset types:

Type|Method Name|Description
----|-----------|-----------
Texture2D|LoadTexture2D|Texture in BMP, TGA, PNG, JPG, GIF, PSD or DDS format. There's optional parameter that determines whether the alpha should be premultiplied. The parameter is ignored if loading dds.
TextureCube|LoadTextureCube|Cube Texture in DDS format.
SpriteFont|LoadSpriteFont|Font in AngelCode's BMFont .fnt format
SoundEffect|LoadSoundEffect|SoundEffect in WAV format
Effect|LoadEffect|Effect in binary form

## Asset Path Resolution

XNAssets supports flexible path resolution for easy, relative-path-friendly asset references:

1. **Relative Paths** (e.g., `"textures/character.png"`): Resolved relative to the current asset's folder context. This enables recursive loading where nested assets can reference sibling assets.

2. **Rooted Paths from Base** (e.g., `"/textures/character.png"`): These are always resolved from the base asset folder, regardless of current context.

3. **Explicit File System Paths** (e.g., `"@C:\Assets\textures\character.png"`): Start with `@` to use absolute file system paths.

4. **Path Normalization**: 
   - Backslashes are normalized to forward slashes
   - `..` sequences resolve to parent folders
   - All paths are processed to their canonical form

**Examples:**
- `assetManager.LoadTexture2D(graphicsDevice, "sprites/player.png")` — loads relative to current folder
- `assetManager.LoadTexture2D(graphicsDevice, "/shared/effects.fxb")` — loads from base folder
- `assetManager.LoadTexture2D(graphicsDevice, "../common/ui.png")` — navigates up one level
- `assetManager.LoadTexture2D(graphicsDevice, "@C:\Assets\external\sprite.png")` — loads from absolute path

## FontStashSharp Support
After referencing XNAssets.FontStashSharp, you can load FontSystem using the following code:
```c#
FontSystem fs = assetManager.LoadFontSystem("arial.ttf");
```

Or load StaticSpriteFont with:
```c#
StaticSpriteFont ssf = assetManager.LoadStaticSpriteFont(graphicsDevice, "arial.fnt");
```
## Loading 3D Models
[DigitalRiseModel](https://github.com/DigitalRiseEngine/DigitalRiseModel) is a library that allows you to load 3D models from GLTF/GLB through XNAssets.

## Additional Documentation
See [AssetManagementBase documentation](https://github.com/rds1983/AssetManagementBase) if you want to learn more (i.e., how to add additional loader methods).
