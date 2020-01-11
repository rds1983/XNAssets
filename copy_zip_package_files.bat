rem delete existing
rmdir "ZipPackage" /Q /S

rem Create required folders
mkdir "ZipPackage"
mkdir "ZipPackage\x64"
mkdir "ZipPackage\x86"
mkdir "ZipPackage\Assets"

set "CONFIGURATION=Release\net45"
set "CONFIGURATION_XENKO=Release\netstandard2.0"

rem Copy output files
copy "src\XNAssets\bin\MonoGame\%CONFIGURATION%\XNAssets.dll" ZipPackage /Y
copy "src\XNAssets\bin\MonoGame\%CONFIGURATION%\XNAssets.pdb" ZipPackage /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\XNAssets.Samples.Test.exe" ZipPackage /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\MonoGame.Framework.dll" "ZipPackage" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\MonoGame.Framework.dll.config" "ZipPackage" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\x64\libSDL2-2.0.so.0" "ZipPackage\x64" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\x64\libopenal.so.1" "ZipPackage\x64" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\x64\SDL2.dll" "ZipPackage\x64" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\x64\soft_oal.dll" "ZipPackage\x64" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\x86\libSDL2-2.0.so.0" "ZipPackage\x86" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\x86\libopenal.so.1" "ZipPackage\x86" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\x86\SDL2.dll" "ZipPackage\x86" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\x86\soft_oal.dll" "ZipPackage\x86" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\libSDL2-2.0.0.dylib" "ZipPackage" /Y
copy "samples\XNAssets.Samples.Test\bin\MonoGame\%CONFIGURATION%\libopenal.1.dylib" "ZipPackage" /Y
xcopy "samples\XNAssets.Samples.Test\Assets\*.*" "ZipPackage\Assets\*.*" /s
