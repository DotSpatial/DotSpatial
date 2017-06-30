// ********************************************************************************************************
// Product Name: DotSpatial.Interfaces Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// Contains properties for both a specified item and an integer index
    /// as well as the option to cancel.
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class IndividualDictionaryCancelEventArgs<TKey, TValue> : CancelEventArgs
    {
        #region Private Variables

        private TKey _key;
        private TValue _value;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inKey">The key that provides direct access to the value member being interacted with</param>
        /// <param name="inValue">an object that is being interacted with in the list</param>
        public IndividualDictionaryCancelEventArgs(TKey inKey, TValue inValue)
        {
            _value = inValue;
            _key = inKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the key for the member referenced by this event
        /// </summary>
        public TKey Key
        {
            get { return _key; }
            protected set { _key = value; }
        }

        /// <summary>
        /// Gets the specific item being accessed
        /// </summary>
        public TValue Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion
    }
}