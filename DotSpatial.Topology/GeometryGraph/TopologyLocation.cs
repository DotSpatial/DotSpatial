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

using System.Collections.Generic;
using System.Text;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// A TopologyLocation is the labelling of a
    /// GraphComponent's topological relationship to a single Geometry.
    /// If the parent component is an area edge, each side and the edge itself
    /// have a topological location.  These locations are named:
    ///  On: on the edge
    ///  Left: left-hand side of the edge
    ///  Right: right-hand side
    /// If the parent component is a line edge or node, there is a single
    /// topological relationship attribute, On.
    /// The possible values of a topological location are
    /// { Location.Null, Location.Exterior, Location.Boundary, Location.Interior }
    /// The labelling is stored in an array location[j] where
    /// where j has the values On, Left, Right.
    /// </summary>
    public class TopologyLocation
    {
        private LocationType[] _location;

        /// <summary>
        ///
        /// </summary>
        /// <param name="location"></param>
        public TopologyLocation(ICollection<LocationType> location)
        {
            Init(location.Count);
        }

        /// <summary>
        /// Constructs a TopologyLocation specifying how points on, to the left of, and to the
        /// right of some GraphComponent relate to some Geometry. Possible values for the
        /// parameters are Location.Null, Location.Exterior, Location.Boundary,
        /// and Location.Interior.
        /// </summary>
        /// <param name="on"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public TopologyLocation(LocationType on, LocationType left, LocationType right)
        {
            Init(3);
            _location[(int)PositionType.On] = on;
            _location[(int)PositionType.Left] = left;
            _location[(int)PositionType.Right] = right;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="on"></param>
        public TopologyLocation(LocationType on)
        {
            Init(1);
            _location[(int)PositionType.On] = on;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gl"></param>
        public TopologyLocation(TopologyLocation gl)
        {
            if (gl == null) return;
            Init(gl._location.Length);
            for (int i = 0; i < _location.Length; i++)
                _location[i] = gl._location[i];
        }

        /// <summary>
        /// Get calls Get(Positions posIndex),
        /// Set calls SetLocation(Positions locIndex, Locations locValue)
        /// </summary>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public virtual LocationType this[PositionType posIndex]
        {
            get
            {
                return Get(posIndex);
            }
            set
            {
                SetLocation(posIndex, value);
            }
        }

        /// <returns>
        /// <c>true</c> if all locations are Null.
        /// </returns>
        public virtual bool IsNull
        {
            get
            {
                for (int i = 0; i < _location.Length; i++)
                    if (_location[i] != LocationType.Null)
                        return false;
                return true;
            }
        }

        /// <returns>
        /// <c>true</c> if any locations are Null.
        /// </returns>
        public virtual bool IsAnyNull
        {
            get
            {
                for (int i = 0; i < _location.Length; i++)
                    if (_location[i] == LocationType.Null)
                        return true;
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsArea
        {
            get
            {
                return _location.Length > 1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsLine
        {
            get
            {
                return _location.Length == 1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="size"></param>
        private void Init(int size)
        {
            _location = new LocationType[size];
            SetAllLocations(LocationType.Null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="posIndex"></param>
        /// <returns></returns>
        public virtual LocationType Get(PositionType posIndex)
        {
            int index = (int)posIndex;
            if (index < _location.Length)
                return _location[index];
            return LocationType.Null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="le"></param>
        /// <param name="locIndex"></param>
        /// <returns></returns>
        public virtual bool IsEqualOnSide(TopologyLocation le, int locIndex)
        {
            return _location[locIndex] == le._location[locIndex];
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Flip()
        {
            if (_location.Length <= 1)
                return;
            LocationType temp = _location[(int)PositionType.Left];
            _location[(int)PositionType.Left] = _location[(int)PositionType.Right];
            _location[(int)PositionType.Right] = temp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="locValue"></param>
        public virtual void SetAllLocations(LocationType locValue)
        {
            for (int i = 0; i < _location.Length; i++)
                _location[i] = locValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="locValue"></param>
        public virtual void SetAllLocationsIfNull(LocationType locValue)
        {
            for (int i = 0; i < _location.Length; i++)
                if (_location[i] == LocationType.Null)
                    _location[i] = locValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="locIndex"></param>
        /// <param name="locValue"></param>
        public virtual void SetLocation(PositionType locIndex, LocationType locValue)
        {
            _location[(int)locIndex] = locValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="locValue"></param>
        public virtual void SetLocation(LocationType locValue)
        {
            SetLocation(PositionType.On, locValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual LocationType[] GetLocations()
        {
            return _location;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="on"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public virtual void SetLocations(LocationType on, LocationType left, LocationType right)
        {
            _location[(int)PositionType.On] = on;
            _location[(int)PositionType.Left] = left;
            _location[(int)PositionType.Right] = right;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gl"></param>
        public virtual void SetLocations(TopologyLocation gl)
        {
            for (int i = 0; i < gl._location.Length; i++)
                _location[i] = gl._location[i];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public virtual bool AllPositionsEqual(LocationType loc)
        {
            for (int i = 0; i < _location.Length; i++)
                if (_location[i] != loc)
                    return false;
            return true;
        }

        /// <summary>
        /// Merge updates only the Null attributes of this object
        /// with the attributes of another.
        /// </summary>
        public virtual void Merge(TopologyLocation gl)
        {
            // if the src is an Area label & and the dest is not, increase the dest to be an Area
            if (gl._location.Length > _location.Length)
            {
                LocationType[] newLoc = new LocationType[3];
                newLoc[(int)PositionType.On] = _location[(int)PositionType.On];
                newLoc[(int)PositionType.Left] = LocationType.Null;
                newLoc[(int)PositionType.Right] = LocationType.Null;
                _location = newLoc;
            }
            for (int i = 0; i < _location.Length; i++)
                if (_location[i] == LocationType.Null && i < gl._location.Length)
                    _location[i] = gl._location[i];
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (_location.Length > 1)
                sb.Append(Location.ToLocationSymbol(_location[(int)PositionType.Left]));
            sb.Append(Location.ToLocationSymbol(_location[(int)PositionType.On]));
            if (_location.Length > 1)
                sb.Append(Location.ToLocationSymbol(_location[(int)PositionType.Right]));
            return sb.ToString();
        }
    }
}