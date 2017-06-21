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
using System.Collections.Generic;

namespace DotSpatial.Topology.KDTree
{
    /// <summary>
    /// K-D Tree node class
    /// </summary>
    public class KdNode
    {
        #region Class variables

        // these are seen by KdTree
        internal readonly HPoint K;
        internal KdNode Left, Right;
        internal object V;

        private bool _isDeleted;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of the KDNode.
        /// </summary>
        /// <param name="key">A Hyper Point representing the key to use for storing this value</param>
        /// <param name="value">A valid object value to use for copying this.</param>
        /// <remarks>The constructor is used only by class; other methods are static</remarks>
        private KdNode(HPoint key, object value)
        {
            K = key;
            V = value;
            Left = null;
            Right = null;
            _isDeleted = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a string representation of this node
        /// </summary>
        /// <param name="depth">The depth of child nodes to search when creating the string.</param>
        /// <returns>A string representation of this node.</returns>
        public string ToString(int depth)
        {
            String s = K + "  " + V + (_isDeleted ? "*" : string.Empty);
            if (Left != null)
            {
                s = s + "\n" + Pad(depth) + "L " + Left.ToString(depth + 1);
            }
            if (Right != null)
            {
                s = s + "\n" + Pad(depth) + "R " + Right.ToString(depth + 1);
            }
            return s;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether this node has been _isDeleted
        /// </summary>
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Inserts a
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// <param name="lev"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        /// <remarks> Method ins translated from 352.ins.c of Gonnet and Baeza-Yates</remarks>
        public static KdNode Insert(HPoint key, object value, KdNode t, int lev, int k)
        {
            if (t == null)
            {
                t = new KdNode(key, value);
            }
            else if (key.Equals(t.K))
            {
                // "re-insert"
                if (t.IsDeleted)
                {
                    t.IsDeleted = false;
                    t.V = value;
                }
                else
                {
                    throw (new KeyDuplicateException());
                }
            }
            else if (key[lev] > t.K[lev])
            {
                t.Right = Insert(key, value, t.Right, (lev + 1) % k, k);
            }
            else
            {
                t.Left = Insert(key, value, t.Left, (lev + 1) % k, k);
            }

            return t;
        }

        /// <summary>
        /// Searches for a specific value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        /// <remarks>Method srch translated from 352.srch.c of Gonnet and Baeza-Yates</remarks>
        public static KdNode Search(HPoint key, KdNode t, int k)
        {
            for (int lev = 0; t != null; lev = (lev + 1) % k)
            {
                if (!t.IsDeleted && key.Equals(t.K))
                {
                    return t;
                }
                t = key[lev] > t.K[lev] ? t.Right : t.Left;
            }

            return null;
        }

        /// <summary>
        /// Searches for values in a range
        /// </summary>
        /// <param name="lowk"></param>
        /// <param name="uppk"></param>
        /// <param name="t"></param>
        /// <param name="lev"></param>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <remarks>Method rsearch translated from 352.range.c of Gonnet and Baeza-Yates</remarks>
        public static void SearchRange(HPoint lowk, HPoint uppk, KdNode t, int lev,
                                       int k, List<KdNode> v)
        {
            if (t == null) return;
            if (lowk[lev] <= t.K[lev])
            {
                SearchRange(lowk, uppk, t.Left, (lev + 1) % k, k, v);
            }
            int j;
            for (j = 0; j < k && lowk[j] <= t.K[j] &&
                        uppk[j] >= t.K[j]; j++)
            {
            }
            if (j == k) v.Add(t);
            if (uppk[lev] > t.K[lev])
            {
                SearchRange(lowk, uppk, t.Right, (lev + 1) % k, k, v);
            }
        }

        /// <summary>
        /// Method Nearest Neighbor from Andrew Moore's thesis. Numbered
        /// comments are direct quotes from there. Step "SDL" is added to
        /// make the algorithm work correctly.  NearestNeighborList solution
        /// courtesy of Bjoern Heckel.
        /// </summary>
        /// <param name="kd"></param>
        /// <param name="target"></param>
        /// <param name="hr"></param>
        /// <param name="maxDistSqd"></param>
        /// <param name="lev"></param>
        /// <param name="k"></param>
        /// <param name="nnl"></param>
        public static void Nnbr(KdNode kd, HPoint target, HRect hr,
                                double maxDistSqd, int lev, int k,
                                NearestNeighborList nnl)
        {
            // 1. if kd is empty then set dist-sqd to infinity and exit.
            if (kd == null)
            {
                return;
            }

            // 2. s := split field of kd
            int s = lev % k;

            // 3. pivot := dom-elt field of kd
            HPoint pivot = kd.K;
            double pivotToTarget = HPoint.SquareDistance(pivot, target);

            // 4. Cut hr into to sub-hyperrectangles left-hr and right-hr.
            //    The cut plane is through pivot and perpendicular to the s
            //    dimension.
            HRect leftHr = hr; // optimize by not cloning
            HRect rightHr = hr.Copy();
            leftHr.Max[s] = pivot[s];
            rightHr.Min[s] = pivot[s];

            // 5. target-in-left := target_s <= pivot_s
            bool targetInLeft = target[s] < pivot[s];

            KdNode nearerKd;
            HRect nearerHr;
            KdNode furtherKd;
            HRect furtherHr;

            if (targetInLeft)
            {
                // 6. if target-in-left then
                //    6.1. nearer-kd := left field of kd and nearer-hr := left-hr
                //    6.2. further-kd := right field of kd and further-hr := right-hr
                nearerKd = kd.Left;
                nearerHr = leftHr;
                furtherKd = kd.Right;
                furtherHr = rightHr;
            }
            else
            {
                //
                // 7. if not target-in-left then
                //    7.1. nearer-kd := right field of kd and nearer-hr := right-hr
                //    7.2. further-kd := left field of kd and further-hr := left-hr
                nearerKd = kd.Right;
                nearerHr = rightHr;
                furtherKd = kd.Left;
                furtherHr = leftHr;
            }

            // 8. Recursively call Nearest Neighbor with paramters
            //    (nearer-kd, target, nearer-hr, max-dist-sqd), storing the
            //    results in nearest and dist-sqd
            Nnbr(nearerKd, target, nearerHr, maxDistSqd, lev + 1, k, nnl);

            double distSqd;

            if (!nnl.IsCapacityReached)
            {
                distSqd = 1.79769e+30; // Double.MaxValue;
            }
            else
            {
                distSqd = nnl.MaxPriority;
            }

            // 9. max-dist-sqd := minimum of max-dist-sqd and dist-sqd
            maxDistSqd = Math.Min(maxDistSqd, distSqd);

            // 10. A nearer point could only lie in further-kd if there were some
            //     part of further-hr within distance sqrt(max-dist-sqd) of
            //     target.  If this is the case then
            HPoint closest = furtherHr.Closest(target);
            if (HPoint.EuclideanDistance(closest, target) < Math.Sqrt(maxDistSqd))
            {
                // 10.1 if (pivot-target)^2 < dist-sqd then
                if (pivotToTarget < distSqd)
                {
                    // 10.1.1 nearest := (pivot, range-elt field of kd)

                    // 10.1.2 dist-sqd = (pivot-target)^2
                    distSqd = pivotToTarget;

                    // add to nnl
                    if (!kd.IsDeleted)
                    {
                        nnl.Insert(kd, distSqd);
                    }

                    // 10.1.3 max-dist-sqd = dist-sqd
                    // max_dist_sqd = dist_sqd;
                    if (nnl.IsCapacityReached)
                    {
                        maxDistSqd = nnl.MaxPriority;
                    }
                    else
                    {
                        maxDistSqd = 1.79769e+30; //Double.MaxValue;
                    }
                }

                // 10.2 Recursively call Nearest Neighbor with parameters
                //      (further-kd, target, further-hr, max-dist_sqd),
                //      storing results in temp-nearest and temp-dist-sqd
                Nnbr(furtherKd, target, furtherHr, maxDistSqd, lev + 1, k, nnl);
                double tempDistSqd = nnl.MaxPriority;

                // 10.3 If tmp-dist-sqd < dist-sqd then
                if (tempDistSqd < distSqd)
                {
                    // 10.3.1 nearest := temp_nearest and dist_sqd := temp_dist_sqd
                    distSqd = tempDistSqd;
                }
            }
            else if (pivotToTarget < maxDistSqd)
            {
                // SDL: otherwise, current point is nearest
                distSqd = pivotToTarget;
            }
        }

        /// <summary>
        /// This method was written by Ted Dunsford by restructuring the nearest neighbor
        /// algorithm presented by Andrew and Bjoern
        /// </summary>
        /// <param name="kd">Since this is recursive, this represents the current node</param>
        /// <param name="target">The target is the HPoint that we are trying to calculate the farthest distance from</param>
        /// <param name="hr">In this case, the hr is the hyper rectangle bounding the region that must contain the furthest point.</param>
        /// <param name="maxDistSqd">The maximum distance that we have calculated thus far, and will therefore be testing against.</param>
        /// <param name="lev">The integer based level of that we have recursed to in the tree</param>
        /// <param name="k">The dimensionality of the kd tree</param>
        /// <param name="fnl">A list to contain the output, prioritized by distance</param>
        public static void FarthestNeighbor(KdNode kd, HPoint target, HRect hr,
                                            double maxDistSqd, int lev, int k,
                                            FarthestNeighborList fnl)
        {
            // 1. if kd is empty then set dist-sqd to infinity and exit.
            if (kd == null)
            {
                return;
            }

            // 2. s := split field of kd
            int s = lev % k;

            // 3. pivot := dom-elt field of kd
            HPoint pivot = kd.K;
            double pivotToTarget = HPoint.SquareDistance(pivot, target);

            // 4. Cut hr into to sub-hyperrectangles left-hr and right-hr.
            //    The cut plane is through pivot and perpendicular to the s
            //    dimension.
            HRect leftHr = hr; // optimize by not cloning
            HRect rightHr = hr.Copy();
            leftHr.Max[s] = pivot[s];
            rightHr.Min[s] = pivot[s];

            // 5. target-in-left := target_s <= pivot_s
            bool targetInLeft = target[s] < pivot[s];

            KdNode nearerKd;
            HRect nearerHr;
            KdNode furtherKd;
            HRect furtherHr;

            if (targetInLeft)
            {
                // 6. if target-in-left then
                //    6.1. nearer-kd := left field of kd and nearer-hr := left-hr
                //    6.2. further-kd := right field of kd and further-hr := right-hr
                nearerKd = kd.Left;
                nearerHr = leftHr;
                furtherKd = kd.Right;
                furtherHr = rightHr;
            }
            else
            {
                //
                // 7. if not target-in-left then
                //    7.1. nearer-kd := right field of kd and nearer-hr := right-hr
                //    7.2. further-kd := left field of kd and further-hr := left-hr
                nearerKd = kd.Right;
                nearerHr = rightHr;
                furtherKd = kd.Left;
                furtherHr = leftHr;
            }

            // 8. Recursively call Nearest Neighbor with paramters
            //    (nearer-kd, target, nearer-hr, max-dist-sqd), storing the
            //    results in nearest and dist-sqd
            //FarthestNeighbor(nearer_kd, target, nearer_hr, max_dist_sqd, lev + 1, K, nnl);

            // This line changed by Ted Dunsford to attempt to find the farther point
            FarthestNeighbor(furtherKd, target, furtherHr, maxDistSqd, lev + 1, k, fnl);

            //KDNode nearest = (KDNode)nnl.Highest;
            double distSqd;

            if (!fnl.IsCapacityReached)
            {
                //dist_sqd = 1.79769e+30; // Double.MaxValue;
                distSqd = 0;
            }
            else
            {
                distSqd = fnl.MinimumPriority;
            }

            // 9. max-dist-sqd := minimum of max-dist-sqd and dist-sqd
            //max_dist_sqd = Math.Min(max_dist_sqd, dist_sqd);

            maxDistSqd = Math.Max(maxDistSqd, distSqd);

            // 10. A nearer point could only lie in further-kd if there were some
            //     part of further-hr within distance sqrt(max-dist-sqd) of
            //     target.  If this is the case then
            // HPoint closest = further_hr.Closest(target);
            HPoint furthest = nearerHr.Farthest(target);
            //if (HPoint.EuclideanDistance(closest, target) < Math.Sqrt(max_dist_sqd))
            if (HPoint.EuclideanDistance(furthest, target) > Math.Sqrt(maxDistSqd))
            {
                // 10.1 if (pivot-target)^2 < dist-sqd then
                if (pivotToTarget > distSqd)
                {
                    // 10.1.1 nearest := (pivot, range-elt field of kd)
                    //nearest = kd;

                    // 10.1.2 dist-sqd = (pivot-target)^2
                    distSqd = pivotToTarget;

                    // add to nnl
                    if (!kd.IsDeleted)
                    {
                        fnl.Insert(kd, distSqd);
                    }

                    // 10.1.3 max-dist-sqd = dist-sqd
                    // max_dist_sqd = dist_sqd;
                    if (fnl.IsCapacityReached)
                    {
                        maxDistSqd = fnl.MinimumPriority;
                    }
                    else
                    {
                        // max_dist_sqd = 1.79769e+30; //Double.MaxValue;
                        maxDistSqd = 0;
                    }
                }

                // 10.2 Recursively call Nearest Neighbor with parameters
                //      (further-kd, target, further-hr, max-dist_sqd),
                //      storing results in temp-nearest and temp-dist-sqd
                FarthestNeighbor(nearerKd, target, nearerHr, maxDistSqd, lev + 1, k, fnl);

                double tempDistSqd = fnl.MinimumPriority;

                // 10.3 If tmp-dist-sqd < dist-sqd then
                if (tempDistSqd > distSqd)
                {
                    // 10.3.1 nearest := temp_nearest and dist_sqd := temp_dist_sqd
                    distSqd = tempDistSqd;
                }
            }
            else if (pivotToTarget < maxDistSqd)
            {
                // SDL: otherwise, current point is nearest
                distSqd = pivotToTarget;
            }
        }

        #endregion

        #region Private Functions

        private static string Pad(int n)
        {
            string s = string.Empty;
            for (int i = 0; i < n; ++i)
            {
                s += " ";
            }
            return s;
        }

        #endregion
    }
}