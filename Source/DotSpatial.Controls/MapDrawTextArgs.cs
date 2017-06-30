// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/18/2008 11:44:37 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// GeoDrawTextArgs
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class MapDrawTextArgs
    {
        #region Private Variables

        private Brush _backBrush;
        private Pen _borderPen;
        private MapDrawArgs _drawArgs;
        private Brush _fontBrush;
        private ILabelSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoDrawTextArgs
        /// </summary>
        public MapDrawTextArgs(MapDrawArgs args, ILabelSymbolizer symbolizer)
        {
            _symbolizer = symbolizer;
            _drawArgs = args;
            _fontBrush = new SolidBrush(symbolizer.FontColor);
            _backBrush = new SolidBrush(symbolizer.BackColor);
            _borderPen = new Pen(symbolizer.BorderColor);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Disposes the font brush, border pen and background brush
        /// </summary>
        public void Dispose()
        {
            _fontBrush.Dispose();
            _borderPen.Dispose();
            _backBrush.Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the TextSymbolizer for this
        /// </summary>
        public ILabelSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            protected set { _symbolizer = value; }
        }

        /// <summary>
        /// Gets the GeoDrawArgs
        /// </summary>
        public MapDrawArgs DrawArgs
        {
            get { return _drawArgs; }
            protected set { _drawArgs = value; }
        }

        /// <summary>
        /// Gets the brush for drawing the background.
        /// </summary>
        public Brush BackBrush
        {
            get { return _backBrush; }
            protected set { _backBrush = value; }
        }

        /// <summary>
        /// Gets the border pen
        /// </summary>
        public Pen BorderPen
        {
            get { return _borderPen; }
            set { _borderPen = value; }
        }

        /// <summary>
        /// Gets the brush used for drawing fonts.
        /// </summary>
        public Brush FontBrush
        {
            get { return _fontBrush; }
            set { _fontBrush = value; }
        }

        #endregion
    }
}