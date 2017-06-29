// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for DotSpatial.Drawing.CustomSymbolizer version 6.0
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
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/21/2009 4:18:14 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is a custom (predefined) symbolizer which is displayed in the 'Predefined symbol' control.
    /// </summary>
    [Serializable]
    public class CustomSymbolizer : ICustomSymbolizer
    {
        #region Private Variables

        private string _categoryName;
        private string _name;
        private IFeatureSymbolizer _symbolizer;
        private SymbolizerType _type;
        private string _uniqueName;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of a custom symbolizer
        /// </summary>
        public CustomSymbolizer()
        {
            _symbolizer = new PointSymbolizer();
            _uniqueName = "symbol 001";
            _name = "symbol 001";
            _categoryName = "default";
            _type = GetSymbolType(_symbolizer);
        }

        /// <summary>
        /// Creates a new instance of CustomSymbolizer
        /// </summary>
        public CustomSymbolizer(IFeatureSymbolizer symbolizer, string uniqueName, string name, string categoryName)
        {
            _symbolizer = symbolizer;
            _uniqueName = uniqueName;
            _name = name;
            _categoryName = categoryName;
            _type = GetSymbolType(_symbolizer);
        }

        #endregion

        #region Methods

        private static SymbolizerType GetSymbolType(IFeatureSymbolizer symbolizer)
        {
            if (symbolizer is PointSymbolizer)
            {
                return SymbolizerType.Point;
            }
            if (symbolizer is LineSymbolizer)
            {
                return SymbolizerType.Line;
            }
            if (symbolizer is PolygonSymbolizer)
            {
                return SymbolizerType.Polygon;
            }
            return SymbolizerType.Unknown;
        }

        #endregion

        #region Properties

        #endregion

        #region ICustomSymbolizer Members

        /// <summary>
        /// Jiri's code to save to XML
        /// </summary>
        /// <param name="xmlDataSource">The xml data source to load the symbology from</param>
        public void SaveToXml(string xmlDataSource)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Jiri's code to load from XML
        /// </summary>
        /// <param name="xmlDataSource">The xml Data source to load the symbology from</param>
        /// <param name="uniqueName">A Unique name for the symbology item</param>
        public void LoadFromXml(string xmlDataSource, string uniqueName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Jiri's code to load from XML
        /// </summary>
        /// <param name="xmlDataSource">The xml Data source to load the symbology from</param>
        /// <param name="group">The organizational group or category</param>
        /// <param name="name">The string name within the specified group or category</param>
        public void LoadFromXml(string xmlDataSource, string group, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the symbolizer for this predifined symbolizer
        /// </summary>
        public IFeatureSymbolizer Symbolizer
        {
            get
            {
                return _symbolizer;
            }
            set
            {
                _symbolizer = value;
                _type = GetSymbolType(_symbolizer);
            }
        }

        /// <summary>
        /// Gets or sets the string name for this predefined symbolizer
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the string group for this predefined symbolizer
        /// </summary>
        public string Category
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value;
            }
        }

        /// <summary>
        /// Gets or sets the unique name for this predefined symbolizer.
        /// </summary>
        public string UniqueName
        {
            get
            {
                return _uniqueName;
            }
            set
            {
                _uniqueName = value;
            }
        }

        /// <summary>
        /// The type of the symbolizer (point, line, polygon)
        /// </summary>
        public SymbolizerType SymbolType
        {
            get
            {
                return _type;
            }
        }

        #endregion
    }
}