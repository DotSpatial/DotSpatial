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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/29/2009 2:03:38 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// MatchableEM
    /// </summary>
    public static class MatchableEM
    {
        /// <summary>
        /// This tests the public properties from the two objects.  If any properties implement
        /// the IMatchable interface, and do not match, this returns false.  If any public
        /// properties are value types, and they are not equal, then this returns false.
        /// </summary>
        /// <param name="self">This matchable item </param>
        /// <param name="other">The other item to compare to this item</param>
        /// <returns>Boolean, true if there is a match</returns>
        public static bool Matches(this IMatchable self, IMatchable other)
        {
            List<string> ignoreMe;
            return self.Matches(other, out ignoreMe);
        }
    }
}