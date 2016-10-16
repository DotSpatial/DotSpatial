// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/29/2009 4:01:00 PM
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
    /// IDescriptor
    /// </summary>
    public interface IProjDescriptor : IProjMatchable, IProjRandomizable, ICloneable
    {
        #region Methods

        /// <summary>
        /// This copies the public descriptor properties from the specified object to
        /// this object.
        /// </summary>
        /// <param name="other">An object that has properties that match the public properties on this object.</param>
        void CopyProperties(object other);

        #endregion
    }
}