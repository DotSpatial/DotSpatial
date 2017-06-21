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
    /// Bjoern Heckel's solution to the KD-Tree n-nearest-neighbor problem
    /// </summary>
    public class FarthestNeighborList
    {
        private readonly SortedList<double, object> _list;

        /// <summary>
        /// Creates a new NearestNeighborList
        /// </summary>
        /// <param name="capacity">An integer indicating the maximum size for the cue</param>
        public FarthestNeighborList(int capacity)
        {
            _list = new SortedList<double, object>(capacity);
        }

        /// <summary>
        /// Gets the minimum priority, or distance.  Since we are looking for the maximum distance, or the
        /// n maximum distances, we want to determine quickly the lowest distance currently contained
        /// in the cue.
        /// </summary>
        /// <returns></returns>
        public double MinimumPriority
        {
            get
            {
                if (_list.Count == 0) return 0;
                return _list.Keys[0];
            }
        }

        /// <summary>
        /// Gets whether or not the length of the cue has reached the capacity
        /// </summary>
        public bool IsCapacityReached
        {
            get
            {
                //   return m_Queue.length() >= m_Capacity;
                return _list.Count == _list.Capacity;
            }
        }

        /// <summary>
        /// Gets the highest object in the nearest neighbor list
        /// </summary>
        /// <returns></returns>
        public object Farthest
        {
            get
            {
                //return m_Queue.front();
                if (_list.Count == 0) return null;
                return _list.Values[_list.Count - 1];
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether or not the cue is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty
        {
            get
            {
                //return m_Queue.length() == 0;
                return _list.Count == 0;
            }
        }

        /// <summary>
        /// Gets the length of the current list
        /// </summary>
        public int Size
        {
            get
            {
                // return m_Queue.length();
                return _list.Count;
            }
        }

        /// <summary>
        /// Inserts an object with a given priority
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public bool Insert(Object obj, double priority)
        {
            if (_list.Count < _list.Capacity)
            {
                // capacity not reached
                _list.Add(priority, obj);
                return true;
            }
            //if (priority > m_Queue.getMaxPriority())
            //{
            //    // do not insert - all elements in queue have lower priority
            //    return false;
            //}

            if (priority < _list.Keys[0])
            {
                // do not insert, all elements in queue have higher priority
                return false;
            }

            //// remove object with highest priority
            //m_Queue.remove();

            _list.RemoveAt(0);
            //m_Queue.remove();
            // add new object
            _list.Add(priority, obj);
            // m_Queue.add(_object, priority);
            return true;
        }

        /// <summary>
        /// Removes the highest member from the cue and returns that object.
        /// </summary>
        /// <returns></returns>
        public object RemoveFarthest()
        {
            // remove object with highest priority
            //return m_Queue.remove();
            if (_list.Count > 0)
            {
                int i = _list.Count - 1;
                object result = _list.Values[i];
                _list.RemoveAt(i);
                return result;
            }
            return null;
        }
    }
}