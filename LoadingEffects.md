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

1. **Generate compiled effects**: Use efscriptgen to compile your .fx shader files with different define combinations, producing compiled effects with define combinations encoded in their names.

2. **Load with XNAssets**: When you load an effect with specific defines, XNAssets automatically constructs the correct filename and loads the pre-compiled effect file.

Example workflow:
```c#
// Your shader compilation (handled by efscriptgen):
// effect.fx -> effect.efb
// effect.fx with /D USE_NORMAL_MAP=1 -> effect_USE_NORMAL_MAP.efb
// effect.fx with /D USE_NORMAL_MAP=1 /D QUALITY_LEVEL=2 -> effect_QUALITY_LEVEL_2_USE_NORMAL_MAP.efb

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
