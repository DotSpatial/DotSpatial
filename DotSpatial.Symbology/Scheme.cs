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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 10:08:43 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// DrawingScheme
    /// </summary>
    public abstract class Scheme : LegendItem
    {
        #region Private Variables

        private List<Break> _breaks; // A temporary list for helping construction of schemes.
        private EditorSettings _editorSettings;
        private Statistics _statistics;
        private List<double> _values;

        #endregion

        #region Nested type: Break

        /// <summary>
        /// Breaks for value ranges
        /// </summary>
        protected class Break
        {
            /// <summary>
            /// A double value for the maximum value for the break
            /// </summary>
            public double? Maximum;

            /// <summary>
            /// The string name
            /// </summary>
            public string Name;

            /// <summary>
            ///  Creates a new instance of a break
            /// </summary>
            public Break()
            {
                Name = string.Empty;
                Maximum = 0;
            }

            /// <summary>
            /// Creates a new instance of a break with a given name
            /// </summary>
            /// <param name="name">The string name for the break</param>
            public Break(string name)
            {
                Name = name;
                Maximum = 0;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DrawingScheme
        /// </summary>
        protected Scheme()
        {
            base.LegendSymbolMode = SymbolMode.None;
            LegendType = LegendType.Scheme;
            _statistics = new Statistics();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the category using a random fill color
        /// </summary>
        /// <param name="fillColor">The base color to use for creating the category</param>
        /// <param name="size">For points this is the larger dimension, for lines this is the largest width</param>
        /// <returns>A new IFeatureCategory that matches the type of this scheme</returns>
        public virtual ICategory CreateNewCategory(Color fillColor, double size)
        {
            // This method should be overridden in child classes

            return null;
        }

        /// <summary>
        /// Draws the regular symbolizer for the specified cateogry to the specified graphics
        /// surface in the specified bounding rectangle.
        /// </summary>
        /// <param name="index">The integer index of the feature to draw.</param>
        /// <param name="g">The Graphics object to draw to</param>
        /// <param name="bounds">The rectangular bounds to draw in</param>
        public abstract void DrawCategory(int index, Graphics g, Rectangle bounds);

        /// <summary>
        /// Adds a new scheme, assuming that the new scheme is the correct type.
        /// </summary>
        /// <param name="category">The category to add</param>
        public abstract void AddCategory(ICategory category);

        /// <summary>
        /// Reduces the index value of the specified category by 1 by
        /// exchaning it with the category before it.  If there is no
        /// category before it, then this does nothing.
        /// </summary>
        /// <param name="category">The category to decrease the index of</param>
        public abstract bool DecreaseCategoryIndex(ICategory category);

        /// <summary>
        /// Removes the specified category
        /// </summary>
        /// <param name="category">The category to insert</param>
        public abstract void RemoveCategory(ICategory category);

        /// <summary>
        /// Inserts the category at the specified index
        /// </summary>
        /// <param name="index">The integer index where the category should be inserted</param>
        /// <param name="category">The category to insert</param>
        public abstract void InsertCategory(int index, ICategory category);

        /// <summary>
        /// Re-orders the specified member by attempting to exchange it with the next higher
        /// index category.  If there is no higher index, this does nothing.
        /// </summary>
        /// <param name="category">The category to increase the index of</param>
        public abstract bool IncreaseCategoryIndex(ICategory category);

        /// <summary>
        /// Suspends the category events
        /// </summary>
        public abstract void SuspendEvents();

        /// <summary>
        /// Resumes the category events
        /// </summary>
        public abstract void ResumeEvents();

        /// <summary>
        /// Clears the categories
        /// </summary>
        public abstract void ClearCategories();

        /// <summary>
        /// Generates the break categories for this scheme
        /// </summary>
        protected void CreateBreakCategories()
        {
            int count = EditorSettings.NumBreaks;
            if (EditorSettings.IntervalMethod == IntervalMethod.Quantile)
            {
                Breaks = GetQuantileBreaks(count);
            }
            else
            {
                Breaks = GetEqualBreaks(count);
            }
            ApplyBreakSnapping();
            SetBreakNames(Breaks);
            List<Color> colorRamp = GetColorSet(count);
            List<double> sizeRamp = GetSizeSet(count);
            ClearCategories();
            int colorIndex = 0;
            Break prevBreak = null;
            foreach (Break brk in Breaks)
            {
                //get the color for the category
                Color randomColor = colorRamp[colorIndex];
                double randomSize = sizeRamp[colorIndex];
                ICategory cat = CreateNewCategory(randomColor, randomSize);

                if (cat != null)
                {
                    //cat.SelectionSymbolizer = _selectionSymbolizer.Copy();
                    cat.LegendText = brk.Name;
                    if (prevBreak != null) cat.Minimum = prevBreak.Maximum;
                    cat.Maximum = brk.Maximum;
                    cat.Range.MaxIsInclusive = true;
                    cat.ApplyMinMax(EditorSettings);
                    AddCategory(cat);
                }
                prevBreak = brk;

                colorIndex++;
            }
        }

        /// <summary>
        /// THe defaul
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected virtual List<double> GetSizeSet(int count)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < count; i++)
            {
                result.Add(20);
            }
            return result;
        }

        /// <summary>
        /// Creates a list of generated colors according to the convention
        /// specified in the EditorSettings.
        /// </summary>
        /// <param name="count">The integer count of the number of colors to create.</param>
        /// <returns>The list of colors created.</returns>
        protected List<Color> GetColorSet(int count)
        {
            List<Color> colorRamp;
            if (EditorSettings.UseColorRange)
            {
                if (!EditorSettings.RampColors)
                {
                    colorRamp = CreateRandomColors(count);
                }
                else if (!EditorSettings.HueSatLight)
                {
                    colorRamp = CreateRampColors(count, EditorSettings.StartColor, EditorSettings.EndColor);
                }
                else
                {
                    Color cStart = EditorSettings.StartColor;
                    Color cEnd = EditorSettings.EndColor;
                    colorRamp = CreateRampColors(count, cStart.GetSaturation(), cStart.GetBrightness(),
                                                 (int)cStart.GetHue(),
                                                 cEnd.GetSaturation(), cEnd.GetBrightness(), (int)cEnd.GetHue(),
                                                 EditorSettings.HueShift, cStart.A, cEnd.A);
                }
            }
            else
            {
                colorRamp = GetDefaultColors(count);
            }
            return colorRamp;
        }

        /// <summary>
        /// Uses the settings on this scheme to create a random category.
        /// </summary>
        /// <returns>A new ICategory</returns>
        public abstract ICategory CreateRandomCategory();

        /// <summary>
        /// Creates the colors in the case where the color range controls are not being used.
        /// This can be overriddend for handling special cases like ponit and line symbolizers
        /// that should be using the template colors.
        /// </summary>
        /// <param name="count">The integer count to use</param>
        /// <returns></returns>
        protected virtual List<Color> GetDefaultColors(int count)
        {
            return EditorSettings.RampColors ? CreateUnboundedRampColors(count) : CreateUnboundedRandomColors(count);
        }

        /// <summary>
        /// The default behavior for creating ramp colors is to create colors in the mid-range for
        /// both lightness and saturation, but to have the full range of hue
        /// </summary>
        /// <param name="numColors"></param>
        /// <returns></returns>
        private static List<Color> CreateUnboundedRampColors(int numColors)
        {
            return CreateRampColors(numColors, .25f, .25f, 0, .75f, .75f, 360, 0, 255, 255);
        }

        private static List<Color> CreateUnboundedRandomColors(int numColors)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            List<Color> result = new List<Color>(numColors);
            for (int i = 0; i < numColors; i++)
            {
                result.Add(rnd.NextColor());
            }
            return result;
        }

        private List<Color> CreateRandomColors(int numColors)
        {
            List<Color> result = new List<Color>(numColors);
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < numColors; i++)
            {
                result.Add(CreateRandomColor(rnd));
            }
            return result;
        }

        /// <summary>
        /// Creates a random color, but accepts a given random class instead of creating a new one.
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        protected Color CreateRandomColor(Random rnd)
        {
            Color startColor = EditorSettings.StartColor;
            Color endColor = EditorSettings.EndColor;
            if (EditorSettings.HueSatLight)
            {
                double hLow = startColor.GetHue();
                double dH = endColor.GetHue() - hLow;
                double sLow = startColor.GetSaturation();
                double ds = endColor.GetSaturation() - sLow;
                double lLow = startColor.GetBrightness();
                double dl = endColor.GetBrightness() - lLow;
                double aLow = (startColor.A) / 255.0;
                double da = (endColor.A - aLow) / 255.0;
                return SymbologyGlobal.ColorFromHsl(rnd.NextDouble() * dH + hLow, rnd.NextDouble() * ds + sLow,
                                                    rnd.NextDouble() * dl + lLow).ToTransparent((float)(rnd.NextDouble() * da + aLow));
            }
            int rLow = Math.Min(startColor.R, endColor.R);
            int rHigh = Math.Max(startColor.R, endColor.R);
            int gLow = Math.Min(startColor.G, endColor.G);
            int gHigh = Math.Max(startColor.G, endColor.G);
            int bLow = Math.Min(startColor.B, endColor.B);
            int bHigh = Math.Max(startColor.B, endColor.B);
            int iaLow = Math.Min(startColor.A, endColor.A);
            int aHigh = Math.Max(startColor.A, endColor.A);
            return Color.FromArgb(rnd.Next(iaLow, aHigh), rnd.Next(rLow, rHigh), rnd.Next(gLow, gHigh), rnd.Next(bLow, bHigh));
        }

        private static List<Color> CreateRampColors(int numColors, Color startColor, Color endColor)
        {
            List<Color> result = new List<Color>(numColors);
            double dR = (endColor.R - (double)startColor.R) / numColors;
            double dG = (endColor.G - (double)startColor.G) / numColors;
            double dB = (endColor.B - (double)startColor.B) / numColors;
            double dA = (endColor.A - (double)startColor.A) / numColors;
            for (int i = 0; i < numColors; i++)
            {
                result.Add(Color.FromArgb((int)(startColor.A + dA * i), (int)(startColor.R + dR * i), (int)(startColor.G + dG * i), (int)(startColor.B + dB * i)));
            }
            return result;
        }

        /// <summary>
        /// Applies the snapping rule directly to the categories, based on the most recently
        /// collected set of values, and the current VectorEditorSettings.
        /// </summary>
        public void ApplySnapping(ICategory category)
        {
            category.ApplySnapping(EditorSettings.IntervalSnapMethod, EditorSettings.IntervalRoundingDigits, Values);
        }

        /// <summary>
        /// Uses the currently calculated Values in order to calculate a list of breaks
        /// that have equal separations.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected List<Break> GetEqualBreaks(int count)
        {
            List<Break> result = new List<Break>();
            double min = Values[0];
            double dx = (Values[Values.Count - 1] - min) / count;

            for (int i = 0; i < count; i++)
            {
                Break brk = new Break();
                // max
                if (i == count - 1)
                {
                    brk.Maximum = null;
                }
                else
                {
                    brk.Maximum = min + (i + 1) * dx;
                }
                result.Add(brk);
            }
            return result;
        }

        /// <summary>
        /// Applies the snapping type to the given breaks
        /// </summary>
        protected void ApplyBreakSnapping()
        {
            if (Values == null || Values.Count == 0) return;
            switch (EditorSettings.IntervalSnapMethod)
            {
                case IntervalSnapMethod.None:
                    break;
                case IntervalSnapMethod.SignificantFigures:
                    foreach (Break item in Breaks)
                    {
                        if (item.Maximum == null) continue;
                        double val = (double)item.Maximum;
                        item.Maximum = SigFig(val, EditorSettings.IntervalRoundingDigits);
                    }
                    break;
                case IntervalSnapMethod.Rounding:
                    foreach (Break item in Breaks)
                    {
                        if (item.Maximum == null) continue;
                        item.Maximum = Math.Round((double)item.Maximum, EditorSettings.IntervalRoundingDigits);
                    }
                    break;
                case IntervalSnapMethod.DataValue:
                    foreach (Break item in Breaks)
                    {
                        if (item.Maximum == null) continue;
                        item.Maximum = NearestValue((double)item.Maximum, Values);
                    }
                    break;
            }
        }

        /// <summary>
        /// Searches the list and returns the nearest value in the list to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static double NearestValue(double value, List<double> values)
        {
            return GetNearestValue(value, values);
        }

        private static double SigFig(double value, int numFigures)
        {
            int md = (int)Math.Ceiling(Math.Log10(Math.Abs(value)));
            md -= numFigures;
            double norm = Math.Pow(10, md);
            return norm * Math.Round(value / norm);
        }

        /// <summary>
        /// Attempts to create the specified number of breaks with equal numbers of members in each.
        /// </summary>
        /// <param name="count">The integer count.</param>
        /// <returns>A list of breaks.</returns>
        protected List<Break> GetQuantileBreaks(int count)
        {
            List<Break> result = new List<Break>();
            int binSize = (int)Math.Ceiling(Values.Count / (double)count);
            for (int iBreak = 1; iBreak <= count; iBreak++)
            {
                if (binSize * iBreak < Values.Count)
                {
                    Break brk = new Break();
                    brk.Maximum = Values[binSize * iBreak];
                    result.Add(brk);
                }
                else
                {
                    // if num breaks is larger than number of members, this can happen
                    Break brk = new Break();
                    brk.Maximum = null;
                    result.Add(brk);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Sets the names for the break categories
        /// </summary>
        /// <param name="breaks"></param>
        protected static void SetBreakNames(IList<Break> breaks)
        {
            for (int i = 0; i < breaks.Count; i++)
            {
                Break brk = breaks[i];
                if (breaks.Count == 1)
                {
                    brk.Name = "All Values";
                }
                else if (i == 0)
                {
                    brk.Name = "<= " + brk.Maximum;
                }
                else if (i == breaks.Count - 1)
                {
                    brk.Name = "> " + breaks[i - 1].Maximum;
                }
                else
                {
                    brk.Name = breaks[i - 1].Maximum + " - " + brk.Maximum;
                }
            }
        }

        private static List<Color> CreateRampColors(int numColors, float minSat, float minLight, int minHue, float maxSat, float maxLight, int maxHue, int hueShift, int minAlpha, int maxAlpha)
        {
            List<Color> result = new List<Color>(numColors);
            double ds = (maxSat - (double)minSat) / numColors;
            double dh = (maxHue - (double)minHue) / numColors;
            double dl = (maxLight - (double)minLight) / numColors;
            double dA = (maxAlpha - (double)minAlpha) / numColors;
            for (int i = 0; i < numColors; i++)
            {
                double h = (minHue + dh * i) + hueShift % 360;
                double s = minSat + ds * i;
                double l = minLight + dl * i;
                float a = (float)(minAlpha + dA * i) / 255f;
                result.Add(SymbologyGlobal.ColorFromHsl(h, s, l).ToTransparent(a));
            }
            return result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the editor settings that control how this scheme operates.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EditorSettings EditorSettings
        {
            get { return _editorSettings; }
            set { _editorSettings = value; }
        }

        /// <summary>
        /// This is cached until a GetValues call is made, at which time the statistics will
        /// be re-calculated from the values.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Statistics Statistics
        {
            get
            {
                return _statistics;
            }
            protected set
            {
                _statistics = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of breaks for this scheme
        /// </summary>
        protected List<Break> Breaks
        {
            get { return _breaks; }
            set { _breaks = value; }
        }

        /// <summary>
        /// Gets the current list of values calculated in the case of numeric breaks.
        /// This includes only members that are not excluded by the exclude expression,
        /// and have a valid numeric value.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<double> Values
        {
            get { return _values; }
            protected set { _values = value; }
        }

        #endregion
    }
}