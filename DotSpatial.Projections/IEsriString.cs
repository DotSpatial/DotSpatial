// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 10:26:47 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    /// <summary>
    /// EsriString
    /// </summary>
    public interface IEsriString
    {
        #region Methods

        /// <summary>
        /// Writes the value in the format that it would appear in within a prj file
        /// </summary>
        /// <returns>The a nested portion of the total esri string.</returns>
        string ToEsriString();

        /// <summary>
        /// This reads the string and attempts to parse the relavent values.
        /// </summary>
        /// <param name="esriString">The string to read</param>
        void ParseEsriString(string esriString);

        #endregion

        #region Properties

        #endregion
    }
}