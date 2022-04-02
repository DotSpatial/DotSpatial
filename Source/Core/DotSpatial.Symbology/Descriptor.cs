// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        /// Initializes a new instance of the <see cref="Descriptor"/> class.
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
        /// <param name="source">The.</param>
        public void CopyProperties(object source)
        {
            OnCopyProperties(source);
        }

        /// <summary>
        /// Compares the properties of this object with the specified IMatchable other.
        /// This does not test every property of other, but does test every property of this item.
        /// As long as the other item has corresponding properties for every property on this item, the items are said to match.
        /// The IMatchable interface allows custom definitions of matching.
        /// For collections to match, all of their sub-members must match.
        /// </summary>
        /// <param name="other">Other IMatchable to check against.</param>
        /// <param name="mismatchedProperties">Resulting list of mismatched Properties.</param>
        /// <returns>False if there were mismatched properties.</returns>
        public bool Matches(IMatchable other, out List<string> mismatchedProperties)
        {
            mismatchedProperties = new List<string>();
            return OnMatch(other, mismatchedProperties);
        }

        /// <summary>
        /// The default behavior is to cycle through all the properties of this
        /// object, and call Randomize on any that implement the IRandomizable interface.
        /// </summary>
        /// <param name="generator">The Random seed generator for controling how the random content is created.</param>
        public void Randomize(Random generator)
        {
            OnRandomize(generator);
        }

        /// <summary>
        /// This occurs while copying properties from the specified source, and
        /// is the default handling for subclasses.
        /// </summary>
        /// <param name="source">Source to copy properties from.</param>
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
                    if (copyValue is ICloneable cloneable)
                    {
                        originalProperty.SetValue(this, cloneable.Clone(), null);
                        continue;
                    }
                }

                // Use a shallow copy where ICloneable is not specifically defined.
                originalProperty.SetValue(this, copyValue, null);
            }

            // Public Fields
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
                    if (copyValue is ICloneable cloneable)
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
        /// with the matching process. This is also where normal matching is performed,
        /// so to replace it, simply don't call the base.OnMatch method. To tweak the results,
        /// the base method should be performed first, and the results can then be modified.
        /// </summary>
        /// <param name="other">Other IMatchable to check against.</param>
        /// <param name="mismatchedProperties">Resulting list of mismatched Properties.</param>
        /// <returns>False if there were mismatched properties.</returns>
        protected virtual bool OnMatch(IMatchable other, List<string> mismatchedProperties)
        {
            Type original = GetType();
            Type copy = other.GetType();

            // Public Properties
            PropertyInfo[] originalProperties = original.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] copyProperties = copy.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo originalProperty in originalProperties)
            {
                if (!copyProperties.Contains(originalProperty.Name))
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
                if (!Match(originalValue, copyValue))
                {
                    mismatchedProperties.Add(originalProperty.Name);
                }
            }

            // Public Fields
            FieldInfo[] originalFields = original.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] copyFields = copy.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo originalField in originalFields)
            {
                if (!copyFields.Contains(originalField.Name))
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
                if (!Match(originalValue, copyValue))
                {
                    mismatchedProperties.Add(originalField.Name);
                }
            }

            return mismatchedProperties.Count <= 0;
        }

        /// <summary>
        /// This allows overrideable behavior that can replace or extend the basic behavior,
        /// which is to call Randomize on any public properties that are listed as randomizeable.
        /// This does nothing to normal properties or non public members and needs to be overriden
        /// to provide the special case functionality for sub-classes.
        /// </summary>
        /// <param name="generator">The random number generator to be used during randomization.</param>
        protected virtual void OnRandomize(Random generator)
        {
            Type original = GetType();
            PropertyInfo[] originalProperties = original.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo originalProperty in originalProperties)
            {
                object prop = originalProperty.GetValue(this, null);
                IRandomizable rnd = prop as IRandomizable;
                rnd?.Randomize(generator);
            }
        }

        private static bool Match(object originalValue, object copyValue)
        {
            if (originalValue is IMatchable originalMatch && copyValue is IMatchable copyMatch)
            {
                bool res = originalMatch.Matches(copyMatch);
                return res;
            }

            // Strings are enumerable, so test them first, since string.Equals should be faster than
            // cycling through each character.
            if (originalValue is string origString)
            {
                if (copyValue is not string mString) return false;

                return origString.Equals(mString);
            }

            // If the object is an enumeration, test the members of the enumeration.
            // If any members fail the match test, then the whole collection fails the match.
            if (originalValue is IEnumerable originalList)
            {
                if (copyValue is IEnumerable copyList)
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
            return originalValue != null && originalValue.Equals(copyValue);
        }

        #endregion
    }
}