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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 1:51:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Because we plan on making these XML serializable, the stroke allows us to open from
    /// a serialized situation without having to know in advance what kind of stroke we
    /// actually have.  The main idea is to allow something like:
    ///
    /// Stroke myStroke = new Stroke();
    /// myStroke.FromXml(myStream);
    /// Pen p = myStroke.ToPen();
    ///
    /// This allows loading, saving and creating pens from the sub-types without ever
    /// knowing exactly what they are.  All the basic subtypes can be obtained simply
    /// by casting this stroke into the appropriate interface.
    /// </summary>
    [Serializable]
    public class Stroke : Descriptor, IStroke
    {
        #region Private Variables

        private IStroke _innerStroke;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Gets a color to represent this line.  If the stroke doesn't work as a color,
        /// then this color will be gray.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetColor()
        {
            return Color.Gray;
        }

        /// <summary>
        /// Sets the color of this stroke to the specified color if possible.
        /// </summary>
        /// <param name="color">The color to assign to this color.</param>
        public virtual void SetColor(Color color)
        {
        }

        /// <summary>
        /// This is an optional expression that allows drawing to the specified GraphicsPath.
        /// Overriding this allows for unconventional behavior to be included, such as
        /// specifying marker decorations, rather than simply returning a pen.  A pen
        /// is also returned publicly for convenience.
        /// </summary>
        /// <param name="g">The Graphics device to draw to</param>
        /// <param name="path">the GraphicsPath to draw</param>
        /// <param name="scaleWidth">This is 1 for symbolic drawing, but could be
        /// any number for geographic drawing.</param>
        public virtual void DrawPath(Graphics g, GraphicsPath path, double scaleWidth)
        {
            if (_innerStroke != null)
            {
                _innerStroke.DrawPath(g, path, scaleWidth);
                return;
            }
            Pen p = ToPen(scaleWidth);
            try
            {
                g.DrawPath(p, path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Writeline Stroke.DrawPath:" + ex);
            }
            p.Dispose();
        }

        /// <summary>
        /// Uses the properties specified in this class to generate a pen.
        /// </summary>
        /// <returns></returns>
        public virtual Pen ToPen(double width)
        {
            if (_innerStroke != null) return _innerStroke.ToPen(width);
            return new Pen(Color.Black, (float)width);
        }

        /// <summary>
        /// Defines this stroke as a new kind of stroke.
        /// </summary>
        /// <param name="style"></param>
        public void CreateNew(StrokeStyle style)
        {
            switch (style)
            {
                case StrokeStyle.Simple:
                    _innerStroke = new SimpleStroke();
                    break;
                case StrokeStyle.Catographic:
                    _innerStroke = new CartographicStroke();
                    break;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the stroke style of the inner stroke
        /// </summary>
        [Serialize("StrokeStyle")]
        public StrokeStyle StrokeStyle
        {
            get
            {
                if (_innerStroke != null) return _innerStroke.StrokeStyle;
                return StrokeStyle.Simple;
            }
        }

        #endregion
    }
}