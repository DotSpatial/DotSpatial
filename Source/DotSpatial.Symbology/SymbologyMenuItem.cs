// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from DotSpatial.Symbology.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/18/2010 12:55:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This class simply stores enough information for the appropriate menu item to be
    /// generated. It is not the Windows Forms MenuItem.
    /// </summary>
    public class SymbologyMenuItem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbologyMenuItem"/> class.
        /// </summary>
        /// <param name="name">The name or text to appear for this item</param>
        /// <param name="image">The icon to draw for this menu item</param>
        /// <param name="clickHandler">The click event handler</param>
        public SymbologyMenuItem(string name, Image image, EventHandler clickHandler)
        {
            MenuItems = new List<SymbologyMenuItem>();
            Name = name;
            ClickHandler = clickHandler;
            Image = image;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbologyMenuItem"/> class.
        /// </summary>
        /// <param name="name">The name or text to appear for this item</param>
        /// <param name="icon">The icon to draw for this menu item</param>
        /// <param name="clickHandler">The click event handler</param>
        public SymbologyMenuItem(string name, Icon icon, EventHandler clickHandler)
        {
            MenuItems = new List<SymbologyMenuItem>();
            Name = name;
            ClickHandler = clickHandler;
            Icon = icon;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbologyMenuItem"/> class.
        /// </summary>
        /// <param name="name">The name or text to appear for this item</param>
        /// <param name="clickHandler">The click event handler</param>
        public SymbologyMenuItem(string name, EventHandler clickHandler)
        {
            MenuItems = new List<SymbologyMenuItem>();
            Name = name;
            ClickHandler = clickHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbologyMenuItem"/> class.
        /// </summary>
        /// <param name="name">The name or text to appear for this item</param>
        public SymbologyMenuItem(string name)
        {
            MenuItems = new List<SymbologyMenuItem>();
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the handler for the click event.
        /// </summary>
        public EventHandler ClickHandler { get; set; }

        /// <summary>
        /// Gets or sets the icon for this menu item
        /// </summary>
        public Icon Icon { get; set; }

        /// <summary>
        /// Gets or sets the image for this menu item
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the set of sub-menu items.
        /// </summary>
        public List<SymbologyMenuItem> MenuItems { get; set; }

        /// <summary>
        /// Gets or sets the name for this menu item
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}