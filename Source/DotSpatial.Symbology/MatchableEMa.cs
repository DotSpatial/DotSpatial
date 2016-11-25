// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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