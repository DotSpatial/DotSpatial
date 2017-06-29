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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2008 6:52:58 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LegendIcon
    /// </summary>
    [ToolboxItem(false)]
    public class LegendIcon : LegendItem
    {
        #region Private Variables

        private Icon _icon;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LegendIcon
        /// </summary>
        public LegendIcon()
        {
            Configure();
        }

        private void Configure()
        {
            base.LegendSymbolMode = SymbolMode.Symbol;
            LegendType = LegendType.Symbol;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the legend symbol size (as an icon size)
        /// </summary>
        /// <returns></returns>
        public override Size GetLegendSymbolSize()
        {
            return _icon != null ? _icon.Size : new Size(16, 16);
        }

        /// <summary>
        /// Draws the icon to the legend
        /// </summary>
        /// <param name="g"></param>
        /// <param name="box"></param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            if (_icon != null)
            {
                g.DrawIcon(_icon, box);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The icon to draw for this legend item
        /// </summary>
        public virtual Icon Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
            }
        }

        #endregion
    }
}