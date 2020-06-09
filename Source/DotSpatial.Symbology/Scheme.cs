// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Categorizable legend item.
    /// </summary>
    public abstract class Scheme : LegendItem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Scheme"/> class.
        /// </summary>
        protected Scheme()
        {
            LegendSymbolMode = SymbolMode.None;
            LegendType = LegendType.Scheme;
            Statistics = new Statistics();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the editor settings that control how this scheme operates.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EditorSettings EditorSettings { get; set; }

        /// <summary>
        /// Gets or sets the statistics. This is cached until a GetValues call is made, at which time the statistics will
        /// be re-calculated from the values.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Statistics Statistics { get; protected set; }

        /// <summary>
        /// Gets or sets the current list of values calculated in the case of numeric breaks.
        /// This includes only members that are not excluded by the exclude expression,
        /// and have a valid numeric value.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<double> Values { get; protected set; }

        /// <summary>
        /// Gets or sets the list of breaks for this scheme.
        /// </summary>
        protected List<Break> Breaks { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new scheme, assuming that the new scheme is the correct type.
        /// </summary>
        /// <param name="category">The category to add.</param>
        public abstract void AddCategory(ICategory category);

        /// <summary>
        /// Applies the snapping rule directly to the categories, based on the most recently
        /// collected set of values, and the current VectorEditorSettings.
        /// </summary>
        /// <param name="category">Category the snapping is applied to.</param>
        public void ApplySnapping(ICategory category)
        {
            category.ApplySnapping(EditorSettings.IntervalSnapMethod, EditorSettings.IntervalRoundingDigits, Values);
        }

        /// <summary>
        /// Clears the categories.
        /// </summary>
        public abstract void ClearCategories();

        /// <summary>
        /// Creates the category using a random fill color.
        /// </summary>
        /// <param name="fillColor">The base color to use for creating the category.</param>
        /// <param name="size">For points this is the larger dimension, for lines this is the largest width.</param>
        /// <returns>A new IFeatureCategory that matches the type of this scheme.</returns>
        public virtual ICategory CreateNewCategory(Color fillColor, double size)
        {
            // This method should be overridden in child classes
            return null;
        }

        /// <summary>
        /// Uses the settings on this scheme to create a random category.
        /// </summary>
        /// <returns>A new ICategory.</returns>
        public abstract ICategory CreateRandomCategory();

        /// <summary>
        /// Reduces the index value of the specified category by 1 by
        /// exchaning it with the category before it. If there is no
        /// category before it, then this does nothing.
        /// </summary>
        /// <param name="category">The category to decrease the index of.</param>
        /// <returns>True, if run successfully.</returns>
        public abstract bool DecreaseCategoryIndex(ICategory category);

        /// <summary>
        /// Draws the regular symbolizer for the specified cateogry to the specified graphics
        /// surface in the specified bounding rectangle.
        /// </summary>
        /// <param name="index">The integer index of the feature to draw.</param>
        /// <param name="g">The Graphics object to draw to.</param>
        /// <param name="bounds">The rectangular bounds to draw in.</param>
        public abstract void DrawCategory(int index, Graphics g, Rectangle bounds);

        /// <summary>
        /// Re-orders the specified member by attempting to exchange it with the next higher
        /// index category. If there is no higher index, this does nothing.
        /// </summary>
        /// <param name="category">The category to increase the index of.</param>
        /// <returns>True, if run successfully.</returns>
        public abstract bool IncreaseCategoryIndex(ICategory category);

        /// <summary>
        /// Inserts the category at the specified index.
        /// </summary>
        /// <param name="index">The integer index where the category should be inserted.</param>
        /// <param name="category">The category to insert.</param>
        public abstract void InsertCategory(int index, ICategory category);

        /// <summary>
        /// Removes the specified category.
        /// </summary>
        /// <param name="category">The category to insert.</param>
        public abstract void RemoveCategory(ICategory category);

        /// <summary>
        /// Resumes the category events.
        /// </summary>
        public abstract void ResumeEvents();

        /// <summary>
        /// Suspends the category events.
        /// </summary>
        public abstract void SuspendEvents();

        /// <summary>
        /// Sets the names for the break categories.
        /// </summary>
        /// <param name="breaks">Breaks to use for naming.</param>
        protected static void SetBreakNames(IList<Break> breaks)
        {
            for (var i = 0; i < breaks.Count; i++)
            {
                var brk = breaks[i];
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

        /// <summary>
        /// Applies the snapping type to the given breaks.
        /// </summary>
        protected void ApplyBreakSnapping()
        {
            if (Values == null || Values.Count == 0) return;

            switch (EditorSettings.IntervalSnapMethod)
            {
                case IntervalSnapMethod.None: break;
                case IntervalSnapMethod.SignificantFigures:
                    foreach (var item in Breaks)
                    {
                        if (item.Maximum == null) continue;

                        var val = (double)item.Maximum;
                        item.Maximum = Utils.SigFig(val, EditorSettings.IntervalRoundingDigits);
                    }

                    break;
                case IntervalSnapMethod.Rounding:
                    foreach (var item in Breaks)
                    {
                        if (item.Maximum == null) continue;

                        item.Maximum = Math.Round((double)item.Maximum, EditorSettings.IntervalRoundingDigits);
                    }

                    break;
                case IntervalSnapMethod.DataValue:
                    foreach (var item in Breaks)
                    {
                        if (item.Maximum == null) continue;

                        item.Maximum = Utils.GetNearestValue((double)item.Maximum, Values);
                    }

                    break;
            }
        }

        /// <summary>
        /// Generates the break categories for this scheme.
        /// </summary>
        protected void CreateBreakCategories()
        {
            var count = EditorSettings.NumBreaks;
            switch (EditorSettings.IntervalMethod)
            {
                case IntervalMethod.EqualFrequency:
                    Breaks = GetQuantileBreaks(count);
                    break;
                case IntervalMethod.NaturalBreaks:
                    Breaks = GetNaturalBreaks(count);
                    break;
                default:
                    Breaks = GetEqualBreaks(count);
                    break;
            }

            ApplyBreakSnapping();
            SetBreakNames(Breaks);
            var colorRamp = GetColorSet(count);
            var sizeRamp = GetSizeSet(count);
            ClearCategories();
            var colorIndex = 0;
            Break prevBreak = null;
            foreach (var brk in Breaks)
            {
                // get the color for the category
                var randomColor = colorRamp[colorIndex];
                var randomSize = sizeRamp[colorIndex];
                var cat = CreateNewCategory(randomColor, randomSize);

                if (cat != null)
                {
                    // cat.SelectionSymbolizer = _selectionSymbolizer.Copy();
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
        /// Creates a random color, but accepts a given random class instead of creating a new one.
        /// </summary>
        /// <param name="rnd">The random generator.</param>
        /// <returns>The created random color.</returns>
        protected Color CreateRandomColor(Random rnd)
        {
            var startColor = EditorSettings.StartColor;
            var endColor = EditorSettings.EndColor;
            if (EditorSettings.HueSatLight)
            {
                double hLow = startColor.GetHue();
                var dH = endColor.GetHue() - hLow;
                double sLow = startColor.GetSaturation();
                var ds = endColor.GetSaturation() - sLow;
                double lLow = startColor.GetBrightness();
                var dl = endColor.GetBrightness() - lLow;
                var aLow = startColor.A / 255.0;
                var da = (endColor.A - aLow) / 255.0;
                return SymbologyGlobal.ColorFromHsl((rnd.NextDouble() * dH) + hLow, (rnd.NextDouble() * ds) + sLow, (rnd.NextDouble() * dl) + lLow).ToTransparent((float)((rnd.NextDouble() * da) + aLow));
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

        /// <summary>
        /// Creates a list of generated colors according to the convention specified in the EditorSettings.
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
                    var cStart = EditorSettings.StartColor;
                    var cEnd = EditorSettings.EndColor;
                    colorRamp = CreateRampColors(count, cStart.GetSaturation(), cStart.GetBrightness(), (int)cStart.GetHue(), cEnd.GetSaturation(), cEnd.GetBrightness(), (int)cEnd.GetHue(), EditorSettings.HueShift, cStart.A, cEnd.A);
                }
            }
            else
            {
                colorRamp = GetDefaultColors(count);
            }

            return colorRamp;
        }

        /// <summary>
        /// Creates the colors in the case where the color range controls are not being used.
        /// This can be overriddend for handling special cases like point and line symbolizers
        /// that should be using the template colors.
        /// </summary>
        /// <param name="count">The integer count to use.</param>
        /// <returns>The default colors.</returns>
        protected virtual List<Color> GetDefaultColors(int count)
        {
            return EditorSettings.RampColors ? CreateUnboundedRampColors(count) : CreateUnboundedRandomColors(count);
        }

        /// <summary>
        /// Uses the currently calculated Values in order to calculate a list of breaks
        /// that have equal separations.
        /// </summary>
        /// <param name="count">Number of breaks that should be added.</param>
        /// <returns>Calculated list of breaks.</returns>
        protected List<Break> GetEqualBreaks(int count)
        {
            var result = new List<Break>();
            var min = Values[0];
            var dx = (Values[Values.Count - 1] - min) / count;

            for (var i = 0; i < count; i++)
            {
                var brk = new Break();

                // max
                if (i == count - 1)
                {
                    brk.Maximum = null;
                }
                else
                {
                    brk.Maximum = min + ((i + 1) * dx);
                }

                result.Add(brk);
            }

            return result;
        }

        /// <summary>
        /// Generates natural breaks.
        /// </summary>
        /// <param name="count">Count of breaks.</param>
        /// <returns>List with breaks.</returns>
        /// <remarks>
        /// Originally this function used an algorithm based on MapWinGIS code that was too buggy.
        /// Updated by dpa May 2018 to use a well known algorithm documented in NaturalBreaks.cs.
        /// </remarks>
        protected List<Break> GetNaturalBreaks(int count)
        {
            var theOutput = new List<Break>(count);
            var natBreaks = new NaturalBreaks(Values, count);
            List<double> theBreakValues = natBreaks.GetResults();

            // remove the first break value since it is the lowest value
            theBreakValues.RemoveAt(0);
            foreach (double oneBreakValue in theBreakValues)
            {
                Break b = new Break();
                b.Maximum = oneBreakValue;
                theOutput.Add(b);
            }

            return theOutput;
        }

        /// <summary>
        /// Attempts to create the specified number of breaks with equal numbers of members in each.
        /// </summary>
        /// <param name="count">The integer count.</param>
        /// <returns>A list of breaks.</returns>
        protected List<Break> GetQuantileBreaks(int count)
        {
            var result = new List<Break>();
            var binSize = (int)Math.Ceiling(Values.Count / (double)count);
            for (var iBreak = 1; iBreak <= count; iBreak++)
            {
                if (binSize * iBreak < Values.Count)
                {
                    var brk = new Break { Maximum = Values[binSize * iBreak] };
                    result.Add(brk);
                }
                else
                {
                    // if num breaks is larger than number of members, this can happen
                    var brk = new Break { Maximum = null };
                    result.Add(brk);
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the default size set.
        /// </summary>
        /// <param name="count">Number of sizes needed.</param>
        /// <returns>The default size set.</returns>
        protected virtual List<double> GetSizeSet(int count)
        {
            var result = new List<double>();
            for (var i = 0; i < count; i++)
            {
                result.Add(20);
            }

            return result;
        }

        private static List<Color> CreateRampColors(int numColors, Color startColor, Color endColor)
        {
            var result = new List<Color>(numColors);
            var dR = (endColor.R - (double)startColor.R) / numColors;
            var dG = (endColor.G - (double)startColor.G) / numColors;
            var dB = (endColor.B - (double)startColor.B) / numColors;
            var dA = (endColor.A - (double)startColor.A) / numColors;
            for (var i = 0; i < numColors; i++)
            {
                result.Add(Color.FromArgb((int)(startColor.A + (dA * i)), (int)(startColor.R + (dR * i)), (int)(startColor.G + (dG * i)), (int)(startColor.B + (dB * i))));
            }

            return result;
        }

        private static List<Color> CreateRampColors(int numColors, float minSat, float minLight, int minHue, float maxSat, float maxLight, int maxHue, int hueShift, int minAlpha, int maxAlpha)
        {
            var result = new List<Color>(numColors);
            var ds = (maxSat - (double)minSat) / numColors;
            var dh = (maxHue - (double)minHue) / numColors;
            var dl = (maxLight - (double)minLight) / numColors;
            var dA = (maxAlpha - (double)minAlpha) / numColors;
            for (var i = 0; i < numColors; i++)
            {
                var h = (minHue + (dh * i)) + (hueShift % 360);
                var s = minSat + (ds * i);
                var l = minLight + (dl * i);
                var a = (float)(minAlpha + (dA * i)) / 255f;
                result.Add(SymbologyGlobal.ColorFromHsl(h, s, l).ToTransparent(a));
            }

            return result;
        }

        /// <summary>
        /// The default behavior for creating ramp colors is to create colors in the mid-range for
        /// both lightness and saturation, but to have the full range of hue.
        /// </summary>
        /// <param name="numColors">Number of colors needed.</param>
        /// <returns>The list with the created colors.</returns>
        private static List<Color> CreateUnboundedRampColors(int numColors)
        {
            return CreateRampColors(numColors, .25f, .25f, 0, .75f, .75f, 360, 0, 255, 255);
        }

        private static List<Color> CreateUnboundedRandomColors(int numColors)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            var result = new List<Color>(numColors);
            for (var i = 0; i < numColors; i++)
            {
                result.Add(rnd.NextColor());
            }

            return result;
        }

        private List<Color> CreateRandomColors(int numColors)
        {
            var result = new List<Color>(numColors);
            var rnd = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < numColors; i++)
            {
                result.Add(CreateRandomColor(rnd));
            }

            return result;
        }

        #endregion

        #region Classes

        /// <summary>
        /// Breaks for value ranges.
        /// </summary>
        protected class Break
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Break"/> class.
            /// </summary>
            public Break()
            {
                Name = string.Empty;
                Maximum = 0;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Break"/> class.
            /// </summary>
            /// <param name="name">The string name for the break.</param>
            public Break(string name)
            {
                Name = name;
                Maximum = 0;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets a double value for the maximum value for the break.
            /// </summary>
            public double? Maximum { get; set; }

            /// <summary>
            /// Gets or sets the string name.
            /// </summary>
            public string Name { get; set; }

            #endregion
        }

        #endregion
    }
}