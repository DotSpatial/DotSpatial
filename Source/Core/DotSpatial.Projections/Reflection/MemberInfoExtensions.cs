// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/2/2009 3:29:27 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotSpatial.Projections.Reflection
{
    internal static class MemberInfoExtensions
    {
        /// <summary>
        /// Gets the first member in the enumerable collection of property info with the specified name.
        /// </summary>
        public static T GetFirst<T>(this IEnumerable<T> self, string name)
            where T : MemberInfo
        {
            Func<T, bool> criteria = current => (current.Name == name);
            return self.First(criteria);
        }

        /// <summary>
        /// Determines whether there is a member with the specified name
        /// </summary>
        public static bool Contains(this IEnumerable<MemberInfo> self, string name)
        {
            return self.Any(info => info.Name == name);
        }
    }
}