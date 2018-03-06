// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using DotSpatial.Symbology;
using DotSpatial.Symbology.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A control that draws a standard colored rectangle to the print layout.
    /// </summary>
    public class LayoutRectangle : LayoutElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRectangle"/> class.
        /// </summary>
        public LayoutRectangle()
        {
            Name = "Rectangle";
            Background = new PolygonSymbolizer(Color.Transparent, Color.Black, 2.0);
            ResizeStyle = ResizeStyle.HandledInternally;
        }

        #endregion

        #region Methods

        
        IPolygonSymbolizer _RectangleBackground = null;


        [TypeConverter(typeof(GeneralTypeConverter)), Browsable(true), Category("Symbol"), Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))]
        public new IPolygonSymbolizer Background
        {
            get { return _RectangleBackground; }
            set
            {
                _RectangleBackground = value; base.UpdateThumbnail(); base.OnInvalidate();

            }
        }
        
        /// <summary>
        /// Doesn't need to do anything now because the drawing code is in the background property of the base class.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="printing">Indicates whether the content is printed or previewed.</param>
        public override void Draw(Graphics g, bool printing)
        {
                        if (Background != null)
            {
                GraphicsPath gp = new GraphicsPath();
                Pen myPen = new Pen(Background.OutlineSymbolizer.GetFillColor(), (int)Background.GetOutlineWidth());
                RectangleF[] rectangles = { this.Rectangle };
                g.DrawRectangles(myPen, rectangles);
                gp.AddRectangle(this.Rectangle);
                SolidBrush Brush = new SolidBrush(Background.GetFillColor());

                g.FillPath(Brush, gp);

                Brush.Dispose();
                myPen.Dispose();

            }
        }

        #endregion
    }
}