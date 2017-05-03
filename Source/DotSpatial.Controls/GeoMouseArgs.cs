// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/5/2008 2:38:20 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// GeoMouseArgs
    /// </summary>
    public class GeoMouseArgs : MouseEventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoMouseArgs"/> class.
        /// </summary>
        /// <param name="e">The mouse event args.</param>
        /// <param name="inMap">The map.</param>
        public GeoMouseArgs(MouseEventArgs e, IMap inMap)
            : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            if (inMap == null) return;
            GeographicLocation = inMap.PixelToProj(e.Location);
            Map = inMap;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the position of the Mouse Event in geographic coordinates.
        /// </summary>
        public Coordinate GeographicLocation { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse event is considered to
        /// be handled and will not be passed to any other functions in the stack.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets a simple interface for the map where these events were generated.
        /// </summary>
        public IMap Map { get; protected set; }

        #endregion
    }
}