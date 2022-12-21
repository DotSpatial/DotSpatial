# Change Log
All notable changes to this project will be documented in this file.

## [Unreleased]

### Added
- A plug-in that converts coordinates from one projection to another (#1469)
- Added a coordinate system type field to ProjectionInfo (#1473)

### Changed

### Fixed

## V4.0

### Changed
- Switched to VS2022
- Switched to .Net 6

### Fixed
- Fixed angle calculation for second and third quadrant (#1405) 

## V3.0.1

### Added
- Label Setup dialog can now accept any font size (#1434)

### Fixed
- Error when using wildcards in Symbology FilterExpression (#1160)

## V3.0

### Added
- JGD2011, EPSG from 6669 to 6687 (#1262)
- Feature of determining the delta azimuth of three consecutive points (#1360)
- InterSectionTool (#945) 

### Changed
- Switched to VS2019
- Switched to .Net Framework 4.7.2
- Updated StyleCop.Analyzers to 1.1.118
- Switched from DotSpatial.NetTopologySuite/DotSpatial.GeoAPI to NetTopologySuite 2.4.0
- Updated NUnit to 3.13.2

### Fixed
- Bug in extent calculation in WebMap plugin (#1367)
- Issues related to the Plate Carree projection (#1078)
- Multiple Changes on handling of GsdNadTable (#1059) 
- Naming of UTMNad1983 coordinate systems (#1423)
- Failed to load spatialite layer (#1415) 

## V2.0.1

Be aware that code written for 1.9 will not work out of the box because DotSpatial.Topology was replaced by DotSpatial.GeoAPI and DotSpatial.NetTopologySuite (#786). Have a look at the [Wiki](https://github.com/DotSpatial/DotSpatial/wiki/Switching-from-DotSpatial-1.9-to-2.0) for more information.

### Added
- Aliases to Satellite of DotSpatial.Positioning
- Switched to NTS/GeoAPI instead of DotSpatial.Topology (#633, #404, #786) 
- Tag property in ActionItem (#338)
- Added property AppManager.BaseDirectory which allows to change base directory for plugins. (#758)
- Support for formatted ESRI projection files (#793)
- XML comments for publicly visible types and members
- "Add layer to Map"-checkbox to the ToolDialog (#147)
- CopySubset overloads with withAttributes parameters
- Jenks Natural Breaks support in categories binning.
- Test that checks correct creation of GpggkSentence objects from string
- Constructing Shapefiles in memory as single zip archives (#885)
- StyleCop.Analyzers to enforce a set of style and consistency rules
- Chm file with DotSpatial API documentation
- Example for Buffer.AddBuffer method (#1002)
- Legend.UseLegendForSelection property to be able to decide whether the legend should be used for selection or not. (#1008)
- Possibility to drag layers out of their group into the parent group (in legend) (#1008)
- Clear parameter to Select function to speed up drawing (#1024)
- LayoutControl.InitialOpenFileDirectory property that allows to set the folder that is shown in the OpenFileDialog that is used to open an existing layout
- FeatureLayer.Snappable to indicate whether the layer can be used for snapping
- The possibility to draw linestrings which are inside a geometry collection (#1061)
- The possibility to use static methods to deserialize objects that were serialized to a dspx file and can't be deserialized correctly via their class constructor (FeatureSet, MapSelfLoadGroup, MapSelfLoadLayers from GdalExtension, SpatiaLiteFeatureSet) (#1061)
- Default mouse cursor button in layout insert toolbar
- A function to get a reprojected clone of a featureset
- Auto display children of MapGroup is now an option of the MapFrame

### Changed
- Switched to VS2017 and C#7
- Switched to .Net Framework 4.5.2 (#1083)
- GdalExtension: Updated to GDAL 1.1.11
- Demo and Apps projects should have build files (#120)
- ExtensionManager & HideReleaseFromEndUser (#798)
- Moved localizeable strings to resource files
- Renamed FilterCollectionEM to FeatureSelectionExt because it contains extension methods for IFeatureSelection
- Move FeatureSet.Filename / FilePath to DataSet (#821)
- Changed Filename to absolute path so it stays the same even if CurrentDirectory is changed
- Renamed IntervalMethod.Quantile to IntervalMethod.EqualFrequency for better consistency.
- IHeaderControl.Add() now returns object which represents added GUI item.
- Added authority and authority code as optional parameters to method ProjectionInfo.FromProj4String.
- Update proj4 strings to EPSG db 9.0 (#870)
- ShapeFile Numeric columns now loaded into double instead of string for up to 15 decimal digits (#893)
- DS Feature refactorings (#906)
- LegendText ReadOnly (#750)
- Made Shapefile class abstract, because we already have FeatureSet for creating unspecified Shapefiles (#890)
- Moved MapFrame extension methods to Group (#1008)
- Drawing functions so selected features are drawn on top (#897)
- ShapeEditors AddFeature and MoveVertex functions, so they snap only to the layers that allow snapping
- ShapeEditors SnapSettingsDialog to allow the users to select the layers the editor functions may snap to
- If a dxf file contains points, lines and polygons at the same time, the dxf file gets added to the map as a group that contains one layer for points, one for lines and one for polygons (#1061)
- If a dxf file contains only a single feature type the dxf file gets added to the map as a single layer with the feature type it contains (#1061)
- dxf files get loaded with their styles (#1061)
- Show buttons from layout toolbars as checked while their function is active
- replaced ContextMenu by ContextMenuStrip inside Legend, so we don't have to draw the images shown in the ContextMenu ourselves (#1069)
- changed the background color of the LayerDialog and TabControlDialog tabs to Control so they have the same background color as the user controls they contain (#1069)

### Removed
- Removed DotSpatial.Topology assembly (#633)
- Removed obsolete methods\properties (#797)
- Removed DotSpatial.Mono assembly. Mono helper now is in DotSpatial.Data assembly.
- Removed unnecessary methods in LayoutControl

### Fixed
- Plate Carree projection (EPSG: 32662) not found (#1078)
- Satellite's missing properties (#958)
- Parameters for the Austrian Bundesmeldenetz in DotSpatial.Projections.ProjectedCategories.NationalGrids are incorrect (#855)
- Raster extent shifts from correct extent (#725)
- Inconsistent use of affine coefficients (#822)
- Fixed the shift in x-coordinate when reprojecting from WGS84 to LAEA (#815)
- Fixed LAEA reprojected y coordinate that resulted in n.def (#813)
- ShapeReader skipping one entry when switching the page (#774)
- DotSpatial.Projections dll file is very big (#27)
- ParseEsriString leaves datums.xml open (#713)
- MultiPolygon shapefile with holes with nested part not read correctly (#779)
- Exception when calling Feature.Difference (#765)
- Coordinate getHashCode() incorrect (#731)
- IsSimple returns true without checking (#656)
- Polygonizer StackOverflowException (#509)
- Topology: Area for a Polygon with Holes (#16)
- Unhandled exception in ScaleBarPlugin (#789)
- No outgoing dirEdge found (#602)
- WKBWriter adds 4 null bytes? (#475)
- Exception when calling feature.Intersects(otherFeature) (#746)
- Remove Map Frame context menu doesn't work (#237)
- ReprojectPoints() not working for more than 1 point for Stereographic transforms. (#781)
- Projection ETRS1989LAEA (#387)
- Problem with LAEA projection (#568)
- Intersection Issue (#785) 
- SqlServer raise WKB is not valid format. (#499)
- Error inside ClipPolygonWithLine that caused an exception because the output filename wasn't set
- Incorrect toWgs84 initialisation in Proj4DatumName set accesor (#732)
- Unhandled exception when zoom to empty Group (#796)
- Excel Join, wrong OleDbAdaptater. (#250)
- Select Layer in the MapFrame (#743)
- Measure Plugin - unhandled exception (#792)
- Error that caused the last value to be missing from the RasterSymbolizer.Scheme
- Bug in tool Reproject Features (#761)
- Clone a featureset with CopyFeatures throws exception (#780)
- Make sure Feature.Copy doesn't throw an error if the Feature doesn't have a ParentFeatureSet
- Symbology.IndexSelection.RemoveRegion is working slow (#718)
- Exception in WebMap plugin if "espg"-substring is lower-case (#777)
- Problem in rendering ECW images (#824)
- Polygon layer - scheme partially resets when open properties window (#842)
- Index Glitch in GdalRaster with Very Large Rasters (#849)
- Map Frame Dragging (#772)
- Fixed reprojection errors between RT90 and SWEREF99, caused by missing TOWGS parameters in transformation attributes (#861)
- Incorrect reprojection between RT90 2,5 gon V and WGS84 (#618)
- SelectByAttributes - Unhandled Exception (#253)
- AttributeTable.Columns.Add (#303)
- Spheroid International_1924 in EuropeanDatum1950UTMZone30N coordinate system (#623)
- AttributeTable - Can't handle all field types, fails to handle null values properly (#880)
- Attributes now preserved from input to output when performing ClipPolygonWithPolygon tool (#892)
- FeatureSet.Open(".shp") throws wrong error if .shx is missing (#903)
- Wrong Expression at Expression Editor mess up Layer Feature Set. (#904)
- Incorrect selection of polygons with Holes (#905)
- ExtendBuffer Map property fixed (#661)
- Polygon Hatch Style is not displayed (#851)
- Is this a bug in DotSpatial.WebControls? (#496)
- NmeaSentence.ParseDilution produces an exception when an nmea string with dilution of precision not greater than 0 is used. (#909)
- Errors in SetPropertiesFromSentence functions of GpggkSentence, GpgsvSentence, GprmcSentence
- Allow feature.DataRow = null in Shape-constructor (#917)
- DS uses InRamImage although GDAL provider was selected (#931)
- Disable editing in identify window (#930)
- ArcMap does show M and Z as NaN (#935)
- Recognize NullShapes not only for polygon / line shapes when in IndexMode, but also in !IndexMode and for points and multipoints (#890)
- Legends are in an opposite order in the map legend and in the Print Preview. (#970)
- FeatureLayer.ClearSelection / SelectAll only work when FeatureLayer is selected in Legend (#659)
- Using the Identifier tool isn't intuitive (#418)
- Selection Options (#283)
- Legend drag line so it doesn't look as if you can move a layer in between categories (#1008)
- Legend selection to be able to select features of a category (#1008)
- Some errors in SetSelectable plugin (#1008)
- Crash when attempting to use a serial GPS device on Mono
- Clear the selection inside FeatureLayer.RemoveSelectedFeatures so the removed features are no longer contained when IFeatureSet.FeatureRemoved is raised
- In InRamImageData.Open don't draw the image unscaled because this can cause the image not to be drawn
- FeatureTypeFromGeometryType Method updated to work with GeometryCollection (#1044)
- The SpatiaLite plugin to be able to load SpatiaLite databases of version 4 and higher (#1061)
- WebMap-Plugin fails fetching tiles for specific WMS (#1074)
- Plugins/WFSClient: Feature fetching fails on systems w NumberFormatInfo.NumberDecimalSeparator != '.' (#1081)
- showMargin can be checked as soon as layoutControl is not null (#1091) 
- don't assign the 'EndRow' property to itself in BinaryRaster.OpenWindow (#1089)
- assign "D_ITRF_1997" to ITRF1997.GeographicInfo.Datum.Name instead of ITRF1997.GeographicInfo.Name because this is the name of the datum and not the GeographicInfo (#1090)
- Update Brutile version in Webmap? (#800)
- SetSelectable Plugin Not Included in Release Build (#1106)
- Error on OpenFile with special SPHEROID string (#1142)
- Shape File Saves with Wrong DataTypes (#1005)
- Calculation of translation param in InRamImageData.GetBitmap is defective (#1203)
- MapImageLayer not drawn correctly on print (#1137)
- MapRasterLayer not drawn correctly on print
- Create Categories for symbology is inconsistent with large datasets (#1242)
- Geographic projections now have a Name property
- Drawing order of labels is given precedence from top to bottom layer (#1226)
- DotSpatial Projection with GridShift is Extremely Slow for NAD27 (#1333)
- Potential Bug in RasterBoundsExt class CellsContainingExtent(...) method (#1332)
- Potential bug in EnvelopeExt (and ExtentExt) class Reproportion(...) method (#1326)
- Bug in AzimuthalEquidistant class (#1342)
- Bug in moving legend items (#1368)
- Bug in ExtentExt.Reproportion discussed in #1351 (#1370)
- False polygon outlines are drawn at map boundary when not in edit mode (#1474)
- Non UI Projects as .netstandard2.0 libraries (#1479 )
