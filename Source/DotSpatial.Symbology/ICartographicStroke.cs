// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 4:16:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    public interface ICartographicStroke : ISimpleStroke
    {
        #region Methods

        /// <summary>
        /// Draws the line with max. 2 decorations. Otherwise the legend line might show only decorations.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path"></param>
        /// <param name="scaleWidth"></param>
        void DrawLegendPath(Graphics g, GraphicsPath path, double scaleWidth);

        /// <summary>
        /// Gets the width and height that is needed to draw this stroke with max. 2 decorations.
        /// </summary>
        Size GetLegendSymbolSize();

        #endregion

        /// <summary>
        /// Gets or sets the OGC line characteristic that controls how connected segments
        /// are drawn where they come together.
        /// </summary>
        LineJoinType JoinType { get; set; }

        /// <summary>
        /// Gets or sets an array of floating point values ranging from 0 to 1 that
        /// indicate the start and end point for where the line should draw.
        /// </summary>
        float[] CompoundArray { get; set; }

        /// <summary>
        /// gets or sets the DashCap for both the start and end caps of the dashes
        /// </summary>
        DashCap DashCap { get; set; }

        /// <summary>
        /// Gets or sets the DashPattern as an array of floating point values from 0 to 1
        /// </summary>
        float[] DashPattern { get; set; }

        /// <summary>
        /// Gets or sets the line decoration that describes symbols that should
        /// be drawn along the line as decoration.
        /// </summary>
        IList<ILineDecoration> Decorations { get; set; }

        /// <summary>
        /// Gets or sets the line cap for both the start and end of the line
        /// </summary>
        LineCap EndCap { get; set; }

        /// <summary>
        /// Gets or sets the line cap for both the start and end of the line
        /// </summary>
        LineCap StartCap { get; set; }

        /// <summary>
        /// This is a cached version of the horizontal pattern that should appear in the custom dash control.
        /// This is only used if DashStyle is set to custom, and only controls the pattern control,
        /// and does not directly affect the drawing pen.
        /// </summary>
        bool[] DashButtons { get; set; }

        /// <summary>
        /// This is a cached version of the vertical pattern that should appear in the custom dash control.
        /// This is only used if DashStyle is set to custom, and only controls the pattern control,
        /// and does not directly affect the drawing pen.
        /// </summary>
        bool[] CompoundButtons { get; set; }

        /// <summary>
        /// Gets or sets the floating poing offset (in pixels) for the line to be drawn to the left of
        /// the original line.  (Internally, this will modify the width and compound array for the
        /// actual pen being used, as Pens do not support an offset property).
        /// </summary>
        float Offset { get; set; }
    }
}