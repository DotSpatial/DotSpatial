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

namespace DotSpatial.Topology.Index.Sweepline
{
    /// <summary>
    ///
    /// </summary>
    public class SweepLineEvent : IComparable
    {
        #region SweepLineEventType enum

        /// <summary>
        ///
        /// </summary>
        public enum SweepLineEventType
        {
            /// <summary>
            ///
            /// </summary>
            Insert = 1,

            /// <summary>
            ///
            /// </summary>
            Delete = 2,
        }

        #endregion

        private readonly SweepLineEventType _eventType;
        private readonly SweepLineEvent _insertEvent; // null if this is an Insert event

        private readonly SweepLineInterval _sweepInt;
        private readonly double _xValue;
        private int _deleteEventIndex;

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="insertEvent"></param>
        /// <param name="sweepInt"></param>
        public SweepLineEvent(double x, SweepLineEvent insertEvent, SweepLineInterval sweepInt)
        {
            _xValue = x;
            _insertEvent = insertEvent;
            _eventType = insertEvent != null ? SweepLineEventType.Delete : SweepLineEventType.Insert;
            _sweepInt = sweepInt;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsInsert
        {
            get
            {
                return _insertEvent == null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsDelete
        {
            get
            {
                return _insertEvent != null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual SweepLineEvent InsertEvent
        {
            get
            {
                return _insertEvent;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int DeleteEventIndex
        {
            get
            {
                return _deleteEventIndex;
            }
            set
            {
                _deleteEventIndex = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual SweepLineInterval Interval
        {
            get
            {
                return _sweepInt;
            }
        }

        #region IComparable Members

        /// <summary>
        /// ProjectionEvents are ordered first by their x-value, and then by their eventType.
        /// It is important that Insert events are sorted before Delete events, so that
        /// items whose Insert and Delete events occur at the same x-value will be
        /// correctly handled.
        /// </summary>
        /// <param name="o"></param>
        public virtual int CompareTo(object o)
        {
            SweepLineEvent pe = (SweepLineEvent)o;
            if (_xValue < pe._xValue) return -1;
            if (_xValue > pe._xValue) return 1;
            if (_eventType < pe._eventType) return -1;
            if (_eventType > pe._eventType) return 1;
            return 0;
        }

        #endregion
    }
}