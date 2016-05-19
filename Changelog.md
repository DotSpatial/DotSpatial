# Change Log
All notable changes to this project will be documented in this file.

## [Unreleased]
### Added
- Switched to NTS/GeoAPI instead of DotSpatial.Topology (#633, #404) 
- Tag property in ActionItem (#338)

### Changed
- GdalExtension: Updated to GDAL 1.1.11

### Removed
- DotSpatial.Topology assembly (#633)

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
