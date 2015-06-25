:: Post-build bat, which copies GDAL native libraries into output directory
if not exist %1gdal md %1gdal
xcopy /s /y %2packages\GDAL.Native.1.9.2\gdal\*.* %1gdal
xcopy /s /y %2packages\GDAL.Plugins.1.9.2\gdal\*.* %1gdal
cd %1gdal 
del /s /q gdal_GEOR.dll
del /s /q ogr_OCI.dll