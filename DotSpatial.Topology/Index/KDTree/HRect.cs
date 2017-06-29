// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.KDTree
{
    /// <summary>
    /// Hyper-Rectangle class supporting KdTree class
    /// </summary>
    public class HRect : ICloneable
    {
        #region Internal Variables

        /// <summary>
        /// Maximum values
        /// </summary>
        internal HPoint Max;

        /// <summary>
        /// Minimum values
        /// </summary>
        internal HPoint Min;

        #endregion

        // Internal is being used for performance for KD calculations, where these would normally be private.

        /// <summary>
        /// Constructs a new instance of a rectangle binding structure based on a specified number of dimensions
        /// </summary>
        /// <param name="numDimensions">An integer representing the number of dimensions.  For X, Y coordinates, this should be 2.</param>
        public HRect(int numDimensions)
        {
            Min = new HPoint(numDimensions);
            Max = new HPoint(numDimensions);
        }

        /// <summary>
        /// Creates a new bounding rectangle based on the two coordinates specified.  It is assumed that
        /// the vmin and vmax coordinates have already been correctly calculated.
        /// </summary>
        /// <param name="vmin"></param>
        /// <param name="vmax"></param>
        public HRect(HPoint vmin, HPoint vmax)
        {
            Min = vmin.Copy();
            Max = vmax.Copy();
        }

        /// <summary>
        /// Gets the current hyper-volume.  For 1D, this is Length.  For 2D this is Area.  For 3D this is Volume.
        /// </summary>
        public double HyperVolume
        {
            get
            {
                double a = 1;

                for (int i = 0; i < Min.NumOrdinates; ++i)
                {
                    a *= (Max[i] - Min[i]);
                }

                return a;
            }
        }

        /// <summary>
        /// Gets or sets the minimum coordinate (containing the smaller value) in all dimensions.
        /// </summary>
        public HPoint Minimum
        {
            get { return Min; }
            set { Min = value; }
        }

        /// <summary>
        /// Gets or sets the maximum coordinate
        /// </summary>
        public HPoint Maximum
        {
            get { return Max; }
            set { Max = value; }
        }

        /// <summary>
        /// Gets the number of ordinates for this rectangular structure (based on the minimum HPoint)
        /// </summary>
        public int NumOrdinates
        {
            get { return Min.NumOrdinates; }
        }

        #region ICloneable Members

        /// <summary>
        /// Creates a duplicate of this object
        /// </summary>
        /// <returns>An object duplicate of this object</returns>
        public object Clone()
        {
            return new HRect(Min, Max);
        }

        #endregion

        /// <summary>
        /// Creates a duplicate of this bounding box using the existing minimum and maximum.
        /// </summary>
        /// <returns>An HRect duplicate of this object</returns>
        public HRect Copy()
        {
            return new HRect(Min, Max);
        }

        /// <summary>
        /// Calculates the closest point on the hyper-extent to the specified point.
        /// from Moore's eqn. 6.6
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public HPoint Closest(HPoint t)
        {
            int len = t.NumOrdinates;
            HPoint p = new HPoint(len);

            for (int i = 0; i < len; ++i)
            {
                if (t[i] <= Min[i])
                {
                    p[i] = Min[i];
                }
                else if (t[i] >= Max[i])
                {
                    p[i] = Max[i];
                }
                else
                {
                    p[i] = t[i];
                }
            }
            return p;
        }

        /// <summary>
        /// This method calculates the furthest point on the rectangle
        /// from the specified point.  This is to determine if it is
        /// possible for any of the members of the closer rectangle
        /// to be positioned further away from the test point than
        /// the points in the hyper-extent that is further from the point.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public HPoint Farthest(HPoint t)
        {
            int len = t.NumOrdinates;
            HPoint p = new HPoint(len);

            for (int i = 0; i < len; ++i)
            {
                if (t[i] <= Min[i])
                {
                    p[i] = Max[i];
                }
                else if (t[i] >= Max[i])
                {
                    p[i] = Min[i];
                }
                else
                {
                    // Calculating the closest position always uses the point, but
                    // to calculate the furthest position, we actually want to
                    // pick the furhter of two extremes, in order to pick up the
                    // diagonal distance.
                    // p[i] = t[i];
                    if (t[i] - Min[i] > Max[i] - t[i])
                    {
                        p[i] = Min[i];
                    }
                    else
                    {
                        p[i] = Max[i];
                    }
                }
            }
            return p;
        }

        /// <summary>
        /// Calculates a new HRect object that has a nearly infinite bounds.
        /// </summary>
        /// <param name="d">THe number of dimensions to use</param>
        /// <returns>A new HRect where the minimum is negative infinity, and the maximum is positive infinity</returns>
        /// <remarks>Used in initial conditions of KdTree.nearest()</remarks>
        public static HRect InfiniteHRect(int d)
        {
            HPoint vmin = new HPoint(d);
            HPoint vmax = new HPoint(d);

            for (int i = 0; i < d; ++i)
            {
                vmin[i] = Double.NegativeInfinity;
                vmax[i] = Double.PositiveInfinity;
            }

            return new HRect(vmin, vmax);
        }

        /// <summary>
        /// If the specified HRect does not intersect this HRect, this returns null.  Otherwise,
        /// this will return a smaller rectangular region that represents the intersection
        /// of the two bounding regions.
        /// </summary>
        /// <param name="region">Another HRect object to intersect with this one.</param>
        /// <returns>The HRect that represents the intersection area for the two bounding boxes.</returns>
        /// <remarks>currently unused</remarks>
        public HRect Intersection(HRect region)
        {
            HPoint newmin = new HPoint(Min.NumOrdinates);
            HPoint newmax = new HPoint(Min.NumOrdinates);

            for (int i = 0; i < Min.NumOrdinates; ++i)
            {
                newmin[i] = Math.Max(Min[i], region.Min[i]);
                newmax[i] = Math.Min(Max[i], region.Max[i]);
                if (newmin[i] >= newmax[i]) return null;
            }

            return new HRect(newmin, newmax);
        }

        /// <summary>
        /// Creates a string that represents this bounding box
        /// </summary>
        /// <returns>A String</returns>
        public override string ToString()
        {
            return Min + "\n" + Max + "\n";
        }
    }
}