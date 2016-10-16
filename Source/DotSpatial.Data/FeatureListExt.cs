// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/2/2010 1:08:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.NTSExtension;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// FeatureListEm
    /// </summary>
    public static class FeatureListExt
    {
        #region Methods

        /// <summary>
        /// adding a single coordinate will assume that the feature type should be point for this featureset, even
        /// if it has not already been specified.
        /// </summary>
        /// <param name="self">This IFeatureList</param>
        /// <param name="point">The point to add to the featureset</param>
        /// <exception cref="FeatureTypeMismatchException">Thrown when the feature type already exists, there are already features in the featureset and the featuretype is not equal to point.</exception>
        public static void Add(this IFeatureList self, Coordinate point)
        {
            if (self.Parent.FeatureType != FeatureType.Point && self.Count > 0)
            {
                throw new FeatureTypeMismatchException();
            }
            self.Parent.FeatureType = FeatureType.Point;
            self.Add(new Feature(new Point(point)));
        }

        /// <summary>
        /// This adds the coordinates and specifies what sort of feature type should be added.
        /// </summary>
        /// <param name="self">This IFeatureList</param>
        /// <param name="points">The list or array of coordinates to be added after it is built into the appropriate feature.</param>
        /// <param name="featureType">The feature type.</param>
        public static void Add(this IFeatureList self, IEnumerable<Coordinate> points, FeatureType featureType)
        {
            if (self.Parent.FeatureType == FeatureType.Unspecified) self.Parent.FeatureType = featureType;
            self.Add(points);
        }

        /// <summary>
        /// If the feature type is specified, then this will automatically generate a new feature from the specified coordinates.
        /// This will not work unless the featuretype is specified.
        /// </summary>
        /// <param name="self">This IFeatureList</param>
        /// <param name="points">
        /// The list or array of coordinates to build into a new feature.
        /// If the feature type is point, then this will create separate features for each coordinate.
        /// For polygons, all the points will be assumed to be in the shell.
        /// </param>
        /// <exception cref="UnspecifiedFeaturetypeException">Thrown if the current FeatureType for the shapefile is unspecified.</exception>
        public static void Add(this IFeatureList self, IEnumerable<Coordinate> points)
        {
            if (self.Parent.FeatureType == FeatureType.Unspecified)
            {
                throw new UnspecifiedFeaturetypeException();
            }
            if (self.Parent.FeatureType == FeatureType.Point)
            {
                self.SuspendEvents();
                foreach (Coordinate point in points)
                {
                    self.Add(new Feature(new Point(point)));
                }
                self.ResumeEvents();
            }
            if (self.Parent.FeatureType == FeatureType.Line)
            {
                self.Add(new Feature(new LineString(points as Coordinate[])));
            }
            if (self.Parent.FeatureType == FeatureType.Polygon)
            {
                self.Add(new Feature(new Polygon(new LinearRing(points as Coordinate[]))));
            }
            if (self.Parent.FeatureType == FeatureType.MultiPoint)
            {
                self.Add(new Feature(new MultiPoint(points.CastToPointArray())));
            }
        }

        /// <summary>
        /// This method will attempt to add the specified geometry to the list.
        /// If the feature type is currently unspecified, this will specify the feature type.
        /// </summary>
        /// <param name="self">This feature list</param>
        /// <param name="geometry">The geometry to create a new feature from.</param>
        /// <exception cref="FeatureTypeMismatchException">Thrown if the new geometry does not match the currently specified feature type.  </exception>
        public static void Add(this IFeatureList self, IGeometry geometry)
        {
            Feature f = new Feature(geometry);
            if (f.FeatureType != self.Parent.FeatureType && self.Parent.FeatureType != FeatureType.Unspecified)
            {
                throw new FeatureTypeMismatchException();
            }
            self.Add(f);
        }

        #endregion
    }
}