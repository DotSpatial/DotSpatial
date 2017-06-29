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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/20/2009 3:49:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PolygonSymbolizerEventArgs
    /// </summary>
    public class PolygonSymbolizerEventArgs : FeatureSymbolizerEventArgs
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PolygonSymbolizerEventArgs
        /// </summary>
        public PolygonSymbolizerEventArgs(IPolygonSymbolizer symbolizer)
            : base(symbolizer)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Symbolizer, casting it to an IPolygonSymbolizer
        /// </summary>
        public new IPolygonSymbolizer Symbolizer
        {
            get { return base.Symbolizer as IPolygonSymbolizer; }
            set { base.Symbolizer = value; }
        }

        #endregion
    }
}