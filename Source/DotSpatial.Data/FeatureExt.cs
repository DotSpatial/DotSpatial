// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/16/2009 12:11:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Data;
using DotSpatial.NTSExtension;
using DotSpatial.Serialization;
using GeoAPI.Geometries;
using GeoAPI.Operation.Buffer;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Buffer;

namespace DotSpatial.Data
{
    /// <summary>
    /// Extension Methods for the Features
    /// </summary>
    public static class FeatureExt
    {
        #region Methods

        /// <summary>
        /// Generates a new feature from the buffer of this feature. The DataRow of
        /// the new feature will be null.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="distance">The double distance</param>
        /// <returns>An IFeature representing the output from the buffer operation</returns>
        public static IFeature Buffer(this IFeature self, double distance)
        {
            IGeometry g = self.Geometry.Buffer(distance);
            return new Feature(g);
        }

        /// <summary>
        /// Generates a new feature from the buffer of this feature. The DataRow of
        /// the new feature will be null.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="distance">The double distance</param>
        /// <param name="endCapStyle">The end cap style to use</param>
        /// <returns>An IFeature representing the output from the buffer operation</returns>
        public static IFeature Buffer(this IFeature self, double distance, EndCapStyle endCapStyle)
        {
            IGeometry g = self.Geometry.Buffer(
                distance,
                new BufferParameters
                {
                    EndCapStyle = endCapStyle
                });
            return new Feature(g);
        }

        /// <summary>
        /// Generates a new feature from the buffer of this feature. The DataRow of
        /// the new feature will be null.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="distance">The double distance</param>
        /// <param name="quadrantSegments">The number of segments to use to approximate a quadrant of a circle</param>
        /// <returns>An IFeature representing the output from the buffer operation</returns>
        public static IFeature Buffer(this IFeature self, double distance, int quadrantSegments)
        {
            IGeometry g = self.Geometry.Buffer(distance, quadrantSegments);
            return new Feature(g);
        }

        /// <summary>
        /// Generates a new feature from the buffer of this feature. The DataRow of
        /// the new feature will be null.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="distance">The double distance</param>
        /// <param name="quadrantSegments">The number of segments to use to approximate a quadrant of a circle</param>
        /// <param name="endCapStyle">The end cap style to use</param>
        /// <returns>An IFeature representing the output from the buffer operation</returns>
        public static IFeature Buffer(this IFeature self, double distance, int quadrantSegments, EndCapStyle endCapStyle)
        {
            IGeometry g = self.Geometry.Buffer(distance, quadrantSegments, endCapStyle);
            return new Feature(g);
        }

        /// <summary>
        /// Generates a buffer, but also adds the newly created feature to the specified output featureset.
        /// This will also compare the field names of the input featureset with the destination featureset.
        /// If a column name exists in both places, it will copy those values to the destination featureset.
        /// </summary>
        /// <param name="self">The feature to calcualate the buffer for.</param>
        /// <param name="distance">The double distance to use for calculating the buffer</param>
        /// <param name="destinationFeatureset">The output featureset to add this feature to and use
        /// as a reference for determining which data columns to copy.</param>
        /// <returns>The IFeature that represents the buffer feature.</returns>
        public static IFeature Buffer(this IFeature self, double distance, IFeatureSet destinationFeatureset)
        {
            IFeature f = Buffer(self, distance);
            destinationFeatureset.Features.Add(f);
            foreach (DataColumn dc in destinationFeatureset.DataTable.Columns)
            {
                if (self.DataRow[dc.ColumnName] != null)
                {
                    f.DataRow[dc.ColumnName] = self.DataRow[dc.ColumnName];
                }
            }

            return f;
        }

        /// <summary>
        /// Generates a buffer, but also adds the newly created feature to the specified output featureset.
        /// This will also compare the field names of the input featureset with the destination featureset.
        /// If a column name exists in both places, it will copy those values to the destination featureset.
        /// </summary>
        /// <param name="self">The feature to calcualate the buffer for.</param>
        /// <param name="distance">The double distance to use for calculating the buffer</param>
        /// <param name="endCapStyle">The end cap style to use</param>
        /// <param name="destinationFeatureset">The output featureset to add this feature to and use
        /// as a reference for determining which data columns to copy.</param>
        /// <returns>The IFeature that represents the buffer feature.</returns>
        public static IFeature Buffer(this IFeature self, double distance, EndCapStyle endCapStyle, IFeatureSet destinationFeatureset)
        {
            IFeature f = Buffer(self, distance, endCapStyle);
            destinationFeatureset.Features.Add(f);
            foreach (DataColumn dc in destinationFeatureset.DataTable.Columns)
            {
                if (self.DataRow[dc.ColumnName] != null)
                {
                    f.DataRow[dc.ColumnName] = self.DataRow[dc.ColumnName];
                }
            }

            return f;
        }

        /// <summary>
        /// Generates a buffer, but also adds the newly created feature to the specified output featureset.
        /// This will also compare the field names of the input featureset with the destination featureset.
        /// If a column name exists in both places, it will copy those values to the destination featureset.
        /// </summary>
        /// <param name="self">The feature to calcualate the buffer for.</param>
        /// <param name="distance">The double distance to use for calculating the buffer</param>
        /// <param name="quadrantSegments">The number of segments to use to approximate a quadrant of a circle</param>
        /// <param name="destinationFeatureset">The output featureset to add this feature to and use
        /// as a reference for determining which data columns to copy.</param>
        /// <returns>The IFeature that represents the buffer feature.</returns>
        public static IFeature Buffer(this IFeature self, double distance, int quadrantSegments, IFeatureSet destinationFeatureset)
        {
            IFeature f = Buffer(self, distance, quadrantSegments);
            destinationFeatureset.Features.Add(f);
            foreach (DataColumn dc in destinationFeatureset.DataTable.Columns)
            {
                if (self.DataRow[dc.ColumnName] != null)
                {
                    f.DataRow[dc.ColumnName] = self.DataRow[dc.ColumnName];
                }
            }

            return f;
        }

        /// <summary>
        /// Generates a buffer, but also adds the newly created feature to the specified output featureset.
        /// This will also compare the field names of the input featureset with the destination featureset.
        /// If a column name exists in both places, it will copy those values to the destination featureset.
        /// </summary>
        /// <param name="self">The feature to calcualate the buffer for.</param>
        /// <param name="distance">The double distance to use for calculating the buffer</param>
        /// <param name="quadrantSegments">The number of segments to use to approximate a quadrant of a circle</param>
        /// <param name="endCapStyle">The end cap style to use</param>
        /// <param name="destinationFeatureset">The output featureset to add this feature to and use
        /// as a reference for determining which data columns to copy.</param>
        /// <returns>The IFeature that represents the buffer feature.</returns>
        public static IFeature Buffer(this IFeature self, double distance, int quadrantSegments, EndCapStyle endCapStyle, IFeatureSet destinationFeatureset)
        {
            IFeature f = Buffer(self, distance, quadrantSegments, endCapStyle);
            destinationFeatureset.Features.Add(f);
            foreach (DataColumn dc in destinationFeatureset.DataTable.Columns)
            {
                if (self.DataRow[dc.ColumnName] != null)
                {
                    f.DataRow[dc.ColumnName] = self.DataRow[dc.ColumnName];
                }
            }

            return f;
        }

        /// <summary>
        /// If the geometry for this shape was loaded from a file, this contains the size
        /// of this shape in 16-bit words as per the Esri Shapefile specification.
        /// </summary>
        /// <param name="self">This feature.</param>
        /// <returns>The content length.</returns>
        public static int ContentLength(this IFeature self)
        {
            return self.ShapeIndex?.ContentLength ?? 0;
        }

        /// <summary>
        /// Calculates a new feature that has a geometry that is the convex hull of this feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <returns>A new feature that is the convex hull of this feature.</returns>
        public static IFeature ConvexHull(this IFeature self)
        {
            return new Feature(self.Geometry.ConvexHull());
        }

        /// <summary>
        /// Calculates a new feature that has a geometry that is the convex hull of this feature.
        /// This also copies the attributes that are shared between this featureset and the
        /// specified destination featureset, and adds this feature to the destination featureset.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="destinationFeatureset">The destination featureset to add this feature to</param>
        /// <returns>The newly created IFeature</returns>
        public static IFeature ConvexHull(this IFeature self, IFeatureSet destinationFeatureset)
        {
            IFeature f = ConvexHull(self);
            destinationFeatureset.Features.Add(f);
            foreach (DataColumn dc in destinationFeatureset.DataTable.Columns)
            {
                if (self.DataRow[dc.ColumnName] != null)
                {
                    f.DataRow[dc.ColumnName] = self.DataRow[dc.ColumnName];
                }
            }

            return f;
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the difference between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric difference between this feature and the specified feature.</returns>
        public static IFeature Difference(this IFeature self, IGeometry other)
        {
            if (other == null) return self.Copy();

            IGeometry g = self.Geometry.Difference(other);
            if (g == null || g.IsEmpty) return null;

            return new Feature(g);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the difference between this feature and the specified feature.
        /// This also copies the attributes that are shared between this featureset and the
        /// specified destination featureset, and adds this feature to the destination featureset.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <param name="destinationFeatureSet">The featureset to add the new feature to.</param>
        /// <param name="joinType">This clarifies the overall join strategy being used with regards to attribute fields.</param>
        /// <returns>A new feature that is the geometric difference between this feature and the specified feature.</returns>
        public static IFeature Difference(this IFeature self, IFeature other, IFeatureSet destinationFeatureSet, FieldJoinType joinType)
        {
            IFeature f = Difference(self, other.Geometry);
            if (f != null)
            {
                UpdateFields(self, other, f, destinationFeatureSet, joinType);
            }

            return f;
        }

        /// <summary>
        /// Creates a new GML string describing the location of this point.
        /// </summary>
        /// <param name="self">This feature.</param>
        /// <returns>A String representing the Geographic Markup Language version of this point</returns>
        public static string ExportToGml(this IFeature self)
        {
            var geo = self.Geometry as Geometry;
            return geo == null ? string.Empty : geo.ToGMLFeature().ToString();
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the intersection between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric intersection between this feature and the specified feature.</returns>
        public static IFeature Intersection(this IFeature self, IGeometry other)
        {
            IGeometry g = self.Geometry.Intersection(other);
            if (g == null || g.IsEmpty) return null;

            return new Feature(g);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the intersection between this feature and the specified feature.
        /// This also copies the attributes that are shared between this featureset and the
        /// specified destination featureset, and adds this feature to the destination featureset.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <param name="destinationFeatureSet">The featureset to add the new feature to.</param>
        /// <param name="joinType">This clarifies the overall join strategy being used with regards to attribute fields.</param>
        /// <returns>A new feature that is the geometric intersection between this feature and the specified feature.</returns>
        public static IFeature Intersection(this IFeature self, IFeature other, IFeatureSet destinationFeatureSet, FieldJoinType joinType)
        {
            IFeature f = Intersection(self, other.Geometry);
            if (f != null)
            {
                UpdateFields(self, other, f, destinationFeatureSet, joinType);
            }

            return f;
        }

        /// <summary>
        /// Gets the integer number of parts associated with this feature.
        /// </summary>
        /// <param name="self">This feature.</param>
        /// <returns>The number of parts.</returns>
        public static int NumParts(this IFeature self)
        {
            int count = 0;

            if (self.FeatureType != FeatureType.Polygon) return self.Geometry.NumGeometries;

            IPolygon p = self.Geometry as IPolygon;
            if (p == null)
            {
                // we have a multi-polygon situation
                for (int i = 0; i < self.Geometry.NumGeometries; i++)
                {
                    p = self.Geometry.GetGeometryN(i) as IPolygon;
                    if (p == null) continue;

                    count += 1; // Shell
                    count += p.NumInteriorRings; // Holes
                }
            }
            else
            {
                // The feature is a polygon, not a multi-polygon
                count += 1; // Shell
                count += p.NumInteriorRings; // Holes
            }

            return count;
        }

        /// <summary>
        /// An index value that is saved in some file formats.
        /// </summary>
        /// <param name="self">This feature.</param>
        /// <returns>The record number.</returns>
        public static int RecordNumber(this IFeature self)
        {
            return self.ShapeIndex?.RecordNumber ?? -1;
        }

        /// <summary>
        /// Rotates the BasicGeometry of the feature by the given radian angle around the given Origin.
        /// </summary>
        /// <param name="self">This feature.</param>
        /// <param name="origin">The coordinate the feature gets rotated around.</param>
        /// <param name="radAngle">The rotation angle in radian.</param>
        public static void Rotate(this IFeature self, Coordinate origin, double radAngle)
        {
            IGeometry geo = self.Geometry.Copy();
            geo.Rotate(origin, radAngle);
            self.Geometry = geo;
            self.UpdateEnvelope();
        }

        /// <summary>
        /// When a shape is loaded from a Shapefile, this will identify whether M or Z values are used
        /// and whether or not the shape is null.
        /// </summary>
        /// <param name="self">This feature.</param>
        /// <returns>The shape type.</returns>
        public static ShapeType ShapeType(this IFeature self)
        {
            return self.ShapeIndex?.ShapeType ?? Data.ShapeType.NullShape;
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the symmetric difference between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric symmetric difference between this feature and the specified feature.</returns>
        public static IFeature SymmetricDifference(this IFeature self, IGeometry other)
        {
            IGeometry g = self.Geometry.SymmetricDifference(other);
            if (g == null || g.IsEmpty) return null;

            return new Feature(g);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the symmetric difference between this feature and the specified feature.
        /// This also copies the attributes that are shared between this featureset and the
        /// specified destination featureset, and adds this feature to the destination featureset.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <param name="destinationFeatureSet">The featureset to add the new feature to.</param>
        /// <param name="joinType">This clarifies the overall join strategy being used with regards to attribute fields.</param>
        /// <returns>A new feature that is the geometric symmetric difference between this feature and the specified feature.</returns>
        public static IFeature SymmetricDifference(this IFeature self, IFeature other, IFeatureSet destinationFeatureSet, FieldJoinType joinType)
        {
            IFeature f = SymmetricDifference(self, other.Geometry);
            if (f != null)
            {
                UpdateFields(self, other, f, destinationFeatureSet, joinType);
            }

            return f;
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the union between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric union between this feature and the specified feature.</returns>
        public static IFeature Union(this IFeature self, IGeometry other)
        {
            IGeometry g = self.Geometry.Union(other);
            if (g == null || g.IsEmpty) return null;

            return new Feature(g);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the union between this feature and the specified feature.
        /// This also copies the attributes that are shared between this featureset and the
        /// specified destination featureset, and adds this feature to the destination featureset.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <param name="destinationFeatureSet">The featureset to add the new feature to.</param>
        /// <param name="joinType">Clarifies how the attributes should be handled during the union</param>
        /// <returns>A new feature that is the geometric symmetric difference between this feature and the specified feature.</returns>
        public static IFeature Union(this IFeature self, IFeature other, IFeatureSet destinationFeatureSet, FieldJoinType joinType)
        {
            IFeature f = Union(self, other.Geometry);
            if (f != null)
            {
                UpdateFields(self, other, f, destinationFeatureSet, joinType);
            }

            return f;
        }

        // This handles the slightly complex business of attribute outputs.
        private static void UpdateFields(IFeature self, IFeature other, IFeature result, IFeatureSet destinationFeatureSet, FieldJoinType joinType)
        {
            if (destinationFeatureSet.FeatureType != result.FeatureType) return;

            destinationFeatureSet.Features.Add(result);
            if (joinType == FieldJoinType.None) return;

            if (joinType == FieldJoinType.All)
            {
                // This overly complex mess is concerned with preventing duplicate field names.
                // This assumes that numbers were appended when duplicates occured, and will
                // repeat the process here to establish one set of string names and values.
                Dictionary<string, object> mappings = new Dictionary<string, object>();
                foreach (DataColumn dc in self.ParentFeatureSet.DataTable.Columns)
                {
                    string name = dc.ColumnName;
                    int i = 1;
                    while (mappings.ContainsKey(name))
                    {
                        name = dc.ColumnName + i;
                        i++;
                    }

                    mappings.Add(name, self.DataRow[dc.ColumnName]);
                }

                foreach (DataColumn dc in other.ParentFeatureSet.DataTable.Columns)
                {
                    string name = dc.ColumnName;
                    int i = 1;
                    while (mappings.ContainsKey(name))
                    {
                        name = dc.ColumnName + i;
                        i++;
                    }

                    mappings.Add(name, other.DataRow[dc.ColumnName]);
                }

                foreach (KeyValuePair<string, object> pair in mappings)
                {
                    if (!result.DataRow.Table.Columns.Contains(pair.Key))
                    {
                        result.DataRow.Table.Columns.Add(pair.Key);
                    }

                    result.DataRow[pair.Key] = pair.Value;
                }
            }

            if (joinType == FieldJoinType.LocalOnly)
            {
                foreach (DataColumn dc in destinationFeatureSet.DataTable.Columns)
                {
                    if (self.DataRow.Table.Columns.Contains(dc.ColumnName))
                    {
                        result.DataRow[dc.ColumnName] = self.DataRow[dc.ColumnName];
                    }
                }
            }

            if (joinType == FieldJoinType.ForeignOnly)
            {
                foreach (DataColumn dc in destinationFeatureSet.DataTable.Columns)
                {
                    if (other.DataRow[dc.ColumnName] != null)
                    {
                        result.DataRow[dc.ColumnName] = self.DataRow[dc.ColumnName];
                    }
                }
            }
        }

        #endregion
    }
}