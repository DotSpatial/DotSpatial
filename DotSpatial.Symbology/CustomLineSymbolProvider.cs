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

using System;
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
    public class CustomLineSymbolProvider : ICustomLineSymbolProvider
    {
        #region Private Variables

        //the list where the custom symbolizers are stored
        private readonly List<CustomLineSymbolizer> _list;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PredefinedLineSymbolProvider
        /// </summary>
        public CustomLineSymbolProvider()
        {
            _list = GetLineSymbols();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string array of all available categories (groups) of custom predefined symbols
        /// </summary>
        public List<string> GetAvailableCategories()
        {
            var query = (from item in _list
                         select item.Category).Distinct();
            return query.ToList();
        }

        /// <summary>
        /// Returns all available predefined custom line symbols
        /// </summary>
        /// <returns>A list of vailable custom line symbols</returns>
        public List<CustomLineSymbolizer> GetAllSymbols()
        {
            return _list;
        }

        /// <summary>
        /// Gets a list of all predefined custom symbols that belong to the specific category
        /// </summary>
        /// <param name="categoryName">The name of the category group</param>
        /// <returns>The list of custom symbols</returns>
        public List<CustomLineSymbolizer> GetSymbolsByCategory(string categoryName)
        {
            if (categoryName != string.Empty)
            {
                var query = (from item in _list
                             where item.Category == categoryName
                             select item);

                return query.ToList();
            }
            return GetAllSymbols();
        }

        #endregion

        #region Properties

        #endregion

        #region Private Methods

        private static List<CustomLineSymbolizer> GetLineSymbols()
        {
            List<CustomLineSymbolizer> lst = new List<CustomLineSymbolizer>();

            LineSymbolizer sym1 = new LineSymbolizer();
            sym1.Strokes.Clear();
            SimpleStroke stroke11 = new SimpleStroke(Color.DarkGray);
            sym1.Strokes.Add(stroke11);
            CustomLineSymbolizer cust1 = new CustomLineSymbolizer();
            cust1.Symbolizer = sym1;
            cust1.UniqueName = "line_0001";
            cust1.Name = "simple line";
            cust1.Category = "Transportation";
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
            cust2.Name = "Road";
            cust2.Category = "Transportation";
            lst.Add(cust2);

            LineSymbolizer sym3 = new LineSymbolizer();
            sym3.Strokes.Clear();
            CartographicStroke stroke3 = new CartographicStroke(Color.Brown);
            stroke3.DashStyle = DashStyle.Dash;
            sym3.Strokes.Add(stroke3);
            CustomLineSymbolizer cust3 = new CustomLineSymbolizer();
            cust3.Symbolizer = sym3;
            cust3.UniqueName = "line_0003";
            cust3.Category = "Transportation";
            cust3.Name = "Path";
            lst.Add(cust3);

            LineSymbolizer sym4 = new LineSymbolizer();
            sym4.Strokes.Clear();
            SimpleStroke stroke41 = new SimpleStroke(4.5, Color.Gray);
            SimpleStroke stroke42 = new SimpleStroke(2.5, Color.Yellow);
            SimpleStroke stroke43 = new SimpleStroke(0.5, Color.Gray);
            sym4.Strokes.Add(stroke41);
            sym4.Strokes.Add(stroke42);
            sym4.Strokes.Add(stroke43);
            CustomLineSymbolizer cust4 = new CustomLineSymbolizer();
            cust4.Symbolizer = sym4;
            cust4.UniqueName = "line_0004";
            cust4.Category = "Transportation";
            cust4.Name = "Highway";
            lst.Add(cust4);

            LineSymbolizer sym5 = new LineSymbolizer();
            sym5.Strokes.Clear();
            CartographicStroke stroke51 = new CartographicStroke(Color.Gray);
            CartographicStroke stroke52 = new CartographicStroke(Color.LightPink);
            stroke52.Width = 4.0;
            stroke52.Offset = 2.5f;
            stroke52.DashStyle = DashStyle.DashDotDot;
            sym5.Strokes.Add(stroke51);
            sym5.Strokes.Add(stroke52);
            CustomLineSymbolizer cust5 = new CustomLineSymbolizer();
            cust5.Symbolizer = sym5;
            cust5.UniqueName = "line_0005";
            cust5.Category = "Boundaries";
            cust5.Name = "Boundary 1";
            lst.Add(cust5);

            LineSymbolizer sym6 = new LineSymbolizer();
            sym6.Strokes.Clear();
            SimpleStroke stroke53 = new SimpleStroke(Color.DarkBlue);
            SimpleStroke stroke54 = new SimpleStroke(Color.LightBlue);
            stroke53.Width = 3;
            sym6.Strokes.Add(stroke53);
            sym6.Strokes.Add(stroke54);
            CustomLineSymbolizer cust6 = new CustomLineSymbolizer();
            cust6.Symbolizer = sym6;
            cust6.UniqueName = "line_0006";
            cust6.Category = "Rivers";
            cust6.Name = "River";
            lst.Add(cust6);

            return lst;
        }

        #endregion

        #region ICustomLineSymbolProvider Members

        /// <summary>
        /// Loads a list of custom line symbolizers from a binary serialized file
        /// </summary>
        /// <param name="fileName">The serialized binary file</param>
        /// <returns>The list of custom line symbolizers</returns>
        public IEnumerable<CustomLineSymbolizer> Load(string fileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            _list.Save(fileName);
        }

        #endregion
    }
}