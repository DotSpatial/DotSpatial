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
    /// This class implements a PriorityQueue. This class
    /// is implemented in such a way that objects are added using an
    /// add function. The add function takes
    /// two parameters an object and a long.
    ///
    /// The object represents an item in the queue, the long indicates
    /// its priority in the queue. The remove function in this class
    /// returns the object first in the queue and that object is removed
    /// from the queue permanently.
    ///
    /// @author Simon Levy
    /// Translation by Marco A. Alvarez
    /// </summary>
    internal class PriorityQueue
    {
        /**
         * The maximum priority possible in this priority queue.
         */
        private readonly double _maxPriority = 1.79769e+30; //Double.MaxValue;
        private int _capacity;
        /**
         * Holds the number of elements currently in the queue.
         */
        private int _count;

        /**
         * This contains the list of objects in the queue.
         */
        private Object[] _data;

        /**
         * This contains the list of prioritys in the queue.
         */
        private double[] _value;

        /**
         * Creates a new <code>PriorityQueue</code> object. The
         * <code>PriorityQueue</code> object allows objects to be
         * entered into the queue and to leave in the order of
         * priority i.e the highest priority get's to leave first.
         */
        public PriorityQueue()
        {
            Init(20);
        }

        /**
         * Creates a new <code>PriorityQueue</code> object. The
         * <code>PriorityQueue</code> object allows objects to
         * be entered into the queue an to leave in the order of
         * priority i.e the highest priority get's to leave first.
         *
         * @param capacity the initial capacity of the queue before
         * a resize
         */

        public PriorityQueue(int capacity)
        {
            Init(capacity);
        }

        /**
         * Creates a new <code>PriorityQueue</code> object. The
         * <code>PriorityQueue</code> object allows objects to
         * be entered into the queue an to leave in the order of
         * priority i.e the highest priority get's to leave first.
         *
         * @param capacity the initial capacity of the queue before
         * a resize
         * @param maxPriority is the maximum possible priority for
         * an object
         */

        public PriorityQueue(int capacity, double maxPriority)
        {
            _maxPriority = maxPriority;
            Init(capacity);
        }

        /**
         * This is an initializer for the object. It basically initializes
         * an array of long called value to represent the prioritys of
         * the objects, it also creates an array of objects to be used
         * in parallel with the array of longs, to represent the objects
         * entered, these can be used to sequence the data.
         *
         * @param size the initial capacity of the queue, it can be
         * resized
         */

        private void Init(int size)
        {
            _capacity = size;
            _data = new Object[_capacity + 1];
            _value = new double[_capacity + 1];
            _value[0] = _maxPriority;
            _data[0] = null;
        }

        /**
         * This function adds the given object into the <code>PriorityQueue</code>,
         * its priority is the long priority. The way in which priority can be
         * associated with the elements of the queue is by keeping the priority
         * and the elements array entrys parallel.
         *
         * @param element is the object that is to be entered into this
         * <code>PriorityQueue</code>
         * @param priority this is the priority that the object holds in the
         * <code>PriorityQueue</code>
         */

        public void Add(Object element, double priority)
        {
            if (_count++ >= _capacity)
            {
                ExpandCapacity();
            }
            /* put this as the last element */
            _value[_count] = priority;
            _data[_count] = element;
            BubbleUp(_count);
        }

        /**
         * Remove is a function to remove the element in the queue with the
         * maximum priority. Once the element is removed then it can never be
         * recovered from the queue with further calls. The lowest priority
         * object will leave last.
         *
         * @return the object with the highest priority or if it's empty
         * null
         */

        public Object Remove()
        {
            if (_count == 0)
                return null;
            Object element = _data[1];
            /* swap the last element into the first */
            _data[1] = _data[_count];
            _value[1] = _value[_count];
            /* let the GC clean up */
            _data[_count] = null;
            _value[_count] = 0L;
            _count--;
            BubbleDown(1);
            return element;
        }

        public Object Front()
        {
            return _data[1];
        }

        public double GetMaxPriority()
        {
            return _value[1];
        }

        /**
         * Bubble down is used to put the element at subscript 'pos' into
         * it's rightful place in the heap (i.e heap is another name
         * for <code>PriorityQueue</code>). If the priority of an element
         * at subscript 'pos' is less than it's children then it must
         * be put under one of these children, i.e the ones with the
         * maximum priority must come first.
         *
         * @param pos is the position within the arrays of the element
         * and priority
         */

        private void BubbleDown(int pos)
        {
            Object element = _data[pos];
            double priority = _value[pos];
            int child;
            /* hole is position '1' */
            for (; pos * 2 <= _count; pos = child)
            {
                child = pos * 2;
                /* if 'child' equals 'count' then there
                   is only one leaf for this parent */
                if (child != _count)

                    /* left_child > right_child */
                    if (_value[child] < _value[child + 1])
                        child++; /* choose the biggest child */
                /* percolate down the data at 'pos', one level
                   i.e biggest child becomes the parent */
                if (priority < _value[child])
                {
                    _value[pos] = _value[child];
                    _data[pos] = _data[child];
                }
                else
                {
                    break;
                }
            }
            _value[pos] = priority;
            _data[pos] = element;
        }

        /**
         * Bubble up is used to place an element relatively low in the
         * queue to it's rightful place higher in the queue, but only
         * if it's priority allows it to do so, similar to bubbleDown
         * only in the other direction this swaps out its parents.
         *
         * @param pos the position in the arrays of the object
         * to be bubbled up
         */

        private void BubbleUp(int pos)
        {
            Object element = _data[pos];
            double priority = _value[pos];
            /* when the parent is not less than the child, end*/
            while (_value[pos / 2] < priority)
            {
                /* overwrite the child with the parent */
                _value[pos] = _value[pos / 2];
                _data[pos] = _data[pos / 2];
                pos /= 2;
            }
            _value[pos] = priority;
            _data[pos] = element;
        }

        /**
         * This ensures that there is enough space to keep adding elements
         * to the priority queue. It is however advised to make the capacity
         * of the queue large enough so that this will not be used as it is
         * an expensive method. This will copy across from 0 as 'off' equals
         * 0 is contains some important data.
         */

        private void ExpandCapacity()
        {
            _capacity = _count * 2;
            Object[] elements = new Object[_capacity + 1];
            double[] prioritys = new double[_capacity + 1];
            Array.Copy(_data, 0, elements, 0, _data.Length);
            Array.Copy(_value, 0, prioritys, 0, _data.Length);
            _data = elements;
            _value = prioritys;
        }

        /**
         * This method will empty the queue. This also helps garbage
         * collection by releasing any reference it has to the elements
         * in the queue. This starts from offset 1 as off equals 0
         * for the elements array.
         */

        public void Clear()
        {
            for (int i = 1; i < _count; i++)
            {
                _data[i] = null; /* help gc */
            }
            _count = 0;
        }

        /**
         * The number of elements in the queue. The length
         * indicates the number of elements that are currently
         * in the queue.
         *
         * @return the number of elements in the queue
         */

        public int Length()
        {
            return _count;
        }
    }
}