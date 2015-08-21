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

using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// A Depth object records the topological depth of the sides
    /// of an Edge for up to two Geometries.
    /// </summary>
    public class Depth
    {
        #region Constant Fields

        /// <summary>
        ///
        /// </summary>
        private const int @null = -1;

        #endregion

        #region Fields

        private readonly int[,] depth = new int[2,3];

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        public Depth()
        {
            // initialize depth array to a sentinel value
            for (int i = 0; i < 2; i++) 
                for (int j = 0; j < 3; j++)                 
                    depth[i,j] = @null;                
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Calls GetDepth and SetDepth.
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public int this[int geomIndex, Positions posIndex]
        {
            get
            {
                return GetDepth(geomIndex, posIndex);
            }
            set
            {
                SetDepth(geomIndex, posIndex, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <param name="_location"></param>
        public void Add(int geomIndex, Positions posIndex, Location _location)
        {
            if (_location == Location.Interior)
                depth[geomIndex, (int)posIndex]++;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lbl"></param>
        public void Add(Label lbl)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    Location loc = lbl.GetLocation(i, (Positions)j);
                    if (loc == Location.Exterior || loc == Location.Interior)
                    {
                        // initialize depth if it is null, otherwise add this location value
                        if (IsNull(i, (Positions)j))
                             depth[i,j]  = DepthAtLocation(loc);
                        else depth[i,j] += DepthAtLocation(loc);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_location"></param>
        /// <returns></returns>
        public static int DepthAtLocation(Location _location)
        {
            if (_location == Location.Exterior) 
                return 0;

            if (_location == Location.Interior) 
                return 1;

            return @null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <returns></returns>
        public int GetDelta(int geomIndex)
        {
            return depth[geomIndex, (int)Positions.Right] - depth[geomIndex, (int)Positions.Left];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public int GetDepth(int geomIndex, Positions posIndex)
        {
            return depth[geomIndex, (int)posIndex];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public Location GetLocation(int geomIndex, Positions posIndex)
        {
            if (depth[geomIndex, (int)posIndex] <= 0) 
                return Location.Exterior;
            return Location.Interior;
        }

        /// <summary>
        /// A Depth object is null (has never been initialized) if all depths are null.
        /// </summary>
        public bool IsNull()
        {                        
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (depth[i,j] != @null)
                            return false;
                    }
                }
                return true;            
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <returns></returns>
        public bool IsNull(int geomIndex)
        {
            return depth[geomIndex,1] == @null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public bool IsNull(int geomIndex, Positions posIndex)
        {
            return depth[geomIndex,(int)posIndex] == @null;
        }

        /// <summary>
        /// Normalize the depths for each point, if they are non-null.
        /// A normalized depth
        /// has depth values in the set { 0, 1 }.
        /// Normalizing the depths
        /// involves reducing the depths by the same amount so that at least
        /// one of them is 0.  If the remaining value is > 0, it is set to 1.
        /// </summary>
        public void Normalize()
        {
            for (int i = 0; i < 2; i++)
            {
                if (!IsNull(i))
                {
                    int minDepth = depth[i,1];
                    if (depth[i,2] < minDepth)
                    minDepth = depth[i,2];

                    if (minDepth < 0) minDepth = 0;
                    for (int j = 1; j < 3; j++)
                    {
                        int newValue = 0;
                        if (depth[i,j] > minDepth)
                            newValue = 1;
                        depth[i,j] = newValue;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <param name="depthValue"></param>
        public void SetDepth(int geomIndex, Positions posIndex, int depthValue)
        {
            depth[geomIndex, (int)posIndex] = depthValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("A: {0},{1} B: {2},{3}", depth[0, 1], depth[0, 2], depth[1, 1], depth[1, 2]);
        }

        #endregion
    }
}