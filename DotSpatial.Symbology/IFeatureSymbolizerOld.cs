// ********************************************************************************************************
// Product Name: DotSpatial.Interfaces Alpha
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

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This contains the set of symbology members that are shared
    /// between points, lines and polygons.
    /// </summary>
    public interface IFeatureSymbolizerOld : ILegendItem
    {
        /// <summary>
        /// Gets or sets the color of the brush used to fill the shape.
        /// Setting this will change the brush to a new SolidBrush.
        /// </summary>
        Color FillColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the brush used to fill the shape.
        /// </summary>
        Brush FillBrush
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether or not to use a texture when drawing these lines.  By default, this either
        /// checks the DefaultLineProvider or else it is false.
        /// </summary>
        bool IsTextured
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets a string name to help identify this Symbolizer
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a float value from 0 to 1 where 0 is completely transparent
        /// and 1 is completely solid.  Setting an alpha of a specific feature, like
        /// FillColor, to something less than 255 will control that feature's transparency
        /// without affecting the others.  The final transparency of the feature will
        /// be that alpha multiplied by this value and rounded to the nearest byte.
        /// </summary>
        float Opacity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string TextureFile to define the fill texture
        /// </summary>
        string TextureFile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the actual bitmap to use for the texture.
        /// </summary>
        Bitmap TextureImage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets a ScaleModes enumeration that determines whether non-coordinate drawing
        /// properties like width or size use pixels or world coordinates.  If pixels are
        /// specified, a back transform is used to approximate pixel sizes.
        /// </summary>
        ScaleMode ScaleMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the smoothing mode to use that controls advanced features like
        /// anti-aliasing.  By default this is set to antialias.
        /// </summary>
        SmoothingMode Smoothing
        {
            get;
            set;
        }
    }
}