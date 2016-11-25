// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/1/2009 9:41:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|---------|---------------------------------------------------------------------
// |      Name       |  Date   |                        Comments
// |-----------------|---------|----------------------------------------------------------------------
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// CloneableEM
    /// </summary>
    public static class CloneableEM
    {
        /// <summary>
        /// The type parameter T is optional, so the intended use would be like:
        /// ObjectType copy = myObject.Copy();
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="original">The original object</param>
        /// <returns>A new object of the same type as the type being copied.</returns>
        public static T Copy<T>(this T original) where T : class, ICloneable
        {
            if (original != null) return original.Clone() as T;
            return null;
        }
    }
}