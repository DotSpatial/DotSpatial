// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/29/2009 1:49:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Matching is defined as being a different object, but having properties
    /// that would make it indistinguishable from the comparision property.
    /// This is an alternative to overriding the equals behavior in cases
    /// where you might ALSO need to see if the object reference is the same.
    /// </summary>
    public interface IProjMatchable
    {
        /// <summary>
        /// Tests this object against the comparison object.  If any of the
        /// value type members are different, or if any of the properties
        /// are IMatchable and do not match, then this returns false.
        /// </summary>
        /// <param name="other">The other IMatcheable object of the same type</param>
        /// <param name="mismatchedProperties">The list of property names that do not match</param>
        /// <returns>Boolean, true if the properties are comparably equal.</returns>
        bool Matches(IProjMatchable other, out List<string> mismatchedProperties);
    }
}