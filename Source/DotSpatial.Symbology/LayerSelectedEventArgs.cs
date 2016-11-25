// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/10/2008 3:05:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
  
    public class LayerSelectedEventArgs : LayerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of LayerEventArgs
        /// </summary>
        public LayerSelectedEventArgs(ILayer layer, bool selected)
            : base(layer)
        {
            IsSelected = selected;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a boolean that indicates whether or not the layer is selected
        /// </summary>
        public bool IsSelected { get; protected set; }

        #endregion
    }
}