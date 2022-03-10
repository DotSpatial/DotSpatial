// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointSchemeFactory.
    /// </summary>
    public class PointSchemeFactory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSchemeFactory"/> class where the data table is specified.
        /// </summary>
        /// <param name="table">Datatable that is used.</param>
        public PointSchemeFactory(DataTable table)
        {
            Table = table;
            Template = new SimpleSymbol(Color.Green, PointShape.Ellipse, 4);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string classification field to use.
        /// </summary>
        public string ClassificationField { get; set; }

        /// <summary>
        /// Gets or sets the string field to use for normalization.
        /// </summary>
        public string NormalizationField { get; set; }

        /// <summary>
        /// Gets or sets the number of categories that will be used for classification schemes that don't
        /// come pre-configured with a given number of categories.
        /// </summary>
        public int NumCategories { get; set; }

        /// <summary>
        /// Gets or sets the data Table that provides necessary information about the attributes for unique values to
        /// be calculated.
        /// </summary>
        public DataTable Table { get; set; }

        /// <summary>
        /// Gets or sets the template symbol to use. If using a color gradient, the shape and size will remain the same.
        /// If using a size gradient, the color and shape will remain the same.
        /// </summary>
        public ISymbol Template { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This causes the creation of a PointScheme.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        /// <param name="endColor">The end color.</param>
        /// <param name="schemeType">The scheme type used for creating the colors.</param>
        /// <returns>Null if an unallowed scheme type is used otherwise the created point scheme.</returns>
        public PointScheme ColorRamp(Color startColor, Color endColor, QuickSchemeType schemeType)
        {
            switch (schemeType)
            {
                case QuickSchemeType.Box:
                    List<Color> colors = RampColors(startColor, endColor, 6);
                    return ColorBox(colors);
            }

            return null;
        }

        private static List<Color> RampColors(Color startColor, Color endColor, int numCategories)
        {
            List<Color> result = new();
            if (numCategories <= 0) return result;

            if (numCategories < 2)
            {
                result.Add(startColor);
                return result;
            }

            if (numCategories == 2)
            {
                result.Add(startColor);
                result.Add(endColor);
                return result;
            }

            double dR = (endColor.R - startColor.R) / (double)(numCategories - 1);
            double dG = (endColor.G - startColor.G) / (double)(numCategories - 1);
            double dB = (endColor.B - startColor.B) / (double)(numCategories - 1);
            double dA = (endColor.A - startColor.A) / (double)(numCategories - 1);
            for (int i = 0; i < numCategories; i++)
            {
                int a = Convert.ToInt32(startColor.A + dA * i);
                int r = Convert.ToInt32(startColor.R + dR * i);
                int g = Convert.ToInt32(startColor.G + dG * i);
                int b = Convert.ToInt32(startColor.B + dB * i);
                result.Add(Color.FromArgb(a, r, g, b));
            }

            return result;
        }

        private PointScheme ColorBox(IEnumerable<Color> colors)
        {
            PointScheme ps = new();
            ps.Categories.Clear();
            foreach (Color color in colors)
            {
                if (Template is IColorable c)
                {
                    c.Color = color;
                }

                PointCategory pc = new(Template);
                ps.Categories.Add(pc);
            }

            ps.Categories[0].FilterExpression = "[" + ClassificationField + "] < ";
            return ps;
        }

        #endregion
    }
}