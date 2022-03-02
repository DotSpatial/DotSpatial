// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This class returns a list of predefined symbolizers.
    /// In the current implementation the symbolizers are 'hard-coded'.
    /// In other implementations they will be loaded from a xml resource file.
    /// </summary>
    public class CustomSymbolProvider
    {
        #region Fields

        // the list where the custom symbolizers are stored
        private readonly List<CustomSymbolizer> _list;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSymbolProvider"/> class.
        /// </summary>
        public CustomSymbolProvider()
        {
            _list = new List<CustomSymbolizer>();
            _list.AddRange(GetBasicPointSymbols());
            _list.AddRange(GetBasicLineSymbols());
            _list.AddRange(GetBasicPolygonSymbols());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string array of all available categories (groups) of custom predefined symbols.
        /// </summary>
        /// <param name="symbolType">Symbol type to search for.</param>
        /// <returns>A string array of all available categories.</returns>
        public string[] GetAvailableCategories(SymbolizerType symbolType)
        {
            var query = (from item in _list where item.SymbolType == symbolType select item.Category).Distinct();

            return query.ToArray();
        }

        private static List<CustomSymbolizer> GetBasicLineSymbols()
        {
            List<CustomSymbolizer> lst = new List<CustomSymbolizer>();

            LineSymbolizer sym1 = new LineSymbolizer();
            CustomLineSymbolizer cust1 = new CustomLineSymbolizer
            {
                Symbolizer = sym1,
                UniqueName = "line_0001",
                Category = "default"
            };
            lst.Add(cust1);

            LineSymbolizer sym2 = new LineSymbolizer();
            sym2.Strokes.Clear();
            SimpleStroke stroke1 = new SimpleStroke(2.5, Color.Brown);
            sym2.Strokes.Add(stroke1);
            SimpleStroke stroke0 = new SimpleStroke(1.0, Color.Yellow);
            sym2.Strokes.Add(stroke0);
            CustomLineSymbolizer cust2 = new CustomLineSymbolizer
            {
                Symbolizer = sym2,
                UniqueName = "line_0002",
                Category = "default"
            };
            lst.Add(cust2);

            LineSymbolizer sym3 = new LineSymbolizer();
            sym3.Strokes.Clear();
            CartographicStroke stroke3 = new CartographicStroke(Color.Brown)
            {
                DashStyle = DashStyle.Dash
            };
            sym3.Strokes.Add(stroke3);
            CustomLineSymbolizer cust3 = new CustomLineSymbolizer
            {
                Symbolizer = sym3,
                UniqueName = "line_0003",
                Category = "travel",
                Name = "path"
            };
            lst.Add(cust3);
            return lst;
        }

        private static List<CustomSymbolizer> GetBasicPointSymbols()
        {
            List<CustomSymbolizer> lst = new List<CustomSymbolizer>();

            PointSymbolizer s1 = new PointSymbolizer();
            const char Ch = '\u21e6';
            s1.Symbols.Add(new CharacterSymbol(Ch, "arial", Color.Blue, 10.0));
            lst.Add(new CustomSymbolizer(s1, "symbol01", "arrow1", "arrows"));

            PointSymbolizer s2 = new PointSymbolizer(Color.Beige, PointShape.Ellipse, 7.0);
            lst.Add(new CustomSymbolizer(s2, "symbol02", "circle1", "default"));

            return lst;
        }

        private static List<CustomSymbolizer> GetBasicPolygonSymbols()
        {
            List<CustomSymbolizer> lst = new List<CustomSymbolizer>();

            PolygonSymbolizer s1 = new PolygonSymbolizer();
            s1.SetFillColor(Color.Beige);
            lst.Add(new CustomSymbolizer(s1, "poly01", "polygon 1", "default"));

            PolygonSymbolizer s2 = new PolygonSymbolizer();
            s2.SetFillColor(Color.LightGreen);
            s2.SetOutlineWidth(2.0);
            s2.OutlineSymbolizer.SetFillColor(Color.Red);
            lst.Add(new CustomSymbolizer(s2, "poly02", "polygon 2", "category2"));

            return lst;
        }

        #endregion
    }
}