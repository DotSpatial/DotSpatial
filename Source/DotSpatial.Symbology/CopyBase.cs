// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Base class for classes with cloning support.
    /// </summary>
    public class CopyBase : ICloneable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyBase"/> class.
        /// </summary>
        protected CopyBase()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a duplicate of this descriptor using MemberwiseClone
        /// </summary>
        /// <returns>A clone of this object as a duplicate</returns>
        object ICloneable.Clone()
        {
            Descriptor copy = MemberwiseClone() as Descriptor;

            OnCopy(copy);
            return copy;
        }

        #endregion

        #region Properties

        /// <summary>
        /// PropertyInfo returns overridden members as separate entries. We would rather work with each members
        /// only one time.
        /// </summary>
        /// <param name="allProperties">All the properties, including duplicates created by overridden members</param>
        /// <returns>An Array of PropertyInfo members</returns>
        protected static PropertyInfo[] DistinctNames(IEnumerable<PropertyInfo> allProperties)
        {
            List<string> names = new List<string>();
            List<PropertyInfo> result = new List<PropertyInfo>();
            foreach (PropertyInfo property in allProperties)
            {
                if (names.Contains(property.Name)) continue;
                result.Add(property);
                names.Add(property.Name);
            }

            return result.ToArray();
        }

        /// <summary>
        /// This occurs during the Copy method and is overridable by sub-classes
        /// </summary>
        /// <param name="copy">The duplicate descriptor</param>
        protected virtual void OnCopy(Descriptor copy)
        {
            // This checks any property on copy, and if it is cloneable, it
            // creates a clone instead
            Type copyType = copy.GetType();

            PropertyInfo[] copyProperties = DistinctNames(copyType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            PropertyInfo[] myProperties = DistinctNames(GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance));
            foreach (PropertyInfo p in copyProperties)
            {
                if (p.CanWrite == false) continue;
                if (myProperties.Contains(p.Name) == false) continue;
                PropertyInfo myProperty = myProperties.GetFirst(p.Name);
                object myValue = myProperty.GetValue(this, null);
                if (myProperty.GetCustomAttributes(typeof(ShallowCopy), true).Length > 0)
                {
                    // This property is marked as shallow, so skip cloning it
                    continue;
                }

                ICloneable cloneable = myValue as ICloneable;
                if (cloneable == null) continue;
                p.SetValue(copy, cloneable.Clone(), null);
            }

            FieldInfo[] copyFields = copyType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] myFields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo f in copyFields)
            {
                if (myFields.Contains(f.Name) == false) continue;
                FieldInfo myField = myFields.GetFirst(f.Name);
                object myValue = myField.GetValue(copy);

                if (myField.GetCustomAttributes(typeof(ShallowCopy), true).Length > 0)
                {
                    // This field is marked as shallow, so skip cloning it
                    continue;
                }

                ICloneable cloneable = myValue as ICloneable;
                if (cloneable == null) continue;
                f.SetValue(copy, cloneable.Clone());
            }
        }

        #endregion
    }
}