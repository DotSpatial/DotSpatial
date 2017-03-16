// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/2/2009 9:11:30 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Contains extensions for <see cref="ICloneable"/> interface.
    /// </summary>
    public static class ICloneableExtensions
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