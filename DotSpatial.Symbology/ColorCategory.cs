// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/19/2008 4:16:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ColorCategory
    /// </summary>
    [ToolboxItem(false), Serializable]
    public class ColorCategory : Category, IColorCategory
    {
        #region Events

        /// <summary>
        /// Occurs when this ColorBreak received instructions to show an editor.  If this is
        /// handled, then no action will be taken.
        /// </summary>
        public event HandledEventHandler EditItem;

        #endregion

        #region Private Variables

        private readonly List<SymbologyMenuItem> _contextMenuItems;
        GradientModel _gradientModel;
        Color _highColor;
        Color _lowColor;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ColorCategory
        /// </summary>
        public ColorCategory()
        {
            _contextMenuItems = new List<SymbologyMenuItem>
                {
                    new SymbologyMenuItem("Remove Break", Remove_Break),
                    new SymbologyMenuItem("Edit Break", Edit_Break)
                };
        }

        /// <summary>
        /// Creates a ColorBreak that has a single value, which can be an object of any type.
        /// The LowValue field will contain this single value.
        /// </summary>
        /// <param name="value">The value to test.</param>
        public ColorCategory(double value)
            : base(value)
        {
            Configure();
        }

        /// <summary>
        /// Creates a ColorBreak that has a single value, which can be an object of any type.
        /// The LowValue field will contain this single value.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="color">The color to use as the low value.</param>
        public ColorCategory(double value, Color color)
            : base(value)
        {
            _lowColor = color;
            _highColor = color;
            Configure();
        }

        /// <summary>
        /// Creates a new color category, but doesn't specify the colors themselves.
        /// </summary>
        /// <param name="startValue">The start value</param>
        /// <param name="endValue">The end value</param>
        public ColorCategory(double? startValue, double? endValue)
            : base(startValue, endValue)
        {
        }

        /// <summary>
        /// Creates a bi-valued colorbreak that will automatically test which of the specified values is higher
        /// and use that as the high value.  The other will become the low value.  This will be set to a
        /// bi-value colorbreak.
        /// </summary>
        /// <param name="startValue">One of the values to use in this colorbreak.</param>
        /// <param name="endValue">The other value to use in this colorbreak.</param>
        /// <param name="lowColor">The color to assign to the higher of the two values</param>
        /// <param name="highColor">The color to assign to the lower of the two values</param>
        public ColorCategory(double? startValue, double? endValue, Color lowColor, Color highColor)
            : base(startValue, endValue)
        {
            _highColor = highColor;
            _lowColor = lowColor;
            Configure();
        }

        /// <summary>
        /// Fires the EditItem event.  If e returns handled, then this will not launch the default editor
        /// </summary>
        /// <param name="e">The HandledEventArgs</param>
        protected virtual void OnEditItem(HandledEventArgs e)
        {
            if (EditItem != null) EditItem(this, e);
        }

        private void Edit_Break(object sender, EventArgs e)
        {
            // Allow this action to be overridden by the event
            var result = new HandledEventArgs(false);
            OnEditItem(result);
            if (result.Handled) return;
            
            var cca = ColorCategoryActions;
            if (cca != null)
            {
                cca.ShowEdit(this);
            }
        }

        /// <summary>
        /// Gets or sets custom actions for ColorCategory
        /// </summary>
        [Browsable(false)]
        public IColorCategoryActions ColorCategoryActions { get; set; }

        /// <inheritdocs/>
        protected override void OnCopyProperties(object source)
        {
            base.OnCopyProperties(source);
            OnItemChanged(this);
        }

        private void Remove_Break(object sender, EventArgs e)
        {
            OnRemoveItem();
        }

        private void Configure()
        {
            base.LegendSymbolMode = SymbolMode.Symbol;
            LegendType = LegendType.Symbol;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This is primarily used in the BiValue situation where a color needs to be generated
        /// somewhere between the start value and the end value.
        /// </summary>
        /// <param name="value">The value to be converted into a color from the range on this color break</param>
        /// <returns>A color that is selected from the range values.</returns>
        public virtual Color CalculateColor(double value)
        {
            if (!Range.Contains(value)) return Color.Transparent;
            if (Minimum == null || Maximum == null)
            {
                return Range.Minimum == null ? HighColor : LowColor;
            }

            // From here on we have a double that falls in the range somewhere

            double lowVal = Minimum.Value;
            double range = Math.Abs(Maximum.Value - lowVal);
            double p = 0; // the portion of the range, where 0 is LowValue & 1 is HighValue
            double ht;
            switch (GradientModel)
            {
                case GradientModel.Linear:
                    p = (value - lowVal) / range;
                    break;
                case GradientModel.Exponential:
                    ht = value;
                    if (ht < 1) ht = 1.0;
                    if (range > 1)
                    {
                        p = (Math.Pow(ht - lowVal, 2) / Math.Pow(range, 2));
                    }
                    else
                    {
                        return LowColor;
                    }
                    break;
                case GradientModel.Logarithmic:
                    ht = value;
                    if (ht < 1) ht = 1.0;
                    if (range > 1.0 && ht - lowVal > 1.0)
                    {
                        p = Math.Log(ht - lowVal) / Math.Log(range);
                    }
                    else
                    {
                        return LowColor;
                    }
                    break;
            }

            int alpha = ByteRange(LowColor.A + Math.Round((HighColor.A - LowColor.A) * p));
            int red = ByteRange(LowColor.R + Math.Round((HighColor.R - LowColor.R) * p));
            int green = ByteRange(LowColor.G + Math.Round((HighColor.G - LowColor.G) * p));
            int blue = ByteRange(LowColor.B + Math.Round((HighColor.B - LowColor.B) * p));
            return Color.FromArgb(alpha, red, green, blue);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets how the color changes are distributed across the
        /// BiValued range.  If IsBiValue is false, this does nothing.
        /// </summary>
        [Serialize("GradientModel")]
        public virtual GradientModel GradientModel
        {
            get { return _gradientModel; }
            set { _gradientModel = value; }
        }

        /// <summary>
        /// Gets or sets the second of two colors to be used.
        /// This is only used for BiValued breaks.
        /// </summary>
        [Serialize("HighColor")]
        public virtual Color HighColor
        {
            get { return _highColor; }
            set { _highColor = value; }
        }

        /// <summary>
        /// This not only indicates that there are two values,
        /// but that the values are also different from one another.
        /// </summary>
        public virtual bool IsBiValue
        {
            get
            {
                if (Range.Minimum == null || Range.Maximum == null) return false;
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the color to be used for this break.  For
        /// BiValued breaks, this only sets one of the colors.  If
        /// this is higher than the high value, both are set to this.
        /// If this equals the high value, IsBiValue will be false.
        /// </summary>
        [Serialize("LowColor")]
        public virtual Color LowColor
        {
            get { return _lowColor; }
            set
            {
                _lowColor = value;
            }
        }

        #endregion

        #region IColorCategory Members

        /// <summary>
        /// Paints legend symbol
        /// </summary>
        /// <param name="g"></param>
        /// <param name="box"></param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            if (box.Height == 0) return;
            Brush b = new LinearGradientBrush(box, _lowColor, _highColor, LinearGradientMode.Horizontal);
            g.FillRectangle(b, box);
            g.DrawRectangle(Pens.Black, box);
        }

        #endregion
    }
}