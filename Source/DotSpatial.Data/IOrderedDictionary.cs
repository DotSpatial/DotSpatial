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
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// This has the indexing and ordering capabilities, but without all the events of an EventDictionary
    /// </summary>
    [Obsolete("Do not use it. This interface is not used in DotSpatial anymore.")] // Marked in 1.7
    public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Two separate forms of count exist and are ambiguous so this provides a single new count
        /// </summary>
        new int Count
        {
            get;
        }

        /// <summary>
        /// For ordered dictionaries, the default accessor returns the Key Value Pair associated with the
        /// index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        KeyValuePair<TKey, TValue> this[int index]
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether the IEventDictionary&lt;TKey, TValue&gt; contains a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the IEventDictionary&lt;TKey, TValue&gt;. The value can be null for reference types.</param>
        /// <returns>true if the IEventDictionary&lt;TKey, TValue&gt; contains an element with the specified value; otherwise, false.</returns>
        bool ContainsValue(TValue value);

        /// <summary>
        /// Obtains a value in the dictionary based on a key.  This happens without consulting the
        /// index at all.
        /// </summary>
        /// <param name="key">The Tkey key to search for.</param>
        /// <returns>The TValue to obtain a value for. </returns>
        TValue GetValue(TKey key);

        /// <summary>
        /// Sets the value associated with a key that is already in the dictionary.
        /// </summary>
        /// <param name="key">The key currently found in the index</param>
        /// <param name="value">the value to be changed</param>
        void SetValue(TKey key, TValue value);

        /// <summary>
        /// Gets a pair based on the index value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        KeyValuePair<TKey, TValue> GetIndex(int index);

        /// <summary>
        /// Sets a pair based on the index value
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        void SetIndex(int index, KeyValuePair<TKey, TValue> item);
    }
}