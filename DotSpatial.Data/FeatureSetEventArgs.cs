// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Original Code is from DotSpatial.Data.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/29/2010 9:05:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An EventArgs specifically tailored to FeatureSet.
    /// </summary>
    public class FeatureSetEventArgs : EventArgs
    {
        private IFeatureSet _featureSet;

        /// <summary>
        /// Initializes a new instance of the FeatureSetEventArgs class.
        /// </summary>
        /// <param name="featureSet">The IFeatureSet that is involved in this event.</param>
        public FeatureSetEventArgs(IFeatureSet featureSet)
        {
            _featureSet = featureSet;
        }

        /// <summary>
        /// Gets the Raster associated with this event.
        /// </summary>
        public IFeatureSet FeatureSet
        {
            get { return _featureSet; }
            protected set { _featureSet = value; }
        }
    }
}