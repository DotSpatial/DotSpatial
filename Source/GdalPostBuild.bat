:: Post-build bat, which copies GDAL native libraries into output directory
echo In_GdalPostBuild.bat
echo %1
if not exist %1gdal md %1gdal
xcopy /s /y %2packages\GDAL.Native.2.4.4\build\gdal\*.* %1gdal
xcopy /s /y %2packages\GDAL.Plugins.2.4.4\build\gdal\*.* %1gdal
cd %1gdal 
del /s /q gdal_GEOR.dll
del /s /q ogr_OCI.dll