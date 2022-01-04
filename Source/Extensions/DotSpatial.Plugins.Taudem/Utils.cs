// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Plugins.Taudem
{
    /// <summary>
    /// Utils provides a collection of methods ranging from file conversion to finding a point on a shape.
    /// </summary>
    ///
    public class Utils
    {
        #region Methods

        /// <summary>
        /// Calculates the area of a part, without taking into consideration any other aspects of the polygon.
        /// </summary>
        /// <param name="polygon">A MapWinGIS.Shape POLYGON, POLYGONZ, or POLYGONM.</param>
        /// <param name="partIndex">The integer index of the part to obtain the area of.
        /// This value will be ignored if the shape only has one part, and the function
        /// will calculate the area of the entire shape.</param>
        /// <returns>A double value that is equal to the area of the part.</returns>
        /// <remarks>Coded by Ted Dunsford 6/23/2006, derived from Angela's Area algorithm
        /// Code reference http://astronomy.swin.edu.au/~pbourke/geometry/polyarea/
        /// Cached in MapWinGeoProc\clsUtils\Documentation\
        /// I don't think that we ever want to return a negative area from this function, even if the part is a hole, because it is being calculated outside of the
        /// context of any other parts.  Only the collective Area function should worry about ascribing a sign value to the individual part areas.
        /// </remarks>
        public static double AreaOfPart(IFeature polygon, int partIndex)
        {
            // TODO why is partindex ignored?
            return polygon.Geometry.Area;
        }

        #endregion
    }
}