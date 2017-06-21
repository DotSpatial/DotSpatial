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

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// A Depth object records the topological depth of the sides
    /// of an Edge for up to two Geometries.
    /// </summary>
    public class Depth
    {
        /// <summary>
        ///
        /// </summary>
        private const int NULL = -1;

        private readonly int[,] _depth = new int[2, 3];

        /// <summary>
        ///
        /// </summary>
        public Depth()
        {
            // initialize depth array to a sentinel value
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 3; j++)
                    _depth[i, j] = NULL;
        }

        /// <summary>
        /// Calls GetDepth and SetDepth.
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public virtual int this[int geomIndex, PositionType posIndex]
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static int DepthAtLocation(LocationType location)
        {
            if (location == LocationType.Exterior)
                return 0;

            return location == LocationType.Interior ? 1 : NULL;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public virtual int GetDepth(int geomIndex, PositionType posIndex)
        {
            return _depth[geomIndex, (int)posIndex];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <param name="depthValue"></param>
        public virtual void SetDepth(int geomIndex, PositionType posIndex, int depthValue)
        {
            _depth[geomIndex, (int)posIndex] = depthValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public virtual LocationType GetLocation(int geomIndex, PositionType posIndex)
        {
            if (_depth[geomIndex, (int)posIndex] <= 0)
                return LocationType.Exterior;
            return LocationType.Interior;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <param name="location"></param>
        public virtual void Add(int geomIndex, PositionType posIndex, LocationType location)
        {
            if (location == LocationType.Interior)
                _depth[geomIndex, (int)posIndex]++;
        }

        /// <summary>
        /// A Depth object is null (has never been initialized) if all depths are null.
        /// </summary>
        public virtual bool IsNull()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_depth[i, j] != NULL)
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
        public virtual bool IsNull(int geomIndex)
        {
            return _depth[geomIndex, 1] == NULL;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public virtual bool IsNull(int geomIndex, PositionType posIndex)
        {
            return _depth[geomIndex, (int)posIndex] == NULL;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lbl"></param>
        public virtual void Add(Label lbl)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    LocationType loc = lbl.GetLocation(i, (PositionType)j);
                    if (loc == LocationType.Exterior || loc == LocationType.Interior)
                    {
                        // initialize depth if it is null, otherwise add this location value
                        if (IsNull(i, (PositionType)j))
                            _depth[i, j] = DepthAtLocation(loc);
                        else _depth[i, j] += DepthAtLocation(loc);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <returns></returns>
        public virtual int GetDelta(int geomIndex)
        {
            return _depth[geomIndex, (int)PositionType.Right] - _depth[geomIndex, (int)PositionType.Left];
        }

        /// <summary>
        /// Normalize the depths for each point, if they are non-null.
        /// A normalized depth
        /// has depth values in the set { 0, 1 }.
        /// Normalizing the depths
        /// involves reducing the depths by the same amount so that at least
        /// one of them is 0.  If the remaining value is > 0, it is set to 1.
        /// </summary>
        public virtual void Normalize()
        {
            for (int i = 0; i < 2; i++)
            {
                if (!IsNull(i))
                {
                    int minDepth = _depth[i, 1];
                    if (_depth[i, 2] < minDepth)
                        minDepth = _depth[i, 2];

                    if (minDepth < 0) minDepth = 0;
                    for (int j = 1; j < 3; j++)
                    {
                        int newValue = 0;
                        if (_depth[i, j] > minDepth)
                            newValue = 1;
                        _depth[i, j] = newValue;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "A: " + _depth[0, 1] + ", " + _depth[0, 2]
                 + " B: " + _depth[1, 1] + ", " + _depth[1, 2];
        }
    }
}