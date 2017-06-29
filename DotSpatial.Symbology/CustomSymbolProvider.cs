// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for DotSpatial.Drawing.PredefinedSymbols version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/15/2009 10:26:52 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        #region Private Variables

        //the list where the custom symbolizers are stored
        private readonly List<CustomSymbolizer> _list;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PredefinedLineSymbolProvider
        /// </summary>
        public CustomSymbolProvider()
        {
            _list = new List<CustomSymbolizer>();
            _list.AddRange(getBasicPointSymbols());
            _list.AddRange(getBasicLineSymbols());
            _list.AddRange(getBasicPolygonSymbols());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string array of all available categories (groups) of custom predefined symbols
        /// </summary>
        public string[] GetAvailableCategories(SymbolizerType symbolType)
        {
            var query = (from item in _list
                         where item.SymbolType == symbolType
                         select item.Category).Distinct();

            return query.ToArray();
        }

        #endregion

        #region Properties

        #endregion

        #region Private Methods

        private static List<CustomSymbolizer> getBasicPointSymbols()
        {
            List<CustomSymbolizer> lst = new List<CustomSymbolizer>();

            PointSymbolizer s1 = new PointSymbolizer();
            const char ch = '\u21e6';
            s1.Symbols.Add(new CharacterSymbol(ch, "arial", Color.Blue, 10.0));
            lst.Add(new CustomSymbolizer(s1, "symbol01", "arrow1", "arrows"));

            PointSymbolizer s2 = new PointSymbolizer(Color.Beige, PointShape.Ellipse, 7.0);
            lst.Add(new CustomSymbolizer(s2, "symbol02", "circle1", "default"));

            return lst;
        }

        private static List<CustomSymbolizer> getBasicLineSymbols()
        {
            List<CustomSymbolizer> lst = new List<CustomSymbolizer>();

            LineSymbolizer sym1 = new LineSymbolizer();
            CustomLineSymbolizer cust1 = new CustomLineSymbolizer();
            cust1.Symbolizer = sym1;
            cust1.UniqueName = "line_0001";
            cust1.Category = "default";
            lst.Add(cust1);

            LineSymbolizer sym2 = new LineSymbolizer();
            sym2.Strokes.Clear();
            SimpleStroke stroke1 = new SimpleStroke(2.5, Color.Brown);
            sym2.Strokes.Add(stroke1);
            SimpleStroke stroke0 = new SimpleStroke(1.0, Color.Yellow);
            sym2.Strokes.Add(stroke0);
            CustomLineSymbolizer cust2 = new CustomLineSymbolizer();
            cust2.Symbolizer = sym2;
            cust2.UniqueName = "line_0002";
            cust2.Category = "default";
            lst.Add(cust2);

            LineSymbolizer sym3 = new LineSymbolizer();
            sym3.Strokes.Clear();
            CartographicStroke stroke3 = new CartographicStroke(Color.Brown);
            stroke3.DashStyle = DashStyle.Dash;
            sym3.Strokes.Add(stroke3);
            CustomLineSymbolizer cust3 = new CustomLineSymbolizer();
            cust3.Symbolizer = sym3;
            cust3.UniqueName = "line_0003";
            cust3.Category = "travel";
            cust3.Name = "path";
            lst.Add(cust3);
            return lst;
        }

        private static List<CustomSymbolizer> getBasicPolygonSymbols()
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