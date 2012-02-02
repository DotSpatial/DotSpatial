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
/* Copyright © 2002-2004 by Aidant Systems, Inc., and by Jason Smith. */

using System;
using System.Collections.Generic;

namespace Iesi.Collections.Generic
{
    /// <summary>
    /// Implements a <c>Set</c> based on a sorted tree.  This gives good performance for operations on very
    /// large data-sets, though not as good - asymptotically - as a <c>HashedSet</c>.  However, iteration
    /// occurs in order.  Elements that you put into this type of collection must implement <c>IComparable</c>,
    /// and they must actually be comparable.  You can't mix <c>string</c> and <c>int</c> values, for example.
    /// </summary>
    [Serializable]
    public class SortedSet<T> : DictionarySet<T>
    {
        /// <summary>
        /// Creates a new set instance based on a sorted tree.
        /// </summary>
        public SortedSet()
        {
            InternalDictionary = new SortedDictionary<T, object>();
        }

        /// <summary>
        /// Creates a new set instance based on a sorted tree.
        /// </summary>
        /// <param name="comparer">The IComparer to use for sorting.</param>
        public SortedSet(IComparer<T> comparer)
        {
            InternalDictionary = new SortedDictionary<T, object>(comparer);
        }

        /// <summary>
        /// Creates a new set instance based on a sorted tree and
        /// initializes it based on a collection of elements.
        /// </summary>
        /// <param name="initialValues">A collection of elements that defines the initial set contents.</param>
        public SortedSet(ICollection<T> initialValues)
            : this()
        {
            base.AddAll(initialValues);
        }

        /// <summary>
        /// Creates a new set instance based on a sorted tree and
        /// initializes it based on a collection of elements.
        /// </summary>
        /// <param name="initialValues">A collection of elements that defines the initial set contents.</param>
        /// <param name="comparer">The IComparer to use for sorting.</param>
        public SortedSet(ICollection<T> initialValues, IComparer<T> comparer)
            : this(comparer)
        {
            base.AddAll(initialValues);
        }
    }
}