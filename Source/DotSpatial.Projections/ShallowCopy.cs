// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/2/2009 9:26:05 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Normally, cloning an object starts with MemberwiseClone, which
    /// creates a shallow copy of the object.  For any members that
    /// derive from the Descriptor, however, any public properties
    /// or fields that implement ICloneable are copied (deep copy behavior).
    /// This is not always desirable, even if the member CAN be copied.
    /// This attribute causes the deep copy behavior to skip over
    /// properties marked with this attribute.
    /// </summary>
    public class ProjShallowCopy : Attribute
    {
    }
}