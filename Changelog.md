# Change Log
All notable changes to this project will be documented in this file.

## [Unreleased]
### Added
- Switched to NTS/GeoAPI instead of DotSpatial.Topology (#633, #404) 
- Tag property in ActionItem (#338)
- Added property AppManager.BaseDirectory which allows to change base directory for plugins. (#758)
- Support for formatted ESRI projection files (#793)

### Changed
- GdalExtension: Updated to GDAL 1.1.11
- Demo and Apps projects should have build files (#120)

### Removed
- Removed DotSpatial.Topology assembly (#633)
- Removed obsolete methods\properties (#797)

### Fixed
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
- No outgoing dirEdge found #602
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