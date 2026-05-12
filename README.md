## XNAssets
[![NuGet](https://img.shields.io/nuget/v/XNAssets.Monogame.svg)](https://www.nuget.org/packages/XNAssets.Monogame/) [![Chat](https://img.shields.io/discord/628186029488340992.svg)](https://discord.gg/ZeHxhCY)

XNAssets is the asset management library for Monogame/FNA. Unlike Content Pipeline, it loads raw assets.

## Adding a Reference
XNAssets consists of the following assemblies (click on the name for MonoGame nuget link):
Name|Description
----|-----------
[XNAssets](https://www.nuget.org/packages/XNAssets.Monogame/)|Base asset types (textures, effects, etc)
[XNAssets.FontStashSharp](https://www.nuget.org/packages/XNAssets.FontStashSharp.Monogame/)|[FontStashSharp](https://github.com/FontStashSharp/FontStashSharp) support

See [this](https://github.com/DigitalRiseEngine/DigitalRiseModel/wiki/Adding-Reference-For-FNA-Project) on how to reference the library in the FNA project.

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
After AssetManager is created, it can be used in the following way to load SpriteFont:
```c#
    SpriteFont font = assetManager.LoadSpriteFont(graphicsDevice, "fonts/arial64.fnt");
```
Or in the following way to load Texture2D:
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

## Loading Effects with Shader Defines

XNAssets provides support for loading Effects with shader preprocessor defines. This allows you to maintain a single shader source file and generate multiple compiled effect files for different feature combinations.

### Basic Usage

To load an effect, use the `LoadEffect` method:
```c#
Effect effect = assetManager.LoadEffect(graphicsDevice, "effects/myshader.fxb");
```

### Using Shader Defines

You can specify shader defines as a dictionary when loading an effect:
```c#
var defines = new Dictionary<string, string>
{
    { "USE_NORMAL_MAP", "1" },
    { "QUALITY_LEVEL", "2" }
};

Effect effect = assetManager.LoadEffect(graphicsDevice, "effects/myshader.fxb", defines);
```

When defines are provided, XNAssets automatically encodes them into the asset key used for caching. The defines are expected to be part of the compiled effect file name. For example, if you provide defines like `USE_NORMAL_MAP=1` and `QUALITY_LEVEL=2`, the asset loader will look for a file named something like:
```
effects/myshader_USE_NORMAL_MAP_QUALITY_LEVEL_2.fxb
```

The naming convention follows these rules:
- Defines are appended to the base filename with underscores
- Define keys are sorted alphabetically
- For defines with value `"1"`, only the key is appended
- For defines with other values, both key and value are appended separated by an underscore

### Integration with efscriptgen

[efscriptgen](https://github.com/rds1983/efscriptgen) is a tool that automatically generates batch scripts to compile shader source files into effect binaries with various define combinations. This integrates seamlessly with XNAssets:

1. **Generate compiled effects**: Use efscriptgen to compile your .fx shader files with different define combinations, producing .fxb files with encoded define combinations in their names.

2. **Load with XNAssets**: When you load an effect with specific defines, XNAssets automatically constructs the correct filename and loads the pre-compiled effect file.

Example workflow:
```c#
// Your shader compilation (handled by efscriptgen):
// effect.fx -> effect.fxb
// effect.fx with /D USE_NORMAL_MAP=1 -> effect_USE_NORMAL_MAP.fxb
// effect.fx with /D USE_NORMAL_MAP=1 /D QUALITY_LEVEL=2 -> effect_QUALITY_LEVEL_2_USE_NORMAL_MAP.fxb

// Loading in your game code:
var defines = new Dictionary<string, string>
{
    { "USE_NORMAL_MAP", "1" },
    { "QUALITY_LEVEL", "2" }
};
Effect effect = assetManager.LoadEffect(graphicsDevice, "effects/effect.fxb", defines);
// This will load: effects/effect_QUALITY_LEVEL_2_USE_NORMAL_MAP.fxb
```

This approach allows you to:
- Maintain a single shader source file
- Generate multiple optimized variants with different feature sets
- Load the correct variant based on runtime conditions (quality settings, available hardware features, etc.)
- Benefit from caching to avoid reloading the same effect variant multiple times

## FontStashSharp Support
After referencing XNAssets.FontStashSharp, you can load FontSystem using the following code:
```c#
FontSystem fs = assetManager.LoadFontSystem("arial.ttf");
```

Or StaticSpriteFont through:
```c#
StaticSpriteFont ssf = assetManager.LoadStaticSpriteFont(graphicsDevice, "arial.fnt");
```
## Loading 3D Models
[DigitalRiseModel](https://github.com/DigitalRiseEngine/DigitalRiseModel) is a library that allows you to load 3D models from GLTF/GLB through XNAssets.

## Additional Documentation
See [AssetManagementBase documentation](https://github.com/rds1983/AssetManagementBase) if you want to learn more (i.e., how to add additional loader methods).
