// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/29/2009 2:08:58 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Descriptors are simple classes that are used for storing symbology or other basic characteristics.
    /// They are presumed to be made up of value types and other descriptors, and are expected to be serializable.
    /// This being said, some basic capabilities are supported for randomizing, copying and comparing the
    /// properties of descriptors.
    /// </summary>
    public class Descriptor : CopyBase, IDescriptor
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of MatchableObject
        /// </summary>
        protected Descriptor()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// For each of the publicly accessible properties found on this object, this attempts
        /// to copy a public property from the source object of the same name, if it can find it.
        /// For each matching property name/type, it will attempt to copy the value.
        /// </summary>
        /// <param name="source">The</param>
        public void CopyProperties(object source)
        {
            OnCopyProperties(source);
        }

        /// <summary>
        /// Compares the properties of this object with the specified IMatchable other.
        /// This does not test every property of other, but does test every property
        /// of this item.  As long as the other item has corresponding properties
        /// for every property on this item, the items are said to match.
        /// The IMatchable interface allows custom definitions of matching.
        /// For collections to match, all of their sub-members must match.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="mismatchedProperties"></param>
        /// <returns></returns>
        public bool Matches(IMatchable other, out List<string> mismatchedProperties)
        {
            mismatchedProperties = new List<string>();
            return OnMatch(other, mismatchedProperties);
        }

        /// <summary>
        /// The default behavior is to cycle through all the properties of this
        /// object, and call Randomize on any that implement the IRandomizable interface.
        /// </summary>
        /// <param name="generator">The Random seed generator for controling how the random content is created</param>
        public void Randomize(Random generator)
        {
            OnRandomize(generator);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This occurs while copying properties from the specified source, and
        /// is the default handling for subclasses
        /// </summary>
        /// <param name="source"></param>
        protected virtual void OnCopyProperties(object source)
        {
            Type original = GetType();
            Type copy = source.GetType();
            PropertyInfo[] originalProperties = DistinctNames(original.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            PropertyInfo[] copyProperties = DistinctNames(copy.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            foreach (PropertyInfo originalProperty in originalProperties)
            {
                if (originalProperty.CanWrite == false) continue;
                if (copyProperties.Contains(originalProperty.Name) == false) continue;
                PropertyInfo copyProperty = copyProperties.GetFirst(originalProperty.Name);
                if (copyProperty == null)
                {
                    // The name of the property was not found on the other object
                    continue;
                }
                object copyValue = copyProperty.GetValue(source, null);
                if (copyProperty.GetCustomAttributes(typeof(ShallowCopy), true).Length == 0)
                {
                    ICloneable cloneable = copyValue as ICloneable;
                    if (cloneable != null)
                    {
                        originalProperty.SetValue(this, cloneable.Clone(), null);
                        continue;
                    }
                }
                // Use a shallow copy where ICloneable is not specifically defined.
                originalProperty.SetValue(this, copyValue, null);
            }

            // Public Fields ---------------------------------------------------------
            FieldInfo[] originalFields = original.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] copyFields = copy.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo originalField in originalFields)
            {
                FieldInfo copyField = copyFields.GetFirst(originalField.Name);
                if (copyFields.Contains(originalField.Name) == false) continue;
                if (copyField == null)
                {
                    continue;
                }

                // If it can be cloned, clone it instead of using a reference copy
                object copyValue = copyField.GetValue(source);
                if (copyField.GetCustomAttributes(typeof(ShallowCopy), true).Length == 0)
                {
                    ICloneable cloneable = copyValue as ICloneable;
                    if (cloneable != null)
                    {
                        originalField.SetValue(this, cloneable.Clone());
                        continue;
                    }
                }

                // Use a reference copy only when there is no other alternative.
                originalField.SetValue(this, copyValue);
            }
        }

        /// <summary>
        /// This gives sub-classes the chance to directly override, control or otherwise tamper
        /// with the matching process.  This is also where normal matching is performed,
        /// so to replace it, simply don't call the base.OnMatch method.  To tweak the results,
        /// the base method should be performed first, and the results can then be modified.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="mismatchedProperties"></param>
        /// <returns></returns>
        protected virtual bool OnMatch(IMatchable other, List<string> mismatchedProperties)
        {
            Type original = GetType();
            Type copy = other.GetType();

            // Public Properties ------------------------------------------------------
            PropertyInfo[] originalProperties = original.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] copyProperties = copy.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo originalProperty in originalProperties)
            {
                if (copyProperties.Contains(originalProperty.Name) == false)
                {
                    mismatchedProperties.Add(originalProperty.Name);
                    continue;
                }
                PropertyInfo copyProperty = copyProperties.GetFirst(originalProperty.Name);
                if (copyProperty == null)
                {
                    // The name of the property was not found on the other object
                    mismatchedProperties.Add(originalProperty.Name);
                    continue;
                }

                object originalValue = originalProperty.GetValue(this, null);
                object copyValue = copyProperty.GetValue(other, null);
                if (Match(originalValue, copyValue) == false)
                {
                    mismatchedProperties.Add(originalProperty.Name);
                }
            }

            // Public Fields ---------------------------------------------------------
            FieldInfo[] originalFields = original.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] copyFields = copy.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo originalField in originalFields)
            {
                if (copyFields.Contains(originalField.Name) == false)
                {
                    mismatchedProperties.Add(originalField.Name);
                    continue;
                }

                FieldInfo copyField = copyFields.GetFirst(originalField.Name);
                if (copyField == null)
                {
                    mismatchedProperties.Add(originalField.Name);
                    continue;
                }

                object originalValue = originalField.GetValue(this);
                object copyValue = copyField.GetValue(other);
                if (Match(originalValue, copyValue) == false)
                {
                    mismatchedProperties.Add(originalField.Name);
                }
            }
            if (mismatchedProperties.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This allows overrideable behavior that can replace or extend the basic behavior,
        /// which is to call Randomize on any public properties that are listed as randomizeable.
        /// This does nothing to normal properties or non public members and needs to be overriden
        /// to provide the special case functionality for sub-classes.
        /// </summary>
        /// <param name="generator">The random number generator to be used during randomization</param>
        protected virtual void OnRandomize(Random generator)
        {
            Type original = GetType();
            PropertyInfo[] originalProperties = original.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo originalProperty in originalProperties)
            {
                object prop = originalProperty.GetValue(this, null);
                IRandomizable rnd = prop as IRandomizable;
                if (rnd != null)
                {
                    rnd.Randomize(generator);
                }
            }
        }

        #endregion

        #region Private Functions

        private static bool Match(object originalValue, object copyValue)
        {
            // If a custom IMatchable description exists use it to determine if there is a match
            IMatchable originalMatch = originalValue as IMatchable;
            IMatchable copyMatch = copyValue as IMatchable;
            if (originalMatch != null && copyMatch != null)
            {
                bool res = originalMatch.Matches(copyMatch);
                return res;
            }

            // Strings are enumerable, so test them first, since string.Equals should be faster than
            // cycling through each character.
            string origString = originalValue as string;
            if (origString != null)
            {
                string mString = copyValue as string;
                if (mString == null) return false;
                return origString.Equals(mString);
            }

            // If the object is an enumeration, test the members of the enumeration.
            // If any members fail the match test, then the whole collection fails the match.
            IEnumerable originalList = originalValue as IEnumerable;
            if (originalList != null)
            {
                IEnumerable copyList = copyValue as IEnumerable;
                if (copyList != null)
                {
                    IEnumerator e = copyList.GetEnumerator();
                    e.MoveNext();
                    foreach (object originalItem in originalList)
                    {
                        if (Match(originalItem, e.Current) == false)
                        {
                            return false;
                        }
                        e.MoveNext();
                    }
                }
                return true;
            }
            if (originalValue == null && copyValue == null) return true;
            // If the objects are not collections and are not IMatchable, the only remaining thing we can
            // realistically test is simple equality.
            // Don't use == here!  It will always be false for boxed value types.
            return originalValue != null && (originalValue.Equals(copyValue));
        }

        #endregion
    }
}