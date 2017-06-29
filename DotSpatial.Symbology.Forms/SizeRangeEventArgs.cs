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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/6/2009 3:21:44 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// PointSizeRangeEventArgs
    /// </summary>
    public class SizeRangeEventArgs : EventArgs
    {
        #region Private Variables

        private double _endSize;
        private double _startSize;
        private IFeatureSymbolizer _template;
        private bool _useSizeRange;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PointSizeRangeEventArgs
        /// </summary>
        public SizeRangeEventArgs(double startSize, double endSize, IFeatureSymbolizer template, bool useSizeRange)
        {
            _startSize = startSize;
            _endSize = endSize;
            _template = template;
            _useSizeRange = useSizeRange;
        }

        /// <summary>
        /// Creates a new instance of the PointSizeRangeEventArgs derived from a PointSizeRange
        /// </summary>
        /// <param name="range"></param>
        public SizeRangeEventArgs(FeatureSizeRange range)
        {
            _startSize = range.Start;
            _endSize = range.End;
            _template = range.Symbolizer;
            _useSizeRange = range.UseSizeRange;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the start size of the size range
        /// </summary>
        public double StartSize
        {
            get { return _startSize; }
            protected set { _startSize = value; }
        }

        /// <summary>
        /// Gets the end size of the range
        /// </summary>
        public double EndSize
        {
            get { return _endSize; }
            protected set { _endSize = value; }
        }

        /// <summary>
        /// Gets a boolean indicating whether the size range should be used
        /// </summary>
        public bool UseSizeRange
        {
            get { return _useSizeRange; }
            protected set { _useSizeRange = value; }
        }

        /// <summary>
        /// Gets the symbolizer template that describes everything not covered by a range parameter
        /// </summary>
        public IFeatureSymbolizer Template
        {
            get { return _template; }
            protected set { _template = value; }
        }

        #endregion
    }
}