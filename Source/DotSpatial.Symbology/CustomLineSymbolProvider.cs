// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for DotSpatial.Drawing.PredefinedSymbols version 6.0
// ********************************************************************************************************
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
    /// This class provides a list of predefined symbolizers.
    /// In the current implementation the symbolizers are 'hard-coded'.
    /// In other implementations they will be loaded from a xml resource file.
    /// </summary>
    public class CustomLineSymbolProvider
    {
        #region Fields

        // the list where the custom symbolizers are stored
        private readonly List<CustomLineSymbolizer> _list;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLineSymbolProvider"/> class.
        /// </summary>
        public CustomLineSymbolProvider()
        {
            _list = GetLineSymbols();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns all available predefined custom line symbols.
        /// </summary>
        /// <returns>A list of available custom line symbols.</returns>
        public List<CustomLineSymbolizer> GetAllSymbols()
        {
            return _list;
        }

        /// <summary>
        /// Gets a list of all available categories (groups) of custom predefined symbols.
        /// </summary>
        /// <returns>A list with the available categories.</returns>
        public List<string> GetAvailableCategories()
        {
            var query = (from item in _list select item.Category).Distinct();
            return query.ToList();
        }

        /// <summary>
        /// Gets a list of all predefined custom symbols that belong to the specific category.
        /// </summary>
        /// <param name="categoryName">The name of the category group</param>
        /// <returns>The list of custom symbols</returns>
        public List<CustomLineSymbolizer> GetSymbolsByCategory(string categoryName)
        {
            if (categoryName != string.Empty)
            {
                var query = from item in _list where item.Category == categoryName select item;
                return query.ToList();
            }

            return GetAllSymbols();
        }

        /// <summary>
        /// Saves the symbol to the given file.
        /// </summary>
        /// <param name="fileName">Name of the file the symbol should be saved to.</param>
        public void Save(string fileName)
        {
            _list.Save(fileName);
        }

        private static List<CustomLineSymbolizer> GetLineSymbols()
        {
            List<CustomLineSymbolizer> lst = new List<CustomLineSymbolizer>();

            LineSymbolizer sym1 = new LineSymbolizer();
            sym1.Strokes.Clear();
            SimpleStroke stroke11 = new SimpleStroke(Color.DarkGray);
            sym1.Strokes.Add(stroke11);
            CustomLineSymbolizer cust1 = new CustomLineSymbolizer
            {
                Symbolizer = sym1,
                UniqueName = "line_0001",
                Name = "simple line",
                Category = "Transportation"
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
                Name = "Road",
                Category = "Transportation"
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
                Category = "Transportation",
                Name = "Path"
            };
            lst.Add(cust3);

            LineSymbolizer sym4 = new LineSymbolizer();
            sym4.Strokes.Clear();
            SimpleStroke stroke41 = new SimpleStroke(4.5, Color.Gray);
            SimpleStroke stroke42 = new SimpleStroke(2.5, Color.Yellow);
            SimpleStroke stroke43 = new SimpleStroke(0.5, Color.Gray);
            sym4.Strokes.Add(stroke41);
            sym4.Strokes.Add(stroke42);
            sym4.Strokes.Add(stroke43);
            CustomLineSymbolizer cust4 = new CustomLineSymbolizer
            {
                Symbolizer = sym4,
                UniqueName = "line_0004",
                Category = "Transportation",
                Name = "Highway"
            };
            lst.Add(cust4);

            LineSymbolizer sym5 = new LineSymbolizer();
            sym5.Strokes.Clear();
            CartographicStroke stroke51 = new CartographicStroke(Color.Gray);
            CartographicStroke stroke52 = new CartographicStroke(Color.LightPink)
            {
                Width = 4.0,
                Offset = 2.5f,
                DashStyle = DashStyle.DashDotDot
            };
            sym5.Strokes.Add(stroke51);
            sym5.Strokes.Add(stroke52);
            CustomLineSymbolizer cust5 = new CustomLineSymbolizer
            {
                Symbolizer = sym5,
                UniqueName = "line_0005",
                Category = "Boundaries",
                Name = "Boundary 1"
            };
            lst.Add(cust5);

            LineSymbolizer sym6 = new LineSymbolizer();
            sym6.Strokes.Clear();
            SimpleStroke stroke53 = new SimpleStroke(Color.DarkBlue);
            SimpleStroke stroke54 = new SimpleStroke(Color.LightBlue);
            stroke53.Width = 3;
            sym6.Strokes.Add(stroke53);
            sym6.Strokes.Add(stroke54);
            CustomLineSymbolizer cust6 = new CustomLineSymbolizer
            {
                Symbolizer = sym6,
                UniqueName = "line_0006",
                Category = "Rivers",
                Name = "River"
            };
            lst.Add(cust6);

            return lst;
        }

        #endregion
    }
}