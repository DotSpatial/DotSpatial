// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 2:45:18 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointSchemeCategory
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),
    Serializable]
    public class LineCategory : FeatureCategory, ILineCategory
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PointSchemeCategory
        /// </summary>
        public LineCategory()
        {
            Symbolizer = new LineSymbolizer();
            SelectionSymbolizer = new LineSymbolizer(true);
        }

        /// <summary>
        /// Creates a new set of cartographic lines that together form a line with a border.  Since a compound
        /// pen is used, it is possible to use this to create a transparent line with just two border parts.
        /// The selection symbolizer will be dark cyan bordering light cyan, but use the same dash and cap
        /// patterns.
        /// </summary>
        /// <param name="fillColor">The fill color for the line</param>
        /// <param name="borderColor">The border color of the line</param>
        /// <param name="width">The width of the entire line</param>
        /// <param name="dash">The dash pattern to use</param>
        /// <param name="caps">The style of the start and end caps</param>
        public LineCategory(Color fillColor, Color borderColor, double width, DashStyle dash, LineCap caps)
        {
            Symbolizer = new LineSymbolizer(fillColor, borderColor, width, dash, caps);
            SelectionSymbolizer = new LineSymbolizer(Color.Cyan, Color.DarkCyan, width, dash, caps);
        }

        /// <summary>
        /// Creates a new line category with the specified color and width
        /// </summary>
        /// <param name="color">The color of the unselected line</param>
        /// <param name="width">The width of both the selected and unselected lines.</param>
        public LineCategory(Color color, double width)
        {
            Symbolizer = new LineSymbolizer(color, width);
            SelectionSymbolizer = new LineSymbolizer(Color.Cyan, width);
        }

        /// <summary>
        /// Creates a new instanec of a default point scheme category where the geographic symbol size has been
        /// scaled to the specified extent.
        /// </summary>
        /// <param name="extent">The geographic extent that is 100 times wider than the geographic size of the points.</param>
        public LineCategory(IEnvelope extent)
        {
            Symbolizer = new LineSymbolizer(extent, false);
            SelectionSymbolizer = new LineSymbolizer(extent, true);
        }

        /// <summary>
        /// Creates a new category based on a symbolizer, and uses the same symbolizer, but with a fill and border color of light cyan
        /// for the selection symbolizer
        /// </summary>
        /// <param name="lineSymbolizer">The symbolizer to use in order to create a category</param>
        public LineCategory(ILineSymbolizer lineSymbolizer)
        {
            Symbolizer = lineSymbolizer;
            ILineSymbolizer select = lineSymbolizer.Copy();
            SelectionSymbolizer = select;
            if (select.Strokes != null && select.Strokes.Count > 0)
            {
                ISimpleStroke ss = select.Strokes[select.Strokes.Count - 1] as ISimpleStroke;
                if (ss != null) ss.Color = Color.Cyan;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This gets a single color that attempts to represent the specified
        /// category.  For polygons, for example, this is the fill color (or central fill color)
        /// of the top pattern.  If an image is being used, the color will be gray.
        /// </summary>
        /// <returns>The System.Color that can be used as an approximation to represent this category.</returns>
        public override Color GetColor()
        {
            if (Symbolizer == null || Symbolizer.Strokes == null || Symbolizer.Strokes.Count == 0) return Color.Gray;
            IStroke p = Symbolizer.Strokes[0];
            return p.GetColor();
        }

        /// <summary>
        /// Sets the specified color as the color for the top most stroke.
        /// </summary>
        /// <param name="color">The color to apply</param>
        public override void SetColor(Color color)
        {
            if (Symbolizer == null || Symbolizer.Strokes == null || Symbolizer.Strokes.Count == 0) return;
            Symbolizer.Strokes[0].SetColor(color);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer for this category
        /// </summary>
        //[TypeConverter(typeof(GeneralTypeConverter)), Editor(typeof(LineSymbolizerEditor), typeof(UITypeEditor))]
        public new ILineSymbolizer Symbolizer
        {
            get { return base.Symbolizer as ILineSymbolizer; }
            set
            {
                base.Symbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the symbolizer to use to draw selected features from this category.
        /// </summary>
        //[TypeConverter(typeof(GeneralTypeConverter)), Editor(typeof(LineSymbolizerEditor), typeof(UITypeEditor))]
        public new ILineSymbolizer SelectionSymbolizer
        {
            get { return base.SelectionSymbolizer as ILineSymbolizer; }
            set { base.SelectionSymbolizer = value; }
        }

        #endregion
    }
}