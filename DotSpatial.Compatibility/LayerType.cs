// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:56:31 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// LayerType
    /// </summary>
    public enum LayerType
    {
        /// <summary>
        /// Raster Layer
        /// </summary>
        Grid,

        /// <summary>
        /// Image Layer
        /// </summary>
        Image,

        /// <summary>
        /// Not a valid layer format
        /// </summary>
        Invalid,

        /// <summary>
        /// Line FeatureSet Layer
        /// </summary>
        LineShapefile,

        /// <summary>
        /// Point FeatureSet Layer
        /// </summary>
        PointShapefile,

        /// <summary>
        /// Polygon FeatureSet Layer
        /// </summary>
        PolygonShapefile,
    }
}