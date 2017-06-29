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
    /// Bjoern Heckel's solution to the KD-Tree n-nearest-neighbor problem
    /// </summary>
    public class NearestNeighborList
    {
        /// <summary>
        /// Indicates whether removal should occur from the highest value or lowest value
        /// </summary>
        public const int REMOVE_HIGHEST = 1;

        /// <summary>
        ///
        /// </summary>
        public const int REMOVE_LOWEST = 2;

        readonly int _capacity;
        readonly PriorityQueue _queue;

        /// <summary>
        /// Creates a new NearestNeighborList
        /// </summary>
        /// <param name="capacity">An integer indicating the maximum size for the cue</param>
        public NearestNeighborList(int capacity)
        {
            _capacity = capacity;
            _queue = new PriorityQueue(_capacity, Double.PositiveInfinity);
        }

        /// <summary>
        /// Gets the maximum priority
        /// </summary>
        /// <returns></returns>
        public double MaxPriority
        {
            get
            {
                if (_queue.Length() == 0)
                {
                    return Double.PositiveInfinity;
                }
                return _queue.GetMaxPriority();
            }
        }

        /// <summary>
        /// Gets whether or not the length fo the cue has reached the capacity
        /// </summary>
        public bool IsCapacityReached
        {
            get
            {
                return _queue.Length() >= _capacity;
            }
        }

        /// <summary>
        /// Gets the highest object in the nearest neighbor list
        /// </summary>
        /// <returns></returns>
        public Object Highest
        {
            get
            {
                return _queue.Front();
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
                return _queue.Length() == 0;
            }
        }

        /// <summary>
        /// Gets the length of the current list
        /// </summary>
        public int Size
        {
            get
            {
                return _queue.Length();
            }
        }

        /// <summary>
        /// Inserts an item with a given priority
        /// </summary>
        /// <param name="item"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public bool Insert(object item, double priority)
        {
            if (_queue.Length() < _capacity)
            {
                // capacity not reached
                _queue.Add(item, priority);
                return true;
            }
            if (priority > _queue.GetMaxPriority())
            {
                // do not insert - all elements in queue have lower priority
                return false;
            }
            // remove item with highest priority
            _queue.Remove();
            // add new item
            _queue.Add(item, priority);
            return true;
        }

        /// <summary>
        /// Removes the highest member from the cue and returns that object.
        /// </summary>
        /// <returns></returns>
        public Object RemoveHighest()
        {
            // remove object with highest priority
            return _queue.Remove();
        }
    }
}