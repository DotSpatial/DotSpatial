// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList
    /// </summary>
    public class CollectiveIndex<T> : EventArgs
    {
        private IEnumerable<T> _collection;
        private int _index = -1;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inCollection">The IEnumerable&lt;T&gt; specified during the event"/></param>
        /// <param name="inIndex">The integer index associated with this event</param>
        public CollectiveIndex(IEnumerable<T> inCollection, int inIndex)
        {
            _index = inIndex;
            _collection = inCollection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the IEnumerable&lt;T&gt; collection of items involved in this event
        /// </summary>
        public IEnumerable<T> Collection
        {
            get { return _collection; }
            protected set { _collection = value; }
        }

        /// <summary>
        /// Gets the index in the list where the event is associated
        /// </summary>
        public int Index
        {
            get { return _index; }
            protected set { _index = value; }
        }

        #endregion
    }
}