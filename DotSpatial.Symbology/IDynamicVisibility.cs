// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Symbology.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/29/2010 10:40:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An shared interface for members that wish to set dynamic visiblity.
    /// </summary>
    public interface IDynamicVisibility
    {
        /// <summary>
        /// Dynamic visibility represents layers that only appear when you zoom in close enough.
        /// This value represents the geographic width where that happens.
        /// </summary>
        double DynamicVisibilityWidth { get; set; }

        /// <summary>
        /// This controls whether the layer is visible when zoomed in closer than the dynamic
        /// visiblity width or only when further away from the dynamic visibility width
        /// </summary>
        DynamicVisibilityMode DynamicVisibilityMode { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether dynamic visibility should be enabled.
        /// </summary>
        bool UseDynamicVisibility
        {
            get;
            set;
        }
    }
}