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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/16/2009 2:15:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// FeatureSetEM contains extension methods that should work for any IFeatureSet
    /// </summary>
    public static class FeatureSetExt
    {
        /// <summary>
        /// Creates a new polygon featureset that is created by buffering each of the individual shapes.
        /// </summary>
        /// <param name="self">The IFeatureSet to buffer</param>
        /// <param name="distance">The double distance to buffer</param>
        /// <param name="copyAttributes">Boolean, if this is true, then the new featureset will have
        /// the same attributes as the original.</param>
        /// <returns>The newly created IFeatureSet</returns>
        public static IFeatureSet Buffer(this IFeatureSet self, double distance, bool copyAttributes)
        {
            // Dimension the new, output featureset.  Buffered shapes are polygons, even if the
            // original geometry is a point or a line.
            IFeatureSet result = new FeatureSet(FeatureType.Polygon);

            result.CopyTableSchema(self);
            result.Projection = self.Projection;
            // Cycle through the features, and buffer each one separately.
            foreach (IFeature original in self.Features)
            {
                // Actually calculate the buffer geometry.
                IFeature buffer = original.Buffer(distance);

                // Add the resulting polygon to the featureset
                result.Features.Add(buffer);

                // If copyAttributes is true, then this will copy those attributes from the original.
                if (copyAttributes)
                {
                    // Accessing the attributes should automatically load them from the datasource if
                    // they haven't been loaded already.
                    buffer.CopyAttributes(original);
                }
            }
            return result;
        }

        /// <summary>
        /// Generates an empty featureset that has the combined fields from this featureset
        /// and the specified featureset.
        /// </summary>
        /// <param name="self">This featureset</param>
        /// <param name="other">The other featureset to combine fields with.</param>
        /// <returns></returns>
        public static IFeatureSet CombinedFields(this IFeatureSet self, IFeatureSet other)
        {
            IFeatureSet result = new FeatureSet(self.FeatureType);
            var uniqueNames = new HashSet<string>();
            foreach (var fs in new []{self, other})
            {
                foreach (DataColumn dc in fs.DataTable.Columns)
                {
                    var name = dc.ColumnName;
                    var i = 1;
                    while (uniqueNames.Contains(name))
                    {
                        name = dc.ColumnName + i;
                        i++;
                    }
                    uniqueNames.Add(name);
                    result.DataTable.Columns.Add(new DataColumn(name, dc.DataType));
                }   
            }
            return result;
        }

        /// <summary>
        /// Calculates a union of any features that have a common value in the specified field.
        /// The output will only contain the specified field.  Example: Disolving a county
        /// shapefile based on state name to produce a single polygon shaped like the state.
        /// </summary>
        /// <param name="self">The original featureSet to disolve the features of</param>
        /// <param name="fieldName">The string field name to use for the disolve operation</param>
        /// <returns>A featureset where the geometries of features with the same attribute in the specified field have been combined.</returns>
        public static IFeatureSet Dissolve(this IFeatureSet self, string fieldName)
        {
            Dictionary<object, IFeature> resultFeatures = new Dictionary<object, IFeature>();
            IFeatureSet result = new FeatureSet(self.FeatureType);
            result.Projection = self.Projection;
            result.DataTable.Columns.Add(fieldName, self.DataTable.Columns[fieldName].DataType);

            foreach (IFeature feature in self.Features)
            {
                object value = feature.DataRow[fieldName];
                if (resultFeatures.ContainsKey(value))
                {
                    IFeature union = resultFeatures[value].Union(feature);
                    union.DataRow = result.DataTable.NewRow();
                    union.DataRow[fieldName] = value;
                    resultFeatures[value] = union;
                }
                else
                {
                    IFeature union = feature.Copy();
                    union.DataRow = result.DataTable.NewRow();
                    union.DataRow[fieldName] = value;
                    resultFeatures.Add(value, feature);
                }
            }

            foreach (IFeature feature in resultFeatures.Values)
            {
                result.Features.Add(feature);
            }
            return result;
        }

        /// <summary>
        /// Tests to see if this feature intersects with the specified envelope
        /// </summary>
        /// <param name="self">This feature</param>
        /// <param name="env">The envelope to test</param>
        /// <returns>Boolean, true if the intersection occurs</returns>
        public static bool Intersects(this IFeature self, IEnvelope env)
        {
            return self.Envelope.Intersects(env) && Geometry.FromBasicGeometry(self.BasicGeometry).Intersects(env.ToPolygon());
        }

        /// <summary>
        /// This method will create a new IFeatureSet with the the shapes that intersect merged into single shapes,
        /// or else it will return a new featureset where EVERY shape is unioned into a single, multi-part shape.
        /// </summary>
        /// <param name="self">The source of features to union</param>
        /// <param name="style">This controls whether intersecting or all features are unioned</param>
        /// <returns>An IFeatureSet with the unioned shapes.</returns>
        public static IFeatureSet UnionShapes(this IFeatureSet self, ShapeRelateType style)
        {
            if (style == ShapeRelateType.Intersecting)
            {
                return UnionIntersecting(self);
            }
            return UnionAll(self);
        }

        private static IFeatureSet UnionAll(IFeatureSet fs)
        {
            FeatureSet fsunion = new FeatureSet();
            fsunion.CopyTableSchema(fs);
            fsunion.Projection = fs.Projection;
            IFeature f = fs.Features[0];
            for (int i = 1; i < fs.Features.Count; i++)
            {
                f = f.Union(fs.Features[i], fsunion, FieldJoinType.LocalOnly);
            }
            fsunion.AddFeature(f);
            return fsunion;
        }

        private static IFeatureSet UnionIntersecting(IFeatureSet fs)
        {
            var fsunion = new FeatureSet();

            // This is needed or else the table won't have the columns for copying attributes.
            fsunion.CopyTableSchema(fs);
            fsunion.Projection = fs.Projection;
            // Create a list of all the original shapes so if we union A->B we don't also union B->A
            var freeFeatures = fs.Features.Select((t, i) => i).ToList();

            while (freeFeatures.Count > 0)
            {
                var fOriginal = fs.Features[freeFeatures[0]];

                // Whether this gets unioned or not, it has been handled and should not be re-done.
                // We also don't want to waste time unioning shapes to themselves.
                freeFeatures.RemoveAt(0);

                // This is the unioned result.  Remember, we may just add the original feature if no
                // shapes present themselves for unioning.
                IFeature fResult = null;

                // This is the list of any shapes that get unioned with our shape.
                var mergedList = new List<int>();
                bool shapeChanged;
                do
                {
                    shapeChanged = false; // reset this each time.
                    foreach (int index in freeFeatures)
                    {
                        var intersectSource = fResult ?? fOriginal;
                        if (intersectSource.Intersects(fs.Features[index]))
                        {
                            fResult = intersectSource.Union(fs.Features[index]);
                            shapeChanged = true;

                            // Don't modify the "freefeatures" list during a loop.  Keep track until later.
                            mergedList.Add(index);

                            // Less double-checking happens if we break rather than finishing the loop
                            // and then retest the whole loop because of a change early in the list.
                            break;
                        }
                    }

                    foreach (var index in mergedList)
                    {
                        // We don't want to add the same shape twice.
                        freeFeatures.Remove(index);
                    }
                } while (shapeChanged);

                // Add fResult, unless it is null, in which case add fOriginal.
                fsunion.Features.Add(fResult ?? fOriginal);
            }
            return fsunion;
        }

        /// <summary>
        /// This tests each feature of the input
        /// </summary>
        /// <param name="self">This featureSet</param>
        /// <param name="other">The featureSet to perform intersection with</param>
        /// <param name="joinType">The attribute join type</param>
        /// <param name="progHandler">A progress handler for status messages</param>
        /// <returns>An IFeatureSet with the intersecting features, broken down based on the join Type</returns>
        public static IFeatureSet Intersection(this IFeatureSet self, IFeatureSet other, FieldJoinType joinType, IProgressHandler progHandler)
        {
            IFeatureSet result = null;
            ProgressMeter pm = new ProgressMeter(progHandler, "Calculating Intersection", self.Features.Count);
            if (joinType == FieldJoinType.All)
            {
                result = CombinedFields(self, other);
                // Intersection is symmetric, so only consider I X J where J <= I
                if (!self.AttributesPopulated) self.FillAttributes();
                if (!other.AttributesPopulated) other.FillAttributes();

                for (int i = 0; i < self.Features.Count; i++)
                {
                    IFeature selfFeature = self.Features[i];
                    List<IFeature> potentialOthers = other.Select(selfFeature.Envelope.ToExtent());
                    foreach (IFeature otherFeature in potentialOthers)
                    {
                        selfFeature.Intersection(otherFeature, result, joinType);
                    }
                    pm.CurrentValue = i;
                }
                pm.Reset();
            }
            else if (joinType == FieldJoinType.LocalOnly)
            {
                if (!self.AttributesPopulated) self.FillAttributes();

                result = new FeatureSet();
                result.CopyTableSchema(self);
                result.FeatureType = self.FeatureType;
                if (other.Features != null && other.Features.Count > 0)
                {
                    pm = new ProgressMeter(progHandler, "Calculating Union", other.Features.Count);
                    IFeature union = other.Features[0];
                    for (int i = 1; i < other.Features.Count; i++)
                    {
                        union = union.Union(other.Features[i]);
                        pm.CurrentValue = i;
                    }
                    pm.Reset();
                    pm = new ProgressMeter(progHandler, "Calculating Intersections", self.NumRows());
                    Extent otherEnvelope = new Extent(union.Envelope);
                    for (int shp = 0; shp < self.ShapeIndices.Count; shp++)
                    {
                        if (!self.ShapeIndices[shp].Extent.Intersects(otherEnvelope)) continue;
                        IFeature selfFeature = self.GetFeature(shp);
                        selfFeature.Intersection(union, result, joinType);
                        pm.CurrentValue = shp;
                    }
                    pm.Reset();
                }
            }
            else if (joinType == FieldJoinType.ForeignOnly)
            {
                if (!other.AttributesPopulated) other.FillAttributes();

                result = new FeatureSet();
                result.CopyTableSchema(other);
                result.FeatureType = other.FeatureType;
                if (self.Features != null && self.Features.Count > 0)
                {
                    pm = new ProgressMeter(progHandler, "Calculating Union", self.Features.Count);
                    IFeature union = self.Features[0];
                    for (int i = 1; i < self.Features.Count; i++)
                    {
                        union = union.Union(self.Features[i]);
                        pm.CurrentValue = i;
                    }
                    pm.Reset();
                    if (other.Features != null)
                    {
                        pm = new ProgressMeter(progHandler, "Calculating Intersection", other.Features.Count);
                        for (int i = 0; i < other.Features.Count; i++)
                        {
                            other.Features[i].Intersection(union, result, FieldJoinType.LocalOnly);
                            pm.CurrentValue = i;
                        }
                    }
                    pm.Reset();
                }
            }
            return result;
        }
    }
}