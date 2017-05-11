// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/29/2009 4:01:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IDescriptor
    /// </summary>
    public interface IDescriptor : IMatchable, IRandomizable, ICloneable
    {
        #region Methods

        /// <summary>
        /// This copies the public descriptor properties from the specified object to this object.
        /// </summary>
        /// <param name="other">An object that has properties that match the public properties on this object.</param>
        void CopyProperties(object other);

        #endregion
    }
}