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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/4/2009 10:46:33 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ColorRange
    /// </summary>
    public class ColorRange
    {
        private Color _color;
        private Range _range;

        /// <summary>
        /// Generates a color range with no limits and the color gray.
        /// </summary>
        public ColorRange()
        {
            _color = Color.Gray;
            _range = new Range(null, null);
        }

        /// <summary>
        /// Generates a color range with no limits of the specified color
        /// </summary>
        /// <param name="color">The Color to use</param>
        public ColorRange(Color color)
        {
            _color = color;
            _range = new Range(null, null);
        }

        /// <summary>
        /// Generates a color range with the specified color and range.
        /// </summary>
        /// <param name="color">The Color to use for this range</param>
        /// <param name="range">The numeric bounds to use for this color.</param>
        public ColorRange(Color color, Range range)
        {
            _color = color;
            _range = range;
        }

        /// <summary>
        /// Creates a new ColorRange using the specified color and the specified
        /// nullable double values.  A null value represents an unbounded range.
        /// </summary>
        /// <param name="color">The Color to use.</param>
        /// <param name="min">A double value representing the minimum value (inclusive).</param>
        /// <param name="max">A double value representing the maximum (exclusive).</param>
        public ColorRange(Color color, double? min, double? max)
        {
            _color = color;
            _range = new Range(min, max);
        }

        /// <summary>
        /// Gets or sets the Color for this range.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the numeric range for which the color is valid.
        /// </summary>
        public Range Range
        {
            get { return _range; }
            set { _range = value; }
        }

        /// <summary>
        /// Gets a boolean that is true if the specified value falls within the specified range.
        /// </summary>
        /// <param name="value">The double value to test</param>
        /// <returns>Boolean, true if the value is within the Range.</returns>
        public bool Contains(double value)
        {
            return _range.Contains(value);
        }
    }
}