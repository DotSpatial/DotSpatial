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

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class IndividualDictionaryEventArgs<TKey, TValue> : EventArgs
    {
        private TKey _key;
        private TValue _value;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inKey">The key of type &lt;TKey&gt; being referenced</param>
        /// <param name="inValue">an object of type &lt;TValue&gt; that is being referenced</param>
        public IndividualDictionaryEventArgs(TKey inKey, TValue inValue)
        {
            _value = inValue;
            _key = inKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the key of the item being referenced by this event
        /// </summary>
        public TKey Key
        {
            get { return _key; }
            protected set { _key = value; }
        }

        /// <summary>
        /// Gets the actual item being referenced by this event
        /// </summary>
        public TValue Value
        {
            get { return _value; }
            protected set { _value = value; }
        }

        #endregion
    }
}