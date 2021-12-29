// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LegendIcon.
    /// </summary>
    [ToolboxItem(false)]
    public class LegendIcon : LegendItem
    {
        #region Fields

        private Icon _icon;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendIcon"/> class.
        /// </summary>
        public LegendIcon()
        {
            Configure();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the icon to draw for this legend item.
        /// </summary>
        public virtual Icon Icon
        {
            get
            {
                return _icon;
            }

            set
            {
                _icon = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the legend symbol size (as an icon size).
        /// </summary>
        /// <returns>The legend symbol size.</returns>
        public override Size GetLegendSymbolSize()
        {
            return _icon?.Size ?? new Size(16, 16);
        }

        /// <summary>
        /// Draws the icon to the legend.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="box">The rectangle used for drawing the icon.</param>
        public override void LegendSymbolPainted(Graphics g, Rectangle box)
        {
            if (_icon != null)
            {
                g.DrawIcon(_icon, box);
            }
        }

        private void Configure()
        {
            LegendSymbolMode = SymbolMode.Symbol;
            LegendType = LegendType.Symbol;
        }

        #endregion
    }
}