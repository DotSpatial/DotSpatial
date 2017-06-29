// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/2/2009 3:10:43 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using DotSpatial.Projections.Reflection;

namespace DotSpatial.Projections
{
    /// <summary>
    /// CopyBase
    /// </summary>
    [Serializable]
    public class ProjCopyBase : ICloneable
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CopyBase
        /// </summary>
        protected ProjCopyBase()
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
            ProjDescriptor copy = MemberwiseClone() as ProjDescriptor;

            OnCopy(copy);
            return copy;
        }

        #endregion

        #region Properties

        /// <summary>
        /// PropertyInfo returns overridden members as separate entries.  We would rather work with each members
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
        protected virtual void OnCopy(ProjDescriptor copy)
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
                if (myProperty.GetCustomAttributes(typeof(ProjShallowCopy), true).Length > 0)
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

                if (myField.GetCustomAttributes(typeof(ProjShallowCopy), true).Length > 0)
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