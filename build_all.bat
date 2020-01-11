dotnet --version
dotnet build build\Monogame\XNAssets.sln /p:Configuration=Release --no-incremental
dotnet build build\FNA\XNAssets.sln /p:Configuration=Release --no-incremental
dotnet build build\Xenko\XNAssets.sln /p:Configuration=Release --no-incremental
call copy_zip_package_files.bat
rename "ZipPackage" "XNAssets.%APPVEYOR_BUILD_VERSION%"
7z a XNAssets.%APPVEYOR_BUILD_VERSION%.zip XNAssets.%APPVEYOR_BUILD_VERSION%
