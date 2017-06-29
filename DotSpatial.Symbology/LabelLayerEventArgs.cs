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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/29/2010 9:05:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An EventArgs specifically tailored to LabelLayerEventArgs.
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class LabelLayerEventArgs : EventArgs
    {
        private ILabelLayer _labelLayer;

        /// <summary>
        /// Initializes a new instance of the LabelLayerEventArgs class.
        /// </summary>
        /// <param name="labelLayer">The ILabelLayer that is involved in this event.</param>
        public LabelLayerEventArgs(ILabelLayer labelLayer)
        {
            _labelLayer = labelLayer;
        }

        /// <summary>
        /// Gets the LabelLayer associated with this event.
        /// </summary>
        public ILabelLayer LabelLayer
        {
            get { return _labelLayer; }
            protected set { _labelLayer = value; }
        }
    }
}