// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 2:08:58 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// ItemTypes
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// The specified element is a folder
        /// </summary>
        Folder,

        /// <summary>
        /// The specified element is an image
        /// </summary>
        Image,

        /// <summary>
        /// The specified element is a vector line file format
        /// </summary>
        Line,

        /// <summary>
        /// The specified element is a vector line point format
        /// </summary>
        Point,

        /// <summary>
        /// The specified element is a vector polygon format
        /// </summary>
        Polygon,

        /// <summary>
        /// The specified element is a raster format
        /// </summary>
        Raster,

        /// <summary>
        /// The specified element is a custom format, so the custom icon is used
        /// </summary>
        Custom
    }
}