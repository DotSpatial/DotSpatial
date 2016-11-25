// *******************************************************************************************************
// Product: DotSpatial.Analysis.RandomGeometry.cs
// Description: Class for random data generation. Put other random geometry functions here such as new
// methods for random point generation in a polygon, random points in a raster, etc. 

// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Dan Ames           |  2/26/2013         |  Initially written.  
// *******************************************************************************************************

using System;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.NTSExtension;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// A class for supporting methods to generate random points.
    /// </summary>
    public static class RandomGeometry
    {
        /// <summary>
        /// Creates a specified number of random point features inside multiple polygon features in a feature set. 
        /// </summary>
        /// <param name="ConstrainingFeatures">Random points will be generated inside all features in this feature set.</param>
        /// <param name="NumberOfPoints">The number of points to be randomly generated.</param>
        /// <param name="fsOut">FeatureSet, the points should be added to.</param>
        /// <param name="cancelProgressHandler">Optional parameter to report progress and cancel entire process if needed.</param>
        /// <returns>A point feature set with the randomly created features.</returns>
        public static IFeatureSet RandomPoints(IFeatureSet ConstrainingFeatures, int NumberOfPoints, IFeatureSet fsOut = null, ICancelProgressHandler cancelProgressHandler = null)
        {
            //This function generates random points within the boundaries of all polygon features in a feature set
            if (fsOut == null)
            {
                fsOut = new FeatureSet();
            }
            fsOut.FeatureType = FeatureType.Point;
            Random r = new Random();
            int i = 0;
            while (i < NumberOfPoints)
            {
                var c = new Coordinate();
                //make a random point somewhere in the rectangular extents of the feature set
                double rndx = r.Next(0, 100000) / 100000.0;
                double rndy = r.Next(0, 100000) / 100000.0;
                c.X = rndx * (ConstrainingFeatures.Extent.MaxX - ConstrainingFeatures.Extent.MinX) + ConstrainingFeatures.Extent.MinX;
                c.Y = rndy * (ConstrainingFeatures.Extent.MaxY - ConstrainingFeatures.Extent.MinY) + ConstrainingFeatures.Extent.MinY;

                Point p = new Point(c);

                //check if the point falls within the polygon featureset
                if (ConstrainingFeatures.Features.Any(f => f.Geometry.Intersects(p)))
                {
                    fsOut.AddFeature(p);
                    i++;
                }
                if (cancelProgressHandler != null)
                {
                    if (cancelProgressHandler.Cancel) { return null; }
                    int progress = Convert.ToInt32(i * 100 / NumberOfPoints);
                    cancelProgressHandler.Progress(String.Empty, progress, String.Empty);
                }
            }
            return fsOut;
        }
        /// <summary>
        /// Creates a specified number of random point features inside a single polygon feature. 
        /// </summary>
        /// <param name="ConstrainingFeature">Random points will be generated inside this polygon feature.</param>
        /// <param name="NumberOfPoints">The number of points to be randomly generated.</param>
        /// <returns>A point feature set with the randomly created features.</returns>
        public static FeatureSet RandomPoints(Feature ConstrainingFeature, int NumberOfPoints)
        {
            //This function generates random points within the boundaries of one polygon feature
            FeatureSet fsOut = new FeatureSet();
            fsOut.FeatureType = FeatureType.Point;
            Coordinate c = new Coordinate();
            Random r = new Random();
            int i = 0;
            while (i < NumberOfPoints)
            {
                c = new Coordinate();
                //make a random point somewhere in the rectangular extents of the feature
                double rndx = r.Next(0, 100000) / 100000.0;
                double rndy = r.Next(0, 100000) / 100000.0;
                c.X = rndx * (ConstrainingFeature.Geometry.EnvelopeInternal.Right() - ConstrainingFeature.Geometry.EnvelopeInternal.MinX) + ConstrainingFeature.Geometry.EnvelopeInternal.MinX;
                c.Y = rndy * (ConstrainingFeature.Geometry.EnvelopeInternal.MaxY - ConstrainingFeature.Geometry.EnvelopeInternal.Bottom()) + ConstrainingFeature.Geometry.EnvelopeInternal.Bottom();
                //check if the point falls within the polygon featureset
               
                Point p = new Point(c);
                if (ConstrainingFeature.Geometry.Intersects(p))
                {
                    fsOut.AddFeature(p);
                    i++;
                }
            }
            return fsOut;
        }
    }
}
