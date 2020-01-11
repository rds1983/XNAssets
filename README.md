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
In order to create AssetManager two parameters must be passed to its constructor: GraphicsDevice and IAssetResolver. XNAAssets provides 3 implementation of latter:
  * FileAssetResolver that opens Stream using File.OpenRead. Sample AssetManager creation code:
```c#
FileAssetResolver assetResolver = new FileAssetResolver(Path.Combine(PathUtils.ExecutingAssemblyDirectory, "Assets"));
AssetManager assetManager = new AssetManager(GraphicsDevice, assetResolver);
```

  * ResourceAssetResolver that opens Stream using Assembly.GetManifestResourceStream. Sample AssetManager creation code:
```c#
AssetManager assetManager = new AssetManager(new ResourceAssetResolver(typeof(MyGame).Assembly, "Resources."));
```


