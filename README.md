# XNAssets
[![NuGet](https://img.shields.io/nuget/v/XNAssets.svg)](https://www.nuget.org/packages/XNAssets/) [![Chat](https://img.shields.io/discord/628186029488340992.svg)](https://discord.gg/ZeHxhCY)

XNAssets is MonoGame/FNA asset management library that - unlike MonoGame Content Pipeline - loads raw assets.

# Adding Reference
## For MonoGame
https://www.nuget.org/packages/XNAssets.MonoGame/

## For FNA
1. Clone this repo.
2. Add src/XNAssets.FNA.csproj or src/XNAssets.FNA.Core.csproj to the solution.
3. The overall folder structure is expected to be following: ![Folder Structure](/images/FolderStructure.png)

# Creating AssetManager
Creating AssetManager that loads files:
```c#
AssetManager assetManager = AssetManager.CreateFileAssetManager(@"c:\MyGame\Assets");
```

Creating AssetManager that loads resources:
```c#
AssetManager assetManager = AssetManager.CreateResourceAssetManager(_assembly, "Resources");
```
If _assembly's name is "Assembly.Name" then the above code will create AssetManager that loads resourcies with prefix "Assembly.Name.Prefix.".

If we don't the assembly's name prepended to the prefix, then pass 'false' as the third param when calling CreateResourceAssetManager. I.e.
```c#
AssetManager assetManager = AssetManager.CreateResourceAssetManager(_assembly, "Full.Path.Resources", false);
```
# Loading Assets
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
Texture2D|LoadTexture2D|Texture in BMP, TGA, PNG, JPG, GIF or PSD format. There's parameter that determines whether the alpha should be premultiplied
SpriteFont|LoadSpriteFont|Font in AngelCode's BMFont .fnt format
SoundEffect|LoadSoundEffect|SoundEffect in WAV format
Effect|LoadEffect|Effect in binary form

# Custom Asset Types
See [AssetManagementBase documentation](https://github.com/rds1983/AssetManagementBase) in order to learn how to add custom asset types.
