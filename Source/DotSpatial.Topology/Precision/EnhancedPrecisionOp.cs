// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://dotspatial.codeplex.com/license and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.Precision
{
    /// <summary>
    /// Provides versions of Geometry spatial functions which use
    /// enhanced precision techniques to reduce the likelihood of robustness problems.
    /// </summary>
    public class EnhancedPrecisionOp
    {
        /// <summary>
        /// Only static methods!
        /// </summary>
        private EnhancedPrecisionOp() { }

        /// <summary>
        /// Computes the set-theoretic intersection of two <c>Geometry</c>s, using enhanced precision.
        /// </summary>
        /// <param name="geom0">The first Geometry.</param>
        /// <param name="geom1">The second Geometry.</param>
        /// <returns>The Geometry representing the set-theoretic intersection of the input Geometries.</returns>
        public static IGeometry Intersection(IGeometry geom0, IGeometry geom1)
        {
            ApplicationException originalEx;
            try
            {
                IGeometry result = geom0.Intersection(geom1);
                return result;
            }
            catch (ApplicationException ex)
            {
                originalEx = ex;
            }
            /*
             * If we are here, the original op encountered a precision problem
             * (or some other problem).  Retry the operation with
             * enhanced precision to see if it succeeds
             */
            try
            {
                CommonBitsOp cbo = new CommonBitsOp(true);
                IGeometry resultEp = cbo.Intersection(geom0, geom1);
                // check that result is a valid point after the reshift to orginal precision
                if (!resultEp.IsValid)
                    throw originalEx;
                return resultEp;
            }
            catch (ApplicationException)
            {
                throw originalEx;
            }
        }

        /// <summary>
        /// Computes the set-theoretic union of two <c>Geometry</c>s, using enhanced precision.
        /// </summary>
        /// <param name="geom0">The first Geometry.</param>
        /// <param name="geom1">The second Geometry.</param>
        /// <returns>The Geometry representing the set-theoretic union of the input Geometries.</returns>
        public static IGeometry Union(IGeometry geom0, IGeometry geom1)
        {
            ApplicationException originalEx;
            try
            {
                IGeometry result = geom0.Union(geom1);
                return result;
            }
            catch (ApplicationException ex)
            {
                originalEx = ex;
            }
            /*
             * If we are here, the original op encountered a precision problem
             * (or some other problem).  Retry the operation with
             * enhanced precision to see if it succeeds
             */
            try
            {
                CommonBitsOp cbo = new CommonBitsOp(true);
                IGeometry resultEp = cbo.Union(geom0, geom1);
                // check that result is a valid point after the reshift to orginal precision
                if (!resultEp.IsValid)
                    throw originalEx;
                return resultEp;
            }
            catch (ApplicationException)
            {
                throw originalEx;
            }
        }

        /// <summary>
        /// Computes the set-theoretic difference of two <c>Geometry</c>s, using enhanced precision.
        /// </summary>
        /// <param name="geom0">The first Geometry.</param>
        /// <param name="geom1">The second Geometry.</param>
        /// <returns>The Geometry representing the set-theoretic difference of the input Geometries.</returns>
        public static IGeometry Difference(IGeometry geom0, IGeometry geom1)
        {
            ApplicationException originalEx;
            try
            {
                IGeometry result = geom0.Difference(geom1);
                return result;
            }
            catch (ApplicationException ex)
            {
                originalEx = ex;
            }
            /*
             * If we are here, the original op encountered a precision problem
             * (or some other problem).  Retry the operation with
             * enhanced precision to see if it succeeds
             */
            try
            {
                CommonBitsOp cbo = new CommonBitsOp(true);
                IGeometry resultEP = cbo.Difference(geom0, geom1);
                // check that result is a valid point after the reshift to orginal precision
                if (!resultEP.IsValid)
                    throw originalEx;
                return resultEP;
            }
            catch (ApplicationException)
            {
                throw originalEx;
            }
        }

        /// <summary>
        /// Computes the set-theoretic symmetric difference of two <c>Geometry</c>s, using enhanced precision.
        /// </summary>
        /// <param name="geom0">The first Geometry.</param>
        /// <param name="geom1">The second Geometry.</param>
        /// <returns>The Geometry representing the set-theoretic symmetric difference of the input Geometries.</returns>
        public static IGeometry SymDifference(IGeometry geom0, IGeometry geom1)
        {
            ApplicationException originalEx;
            try
            {
                IGeometry result = geom0.SymmetricDifference(geom1);
                return result;
            }
            catch (ApplicationException ex)
            {
                originalEx = ex;
            }
            /*
             * If we are here, the original op encountered a precision problem
             * (or some other problem).  Retry the operation with
             * enhanced precision to see if it succeeds
             */
            try
            {
                CommonBitsOp cbo = new CommonBitsOp(true);
                IGeometry resultEP = cbo.SymDifference(geom0, geom1);
                // check that result is a valid point after the reshift to orginal precision
                if (!resultEP.IsValid)
                    throw originalEx;
                return resultEP;
            }
            catch (ApplicationException)
            {
                throw originalEx;
            }
        }

        /// <summary>
        /// Computes the buffer of a <c>Geometry</c>, using enhanced precision.
        /// This method should no longer be necessary, since the buffer algorithm
        /// now is highly robust.
        /// </summary>
        /// <param name="geom">The first Geometry.</param>
        /// <param name="distance">The buffer distance.</param>
        /// <returns>The Geometry representing the buffer of the input Geometry.</returns>
        [Obsolete("This method should no longer be necessary, since the buffer algorithm now is highly robust.")]
        public static IGeometry Buffer(Geometry geom, double distance)
        {
            ApplicationException originalEx;
            try
            {
                Geometry result = (Geometry)geom.Buffer(distance);
                return result;
            }
            catch (ApplicationException ex)
            {
                originalEx = ex;
            }
            /*
             * If we are here, the original op encountered a precision problem
             * (or some other problem).  Retry the operation with
             * enhanced precision to see if it succeeds
             */
            try
            {
                CommonBitsOp cbo = new CommonBitsOp(true);
                IGeometry resultEP = cbo.Buffer(geom, distance);
                // check that result is a valid point after the reshift to orginal precision
                if (!resultEP.IsValid)
                    throw originalEx;
                return resultEP;
            }
            catch (ApplicationException)
            {
                throw originalEx;
            }
        }
    }
}