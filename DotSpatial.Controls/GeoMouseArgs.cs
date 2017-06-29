// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/5/2008 2:38:20 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;
using DotSpatial.Topology;

namespace DotSpatial.Controls
{
    public class GeoMouseArgs : MouseEventArgs
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of MouseArgs
        /// </summary>
        /// <param name="e"></param>
        /// <param name="inMap"></param>
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
        /// Gets the position of the Mouse Event in geographic coordinates
        /// </summary>
        public Coordinate GeographicLocation { get; protected set; }

        /// <summary>
        /// Gets a simple interface for the map where these events were generated
        /// </summary>
        public IMap Map { get; protected set; }

        /// <summary>
        /// Gets or sets a handled.  If this is set to true, then the mouse event is considered to
        /// be handled and will not be passed to any other functions in the stack.
        /// </summary>
        public bool Handled { get; set; }

        #endregion
    }
}