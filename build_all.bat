dotnet --version
dotnet build build\XNAssets.MonoGame.sln /p:Configuration=Release --no-incremental
dotnet build build\XNAssets.Xenko.sln /p:Configuration=Release --no-incremental
call copy_zip_package_files.bat
rename "ZipPackage" "XNAssets.%APPVEYOR_BUILD_VERSION%"
7z a XNAssets.%APPVEYOR_BUILD_VERSION%.zip XNAssets.%APPVEYOR_BUILD_VERSION%
