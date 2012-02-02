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
using System.Collections;
using System.Linq;

namespace Iesi.Collections
{
    /// <summary>
    /// <p><c>DictionarySet</c> is an abstract class that supports the creation of new <c>Set</c>
    /// types where the underlying data store is an <c>IDictionary</c> instance.</p>
    ///
    /// <p>You can use any object that implements the <c>IDictionary</c> interface to hold set data.
    /// You can define your own, or you can use one of the objects provided in the Framework.
    /// The type of <c>IDictionary</c> you choose will affect both the performance and the behavior
    /// of the <c>Set</c> using it. </p>
    ///
    /// <p>To make a <c>Set</c> typed based on your own <c>IDictionary</c>, simply derive a
    /// new class with a constructor that takes no parameters.  Some <c>Set</c> implmentations
    /// cannot be defined with a default constructor.  If this is the case for your class,
    /// you will need to override <c>Clone()</c> as well.</p>
    ///
    /// <p>It is also standard practice that at least one of your constructors takes an <c>ICollection</c> or
    /// an <c>ISet</c> as an argument.</p>
    /// </summary>
    [Serializable]
    public abstract class DictionarySet : Set
    {
        private static readonly object _placeholderObject = new object();

        /// <summary>
        /// Provides the storage for elements in the <c>Set</c>, stored as the key-set
        /// of the <c>IDictionary</c> object.  Set this object in the constructor
        /// if you create your own <c>Set</c> class.
        /// </summary>
        protected IDictionary InternalDictionary { get; set; }

        /// <summary>
        /// The placeholder object used as the value for the <c>IDictionary</c> instance.
        /// </summary>
        /// <remarks>
        /// There is a single instance of this object globally, used for all <c>Sets</c>.
        /// </remarks>
        protected object Placeholder
        {
            get { return _placeholderObject; }
        }

        /// <summary>
        /// Returns <c>true</c> if this set contains no elements.
        /// </summary>
        public override bool IsEmpty
        {
            get { return InternalDictionary.Count == 0; }
        }

        /// <summary>
        /// The number of elements contained in this collection.
        /// </summary>
        public override int Count
        {
            get { return InternalDictionary.Count; }
        }

        /// <summary>
        /// None of the objects based on <c>DictionarySet</c> are synchronized.  Use the
        /// <c>SyncRoot</c> property instead.
        /// </summary>
        public override bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Returns an object that can be used to synchronize the <c>Set</c> between threads.
        /// </summary>
        public override object SyncRoot
        {
            get { return InternalDictionary.SyncRoot; }
        }

        /// <summary>
        /// Adds the specified element to this set if it is not already present.
        /// </summary>
        /// <param name="o">The object to add to the set.</param>
        /// <returns><c>true</c> is the object was added, <c>false</c> if it was already present.</returns>
        public override bool Add(object o)
        {
            if (InternalDictionary[o] != null)
                return false;
            //The object we are adding is just a placeholder.  The thing we are
            //really concerned with is 'o', the key.
            InternalDictionary.Add(o, _placeholderObject);
            return true;
        }

        /// <summary>
        /// Adds all the elements in the specified collection to the set if they are not already present.
        /// </summary>
        /// <param name="c">A collection of objects to add to the set.</param>
        /// <returns><c>true</c> is the set changed as a result of this operation, <c>false</c> if not.</returns>
        public override bool AddAll(ICollection c)
        {
            return c.Cast<object>().Aggregate(false, (current, o) => current | Add(o));
        }

        /// <summary>
        /// Removes all objects from the set.
        /// </summary>
        public override void Clear()
        {
            InternalDictionary.Clear();
        }

        /// <summary>
        /// Returns <c>true</c> if this set contains the specified element.
        /// </summary>
        /// <param name="o">The element to look for.</param>
        /// <returns><c>true</c> if this set contains the specified element, <c>false</c> otherwise.</returns>
        public override bool Contains(object o)
        {
            return InternalDictionary[o] != null;
        }

        /// <summary>
        /// Returns <c>true</c> if the set contains all the elements in the specified collection.
        /// </summary>
        /// <param name="c">A collection of objects.</param>
        /// <returns><c>true</c> if the set contains all the elements in the specified collection, <c>false</c> otherwise.</returns>
        public override bool ContainsAll(ICollection c)
        {
            return c.Cast<object>().All(Contains);
        }

        /// <summary>
        /// Removes the specified element from the set.
        /// </summary>
        /// <param name="o">The element to be removed.</param>
        /// <returns><c>true</c> if the set contained the specified element, <c>false</c> otherwise.</returns>
        public override bool Remove(object o)
        {
            bool contained = Contains(o);
            if (contained)
            {
                InternalDictionary.Remove(o);
            }
            return contained;
        }

        /// <summary>
        /// Remove all the specified elements from this set, if they exist in this set.
        /// </summary>
        /// <param name="c">A collection of elements to remove.</param>
        /// <returns><c>true</c> if the set was modified as a result of this operation.</returns>
        public override bool RemoveAll(ICollection c)
        {
            return c.Cast<object>().Aggregate(false, (current, o) => current | Remove(o));
        }

        /// <summary>
        /// Retains only the elements in this set that are contained in the specified collection.
        /// </summary>
        /// <param name="c">Collection that defines the set of elements to be retained.</param>
        /// <returns><c>true</c> if this set changed as a result of this operation.</returns>
        public override bool RetainAll(ICollection c)
        {
            //Put data from C into a set so we can use the Contains() method.
            Set cSet = new HybridSet(c);

            //We are going to build a set of elements to remove.
            Set removeSet = new HybridSet();

            foreach (object o in this)
            {
                //If C does not contain O, then we need to remove O from our
                //set.  We can't do this while iterating through our set, so
                //we put it into RemoveSet for later.
                if (!cSet.Contains(o))
                    removeSet.Add(o);
            }

            return RemoveAll(removeSet);
        }

        /// <summary>
        /// Copies the elements in the <c>Set</c> to an array.  The type of array needs
        /// to be compatible with the objects in the <c>Set</c>, obviously.
        /// </summary>
        /// <param name="array">An array that will be the target of the copy operation.</param>
        /// <param name="index">The zero-based index where copying will start.</param>
        public override void CopyTo(Array array, int index)
        {
            InternalDictionary.Keys.CopyTo(array, index);
        }

        /// <summary>
        /// Gets an enumerator for the elements in the <c>Set</c>.
        /// </summary>
        /// <returns>An <c>IEnumerator</c> over the elements in the <c>Set</c>.</returns>
        public override IEnumerator GetEnumerator()
        {
            return InternalDictionary.Keys.GetEnumerator();
        }
    }
}