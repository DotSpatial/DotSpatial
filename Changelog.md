# Change Log
All notable changes to this project will be documented in this file.

## [Unreleased]
### Added
- Switched to NTS/GeoAPI instead of DotSpatial.Topology (#633, #404) 
- Tag property in ActionItem (#338)
- Added property AppManager.BaseDirectory which allows to change base directory for plugins. (#758)
- Support for formatted ESRI projection files (#793)
- XML comments for publicly visible types and members
- "Add layer to Map"-checkbox to the ToolDialog (#147)
- CopySubset overloads with withAttributes parameters
- Jenks Natural Breaks support in categories binning.

### Changed
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

### Removed
- Removed DotSpatial.Topology assembly (#633)
- Removed obsolete methods\properties (#797)

### Fixed
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