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

namespace DotSpatial.Topology.GeometriesGraph.Index
{
    /// <summary>
    ///
    /// </summary>
    public class SweepLineEvent : IComparable
    {
        #region Constant Fields

        private const int Delete = 2;
        private const int Insert = 1;

        #endregion

        #region Fields

        private readonly int _eventType;
        private readonly SweepLineEvent _insertEvent; // null if this is an Insert event
        private readonly object _label; // used for red-blue intersection detection
        private readonly object _obj;
        private readonly double _xValue;
        private int _deleteEventIndex;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an INSERT event.
        /// </summary>
        /// <param name="label">The edge set label for this object.</param>
        /// <param name="x">The event location</param>
        /// <param name="obj">the object being inserted</param>
        public SweepLineEvent(object label, double x, object obj)
        {
            _eventType = Insert;
            _label = label;
            _xValue = x;
            _obj = obj;
        }

        /// <summary>
        /// Creates a DELETE event.
        /// </summary>
        /// <param name="x">The event location</param>
        /// <param name="insertEvent">The corresponding INSERT event</param>
        public SweepLineEvent(double x, SweepLineEvent insertEvent)
        {
            _eventType = Delete;
            _xValue = x;
            _insertEvent = insertEvent;
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public int DeleteEventIndex
        {
            get { return _deleteEventIndex; }
            set { _deleteEventIndex = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public SweepLineEvent InsertEvent
        {
            get { return _insertEvent; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsDelete
        {
            get { return _eventType == Delete; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsInsert
        {
            get { return _eventType == Insert; }
        }

        /// <summary>
        ///
        /// </summary>
        public object Object
        {
            get { return _obj; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Events are ordered first by their x-value, and then by their eventType.
        /// Insert events are sorted before Delete events, so that
        /// items whose Insert and Delete events occur at the same x-value will be
        /// correctly handled.
        /// </summary>
        /// <param name="o"></param>
        public int CompareTo(object o)
        {
            SweepLineEvent pe = (SweepLineEvent)o;
            if (_xValue < pe._xValue)
                return -1;
            if (_xValue > pe._xValue)
                return 1;
            if (_eventType < pe._eventType)
                return -1;
            if (_eventType > pe._eventType)
                return 1;
            return 0;
        }

        public bool IsSameLabel(SweepLineEvent ev)
        {
            // no label set indicates single group
            if (_label == null)
                return false;
            return _label == ev._label;
        }

        #endregion
    }
}