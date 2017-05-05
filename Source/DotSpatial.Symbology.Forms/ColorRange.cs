// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
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
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRange"/> class with no limits and the color gray.
        /// </summary>
        public ColorRange()
        {
            Color = Color.Gray;
            Range = new Range(null, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRange"/> class with no limits of the specified color.
        /// </summary>
        /// <param name="color">The Color to use</param>
        public ColorRange(Color color)
        {
            Color = color;
            Range = new Range(null, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRange"/> class with the specified color and range.
        /// </summary>
        /// <param name="color">The Color to use for this range</param>
        /// <param name="range">The numeric bounds to use for this color.</param>
        public ColorRange(Color color, Range range)
        {
            Color = color;
            Range = range;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRange"/> class using the specified color and the specified
        /// nullable double values. A null value represents an unbounded range.
        /// </summary>
        /// <param name="color">The Color to use.</param>
        /// <param name="min">A double value representing the minimum value (inclusive).</param>
        /// <param name="max">A double value representing the maximum (exclusive).</param>
        public ColorRange(Color color, double? min, double? max)
        {
            Color = color;
            Range = new Range(min, max);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Color for this range.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the numeric range for which the color is valid.
        /// </summary>
        public Range Range { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a boolean that is true if the specified value falls within the specified range.
        /// </summary>
        /// <param name="value">The double value to test</param>
        /// <returns>Boolean, true if the value is within the Range.</returns>
        public bool Contains(double value)
        {
            return Range.Contains(value);
        }

        #endregion
    }
}