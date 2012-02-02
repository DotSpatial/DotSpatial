@echo off
if "%1" == "setenv" goto setenv

%comspec% /k SDKShell.bat setenv %1
goto exit

:setenv
@echo Setting environment for using the GDAL and MapServer tools.

if "%2"=="hideoci" goto hideoci

set ocipath=0
set _path="%PATH:;=" "%"
for %%p in (%_path%) do if not "%%~p"=="" if exist %%~p\oci.dll set ocipath=1

if "%ocipath%"=="0" goto hideoci
@echo WARNING: If you encounter problems with missing oci libraries then type:
@echo   SDKShell hideoci
goto setenv2

:hideoci
@echo Hiding the OCI plugin library.
if not exist %CD%\bin\gdal\plugins-optional mkdir %CD%\bin\gdal\plugins-optional
if exist %CD%\bin\gdal\plugins\ogr_OCI.dll move %CD%\bin\gdal\plugins\ogr_OCI.dll %CD%\bin\gdal\plugins-optional\ogr_OCI.dll
if exist %CD%\bin\gdal\plugins\gdal_GEOR.dll move %CD%\bin\gdal\plugins\gdal_GEOR.dll %CD%\bin\gdal\plugins-optional\gdal_GEOR.dll

:setenv2
SET PATH=%CD%\bin;%CD%\bin;%CD%\bin\gdal\python\osgeo;%CD%\bin\proj\apps;%CD%\bin\gdal\apps;%CD%\bin\ms\apps;%CD%\bin\gdal\csharp;%CD%\bin\ms\csharp;%CD%\bin\curl;%PATH%
SET GDAL_DATA=%CD%\bin\gdal-data
SET GDAL_DRIVER_PATH=%CD%\bin\gdal\plugins
SET PYTHONPATH=%CD%\bin\gdal\python\osgeo
SET PROJ_LIB=%CD%\bin\proj\SHARE


:exit