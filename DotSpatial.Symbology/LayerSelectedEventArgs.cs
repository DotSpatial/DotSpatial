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