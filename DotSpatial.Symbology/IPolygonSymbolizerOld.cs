// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An interface for a symbolizer specific to polygons.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface IPolygonSymbolizerOld : IFeatureSymbolizerOld
    {
        /// <summary>
        /// Gets or sets the border symbolizer
        /// </summary>
        ILineSymbolizer BorderSymbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that determines whether or not the polygon border should be drawn.
        /// </summary>
        bool BorderIsVisible
        {
            get;
            set;
        }
    }
}