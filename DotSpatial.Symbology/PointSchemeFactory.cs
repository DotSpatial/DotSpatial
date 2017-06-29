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
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/23/2009 2:40:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointSchemeFactory
    /// </summary>
    public class PointSchemeFactory
    {
        #region Private Variables

        private string _classificationField;
        private string _normalizationField;
        private int _numCategories;
        private DataTable _table;
        private ISymbol _template;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PointSchemeFactory where the data Table is specified.
        /// </summary>
        public PointSchemeFactory(DataTable table)
        {
            _table = table;
            _template = new SimpleSymbol(Color.Green, PointShape.Ellipse, 4);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This causes the creation of a PointScheme
        /// </summary>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <param name="schemeType"></param>
        /// <returns></returns>
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the template symbol to use.  If using a color gradient, the shape and size will remain the same.
        /// If using a size gradient, the color and shape will remain the same.
        /// </summary>
        public ISymbol Template
        {
            get { return _template; }
            set { _template = value; }
        }

        /// <summary>
        /// Gets or sets the data Table that provides necessary information about the attributes for unique values to
        /// be calculated
        /// </summary>
        public DataTable Table
        {
            get { return _table; }
            set { _table = value; }
        }

        /// <summary>
        /// Gets or sets the string classification field to use
        /// </summary>
        public string ClassificationField
        {
            get { return _classificationField; }
            set { _classificationField = value; }
        }

        /// <summary>
        /// Gets or sets the string field to use for normalization.
        /// </summary>
        public string NormalizationField
        {
            get { return _normalizationField; }
            set { _normalizationField = value; }
        }

        /// <summary>
        /// Gets or sets the number of categories that will be used for classification schemes that don't
        /// come pre-configured with a given number of categories.
        /// </summary>
        public int NumCategories
        {
            get { return _numCategories; }
            set { _numCategories = value; }
        }

        #endregion

        #region Private Methods

        private static List<Color> RampColors(Color startColor, Color endColor, int numCategories)
        {
            List<Color> result = new List<Color>();
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
            PointScheme ps = new PointScheme();
            ps.Categories.Clear();
            foreach (Color color in colors)
            {
                IColorable c = _template as IColorable;
                if (c != null)
                {
                    c.Color = color;
                }
                PointCategory pc = new PointCategory(_template);
                ps.Categories.Add(pc);
            }
            ps.Categories[0].FilterExpression = "[" + _classificationField + "] < ";
            return ps;
        }

        #endregion
    }
}