// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/5/2009 2:42:17 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SizeRange
    /// </summary>
    public class FeatureSizeRange
    {
        #region Private Variables

        private double _end;
        private double _start;
        private IFeatureSymbolizer _symbolizer;
        private bool _useSizeRange;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SizeRange
        /// </summary>
        public FeatureSizeRange()
        {
        }

        /// <summary>
        /// Gets or sets the Point Size Range
        /// </summary>
        /// <param name="symbolizer"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public FeatureSizeRange(IFeatureSymbolizer symbolizer, double start, double end)
        {
            _symbolizer = symbolizer;
            _start = start;
            _end = end;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Given a size, this will return the native
        /// symbolizer that has been adjusted to the
        /// specified size.
        /// </summary>
        /// <param name="size">The size of the symbol</param>
        /// <param name="color">The color of the symbol</param>
        /// <returns></returns>
        public IFeatureSymbolizer GetSymbolizer(double size, Color color)
        {
            IFeatureSymbolizer copy = _symbolizer.Copy();
            // preserve aspect ratio, larger dimension specified
            IPointSymbolizer ps = copy as IPointSymbolizer;
            if (ps != null)
            {
                Size2D s = ps.GetSize();
                double ratio = size / Math.Max(s.Width, s.Height);
                s.Width *= ratio;
                s.Height *= ratio;
                ps.SetSize(s);
                ps.SetFillColor(color);
            }
            ILineSymbolizer ls = copy as ILineSymbolizer;
            if (ls != null)
            {
                ls.SetWidth(size);
                ls.SetFillColor(color);
            }
            return copy;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer that controls everything except for size.
        /// </summary>
        public IFeatureSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            set { _symbolizer = value; }
        }

        /// <summary>
        /// Minimum size
        /// </summary>
        public double Start
        {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// Maximum size
        /// </summary>
        public double End
        {
            get { return _end; }
            set { _end = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if the size range should be used.
        /// </summary>
        public bool UseSizeRange
        {
            get { return _useSizeRange; }
            set { _useSizeRange = value; }
        }

        #endregion
    }
}