// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The original content was ported from the C language from the 4.6 version of Proj4 libraries.
// Frank Warmerdam has released the full content of that version under the MIT license which is
// recognized as being approximately equivalent to public domain.  The original work was done
// mostly by Gerald Evenden.  The latest versions of the C libraries can be obtained here:
// http://trac.osgeo.org/proj/
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 11:44:55 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// Elliptical Transform supports a built in framework for assuming a
    /// separate function occurs if the spheroid is elliptical
    /// </summary>
    public class EllipticalTransform : Transform
    {
        /// <inheritdoc />
        protected override void OnForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            if (IsElliptical)
            {
                EllipticalForward(lp, xy, startIndex, numPoints);
            }
            else
            {
                SphericalForward(lp, xy, startIndex, numPoints);
            }
        }

        /// <summary>
        /// Calculates the forward transformation from geodetic lambda and phi coordinates to
        /// linear xy coordinates for projections that have elliptical earth models.
        /// </summary>
        /// <param name="lp">The input interleaved lambda-phi coordinates where lambda is longitude in radians and phi is latitude in radians.</param>
        /// <param name="xy">The resulting interleaved x-y coordinates</param>
        /// <param name="startIndex">The zero based integer start index in terms of coordinate pairs</param>
        /// <param name="numPoints">The integer number of xy pairs to evaluate.</param>
        protected virtual void EllipticalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
        }

        /// <summary>
        /// Calculates the forward transformation from geodetic lambda and phi coordinates to
        /// linear xy coordinates for projections that have spherical earth models.
        /// </summary>
        /// <param name="lp">The input interleaved lambda-phi coordinates where lambda is longitude in radians and phi is latitude in radians.</param>
        /// <param name="xy">The resulting interleaved x-y coordinates</param>
        /// <param name="startIndex">The zero based integer start index in terms of coordinate pairs</param>
        /// <param name="numPoints">The integer number of xy pairs to evaluate.</param>
        protected virtual void SphericalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
        }

        /// <inheritdoc />
        protected override void OnInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
            if (IsElliptical)
            {
                EllipticalInverse(xy, lp, startIndex, numPoints);
            }
            else
            {
                SphericalInverse(xy, lp, startIndex, numPoints);
            }
        }

        /// <summary>
        /// Calculates the inverse transformation from linear xy coordinates to geodetic lambda and phi coordinates
        /// for projections that have elliptical earth models.
        /// </summary>
        /// <param name="xy">The input interleaved x-y coordinates</param>
        /// <param name="lp">The output interleaved lambda-phi coordinates where lambda is longitude in radians and phi is latitude in radians.</param>
        /// <param name="startIndex">The zero based integer start index in terms of coordinate pairs</param>
        /// <param name="numPoints">The integer number of xy pairs to evaluate.</param>
        protected virtual void EllipticalInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
        }

        /// <summary>
        /// Calculates the inverse transformation from linear xy coordinates to geodetic lambda and phi coordinates
        /// for projections that have spherical earth models.
        /// </summary>
        /// <param name="xy">The input interleaved x-y coordinates</param>
        /// <param name="lp">The output interleaved lambda-phi coordinates where lambda is longitude in radians and phi is latitude in radians.</param>
        /// <param name="startIndex">The zero based integer start index in terms of coordinate pairs</param>
        /// <param name="numPoints">The integer number of xy pairs to evaluate.</param>
        protected virtual void SphericalInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
        }
    }
}