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
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/2/2009 3:29:27 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotSpatial.Symbology
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