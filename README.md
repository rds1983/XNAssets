## XNAssets
[![NuGet](https://img.shields.io/nuget/v/XNAssets.Monogame.svg)](https://www.nuget.org/packages/XNAssets.Monogame/) [![Chat](https://img.shields.io/discord/628186029488340992.svg)](https://discord.gg/ZeHxhCY)

XNAssets is the asset management library for Monogame/FNA. Unlike Content Pipeline, it loads raw assets.

## Adding Reference
### For MonoGame
https://www.nuget.org/packages/XNAssets.MonoGame/

### For FNA
1. Clone this repo.
2. Add src/XNAssets.FNA.csproj or src/XNAssets.FNA.Core.csproj to the solution.
3. The overall folder structure is expected to be following: ![Folder Structure](/images/FolderStructure.png)

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

XNAssets allows to load following asset types out of the box:

Type|Method Name|Description
----|-----------|-----------
Texture2D|LoadTexture2D|Texture in BMP, TGA, PNG, JPG, GIF, PSD or DDS format. There's optional parameter that determines whether the alpha should be premultiplied. The parameter is ignored if loading dds.
TextureCube|LoadTextureCube|Cube Texture in DDS format.
SpriteFont|LoadSpriteFont|Font in AngelCode's BMFont .fnt format
SoundEffect|LoadSoundEffect|SoundEffect in WAV format
Effect|LoadEffect|Effect in binary form

## FontStashSharp Support
If you're using [FontStashSharp](https://github.com/FontStashSharp/FontStashSharp), then following code snippet will allow to load FontSystems and StaticSpriteFonts through XNAssets:
```c#
using System.IO;
using AssetManagementBase;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal static class FSSLoaders
{
    private class FontSystemLoadingSettings: IAssetSettings
    {
        public Texture2D ExistingTexture { get; set; }
        public Rectangle ExistingTextureUsedSpace { get; set; }
        public string[] AdditionalFonts { get; set; }

        public string BuildKey() => string.Empty;
    }

    private static AssetLoader<FontSystem> _fontSystemLoader = (manager, assetName, settings, tag) =>
    {
        var fontSystemSettings = new FontSystemSettings();

        var fontSystemLoadingSettings = (FontSystemLoadingSettings)settings;
        if (fontSystemLoadingSettings != null)
        {
            fontSystemSettings.ExistingTexture = fontSystemLoadingSettings.ExistingTexture;
            fontSystemSettings.ExistingTextureUsedSpace = fontSystemLoadingSettings.ExistingTextureUsedSpace;
        };

        var fontSystem = new FontSystem(fontSystemSettings);
        var data = manager.ReadAsByteArray(assetName);
        fontSystem.AddFont(data);
        if (fontSystemLoadingSettings != null && fontSystemLoadingSettings.AdditionalFonts != null)
        {
            foreach (var file in fontSystemLoadingSettings.AdditionalFonts)
            {
                data = manager.LoadByteArray(file, false);
                fontSystem.AddFont(data);
            }
        }

        return fontSystem;
    };

    private static AssetLoader<StaticSpriteFont> _staticFontLoader = (manager, assetName, settings, tag) =>
    {
        var fontData = manager.ReadAsString(assetName);
        var graphicsDevice = (GraphicsDevice)tag;

        return StaticSpriteFont.FromBMFont(fontData,
                    name =>
                    {
                        var imageData = manager.LoadByteArray(name, false);
                        return new MemoryStream(imageData);
                    },
                    graphicsDevice);
    };

    public static FontSystem LoadFontSystem(this AssetManager assetManager, string assetName, string[] additionalFonts = null, Texture2D existingTexture = null, Rectangle existingTextureUsedSpace = default(Rectangle))
    {
        FontSystemLoadingSettings settings = null;
        if (additionalFonts != null || existingTexture != null)
        {
            settings = new FontSystemLoadingSettings
            {
                AdditionalFonts = additionalFonts,
                ExistingTexture = existingTexture,
                ExistingTextureUsedSpace = existingTextureUsedSpace
            };
        }

        return assetManager.UseLoader(_fontSystemLoader, assetName, settings);
    }

    public static StaticSpriteFont LoadStaticSpriteFont(this AssetManager assetManager, GraphicsDevice graphicsDevice, string assetName)
    {
        return assetManager.UseLoader(_staticFontLoader, assetName, tag: graphicsDevice);
    }
}
```

Now it would be possible to load FontSystem through following code:
```c#
FontSystem fs = assetManager.LoadFontSystem("arial.ttf");
```

Or StaticSpriteFont through:
```c#
StaticSpriteFont ssf = assetManager.LoadStaticSpriteFont(graphicsDevice, "arial.fnt");
```

## Additional Documentation
See [AssetManagementBase documentation](https://github.com/rds1983/AssetManagementBase) if you want to learn more(i.e. how to add additional loader methods).
