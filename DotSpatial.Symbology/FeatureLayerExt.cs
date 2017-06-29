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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/23/2009 4:23:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    public static class FeatureLayerExt
    {
        #region Methods

        /// <summary>
        /// Inverts the selection
        /// </summary>
        /// <param name="featureLayer"></param>
        public static void InvertSelection(this IFeatureLayer featureLayer)
        {
            IEnvelope ignoreMe;
            IEnvelope env = featureLayer.Extent.ToEnvelope();
            featureLayer.InvertSelection(env, env, SelectionMode.IntersectsExtent, out ignoreMe);
        }

        #endregion

    }
}