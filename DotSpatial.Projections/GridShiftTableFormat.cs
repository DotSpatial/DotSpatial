// ********************************************************************************************************
// Product Name: DotSpatial.Projections.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/17/2010 11:27:14 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    /// <summary>
    /// GridShiftTableFormats
    /// </summary>
    public enum GridShiftTableFormat
    {
        /// <summary>
        /// The data format used by ntv1
        /// </summary>
        DAT,
        /// <summary>
        /// The data format used by ntv2 and many others
        /// </summary>
        GSB,
        /// <summary>
        /// The data format use by several other grid types
        /// </summary>
        LLA,
        /// <summary>
        /// The data format used by National Geodetic Survey (NGS). Note
        /// there is always a matching LAS file.
        /// </summary>
        LOS
    }
}