// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// Extension Methods for the Features
    /// </summary>
    public static class FeatureExt
    {
        /// <summary>
        /// Calculates the area of the geometric portion of this feature.  This is 0 unless the feature
        /// is a polygon, or multi-polygon.
        /// </summary>
        /// <param name="self">The feature to test</param>
        /// <returns>The double valued area</returns>
        public static double Area(this IFeature self)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Area;
        }

        /// <summary>
        /// Generates a new feature from the buffer of this feature.  The DataRow of
        /// the new feature will be null.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="distance">The double distance</param>
        /// <returns>An IFeature representing the output from the buffer operation</returns>
        public static IFeature Buffer(this IFeature self, double distance)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Buffer(distance);
            return new Feature(g);
        }

        /// <summary>
        /// Generates a new feature from the buffer of this feature.  The DataRow of
        /// the new feature will be null.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="distance">The double distance</param>
        /// <param name="endCapStyle">The end cap style to use</param>
        /// <returns>An IFeature representing the output from the buffer operation</returns>
        public static IFeature Buffer(this IFeature self, double distance, BufferStyle endCapStyle)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Buffer(distance, endCapStyle);
            return new Feature(g);
        }

        /// <summary>
        /// Generates a new feature from the buffer of this feature.  The DataRow of
        /// the new feature will be null.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="distance">The double distance</param>
        /// <param name="quadrantSegments">The number of segments to use to approximate a quadrant of a circle</param>
        /// <returns>An IFeature representing the output from the buffer operation</returns>
        public static IFeature Buffer(this IFeature self, double distance, int quadrantSegments)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Buffer(distance, quadrantSegments);
            return new Feature(g);
        }

        /// <summary>
        /// Generates a new feature from the buffer of this feature.  The DataRow of
        /// the new feature will be null.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="distance">The double distance</param>
        /// <param name="quadrantSegments">The number of segments to use to approximate a quadrant of a circle</param>
        /// <param name="endCapStyle">The end cap style to use</param>
        /// <returns>An IFeature representing the output from the buffer operation</returns>
        public static IFeature Buffer(this IFeature self, double distance, int quadrantSegments, BufferStyle endCapStyle)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Buffer(distance, quadrantSegments, endCapStyle);
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
        public static IFeature Buffer(this IFeature self, double distance, BufferStyle endCapStyle, IFeatureSet destinationFeatureset)
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
        public static IFeature Buffer(this IFeature self, double distance, int quadrantSegments, BufferStyle endCapStyle, IFeatureSet destinationFeatureset)
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
        /// Returns a feature constructed from the centroid of this feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <returns>An IFeature that is also a point geometry</returns>
        public static IFeature Centroid(this IFeature self)
        {
            return new Feature(Geometry.FromBasicGeometry(self.BasicGeometry).Centroid);
        }

        /// <summary>
        /// Gets a boolean that is true if this feature contains the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if this feature contains the other feature</returns>
        public static bool Contains(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Contains(Geometry.FromBasicGeometry(other.BasicGeometry));
        }

        /// <summary>
        /// Gets a boolean that is true if this feature contains the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if this feature contains the other feature</returns>
        public static bool Contains(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Contains(other);
        }

        /// <summary>
        /// Calculates a new feature that has a geometry that is the convex hull of this feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <returns>A new feature that is the convex hull of this feature.</returns>
        public static IFeature ConvexHull(this IFeature self)
        {
            return new Feature(Geometry.FromBasicGeometry(self.BasicGeometry).ConvexHull());
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
        /// Gets a boolean that is true if the geometry of this feature is covered by the geometry
        /// of the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns>Boolean, true if this feature is covered by the specified feature</returns>
        public static bool CoveredBy(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).CoveredBy(Geometry.FromBasicGeometry(other));
        }

        /// <summary>
        /// Gets a boolean that is true if the geometry of this feature is covered by the geometry
        /// of the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns>Boolean, true if this feature is covered by the specified feature</returns>
        public static bool CoveredBy(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).CoveredBy((other));
        }

        /// <summary>
        /// Gets a boolean that is true if the geometry of this feature covers the geometry
        /// of the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns>Boolean, true if this feature covers the specified feature</returns>
        public static bool Covers(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Covers(Geometry.FromBasicGeometry(other));
        }

        /// <summary>
        /// Gets a boolean that is true if the geometry of this feature covers the geometry
        /// of the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns>Boolean, true if this feature covers the specified feature</returns>
        public static bool Covers(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Covers(other);
        }

        /// <summary>
        /// Gets a boolean that is true if the geometry of this feature crosses the geometry
        /// of the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns>Boolean, true if this feature crosses the specified feature</returns>
        public static bool Crosses(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Crosses(Geometry.FromBasicGeometry(other));
        }

        /// <summary>
        /// Gets a boolean that is true if the geometry of this feature crosses the geometry
        /// of the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns>Boolean, true if this feature crosses the specified feature</returns>
        public static bool Crosses(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Crosses(other);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the difference between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric difference between this feature and the specified feature.</returns>
        public static IFeature Difference(this IFeature self, IFeature other)
        {
            if (other == null) return self.Copy();
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Difference(Geometry.FromBasicGeometry(other.BasicGeometry));
            if (g == null) return null;
            if (g.IsEmpty) return null;
            return new Feature(g);
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
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Difference(other);
            if (g == null) return null;
            if (g.IsEmpty) return null;
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
            IFeature f = Difference(self, other);
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

        /// <summary>
        /// Gets a boolean that is true if the geometry of this feature is disjoint with the geometry
        /// of the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns>Boolean, true if this feature is disjoint with the specified feature</returns>
        public static bool Disjoint(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Disjoint(Geometry.FromBasicGeometry(other));
        }

        /// <summary>
        /// Gets a boolean that is true if the geometry of this feature is disjoint with the geometry
        /// of the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns>Boolean, true if this feature is disjoint with the specified feature</returns>
        public static bool Disjoint(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Disjoint(other);
        }

        /// <summary>
        /// Distance between features.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns></returns>
        public static double Distance(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Distance(Geometry.FromBasicGeometry(other));
        }

        /// <summary>
        /// Gets a boolean that is true if the geometry of this feature is disjoint with the geometry
        /// of the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The feature to compare this feature to</param>
        /// <returns>Boolean, true if this feature is disjoint with the specified feature</returns>
        public static double Distance(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Distance(other);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the intersection between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric intersection between this feature and the specified feature.</returns>
        public static IFeature Intersection(this IFeature self, IFeature other)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Intersection(Geometry.FromBasicGeometry(other.BasicGeometry));
            if (g == null) return null;
            if (g.IsEmpty) return null;
            return new Feature(g);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the intersection between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric intersection between this feature and the specified feature.</returns>
        public static IFeature Intersection(this IFeature self, IGeometry other)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Intersection(other);
            if (g == null) return null;
            if (g.IsEmpty) return null;
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
            IFeature f = Intersection(self, other);
            if (f != null)
            {
                UpdateFields(self, other, f, destinationFeatureSet, joinType);
            }
            return f;
        }

        /// <summary>
        /// Gets a boolean that is true if this feature intersects the other feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if the two IFeatures intersect</returns>
        public static bool Intersects(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Intersects(Geometry.FromBasicGeometry(other.BasicGeometry));
        }

        /// <summary>
        /// Gets a boolean that is true if this feature intersects the other feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if the two IFeatures intersect</returns>
        public static bool Intersects(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Intersects(other);
        }

        /// <summary>
        /// This tests the current feature to see if the geometry intersects with the specified
        /// coordinate.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="coordinate">The coordinate</param>
        /// <returns>Boolean if the coordinate intersects with this feature</returns>
        public static bool Intersects(this IFeature self, Coordinate coordinate)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Intersects(new Point(coordinate));
        }

        /// <summary>
        /// Gets a boolean that is true if this feature is within the specified distance of the other feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <param name="distance">The double distance criteria</param>
        /// <returns>Boolean, true if the other feature is within the specified distance of this feature</returns>
        public static bool IsWithinDistance(this IFeature self, IFeature other, double distance)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).IsWithinDistance(Geometry.FromBasicGeometry(other.BasicGeometry), distance);
        }

        /// <summary>
        /// Gets a boolean that is true if this feature is within the specified distance of the other feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <param name="distance">The double distance criteria</param>
        /// <returns>Boolean, true if the other feature is within the specified distance of this feature</returns>
        public static bool IsWithinDistance(this IFeature self, IGeometry other, double distance)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).IsWithinDistance(other, distance);
        }

        /// <summary>
        /// Gets a boolean that is true if this feature overlaps the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if the two IFeatures overlap</returns>
        public static bool Overlaps(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Overlaps(Geometry.FromBasicGeometry(other.BasicGeometry));
        }

        /// <summary>
        /// Gets a boolean that is true if this feature overlaps the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if the two IFeatures overlap</returns>
        public static bool Overlaps(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Overlaps(other);
        }

        /// <summary>
        /// Gets a boolean that is true if the relationship between this feature and the other feature
        /// matches the relationship matrix specified by other
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <param name="intersectionPattern">The string relationship pattern to test</param>
        /// <returns>Boolean, true if the other feature's relationship to this feature matches the relate expression.</returns>
        public static bool Relates(this IFeature self, IFeature other, string intersectionPattern)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Relate(Geometry.FromBasicGeometry(other.BasicGeometry), intersectionPattern);
        }

        /// <summary>
        /// Gets a boolean that is true if the relationship between this feature and the other feature
        /// matches the relationship matrix specified by other
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <param name="intersectionPattern">The string relationship pattern to test</param>
        /// <returns>Boolean, true if the other feature's relationship to this feature matches the relate expression.</returns>
        public static bool Relates(this IFeature self, IGeometry other, string intersectionPattern)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Relate(other, intersectionPattern);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the symmetric difference between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric symmetric difference between this feature and the specified feature.</returns>
        public static IFeature SymmetricDifference(this IFeature self, IFeature other)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).SymmetricDifference(Geometry.FromBasicGeometry(other.BasicGeometry));
            if (g == null) return null;
            if (g.IsEmpty) return null;
            return new Feature(g);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the symmetric difference between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric symmetric difference between this feature and the specified feature.</returns>
        public static IFeature SymmetricDifference(this IFeature self, IGeometry other)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).SymmetricDifference(other);
            if (g == null) return null;
            if (g.IsEmpty) return null;
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
            IFeature f = SymmetricDifference(self, other);
            if (f != null)
            {
                UpdateFields(self, other, f, destinationFeatureSet, joinType);
            }
            return f;
        }

        /// <summary>
        /// Gets a boolean that is true if this feature touches the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if the two IFeatures touch</returns>
        public static bool Touches(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Touches(Geometry.FromBasicGeometry(other.BasicGeometry));
        }

        /// <summary>
        /// Gets a boolean that is true if this feature touches the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if the two IFeatures touch</returns>
        public static bool Touches(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Touches(other);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the union between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric union between this feature and the specified feature.</returns>
        public static IFeature Union(this IFeature self, IFeature other)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Union(Geometry.FromBasicGeometry(other.BasicGeometry));
            if (g == null) return null;
            if (g.IsEmpty) return null;
            return new Feature(g);
        }

        /// <summary>
        /// Creates a new Feature that has a geometry that is the union between this feature and the specified feature.
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to compare to.</param>
        /// <returns>A new feature that is the geometric union between this feature and the specified feature.</returns>
        public static IFeature Union(this IFeature self, IGeometry other)
        {
            IGeometry g = Geometry.FromBasicGeometry(self.BasicGeometry).Union(other);
            if (g == null) return null;
            if (g.IsEmpty) return null;
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
            IFeature f = Union(self, other);
            if (f != null)
            {
                UpdateFields(self, other, f, destinationFeatureSet, joinType);
            }
            return f;
        }

        /// <summary>
        /// Gets a boolean that is true if this feature is within the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if this feature is within the specified feature</returns>
        public static bool Within(this IFeature self, IFeature other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Within(Geometry.FromBasicGeometry(other.BasicGeometry));
        }

        /// <summary>
        /// Gets a boolean that is true if this feature is within the specified feature
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="other">The other feature to test</param>
        /// <returns>Boolean, true if this feature is within the specified feature</returns>
        public static bool Within(this IFeature self, IGeometry other)
        {
            return Geometry.FromBasicGeometry(self.BasicGeometry).Within(other);
        }
    }
}