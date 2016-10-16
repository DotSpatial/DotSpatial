// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/29/2009 3:49:49 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// AnalyticCodes
    /// </summary>
    [Flags]
    public enum AnalyticModes
    {
        /// <summary>
        /// Derivatives of lon analytic
        /// </summary>
        IsAnalXlYl = 0x1,
        /// <summary>
        /// Derivatives of lat analytic
        /// </summary>
        IsAnalXpYp = 0x2,
        /// <summary>
        /// h and k are analytic
        /// </summary>
        IsAnalHk = 0x4,
        /// <summary>
        /// convergence analytic
        /// </summary>
        IsAnalConv = 0x8,
    }
}