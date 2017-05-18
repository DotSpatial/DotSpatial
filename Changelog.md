# Change Log
All notable changes to this project will be documented in this file.

## [Unreleased]

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

### Changed
- Switched to VS2015 and C#6
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

### Removed
- Removed DotSpatial.Topology assembly (#633)
- Removed obsolete methods\properties (#797)
- Removed DotSpatial.Mono assembly. Mono helper now is in DotSpatial.Data assembly.

### Fixed
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